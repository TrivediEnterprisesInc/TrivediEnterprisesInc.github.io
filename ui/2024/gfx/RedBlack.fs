module RBTree =
    //src:fssnip
    
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
            
    let tests() =             
        show [2;5;8;7;10;3;4;1;9;6]
        show [1..10]
        //isMember t 18 |> printfn "isMember 18: %A"
        (* These two create the same trees (ie sorts on ID are auto)
        let charL = {'a'..'z'} |> List.ofSeq
        List.map (fun i -> TreePL (i,(charL.[i]))) [0..25]
        |> show //Main.RBTree.Tree.Node[Main.RBTree.TreePL[Char]]
        show [0..25]  //Main.RBTree.Tree.Node[Int32]
        *)
    printfn "eof.."
