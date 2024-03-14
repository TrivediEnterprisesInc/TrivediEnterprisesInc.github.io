module RBTree =

/*  Notes:
    (i) RedBlkTree Does work as advertised (see RedBlack.fs)
    (ii) No gd for us for current use-case (how low-lvl do we nd?)
    (iii) Apres Okasaki there've been developments 
    (2011: left-leaning tree - no red has red-right child: 
    fewer match cases in balancing; therefore more efficient)
    (iv) oCaml impl used in their Set.  If so, and since we have no hi-Vol data here, just stick w/standard data strucs.
    (v) there are many ways to optimize for our use-case e.g.:
        [a] preGen a 4m rec empty RBTree >> recurse on demand to return an intPointer to the rec nded.  
        [b] gen subTrees in parallel and fill a fixed-ht preBalanced tree by splitting to known paralellizeable chunks).  However, there are only 4m rec-batches and mongo for instance can easily handle this in microSecs.
        [c] preGen all 4.5k n,p seq.s, serToDisk; onDemand [read to mem >> proc >> next] this is a bkgrndTask
        [d] same as above but save to db >> id is hash of n,p,itemId >> directAccess.

    Move the fs file 2 snippets for ref
    src:fssnip
    for tree html rendering, see: https://stackoverflow.com/questions/15000341/how-to-display-a-binary-search-tree-using-css-html-and-a-bit-of-javascript (the css ans on the bottom; all links're only for tree algo.s)


WorkFlow for angLee

* To avoid creating a 4m rec tbl for ea user; consider:
  - UserInit gens 2 params (say, n & p)
    n = probably fixed but keep open coz some usrs might wish to endProc/lose interest
    p = range betw 15 & 30(?)
  - Therefore, you could preGen "routes" with params (n,p) which wld be only 4.5k tot, usu less
  - AdminTbl.UsrRec stores params and the one table (ea rec has 4m vals; keyed on n/p hash)
    supplies the user's specific route on demand.
  - Note that on repl gen the 4m was almost instant (inMem)

Flow:
  User finishes initProc and is accepted/validated
  AllRoutesTbl.getRoutes(usr,n,p) 
  -> returns 100 vals -> stored in UsrRec.embddFld _or_ UsrTbl
  On each call AdminTbl.UsrRec.fld_int_NxtRouteNum ++
  If fld_int_NxtRouteNum = 50 
  -> BackgroundTsk_RefillUsrRoutes(usr,n,p) -> returns 50 for refill
*/

    open Trivedi.Core
    open System.IO
    //open System.Text.Encoding

    (*  PwrShl stuff:
        Get-Clipboard -format text > rb.fs
        ./fsc rb.fs --target:exe --platform:x64 -I:C:\windows\Microsoft.NET\Framework64\v4.0.30319 -r:Trivedi.Core.dll
    *)

    type TreePL<'p> = | TreePL of nodeId:int * pl:'p
    
    type nodeColor = Red | Black with
        member x.toStr() = 
            match x with
            | Red -> "Red"
            | _ -> "Black"
    
    type 'a Tree = 
        | Empty
        | Node of 'a TreeNode
    and 'a TreeNode = { value : 'a; nodeColor : nodeColor; left : 'a Tree; right : 'a Tree }


    let empty : 'a Tree = Empty

    /// please note: the compiler got issues if you use '==' on v and v'
    let rec isMember (t : 'a Tree) (v : 'a) : bool =
        match t with
        | Empty -> false
        | Node { value = v'; nodeColor = _; left = l; right = r } ->
            if v < v' then isMember l v
            else if v > v' then isMember r v
            else true

    /// inserts a new Element
    let insert (x : 'a) (t : 'a Tree) : 'a Tree =
        // force resulting node's nodeColor to be black
        let makeBlack = function
            | Node { value = y; nodeColor = _; left = a; right = b} -> Node { value = y; nodeColor = Black; left = a; right = b }
            | Empty -> failwith "unexpected case"

        // balance the tree
        let rec balance (nodeColor : nodeColor) (a : 'a Tree) (x : 'a) (b : 'a Tree) =
            // rather unreadable - see the mentioned article for details
            match (nodeColor, a, x, b) with
            | (Black, Node { value = y; nodeColor = Red; left = Node { value = x; nodeColor = Red; left = a; right = b }; right = c}, z, d)
            | (Black, Node { value = x; nodeColor = Red; left = a; right = Node { value = y; nodeColor = Red; left = b; right = c }; }, z, d)
            | (Black, a, x, Node { value = z; nodeColor = Red; left = Node { value = y; nodeColor = Red; left = b; right = c }; right = d; })
            | (Black, a, x, Node { value = y; nodeColor = Red; left = b; right = Node { value = z; nodeColor = Red; left = c; right = d }; }) ->
                Node {  value = y; nodeColor = Red; 
                        left = Node {value = x; nodeColor = Black; left = a; right = b}; 
                        right = Node {value = z; nodeColor = Black; left = c; right = d}
                     }
            | _ -> Node { value = x; nodeColor = nodeColor; left = a; right = b }

        // recursive insert
        let rec ins t =
            match t with
            // initialise a new node's nodeColor to red
            | Empty -> Node { value = x; nodeColor = Red; left = Empty; right = Empty }
            | Node { value = y; nodeColor = nodeColor; left = a; right = b } ->
                if x < y then balance nodeColor (ins a) y b
                else if x > y then balance nodeColor a y (ins b)
                else Node { value = y; nodeColor = nodeColor; left = a; right = b }

        makeBlack (ins t)

    /// insert many values
    let insertMany (xs : 'a seq) (t : 'a Tree) : 'a Tree =
        let switch f = fun y x -> f x y
        xs |> Seq.fold (switch insert) t

    let treeToString tree = 
        let rec loop t cont =
            match t with
            | Empty -> cont ""
            | Node { value = v; nodeColor = c; left = Empty; right = Empty } -> cont <| (v).ToString() + " " + c.toStr()
            | Node { value = v; nodeColor = c; left = lt; right = rt } ->
                loop lt <| fun lstr -> loop rt <| fun rstr -> cont <| ((v).ToString() + " " + c.toStr()) + " (" + lstr + "," + rstr + ") "
        loop tree id

    let bldTree = fun li -> Empty |> insertMany li

    let show = fun li ->
                    let t = bldTree li
                    printfn "treeTy: %A\n tree: %A" (t.GetType()) t
                    printfn "tree ToStr: %A" (treeToString t)
            
    let tests =
        let l1 = [2;5;8;7;10;3;4;1;9;6]
        show l1
        show [1..10]
        //isMember t 18 |> printfn "isMember 18: %A"
        (* These two create the same trees (ie sorts on ID are auto)
        let charL = {'a'..'z'} |> List.ofSeq
        List.map (fun i -> TreePL (i,(charL.[i]))) [0..25]
        |> show //Main.RBTree.Tree.Node[Main.RBTree.TreePL[Char]]
        show [0..25]  //Main.RBTree.Tree.Node[Int32]
        *)
        liShuffle l1 (getRandS()) |> printfn "res of liShuffle: %A"
        //File.ReadAllText("allImgs.64") |> b64Dec |> deSerBA
        deSerToList "allImgs.64"
        |> printfn "res of b64: %A"
    printfn "eof.."
