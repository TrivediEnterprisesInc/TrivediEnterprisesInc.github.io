(*  brij (TM)
    Copyright (c) M. P. Trivedi 2016-2023.  All rights reserved. 
    TrivediBldHdr->
    |Trivedi SrcCtrlHdr File.  Copyright (c) 2015-2023 M. P. Trivedi.  All rights reserved.|637901455|winFrms.fs|none|- - - - - - -
>* rockdale * Beckham * YOAKUM * jimwells * FLOYD * Pushmataha * baltimore * 
<* SanMateo * CONWAY * frontier * CONECUH * Bingham * chittenden * Valley * 
<* GILLIAM * nacogdoches * BROOKS * Broomfield,CityandCountyof * montana * Houghton * GOLDENVALLEY * <* california * IZARD * Haywood * broome * Cache * JOHNSON * centre * 
<* PRINCEGEORGE * Wilcox * massachusetts * LasAnimas * BEXAR * dakota * PROVIDENCE * <

  Contains the foll modules:

	GridTester		Ide
	Ext (bookmarkPainter/addTreeVwZip/bldPSlideTester)
	Cambattable
	deck		
	perms (combines earlier Base + Red)
	Bhojpuri (replaces cPnl, rPnl, etc.; incl new tys)
	jimmy			Tokenizer
	Folding			FilePanelUpdates
	Outliner		frmDelta (ty DeltaTracker...)
	clientInit (UI/UIAux/Brij updates) <- !!contains new Keywds!!
	Dnd_ops
	main
	
New reqmt:  Zeep :  user auth/registration/assign API keys.  Is this doable via webhooks/gitEmail?

VisualDb.com ('advanced filtering and/or'|100k recs/vw|Unlim|BYOD|SQL)

github.com/refinedev/refine|MIT| 15k devs 'headlessUI incl material|ProjCreationWizrd
github.com/marmelab/react-admin
"Refine & react-admin are the same noco solutions & are going after retool..."

retool: dashbrdMaker/React/'MSAcc on steroids'
junjat.com: loco uses Refine as a UI frmwrk 2 rndr data models

github.com/BudiBase
github.com/appsmithorg
github.com/ToolJet
github.com/lowdefy
github.com/windmill-labs/windmill
Onu
frappeframework.com : OSS loco
Flask AppBldr competes w/ MS PowerApps
superblocks.com

sql.ophir.dev: bld entire apps & intl tools
Hansura|Supabase


marmelab.com/blog/2023/07/04/react-admin-...
*)

namespace Trivedi

#nowarn "20" "25" "58" "66" "67" "64" "760" "1182" "1558"

#if !modPerms
module perms =
    open System
    open System.Diagnostics
    //open System.Diagnostics.PerformanceCounter
    //open System.Windows.Forms
    open Trivedi.Core

    printfn "...init module perms..."
    System.Console.ReadKey(true) |> ignore

#if Notes
    PermsBase returned a result where res/20 = 2.3M
    we nd to optimize for (Vlen + Plen)

        ///UsrCycleInitCounter
        type UCIC = | UCIC of int with
            member this.getNext() = usrCfg.UCIC <- Rnd(0, (vGenLi.Len - 1))
              
        ///UsrCurrCounter
        type UCC = | UCC of int with
            member this.getNext(UCIC) = 
                match (UCC < UCIC - 50) with
                | true -> sendSystemMsg(Level.Critical, "User " + usrCfg.UserNm + " about to reach vCycle completion!")
                | _ -> ()
                match (not (UCIC < (vGenLi.len - 1)))
                | true -> usrCfg.UCC <- 0; ret vGenLi.head
                | _ -> usrCfg.UCC <- =+ 1; ret vGenLi.[usrCfg.UCC]

    Formula 2 manually calc perms ->
        https://www.mathsisfun.com/combinatorics/combinations-permutations.html
        n! / (n − r)!
        where n is the number of things to choose from,
            and we choose r of them,
            no repetitions, order matters.

#endif //Notes


#if failsOnMono
    let memStat() =
        //see: https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.process.privatememorysize64?view=net-7.0
        use proc = Process.GetCurrentProcess()
        (proc.PrivateMemorySize64 / ((1024*1024) |> int64) |> string) + "\n" + 
        "CPU usage: " + (PerformanceCounter("Processor", "% Processor Time", "_Total").NextValue()) + "%\n" + 
        "RAM usage: " + (PerformanceCounter("Memory", "Available MBytes").NextValue()) + " MB"
#endif //failsOnMono

    type PermCfg = | PermCfg of p:int * v:int with
        member this.pLen() = 
            let (PermCfg(p, v)) = this
            p
        member this.vLen() = 
            let (PermCfg(p, v)) = this
            v

    let BaseCfg = PermCfg(30,60)           //1,171,800
    let Reduced_vLen_Cfg = PermCfg(30,30)  //585,900
    let Reduced_pLen_Cfg = PermCfg(15,60)  //502,200
    let Optimized_Cfg = PermCfg(20,60)     //725,400
    let min_Cfg = PermCfg(15,30)           //243 k <- produces 4,860 k tot 
    let chosenCfg = min_Cfg

    let flattenOb = fun (o:option<_>) -> o.Value
    let flatLocal = fun (li:option<_> list) -> li |> List.filter (fun (x:option<_>) -> x.IsSome) |> List.map (fun x -> flattenOb x)
    let getPermutations li =
        //see TomP @ https://stackoverflow.com/questions/286427/calculating-permutations-in-f
        let rec perms inli takn = 
            seq { if Set.count takn = List.length inli then yield [] else
                    for l in inli do
                    if not (Set.contains l takn) then 
                        for perm in perms inli (Set.add l takn)  do
                        yield l::perm }
        perms li Set.empty

    let liT = [1;2;3]
    let eaTy = ["Cd";"P*";"Tr";"V1";"V2"]
    let P = lim (fun el -> "P_" + (string el)) [0..(chosenCfg.pLen() - 1)]
    let Pr = lim (fun el -> "Pr_" + (string el)) [0..(chosenCfg.pLen() - 1)]
    let Pi = lim (fun el -> "Pi_" + (string el)) [0..(chosenCfg.pLen() - 1)]
    let Pri = lim (fun el -> "Pri_" + (string el)) [0..(chosenCfg.pLen() - 1)]

    let r1 = (List.windowed 5 P)

    //len:26
    //printfn "r1:%A len: %A" r1 (r1.Length)

    let cyclicalOrder =
        fun (l:list<_>) ->
            let cyc = [0;5;10;15;20;25]
            List.map (fun x ->
                List.map(fun cy -> 
                        match ((x + cy) < l.Length) with
                        | true -> Some(l.[x + cy])
                        | _ -> None
                        ) cyc) [0..4]
          //|> @ this pt we nd to randomize `in situ`, so:  
          //   unzip >> shuffle >> zip >> carry on...
            |> List.concat |> flatLocal

    let getWins() =
        ((List.windowed 5 P) |> cyclicalOrder) @ ((List.windowed 5 Pr) |> cyclicalOrder)
        @ ((List.windowed 5 Pi) |> cyclicalOrder) @ ((List.windowed 5 Pri) |> cyclicalOrder)

    printfn "getWins() res:%A len:%A" (getWins()) ((getWins()).Length)
    //len:104 min_Cfg: 44
    hr()
    printfn "cyclicalOrder res:%A len:%A" (cyclicalOrder r1) ((cyclicalOrder r1).Length)
    //len:26 min_Cfg: 11
    hr()

    let procAndReplEl elm li replLi =
        lim (fun eaL ->
                match List.contains elm eaL with
                | true -> 
                    let idx = (List.tryFindIndex (fun el -> el = elm) eaL).Value
                    lim (fun repl -> List.updateAt idx repl eaL) (replLi)
                | _ -> [eaL] ) li


    //printfn "below the windowed are represented by W0..W104" //all poss win combos
    let wins() = lifo (fun s x -> s @ ["W" + x.ToString()]) [] [0..44]
        //res_Ps_Replaced_With_All_Variables
    let res2 = procAndReplEl "P*" ((getPermutations eaTy) |> List.ofSeq) (wins())
    let AllPossV1s() = lifo (fun s x -> s @ ["V_1" + x.ToString()]) [] [0..(chosenCfg.vLen() - 1)]
    let res3 = procAndReplEl "V1" (res2 |> List.concat) (AllPossV1s()) |> List.concat
    printfn "Replaced_V1s_#3: %A len.0:%A len:%A" (res3.[0]) ((res3.[0]).Length) (res3.Length)
    //For min_Cfg: ["Cd"; "W0"; "Tr"; "V_10"; "V2"] len.0:5 len:162000
    hr()

    let res4Alt (inL:list<_>) =
       printfn "...in res4Alt; fed li.0:%A len: %A uniqueLen:%A" (inL.[0])(inL.Length) ((inL |> Seq.distinct |> List.ofSeq).Length)
       //minCfg: 8100
       let innrRes:list<_> = 
        inL |>
          limi (fun i eaL ->
                  match List.contains "V2" eaL with
                  | true -> 
                    let idx = (List.tryFindIndex (fun el -> el = "V2") eaL)
                    let res = 
                        lifo (fun s x -> s @ ["V_2" + x.ToString()]) [] [0..29]
                        |> lim (fun repl -> eaL.[0..3] @ [repl])
                    if i = 0 then 
                      printfn "for i=0 res of fold/mapi is:%A" res
                    match (i % 10000 = 0) with
                    | true ->  printfn "i:%A resL:%A" i (lilen res)
                    | _ -> ()
                    res
                  | _ -> [eaL] ) |> List.concat
       //printfn "Replaced_V1s_#4.0: %A len:%A uniqueLen:%A" innrRes.[0] (innrRes.Length) ((innrRes |> Seq.distinct |> List.ofSeq).Length)
       //printfn "concLen: %A 1st:%A last:%A" (concL.Length) (concL.[0]) ((List.last concL))
       printfn "concLen: %A 1st:%A last:%A" (innrRes.Length) (innrRes.[0]) ((List.last innrRes))
       //for min_Cfg 
       //concLen: 243000 1st:["Cd"; "W0"; "Tr"; "V_10"; "V_20"] last:["Cd"; "W44"; "V2"; "V_129"; "V_229"]
       //serToFile
       hr()

    let run() = List.splitInto 20 res3 |> List.head |> res4Alt

    //below work done on Aug_14_23 for lazy ver to fetch only bckt
    
    let res4Alt2 start (inL:list<_>) =
        (inL |>
          limi (fun i eaL ->
                  match List.contains "V2" eaL with
                  | true -> 
                    let idx = (List.tryFindIndex (fun el -> el = "V2") eaL)
                    let res = 
                        lifo (fun s x -> s @ ["V_2" + x.ToString()]) [] [0..29]
                        |> lim (fun repl -> eaL.[0..3] @ [repl])
(*
                    if i = 0 then 
                      printfn "for i=0 res of fold/mapi is:%A" res
                    match (i % 10000 = 0) with
                    | true ->  printfn "i:%A resL:%A" i (lilen res)
                    | _ -> ()
*)
                    res
                  | _ -> [eaL] ) |> List.concat).[start..(start+11)]

    //slightly buggy but will do in a pinch (just a standin, anyway)
    let rec findWin num winSz optChk =
        match optChk with
        | Some o -> 
            if num > (winSz * o) then o 
             elif num =  (winSz * o) then o - 1
             else findWin num winSz (Some(o-1))
        | _ -> if num > (winSz * 19) then 19
                elif num = (winSz * 19) then 18
                else findWin num winSz (Some(18))

    let getBuck forNum =
        let w = findWin forNum 243000 None
        let start = forNum % 243000
        printfn "running alt 4 win:%A start:%A" w start
        res4Alt2 start (List.splitInto 20 res3).[w]

    let newSeed() = getRnum 0 4860000
    let runAlt42() =
    [0..5]
    |> limi (fun i ctr -> 
              let res = getBuck (newSeed() + (12*i))
              printfn "res %A for %A: %A" i newSeed res) |> ignore
(*
Output:
    running alt 4 win:6 start:185025
res 0 for <fun:Pipe #5 stage #1 at line 218@220-4>: 
[["W25"; "V_117"; "V2"; "Cd"; "V_215"]; ["W25"; "V_117"; "V2"; "Cd"; "V_216"];
 ["W25"; "V_117"; "V2"; "Cd"; "V_217"]; ["W25"; "V_117"; "V2"; "Cd"; "V_218"];
 ["W25"; "V_117"; "V2"; "Cd"; "V_219"]; ["W25"; "V_117"; "V2"; "Cd"; "V_220"];
 ["W25"; "V_117"; "V2"; "Cd"; "V_221"]; ["W25"; "V_117"; "V2"; "Cd"; "V_222"];
 ["W25"; "V_117"; "V2"; "Cd"; "V_223"]; ["W25"; "V_117"; "V2"; "Cd"; "V_224"];
 ["W25"; "V_117"; "V2"; "Cd"; "V_225"]; ["W25"; "V_117"; "V2"; "Cd"; "V_226"];
 ["W25"; "V_117"; "V2"; "Cd"; "V_227"]]
    running alt 4 win:6 start:185037
res 1 for <fun:Pipe #5 stage #1 at line 218@220-4>: 
[["W25"; "V_117"; "V2"; "Cd"; "V_227"]; ["W25"; "V_117"; "V2"; "Cd"; "V_228"];
 ["W25"; "V_117"; "V2"; "Cd"; "V_229"]; ["W25"; "V_118"; "V2"; "Cd"; "V_20"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_21"]; ["W25"; "V_118"; "V2"; "Cd"; "V_22"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_23"]; ["W25"; "V_118"; "V2"; "Cd"; "V_24"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_25"]; ["W25"; "V_118"; "V2"; "Cd"; "V_26"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_27"]; ["W25"; "V_118"; "V2"; "Cd"; "V_28"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_29"]]
    running alt 4 win:6 start:185049
res 2 for <fun:Pipe #5 stage #1 at line 218@220-4>: 
[["W25"; "V_118"; "V2"; "Cd"; "V_29"]; ["W25"; "V_118"; "V2"; "Cd"; "V_210"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_211"]; ["W25"; "V_118"; "V2"; "Cd"; "V_212"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_213"]; ["W25"; "V_118"; "V2"; "Cd"; "V_214"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_215"]; ["W25"; "V_118"; "V2"; "Cd"; "V_216"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_217"]; ["W25"; "V_118"; "V2"; "Cd"; "V_218"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_219"]; ["W25"; "V_118"; "V2"; "Cd"; "V_220"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_221"]]
    running alt 4 win:6 start:185061
res 3 for <fun:Pipe #5 stage #1 at line 218@220-4>: 
[["W25"; "V_118"; "V2"; "Cd"; "V_221"]; ["W25"; "V_118"; "V2"; "Cd"; "V_222"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_223"]; ["W25"; "V_118"; "V2"; "Cd"; "V_224"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_225"]; ["W25"; "V_118"; "V2"; "Cd"; "V_226"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_227"]; ["W25"; "V_118"; "V2"; "Cd"; "V_228"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_229"]; ["W25"; "V_119"; "V2"; "Cd"; "V_20"];
 ["W25"; "V_119"; "V2"; "Cd"; "V_21"]; ["W25"; "V_119"; "V2"; "Cd"; "V_22"];
 ["W25"; "V_119"; "V2"; "Cd"; "V_23"]]
    running alt 4 win:6 start:185073
res 4 for <fun:Pipe #5 stage #1 at line 218@220-4>: 
[["W25"; "V_119"; "V2"; "Cd"; "V_23"]; ["W25"; "V_119"; "V2"; "Cd"; "V_24"];
 ["W25"; "V_119"; "V2"; "Cd"; "V_25"]; ["W25"; "V_119"; "V2"; "Cd"; "V_26"];
 ["W25"; "V_119"; "V2"; "Cd"; "V_27"]; ["W25"; "V_119"; "V2"; "Cd"; "V_28"];
 ["W25"; "V_119"; "V2"; "Cd"; "V_29"]; ["W25"; "V_119"; "V2"; "Cd"; "V_210"];
 ["W25"; "V_119"; "V2"; "Cd"; "V_211"]; ["W25"; "V_119"; "V2"; "Cd"; "V_212"];
 ["W25"; "V_119"; "V2"; "Cd"; "V_213"]; ["W25"; "V_119"; "V2"; "Cd"; "V_214"];
 ["W25"; "V_119"; "V2"; "Cd"; "V_215"]]
    running alt 4 win:6 start:185085
res 5 for <fun:Pipe #5 stage #1 at line 218@220-4>: 
[["W25"; "V_119"; "V2"; "Cd"; "V_215"]; ["W25"; "V_119"; "V2"; "Cd"; "V_216"];
 ["W25"; "V_119"; "V2"; "Cd"; "V_217"]; ["W25"; "V_119"; "V2"; "Cd"; "V_218"];
 ["W25"; "V_119"; "V2"; "Cd"; "V_219"]; ["W25"; "V_119"; "V2"; "Cd"; "V_220"];
 ["W25"; "V_119"; "V2"; "Cd"; "V_221"]; ["W25"; "V_119"; "V2"; "Cd"; "V_222"];
 ["W25"; "V_119"; "V2"; "Cd"; "V_223"]; ["W25"; "V_119"; "V2"; "Cd"; "V_224"];
 ["W25"; "V_119"; "V2"; "Cd"; "V_225"]; ["W25"; "V_119"; "V2"; "Cd"; "V_226"];
 ["W25"; "V_119"; "V2"; "Cd"; "V_227"]]
 *)

#endif //modPerms

#if Remmed_Aug14_23_mbi
module mockADO = 
    open System
    open System.IO
    open Trivedi.Core
    open System.Diagnostics
    
    printfn "in mod mockADO..."
    System.Console.ReadKey(true) |> ignore

    let getBucket i =
        async {
            try
              printfn "1"
              let buckNo = 
                match i < 251100 with
                | true -> 1
                | _ -> (i/251100) + 1
              //@hardCoded :fable workaround: last choice shd be throw "impossible err"
              printfn "2"
              //let buck = function | 1 -> l1 | 2 -> l2 | 3 -> l3 | 4 -> l4 | 5 -> l5 | _ -> l1
              printfn "3 for %A" i
              let path = buckNo.ToString() + ".out"
              printfn "4"
              let! bytes = File.ReadAllBytesAsync(path) |> Async.AwaitTask
              printfn "5"
              let tB = deSerBA (bytes) :?> list<list<string>>
              printfn "6"
              let rr = i % 251100 //remainder
              let tI = tB.[rr]
              printfn "7"
              return ( match ((rr + 12) > (List.length tB)) with
                        | true -> List.skip ((tB).Length - 12) (tB)
                        | _ -> 
                            //@hardCoded: in prod nd to open nxt (hehe, mgo)
                            lim (fun ctr -> tB.[ctr]) [rr..(rr+11)])
            with
                | ex -> 
                    do printfn "Exc in getBucket for %A -> msg:%A" i (ex.Message)
                    return []
            }

    let runWith =
        fun params ->
          let (stopW, l) = params
          stopW.Start()
          l 
          |> Seq.ofList
          |> Seq.map getBucket
          |> Async.Parallel
          |> Async.RunSynchronously
          |> List.ofSeq
          |> printfn "result of getBucket:\n%A" |> ignore

    let postProc =
      fun params ->
          let (stopW, l) = params
          stopW.Stop()
          let ts = stopW.Elapsed
          printfn $"getBucket took: {ts.Minutes} min: {ts.Seconds} secs: {ts.Milliseconds / 10} millis"

    hr()
    printfn "running getBucket..."
    //[4;94;64;45;80;88;22]
    //[251200;951100]
    મિંગ runWith ([251200;951100], (new Stopwatch())) postProc
#endif //Remmed_Aug14_23_mbi

module permsHarness = 
    open System
    open System.IO
    open System.Data
    open System.Diagnostics
    open Trivedi.Core
    open Trivedi.UI
    open Trivedi.Brij
    open FsCheck
    open FsCheck.FSharp

    printfn "in mod permsHarness..."
    System.Console.ReadKey(true) |> ignore
    
    let getStats() =
      [0;10;19] |>
      lim (fun n ->
            let thisFn = n.ToString() + ".out"
            let li = deSerBA (File.ReadAllBytes(thisFn)) :?> list<list<string>>
            printfn " ***  For n=%A, len:%A" n (li.Length))
      (* Output:
         ***  For n=0, len:251100
         ***  For n=10, len:251100
         ***  For n=19, len:251100
         min_Cfg; exact tot: 5,022,000
         Ea Test Case:
            needs a batch: say, dz
            [0..5,022,000] -> get batch -> run test
        *)

    let chkFsCheckSetup() =
      let revRevIsOrig (xs:list<int>) = List.rev(List.rev xs) = xs
  
      let revIsOrig (xs:list<int>) = List.rev xs = xs
  
      printfn "running FsChk on revRevIsOrig..."
      Check.Quick revRevIsOrig
      hr()
      printfn "running FsChk verbose on revRevIsOrig..."
      Check.Verbose revRevIsOrig
      hr()
      printfn "running FsChk on revIsOrig..."
      Check.Quick revIsOrig
      hr()
      printfn "running FsChk verbose on revIsOrig..."
      Check.Verbose revIsOrig
      hr()

    let genConst() = 
      Gen.constant (1, "Foo") |> Gen.sampleWithSize 0 10 |> printfn "gen.const: %A"
      //gen.const: [|(1, "Foo"); (1, "Foo"); (1, "Foo"); (1, "Foo"); (1, "Foo"); (1, "Foo");
                    //(1, "Foo"); (1, "Foo"); (1, "Foo"); (1, "Foo")|]

    printfn "module arbs..."

    type Slotd = |Slotd of Cd:string * P:string * Tr:string * Vi:string * Vii:string
    //let eaTy = ["Cd";"P*";"Tr";"V1";"V2"]
    //["Cd"; "W0"; "Tr"; "V_10"; "V_230"]
    
    //let myprop (i:int) = i >= 0
    let mygen = ArbMap.defaults |> ArbMap.arbitrary<int> |> Arb.mapFilter (fun i -> Math.Abs i) (fun i -> i >= 0) //|> Arb.toGen |> Gen.resize 20 |> Arb.fromGen

#if tbfo
    let myGen = 
      let fetchBatch =
        fun i ->
          ...
      gen { let! i = Gen.choose (0, 5021999) 
            return fetchBatch i }
#endif //tbfo

    let helper = "a string"
    let private helper' = true
    
    type permHrnssMrkr = class end
    let cfg = Config.Verbose.WithArbitrary([typeof<permHrnssMrkr>.DeclaringType])
    Check.All(cfg, typeof<permHrnssMrkr>.DeclaringType)

    chkFsCheckSetup()


#if ModuleBhojpuri_Remmed_Jul21_2023
module Bhojpuri =
    open System
    open System.Drawing
    open System.IO
    open System.IO.Compression
    open System.Windows.Forms
    open System.Text.RegularExpressions
    open System.Diagnostics
    open DiffMatchPatch
    open Trivedi
    open Trivedi.Core
    open Trivedi.UI
//    open GridTester ModGridTester_RemmedForMonkeyBastas_Jul12_2023

    ///Jul 21 '23: Combined all prior vers & removed redundancies...
    ///            cPnl, rPnl et al were v. rudimentary, all removed

    printfn "in mod winFrms.Bhojpuri..."


    type BhojPuriM =  | SevardhanM of string list
                      | KutkaaM of string * string * string list
                      | CheepsM of C
                      | MeetheeM of T
                      | DilKhushM of Prize //, tbfo... with

    type BhojPuri_Supaari =  | Sevardhan of SevardhanM
                             | Kutkaa of KutkaaM
                             | Cheeps of CheepsM
                             | Meethee of string
                             | DilKhush of int with
        static member New(s:string) = Meethee(s)
        static member New(i:int) = DilKhush(i)
        static member New(c:SevardhanM, bp) = 
           let pwd = new SecureString()
           let retPtr = ref IntPtr.Zero
           let SupaariPnl = new TableLayoutPanel(RowCount = 5, ColumnCount = 1, Dock = doc "F", AutoScroll = true, BackColor = Color.Azure)
           match (len inLi) with
           | 3 ->
               lim (fun i v -> 
                        let pl = if i < 3 then " +" else ""
                        SupaariPnl.Add(new Label(Text = v + pl, AutoSize = true, Anchor = anc "F"),0,(i+1))
                     ) inLi
           | _ -> raise "BhojPuri_Supaari.New(c:SevardhanM, bp): invalid input encountered"
           let inpBox = new TextBox(AutoSize = true, Anchor = anc "F", PasswordChar = '*')
           inpBox.KeyDown.Add(fun o (e:KeyEventArgs) -> 
               match e.KeyCode with
               | Keys.Enter -> 
                   assignPtr [Marshal.SecureStringToGlobalAllocUnicode(pwd)]
                   pwd.Dispose()
                   //auto-Adv + trigger submit if necc
                   bp.updPanel()
               | Keys.Back -> //chk Back vs BackSpc
                   pwd.RemoveAt(pwd.Length - 1)
               | _ -> 
                   let k = e.KeyValue
                   if ( not e.Shift && k >= (int) Keys.A && k <= (int) Keys.Z ) then
                      pwd.AppendChar((char)(k + 32))
                   textBox.Text <- textBox.Text + "*" )
           SupaariPnl.Add(inpBox,0,4)
        static member New(c:KutkaaM, bp) = 
           let SupaariPnl = new TableLayoutPanel(RowCount = 5, ColumnCount = 1, Dock = doc "F", AutoScroll = true, BackColor = Color.Azure)
           let bgBox = new ComboBox(AllowSelection = false, Text = bg, AutoSize = true, Dock = doc "F", DropDownStyle = ComboBoxStyle.DropDownList, AutoCompleteMode = AutoCompleteMode.None, DataSource = [|bg|])
           let enBox = new ComboBox(AllowSelection = false, Text = en, AutoSize = true, Dock = doc "F", DropDownStyle = ComboBoxStyle.DropDownList, AutoCompleteMode = AutoCompleteMode.None, DataSource = [|en|])
           let cb1 = new ComboBox(AutoSize = true, Dock = doc "F", DropDownStyle = ComboBoxStyle.DropDownList, AutoCompleteMode = AutoCompleteMode.None, DataSource = [|cbli|])
           let cb2 = new ComboBox(AutoSize = true, Dock = doc "F", DropDownStyle = ComboBoxStyle.DropDownList, AutoCompleteMode = AutoCompleteMode.None, DataSource = [|cbli|])
           let cb3 = new ComboBox(AutoSize = true, Dock = doc "F", DropDownStyle = ComboBoxStyle.DropDownList, AutoCompleteMode = AutoCompleteMode.None, DataSource = [|cbli|])
           let updateH = new EventHandler(fun o e -> 
                if ((not (cb1.SelectedItem = null)) && (not (cb2.SelectedItem = null)) && (not (cb3.SelectedItem = null))) then
                     assignPtr [bg; (cb1.SelectedItem); (cb2.SelectedItem); (cb3.SelectedItem); en]
                     //auto-Adv + trigger submit if necc
                     bp.updPanel()
                  )
           cb1.SelectedIndexChanged.AddHandler(updateH)
           cb2.SelectedIndexChanged.AddHandler(updateH)
           cb3.SelectedIndexChanged.AddHandler(updateH)
           SupaariPnl.Add(bgBox,0,0)
           SupaariPnl.Add(cb1,0,1)
           SupaariPnl.Add(cb2,0,2)
           SupaariPnl.Add(cb3,0,3)
           SupaariPnl.Add(enBox,0,4)
           //if up/dn btns deemed necc code exists in prior version of this file
        static member New(c:CheepsM, bp) = 
           let SupaariPnl = new TableLayoutPanel(RowCount = 2, ColumnCount = 5, Dock = doc "F", AutoScroll = true, BackColor = Color.Azure)
           let TitlLbl = 
              match CheepsM with
              | 3ty -> new Label(Text = "3ty Title Lbl", AutoSize = true, Anchor = anc "F")
              | _ -> new Label(Text = "Other Title Lbl", AutoSize = true, Anchor = anc "F")
           SupaariPnl.Add(TitlLbl, 0,0)
           SupaariPnl.SetColumnSpan(TitlLbl, 4)
           match CheepsM with
           | 3ty -> SupaariPnl.ColumnCount <- 3
           | _ -> SupaariPnl.ColumnCount <- 5
           lim (fun i c -> 
                   let pb = new PictureBox(BorderStyle = BorderStyle.None, Image = ...)
                   pb.Click.AddHandler(fun o e -> 
						match isThisLastItm with
                                                | true -> 
                                                    let allSelects = thisItm :: (!!~ "priorSelects" SupaariPnl)
                                                    assignPtr allSelects
                                                    //auto-Adv + trigger submit if necc
                                                    bp.updPanel()
                                                | _ -> 
                                                    //if necc. add code to disable via greying out
                                                    thisItm.BorderStyle = BorderStyle.Fixed3D
                                                    let updSelects = thisItm :: (!!~ "priorSelects" SupaariPnl) )
                                                    !!^ ["priorSelects", box updSelects] SupaariPnl) )
                   SupaariPnl.Add(pb, i,1)) [0..ct]
           


    type BhojPuri(t, w) as bp = 
        inherit System.Windows.Forms.Form(StartPosition = FormStartPosition.CenterScreen, WindowState = FormWindowState.Normal, Visible = false, Text = "winFrms Test Form: Copyright (c) M. P. Trivedi 2016-2023.  All rights reserved.", TopMost=true, Font=defFont, Width = 900, Height = 600)
        do printfn "in BhojPuri ctor..."
        let cfgTpl:list<BhojPuri_Supaari> = t
        //these are moved 2 be in scope, just retrofit to gappa
        //Note Jul 21: reconsider...
        let btnP = new TableLayoutPanel(Dock = doc "B", Width = rp.Width)
        let midP = new TableLayoutPanel(RowCount = 1, ColumnCount = 1,Dock = doc "F", AutoScroll = true, BackColor = Color.Azure)
        let okButton = new Button(AutoSize = true, DialogResult = DialogResult.OK, Text = "&OK")
        let cancelButton = new Button(AutoSize = true, DialogResult = DialogResult.Cancel, Text = "&Cancel")
        let titTxt = new TextBox(AutoSize = true, Dock = doc "F", Enabled = false, Text = "Step", ForeColor = dCobaltBlue, ReadOnly = true, Multiline = false, TextAlign = HorizontalAlignment.Center, BorderStyle = BorderStyle.None, BackColor = Color.OldLace)
        let icnLbl = new Label(Image = Bitmap (Bitmap.FromFile("E:\\src\\Data\\images\\brij.png"), new Size(200, 86)), Size = Size(200, 86), Anchor = anc "N")
        let init =
            !!^ ["currFrame", box 0] bp
            !!^ ["totFrames", box 5] bp  //nds to come in via state
        let setupBtnP =
            let btnFP = new FlowLayoutPanel(FlowDirection = FlowDirection.LeftToRight, Anchor = anc "N", AutoSize = true, BackColor = Color.White)
            btnFP.Controls.Add(okButton)
            btnFP.Controls.Add(cancelButton)
            btnP.Controls.Add(btnFP)
            bp.Controls.Add(btnP)
        let setupTitleP =
            let titleP = new TableLayoutPanel(RowCount = 1, ColumnCount = 5, Dock = doc "T", BackColor = Color.OldLace, AutoSize = true, Width = rp.Width , Height = ((int) (titTxt.Height * 3)))
            titleP.SuspendLayout()
            titleP.RowStyles.Clear()
            let icnLbl = new Label(Image = brijLogo, Size = (new Size(brijLogo.Width, brijLogo.Height)), Anchor = anc "N", BackColor = Color.Transparent, ForeColor = (currentScheme ((wRef dsk).Value)).Icn())
            let titTxt = new TextBox(AutoSize = true, Dock = doc "T", Enabled = false, Text = "Meethoo Def Document for " + nm, ReadOnly = true, Multiline = false, Width = f.Width - 50, TextAlign = HorizontalAlignment.Center, BorderStyle = BorderStyle.None, ForeColor = (currentScheme ((wRef dsk).Value)).titFore(), BackColor = (currentScheme ((wRef dsk).Value)).titBack())
            let titleP = new TableLayoutPanel(RowCount = 1, ColumnCount = 6, Dock = doc "T", BackColor = (currentScheme ((wRef dsk).Value)).titBack(), AutoSize = true, Width = f.Width , Height = ((int) (titTxt.Height * 3)))
            let statLbl = new Label(Text = "", Anchor = anc "N", BackColor = Color.Transparent, ForeColor = (currentScheme ((wRef dsk).Value)).Icn())
            titleP.RowStyles.Clear()
            titleP.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f))
            titleP.Controls.Add(icnLbl, 0, 0)
            titleP.Controls.Add(titTxt, 1, 0)
            titleP.Controls.Add(statLbl, 5, 0)
            titleP.SetColumnSpan(titTxt, 4)
            titleP.ResumeLayout(false)
            bp.Controls.Add(titleP)
        let updStatLbl() = 
            statLbl.Text <- ((!!~ "currFrame" bp) :?> string) + " of " + ((!!~ "totFrames" bp) :?> string)
            let currIcn = 
                match (cfgTpl.[!!^ "currFrame" bp] with
                | Sevardhan -> Bitmap (Bitmap.FromFile("E:\\src\\Data\\images\\Sevardhan.png")
                | Kutkaa -> Bitmap (Bitmap.FromFile("E:\\src\\Data\\images\\Kutkaa.png")
                | Cheeps -> Bitmap (Bitmap.FromFile("E:\\src\\Data\\images\\Cheeps.png")
                | Meethee -> Bitmap (Bitmap.FromFile("E:\\src\\Data\\images\\Meethee.png")
                | DilKhush -> Bitmap (Bitmap.FromFile("E:\\src\\Data\\images\\DilKhush.png")
                | _ -> getEmptyIcon()
            statLbl.Image <- currIcn
        let updPanel() =
            midP.SuspendLayout()
            let currF = !!~ "currFrame" bp
            match ((currf + 1) = (!!~ "totFrames" bp)) with
            | true -> bp.submit()
            | _ -> 
                midP.Controls.Clear()
                let SupaariPnl =
                    let currM = cfgTpl.[!!^ "currFrame" bp]            
                    match currM with
                    | SevarthanM -> BhojPuri_Supaari.New(currM, bp)
                    | KutkaaM -> BhojPuri_Supaari.New(currM, bp)
                    | CheepsM -> BhojPuri_Supaari.New(currM, bp)
                    | MeetheeM -> BhojPuri_Supaari.New(currM, bp)
                    | DilKhushM -> BhojPuri_Supaari.New(currM, bp)
                    | _ -> raise "Bhojpuri: invalid Supaari supplied..."
                midP.Controls.Add(SupaariPnl)
                bp.Controls.Add(midP)
                bp.updPanel()
        member bp.submit() =
           do printfn "request to submit recd..."
        do printfn "Bhojpuri ctor init updPanel()..."
        do bp.updStatLbl()
        do bp.updPanel()
        bp.Show()
#endif //ModuleBhojpuri_Remmed_Jul21_2023

#if Jimmy_Remmed_Jul21_2023
module jimmy =
    open System
    open System.Diagnostics
    open System.Windows.Forms
    open Trivedi.Core
    open ikvm
    open com.google.common.jimfs

    printfn "...init mod jimmy..."

    //Add references to all jar files that you use not directly
    //ikvm.runtime.Startup.addBootClassPathAssemby(Assembly.Load("second"))
//    ikvm.runtime.Startup.addBootClassPathAssembly(Assembly.Load("guava.jar"))
    //java.lang.Class clazz = typeof(hello.HelloWorld)
    printfn "init 0..."
    let clazz = typeof<Jimfs>
    printfn "init 1..."
//    java.lang.Thread.currentThread().setContextClassLoader(clazz.getClassLoader())

    let obj = (Jimfs).newFileSystem(Configuration.windows())
    printfn "newfs..."

    let fs = Jimfs.newFileSystem(Configuration.windows())
    printfn "init 2..."
    let foo = fs.getPath("/foo")
    printfn "init 3..."
    java.nio.file.Files.createDirectory(foo)
    printfn "init 4..."
    let hello = foo.resolve("hello.txt") // /foo/hello.txt


    printfn "...Jimmy: init completed..."

#endif //Jimmy_Remmed_Jul21_2023


#if Tokenizer_Remmed_Jul22_2023
module Tokenizer =
    open System
    open System.IO 
    open System.Diagnostics
    open System.Text
    open System.Text.RegularExpressions
    open Trivedi.Core
    open Trivedi.UI
    //open FSharp.Compiler.SourceCodeServices
    //open FSharp.Compiler.Tokenization
    
    printfn "in mod winForms_Tester..."


https://github.com/dotnet/fsharp/blob/2c6344dd627f05c69dfca8a8e0419bb4b440324f/docs/fcs/tokenizer.fsx#L33

    //https://github.com/dotnet/fsharp/blob/main/fcs-samples/Tokenizer/Program.fs
    //https://fsharp.github.io/fsharp-compiler-docs/fcs/tokenizer.html
    
    let sourceTok = FSharpSourceTokenizer([], Some ".\main.fs")
    
    let tokenizeLines (lines:string[]) =
      [ let state = ref FSharpTokenizerLexState.Initial
        for n, line in lines |> Seq.zip [ 0 .. lines.Length ] do
          let tokenizer = sourceTok.CreateLineTokenizer(line)
          let rec parseLine() = seq {
            match tokenizer.ScanToken(!state) with
            | Some(tok), nstate ->
                let str = line.Substring(tok.LeftColumn, tok.RightColumn - tok.LeftColumn + 1)
                yield str, tok
                state := nstate
                yield! parseLine()
            | None, nstate -> state := nstate }
          yield n, parseLine() |> List.ofSeq ]
    
    let tokenizedLinesHardCoded = 
    tokenizeLines
        [| "// Sets the hello wrold variable"
           "(* Multi-line comment #1"
           "Multi-line comment #2"
           "*)"
           "let a = 123f"
           "let hello = \"Hello world\" " |]

    let tokenizedLines = tokenizeLines (File.ReadAllLines("main.fs"))
    
    let runTokenizer() =
        printfn "...running Tokenizer..."
        for lineNo, lineToks in tokenizedLines do
          printfn "%d:  " lineNo
          for str, info in lineToks do printfn "       [nm:%s|str:'%s'|clr:''%A'|char:''%A'|locL:''%A'|locR:''%A']" info.TokenName str (info.ColorClass) (info.CharClass) (info.LeftColumn) (info.RightColumn) 


//output: (partly)
249:  
       [nm:WHITESPACE|str:'                '|clr:''Default'|char:''WhiteSpace'|locL:''0'|locR:''15']
       [nm:IDENT|str:'printfn'|clr:''Identifier'|char:''Identifier'|locL:''16'|locR:''22']
       [nm:WHITESPACE|str:' '|clr:''Default'|char:''WhiteSpace'|locL:''23'|locR:''23']
       [nm:STRING_TEXT|str:'"'|clr:''String'|char:''String'|locL:''24'|locR:''24']
       [nm:STRING_TEXT|str:'Esc'|clr:''String'|char:''String'|locL:''25'|locR:''27']
       [nm:STRING_TEXT|str:'.'|clr:''String'|char:''String'|locL:''28'|locR:''28']
       [nm:STRING_TEXT|str:'.'|clr:''String'|char:''String'|locL:''29'|locR:''29']
       [nm:STRING_TEXT|str:'.'|clr:''String'|char:''String'|locL:''30'|locR:''30']
       [nm:STRING|str:'"'|clr:''String'|char:''String'|locL:''31'|locR:''31']
250:  
       [nm:WHITESPACE|str:'            '|clr:''Default'|char:''WhiteSpace'|locL:''0'|locR:''11']
       [nm:BAR|str:'|'|clr:''Punctuation'|char:''Delimiter'|locL:''12'|locR:''12']
       [nm:WHITESPACE|str:' '|clr:''Default'|char:''WhiteSpace'|locL:''13'|locR:''13']
       [nm:UNDERSCORE|str:'_'|clr:''Identifier'|char:''Identifier'|locL:''14'|locR:''14']
       [nm:WHITESPACE|str:' '|clr:''Default'|char:''WhiteSpace'|locL:''15'|locR:''15']
       [nm:RARROW|str:'->'|clr:''Keyword'|char:''Keyword'|locL:''16'|locR:''17']
       [nm:WHITESPACE|str:' '|clr:''Default'|char:''WhiteSpace'|locL:''18'|locR:''18']
251:  
       [nm:WHITESPACE|str:'                '|clr:''Default'|char:''WhiteSpace'|locL:''0'|locR:''15']
       [nm:IDENT|str:'printfn'|clr:''Identifier'|char:''Identifier'|locL:''16'|locR:''22']
       [nm:WHITESPACE|str:' '|clr:''Default'|char:''WhiteSpace'|locL:''23'|locR:''23']
       [nm:STRING_TEXT|str:'"'|clr:''String'|char:''String'|locL:''24'|locR:''24']
       [nm:STRING_TEXT|str:'.'|clr:''String'|char:''String'|locL:''25'|locR:''25']
       [nm:STRING_TEXT|str:'.'|clr:''String'|char:''String'|locL:''26'|locR:''26']
       [nm:STRING_TEXT|str:'.'|clr:''String'|char:''String'|locL:''27'|locR:''27']
       [nm:STRING|str:'"'|clr:''String'|char:''String'|locL:''28'|locR:''28']

/// Gives an indication of the color class to assign to the token an IDE
/// src/Compiler/Service/ServiceLexing.fsi#L42
type FSharpTokenColorKind =
    | Default = 0           Glot/Eclipse
    | Text = 0 
    | Keyword = 1           red
    | Comment = 2           green
    | Identifier = 3        brown
    | String = 4            brightBlue / drkBlue(Chrome)
    | UpperIdentifier = 5
    | InactiveCode = 7
    | PreprocessorKeyword = 8   sameAsKeywd
    | Number = 9
    | Operator = 10             sameAsKeywd
    | Punctuation = 11          grey(Chrome)

#endif //Tokenizer_Remmed_Jul22_2023

#if Folding_Remmed_Jul22_2023
module Folding =

    printfn "...init module Folding..."

    //repurposed from utils/mapper.fs
    let parseLn =
        fun i str ->
            match str with
             | ParseRegex "\A\s{0,4}type\s(.*?)=" [tyNm] ->
                  Some( i, " type " + tyNm )
             | ParseRegex "\Amodule\s(.*?)=" [modNm]
                  -> Some( i, "module " + modNm)
             | ParseRegex "\A\s{4}let\s(.*?)=" [fnNm]
                  -> Some( i, " let " + fnNm)
             | ParseRegex "\A\#(.*?)\Z" [dirNm]
                  -> Some( i, " #" + dirNm)
             | _ -> None

#if Remmed_Jul20_mbi_stable

    let balancedMatches =
        //see: https://www.codeproject.com/Articles/21080/In-Depth-with-RegEx-Matching-Nested-Constructions
        //Allows push/pop to-from stacks, v. useful BUT we probably nd more control (granular)
        //Shd suffice if we're only looking for top-level lets.  Tinker & determine.
        let pattern = @"(?>#if(?<DEPTH>)|#endif(?<QUOTE-DEPTH>)|.?)*(?(DEPTH)(?!))"
        let source =  """
#if !Wadena
#if Sibley
The result of squaring the integer 4573 and adding 3 is 20912332
#else
The result of applying the 2nd sample function to (7 + 4) is 243
#endif
The result of applying the 3rd sample function to (6.5 + 4.5) is 242.800000
'otherNumber' is 2
#endif
'otherNumber' changed to be 3
#if Barbour
processing [1; 2; 3; 4; 5] through 'squareOddValuesAndAddOne' produces: [2; 10; 26]
processing [1; 2; 3; 4; 5] through 'squareOddValuesAndAddOneNested' produces: [2; 10; 26]
processing [1; 2; 3; 4; 5] through 'squareOddValuesAndAddOnePipeline' produces: [2; 10; 26]
processing [1; 2; 3; 4; 5] through 'squareOddValuesAndAddOneShorterPipeline' produces: [2; 10; 26]
#else
#if Jeffersona
Factorial of 6 is: 720
The Greatest Common Factor of 300 and 620 is 20
#endif
The sum 1-10 is 55
#endif
    """
        let mtch = Regex.Match(source, pattern, RegexOptions.IgnorePatternWhitespace)
        printfn "res:%A" (mtch.Success.ToString()) 
        mtch.Groups |> Seq.cast |> List.ofSeq |> printfn "res grps:%A"

    let parseTxt =
        fun str ->
            match str with
             | ParseRegex2 "\n\s{0,4}type\s(.*?)\n" [tyNm, i, l] ->
                  Some( " type " + tyNm + " " + i.ToString() + " " + l.ToString())
             | ParseRegex2 "\Amodule\s(.*?)=" [modNm, i, l]
                  -> Some( "module " + modNm)
             | ParseRegex2 "\A\s{4}let\s(.*?)=" [fnNm, i, l]
                  -> Some( " let " + fnNm)
             | ParseRegex2 "\A\#(.*?)\Z" [dirNm, i, l]
                  -> Some( " #" + dirNm)
             | _ -> None


#if tbdb
    let parseTxt2 =
        fun str ->
          //The last module may not end w/\n, so for ea file add \n @ end
          //Manually bld mod foldables coz we don't match entire mod, just hdrs
          //This won't do rec via indentation, just straight 4 now
          let isFoldedLi = !!~ "isFoldedli" rtb
          let isFoldableLi = !!~ "isFoldedli" rtb
          let expAll = !!^ ("isFoldedLi", box [])
          let collAll = !!^ ("isFoldedLi", box (!!~ "isFoldableLi" rtb))
          
          let modMatches = Regex.Matches("\s{0,8}module\s(.*?)=",str).Groups |> Seq.cast |> List.ofSeq
          limi (fun i m -> 
                    match (i = len mods - 1) with
                    | true -> 
                      //last mod in file
                      let fstLn = rtb.GetLineFromCharIndex(m.Index)
                      let lstLn = rtb.GetLineFromCharIndex((len rtb.Text) - 1)
                      match (fstLn < lstLn) with
                      | true -> ()
                      | _ -> (fstLn, lstLn) :: [isFoldableLi]
                    | _ -> 
                      let fstLn = rtb.GetLineFromCharIndex(m.Index)
                      let lstLn = rtb.GetLineFromCharIndex((mods.[i+1]).Index - 1)
                      match (fstLn < lstLn) with
                      | true -> ()
                      | _ -> (fstLn, lstLn) :: [isFoldableLi]
                    ) 
          
          let typeMatches = Regex("\s{0,8}type\s(.*?)\n").Match(str)
          if typeMatches.Success then
            m.Groups |> Seq.cast |> List.ofSeq
            |> lifo (fun s x -> 
                      printfn "$A) mat:%A i:%A l:%A" x.Value x.Index x.Length
                      let fstLn = rtb.GetLineFromCharIndex(Index)
                      let lstLn = rtb.GetLineFromCharIndex(Index + Length)
                      match (fstLn < lstLn) with
                      | true -> ()
                      | _ -> (fstLn, lstLn) :: [isFoldableLi]
                      ) [isFoldableLi]

      let dirMatches = Regex("(?>#if(?<DEPTH>)|#endif(?<QUOTE-DEPTH>)|.?)*(?(DEPTH)(?!))").Match(str)
          if typeMatches.Success then
            m.Groups |> Seq.cast |> List.ofSeq
            |> lifo (fun s x -> 
                      printfn "$A) mat:%A i:%A l:%A" x.Value x.Index x.Length
                      ...add to FoldableLi...

      let letMatches = Regex("([^\S\r\n]{0,8}let\s(.*?)\n\n)").Match(str)
      ...add to FoldableLi...
      //(multi-line only) 
      let commentMatches = Regex("([^\S\r\n]{0,8}\(\*(.*?)\*\)\n)")
        ...add to FoldableLi...
        let fstLn = rtb.GetLineFromCharIndex(Index)
        let lstLn = rtb.GetLineFromCharIndex(Index + Length)...
      
      
#endif //tbdb


    let runParser() = 
      hr()
      parseTxt (File.ReadAllText("UI_Jan18.fs")) |> printfn "res:\n%A"

#endif //Remmed_Jul20_mbi_stable

#endif //Folding_Remmed_Jul22_2023

#if FilePanelUpdates_Remmed_Jul21_2023
module FilePanelUpdates =
    open System
    open System.IO
    open System.IO.Compression
    open System.Drawing
    open System.Windows.Forms
    open System.Text.RegularExpressions
    open System.Diagnostics

    printfn "...init module FilePanelUpdates..."


(*
	This version uses a flat fileSystem BUT we'll have to rever to using a tree because
          (i) lV.SubItems have no icons & we nd em
          (ii) Creating new items via lV.Insert(idx...) still adds to the root so to 
               distinguish we'd have to label it dir1\dir2\file.txt which won't do.
        The added ben of using a TV is it manages itself.
*)
    let getFilePnlDepr() =
        let zipBA = File.ReadAllBytes(@"E:\tmp\zipTest.zip")
        let pnlImgLi = new ImageList()
        pnlImgLi.Images.Add(Image.FromFile(@"E:\src\Data\images\google\folder.png"))
        pnlImgLi.Images.Add(Image.FromFile(@"E:\src\Data\images\google\folder_open.png"))
        pnlImgLi.Images.Add(Image.FromFile(@"E:\src\Data\images\google\doc.png"))
        let lV = new ListView(MultiSelect = false, Dock = doc "F", CheckBoxes = false, FullRowSelect = true, HeaderStyle = ColumnHeaderStyle.Nonclickable, LabelEdit = false, View = View.Details, SmallImageList=pnlImgLi, LargeImageList=pnlImgLi)
        lV.SuspendLayout()
        use memStream = new MemoryStream(zipBA)
        let zipArc = new ZipArchive(memStream, ZipArchiveMode.Update)
        lV.Layout.AddHandler(new LayoutEventHandler( fun (sender:obj) (e:System.Windows.Forms.LayoutEventArgs) -> 
            lV.Items.Clear()
#if remmed
            //1st pass: no dirs scanned
            zipArc.Entries |> Seq.cast |> List.ofSeq 
            |> lim (fun en -> 
                       match (en.FullName.EndsWith("\")) with
                       | true -> 
                           //create dirNode
                           let dirNode = new ListViewItem(en,0)
                           dir.ForeColor <- dCobaltBlue
                           lV.Items.Add(dirNode)
                       | _ -> 
                           //create filNode ie, in root
                           lV.Items.Add(new ListViewItem(en,2))
                    ) |> ignore
#endif //remmed
            printfn "...eof..."  ))


        lV.Click.AddHandler(new EventHandler(fun o e -> 
                 let selItm = lV.SelectedItems |> Seq.cast |> List.ofSeq |> List.head
                 "click recd on " + (selItm |> string) + " img idx " + ( (selItm.ImageIndex) |> string)
                 |> tibbie 
                 match selItm.ImageIndex with
                 | 0 -> 
                     //currState:folder(closed)
                     zipArc.Entries |> Seq.cast |> List.ofSeq |> List.filter(fun en -> (en.FullName.Contains(selItm)) && not (en.FullName.EndsWith("\")))
                     |> lifo (fun s en -> 
                       match (en.FullName.EndsWith("\")) with
                       | true -> 
                           //create dirNode
                           let dirNode = new ListViewItem(en,0)
                           dir.ForeColor <- dCobaltBlue
                           lV.Items.Add(dirNode)
                       | _ -> 
                           //create filNode ie, in root
                           lV.Items.Add(new ListViewItem(en,2))
                               ) selItm

                 | 1 -> 
                     //currState:folder_open
                 | 2 -> 
                     //doc
                     ()
                 //lim (fn x -> selItm.SubItems.Add("SubItem# " + x) [0..3] |> 
                 ))
        lV

    let getFilePnl() =
        let zipBA = File.ReadAllBytes(@"E:\tmp\zipTest.zip")
        let pnlImgLi = new ImageList()
        pnlImgLi.Images.Add(Image.FromFile(@"E:\src\Data\images\google\folder.png"))
        pnlImgLi.Images.Add(Image.FromFile(@"E:\src\Data\images\google\folder_open.png"))
        pnlImgLi.Images.Add(Image.FromFile(@"E:\src\Data\images\google\doc.png"))
        let treeVw = new TreeView(ImageList = pnlImgLi, ShowRootLines = true, ShowPlusMinus=true)
        treeVw.SuspendLayout()
        use memStream = new MemoryStream(zipBA)
        let zipArc = new ZipArchive(memStream, ZipArchiveMode.Update)
        treeVw.Layout.AddHandler(new LayoutEventHandler( fun (sender:obj) (e:System.Windows.Forms.LayoutEventArgs) -> 
            treeVw.Nodes.Clear()
            zipArc.Entries |> Seq.cast |> List.ofSeq 
            |> lim (fun en -> 
                       match (en.FullName.EndsWith("\")) with
                       | true -> 
                           let dirNode = new TreeNode(en,0)
                           dir.ForeColor <- dCobaltBlue
                           treeVw.Nodes.Add(dirNode)
                       | _ -> 
                           let (parentDir, fileNm) = getSlugs en
                           let filNode = new TreeNode(en,2)
                           treeVw.Nodes.Add(filNode)
                    ) |> ignore
            printfn "...eof..."  ))
        treeVw.Click.AddHandler(new EventHandler(fun o e -> 
                 let selItm = treeVw.SelectedNode
                 match debugMode with
                 | true -> 
	                 "click recd on " + (selItm |> string) + " img idx " + ( (selItm.ImageIndex) |> string)
        	         |> tibbie 
                 | _ -> ()
                 match selItm.ImageIndex with
                 | 0 -> 
			//closed foldr; chng icn + expnd
                        selItm.ImageIndex <- 1
                        selItm.Expand()
                 | 1 -> 
			//open foldr; chng icn + collpse
                        selItm.ImageIndex <- 0
                        selItm.Collapse()
                 | 2 -> ()  //doc; do nothing (dbl-Click to process it)
                 //lim (fn x -> selItm.SubItems.Add("SubItem# " + x) [0..3] |> 
                 ))
        treeVw.DoubleClick.AddHandler(new EventHandler(fun o e -> 
                 let selItm = treeVw.SelectedNode
                 match (len rtb.Text) > 0 with
                 | true -> 
			match ("(@ToDo: internStr) currTxt will be replaced.  Continue?", choiceDlg()) with
                        | true -> 
                              //Load on overloaded ty also Parses
                              rtb.Load(File.ReadAllText(selItm.FullPath), selItm.FullPath)
                        | _ -> ()
                 | _ -> ()
                 ))
        lV
#endif //FilePanelUpdates_Remmed_Jul21_2023

#if Outliner_Remmed_Jul22_2023
module Outliner =
    This is a new version offering a multi-col dlg

    //repurposed from mod Folding which repurposed utils/mapper
    //Earlier vers had spl handling for Brij ass, amongst other deltas
    let parseLn =
        fun i str ->
            match str with
             | ParseRegex "\A\s{0,4}type\s(.*?)=" [tyNm] ->
                  Some( i, " type " , tyNm )
             | ParseRegex "\Amodule\s(.*?)=" [modNm]
                  -> Some( i, "module " , modNm)
             | ParseRegex "\A\s{4}let\s(.*?)=" [fnNm]
                  -> Some( i, " let " , fnNm)
             | ParseRegex "\A\#(.*?)\Z" [dirNm]
                  -> Some( i, " #" , dirNm)
             | _ -> None

    type outLineLbl(outlineTpl) =
        override Label()
        let init() =
           !!^ ["lineNum", box lineNo] this
        member this.onPaint(args ->
            //multi-color logic in drawstring
            let (lineN, slg, ident) = outlineTpl
            match slg with
            | itmTy -> DrawString( lineNo)  slg)
                       DrawString( Color, ident)
         )

    let run =
	limi (fun i l -> parseLn i l કન્ટેનર્સ ) spl
	|> flatLocal
	|> lim (fun itm ->
  		   let (lineN, slg, ident) = itm
                   new outLineLbl(itm))
        |> feed to lV
        |> get listDlg
        |> attach 
             lbl.Click.AddHandler(new Handler(args ->
               let ln = !!~ "lineNum" this
		//note: if Folding impl will also nd auto-expansion of target
               match (કન્ટેનર્સ  ln isFolded) with
               | true -> let topOfFold = getTopOfFold(ln)
 			 expandFold(topOfFold)
               | _ -> ()
	       rtb.GotoLine(ln)  //ensure it's top of scn by bott() if necc
           )


#endif //Outliner_Remmed_Jul22_2023


#if frmDelta_Remmed_Jul29_2023
module frmDelta =
    open System
    open System.Diagnostics
    open System.Windows.Forms
    open Trivedi.Core
    open Trivedi.UI

    printfn "...init module frmDelta..."

    //mbis: no nd 2 get frustrated...
    //https://ocaml.org/docs/ocaml-on-windows
    //https://stackoverflow.com/questions/40336452/recommended-way-to-add-typescript-to-an-existing-asp-net-4-webforms-project#:~:text=Yes%2C%20Web%20forms%20can%20do%20TypeScript%20and%20Gulp.,Gulp-Watch%20A%20complete%20solution%20can%20be%20found%20here.
    //java: https://sourceforge.net/p/ikvm/wiki/Tutorial/
    //(plus: you can always ask some high-schl kids to teach you coding...)

    printfn "in mod frmDelta..."

    //current workflow (partly impl.) ->
    //કલકતી_પાન >> (get list<DesDocAux<'t>>) >> (pick 1st_or_Default બનારસી_મસાલો) >>  bld&show(no_tpl_passed)

    type DeltaTracker = | DeltaTracker of (fldNm:string * fldTy:FrmFieldType * initVal:obj * deltaVal:option<obj>) list

    //this nds 2 be in ALL frms; plus store a ref to the DT
    //create an overloaded brijForm ty with these methds
        member setDelta(f d) =
            this.DT <- lim (fun x ->
                   discombob...
                   match (initVal = d) with
                   | _ -> Some(d)
                   val)
        member getDeltas() = 
           lim (all items -> flat) |> Option
           |> OMap |> if isSome then getList() 
             -> push to docFld (currNonExisting in CoreMod) (deltas * userNm * dtTime)
             -> attach to list above (w/fldNm 4 CoreMod.delta fld)
        member submit() = oMap(fun x -> getDeltas() -> svr.request(x))

    //eventually we nd ->
    //tpl_passed into બનારસી_મસાલો generates a DeltaTracker >> (!!^ "deltaTracker")


    //this fn nds to be triggered on ea wid upd 
    //(trigs'll vary; chk/ins into widBlder or wherever appropriate)
    //wids will take cTor fldNms (probably already do so) which're passed on...
    let registerDelta = fun fldNm:string newVal:obj -> thisFrm.deltaTracker.setDelta(newVal)
     
//WidTys from ફીલ્ડ_પેનલ_Aux are given below
type FrmFldType =  | FldString | tibbie >> shd be TextBox >> TextChanged event
                   | FldNumber | tibbie >> shd be TextBox >> TextChanged event
                   | FldCurrency | tibbie >> shd be TextBox >> TextChanged event
                   | FldLongString | TextBox >> TextChanged event
                   | FldAttachment | tibbie >> shd be Btn >> attach event to FilePickerDlg code
                   | FldBoolean | CheckBox >> CheckedChanged event
                   | FldChoiceList | ComboBox >> SelectedIndexChanged event
                   | FldRadioBtn | tibbie
                   | FldRange | Label (autoUpd from TrackBar) >> attach event to btn.lblUpdate code
                   | FldNumUpDn | NumericUpDown	>> TextChanged event (chk ValueChngd 2)
                   | FldDate | DateTimePicker >> TextChanged event
                   | FldDateTime | tibbie
                   | FldColor | colBtn >> attach event to colPickerDlg code
                   | FldFont | fontBtn >> attach event to fntPickerDlg code
                   | FldInfoBox | tibbie >> ()
                   | FldBlankRow | TextBox >> ()
                   | UserInput | TextBox >> ()
                   | FldBtn | Button >> ()
                   | FldValidBtn | Button >> ()

//in the wrkflow 4 wids we nd two sep. wids 4 FldChoiceList:
//a ComboBox w/w-o ability to add|edit entries.
//| FldRWComboBox -> ComboBoxStyle=DropDown (allows entering new text/editing list)
//| FldROComboBox -> ComboBoxStyle=DropDownList (allows only picking frm li)

#endif //frmDelta_Remmed_Jul29_2023

#if Aug31
Notes Aug 28: DeltaTracker: 
	- if currFldVal.trim() <> origVal push to Deltas
	- Add to DeltaInf: @user : dtMod : fldsMod [1;2;3...]
	- onDocOpen (NOT pushed with orig payload) if hasPriorVers then req(PriorversFor id);
	  asynch.await(x -> populate to docVersProp; enable viewPVButton)
	- onDocOpen(d -> if doc.isPriorVer then (getModFldLi >> hiliteToRed)
	- note also that we need an easy/efficient way (poss via !!^ [] pnl) to get fldNm|Val tpl)

Aug_31_23 addenda: 
1.  If we embed array w/PriorVers in doc; we nd a hook to update on changes/MakeCurrent
2.  ACLs: Nd to ensure that svrSide applies ACL on qry so that flds are not sent at all in the mTpl;
          on bld gets dfltVal if user hasn't hidden
3.  Grouping ability for fldPs?  Might cause havoc w/rearrangement; make bkgrnd-only rendered? 
    (cells move on surface in-out of groupBox?)
4.  Validation rules on compose: see if we can use the existing forms/err via passing in fns for vdn.

Sept_1_23 notes:
    //Bottom line 4 promotePriorVers:
    //we do not amend any existing docs merely
    //- set PriorVerInf.fldsMod to ALL flds (get from fldDef)
    //- promote prior to curr
    //- (regular route) save curr as latest Ver
    //This retains immutablity + reduces overhead related to tracking deltas


///PriorVers CarryFwd logic for deltas 2. & 3. above

    //note:instd of storing ea UNID (easier) we cld merely store ver#
    //  (svr can deduce the id) @TBD: is this useful?
    type PriorVerInf = | PriorVerInf of userNm:string * dtMod:DateTime * fldsMod:list<string> * docUNID:string

    type AdditionalDocFld = | AdditionalDocFld of PriorVers:list<PriorVerInf>

    //Bottom line 4 promotePriorVers:
    //we do not amend any existing docs merely
    //- set PriorVerInf.fldsMod to ALL flds (get from fldDef)
    //- promote prior to curr
    //- (regular route) save curr as latest Ver
    //This retains immutablity + reduces overhead related to tracking deltas
    let promoteBtn = getBtn "Promote" "Make this Prior Version the Current Document"
    promoteBtn.Click.Add(fun e -> 
        svr.request("promote" Unid None) //ROMode = None 
    )
    let svrPromoteHandler = 
        fun unid ->
            let currDoc = getCurrDocFor unid
            let (userNm,dtMod,fldsMod,docUNID) = currDoc.PriorVerInf
            currDoc.PriorVerInf <- (userNm,dtMod,(getAllFlds SaadoM),docUNID)
            //reconcile currTpl w/priorTpl for vals
            svrSaveHandler(currDoc)

    ViewPriorVersions.click.Add(fun e -> 
           let currVerNo = brijDoc.getCurrVerNo thisDoc.unid
           match (currVerNo - 1) = PriorVers.len with
           | true ->
               લ_રાન્ડ_આઈ (fun _ pv _ ->
                        discomb
                        //what about col titles? does that gappa accept in ctor?
                        (ind, userNm, lastMod)) <| PriorVers
               gappa.itms.DblClick.Add(fun e -> 
                            let tgtItm = PriorVers.[(getIndex targt)]
                            discomb
                            server.req("docOpen " (tgtDocUNID ROMode)))
                let dlg = gappa list
                dlg.Show()
           | _ -> error("corrupt data")
           )

///4. - onDocOpen(d -> if doc.isPriorVer then (getModFldLi >> hiliteToRed)
        SaadoM nds new ctor param for isPrV
        match isPrV with
        | true -> 
              ctor called w/isPrV=true; 
              within SaadoP
              match isPrV w/
              | true -> 
                (dynamic) add txtBox below titlPn: "You are curr vwing a Prior Revision Document.  This document was last edited by user@x.com on DateTime": this info comes from PriorVerInf
                ea fldP gets enabled <- false
                ToolBar's MakeCurrVer btn gets auto-enabled
                ALSO, within ea fld add logic
                if fldNm in PriorVerInf.fldsMod; foreColor <- PVHiLite (Red (assignable))
                taking this back needs a ctor param for fldP
        | _ -> normal flow

#endif //Aug31

module dizCopy =
    open System
    open System.Diagnostics
    open System.Windows.Forms
    open Trivedi.Core
    open Trivedi.UI

    printfn "...init module dizCopy..."

(*
    logdRnr:

    let getOpenHnd = 
///        fun s id ->
ins:        fun s cmd id -> (to match on cmd)
            new EventHandler(fun (sender:obj) (e:EventArgs) ->
                match (isOpen tblID s) with
                | Some w -> switchToChild w
                | _ -> 
                    //FIRST add & then launch w pId 
                    let winH = BrijWin((getUNID.ToString() + ^pId), tblID, docId)
                    openWins <- (winH :: openWins)
                    match s with
                    | "DataView" -> tibbie ("icn cmd " + s + " for dvID -> " + id + crlf + "launch tbfo")
                    | _ -> openDes id )
**Note: this calls Brij.Gullo.openDes (tblty:'a)
**Note also:
     This nds 2 be reconciled with logd/dskOpenHnd which currently matches on dTy; nds 2 be generic

    type સાદુ_પાન_Jan (સુપારી,લવલી,ગુલકંદ, ક્વિમામ, પીચાક,બનાવો, સ્તિતિ) as dsk =
        ...
                    let DesignDVMenuItem = new ToolStripMenuItem("Open in Design Mode")
                    icnCtxt.Items.Add(DesignDVMenuItem) |> ignore
///                    DesignDVMenuItem.Click.AddHandler(new EventHandler(fun (sender:obj) (e:EventArgs) ->
///                                openDes (tblT.GetType()) ))
ins:                  DesignDVMenuItem.Click.AddHandler(getOpenHnd s openDes (tblT.GetType()) )


ADDENDA:
                    let CopyDesignMenuItem = new ToolStripMenuItem("New Table (Copy Design)")
                    icnCtxt.Items.Add(CopyDesignMenuItem) |> ignore
                    CopyDesignMenuItem.Click.AddHandler(new EventHandler(fun (sender:obj) (e:EventArgs) ->
                                copyDes (tblT.GetType()) ))

//extension 2 Brij.Gullo //follows openDes (tblty:'a)
    let copyDes (tblty:'a) =
       let newNm = ref (ગપ્પા_પાન (SizeM,Some("Please enter..."),None , Some(box ("nm")), None, frm, inputDlg()))
       match chkNm with
       | false -> newNm <- ગપ્પા_પાન ("taken, please offer alt...")
       | _ -> ()
       //now the logic for logd/dskOpenHnd (see notes above)
       match cfg with
       | Case1_AllDzInAdmin -> 
            let dox:list<DesDoc<_>> = getDat dz tblty
            lim (fun d -> d.bldCpyFor(tblty)) ** (see below)
       | Case2_SepFIles -> () //...

//addenda 4 Brij.type DesDocAux<'t>
      member this.bldCpyFor(tblty) =
           chk member.genDefault; we'll nd 2 rebuild docInfs/slugs while retaining els

moreover, each dzDoc (e.g. Calcutti) will nd to have a `bldCopyFor('a)` member to return a new inst
Test 2 ensure no refs to earlier ty remain.
*)


module clientInit =
    open System
    open System.Diagnostics
    open System.Windows.Forms
    open Trivedi.Core
    open Trivedi.UI

    printfn "...init module clientInit..."

(*
   From UI.ટેબલ_પાન<'a>: Two entry pts into svrReq (attached 2 icns) ->
      (1) let openHnd = new EventHandler(fun (sender:obj) (e:EventArgs) ->
        tibbie ("icn openDV for dvID -> " + tblID))
      (2) DesignDVMenuItem.Click.AddHandler(new EventHandler(fun (sender:obj) (e:EventArgs) ->
        openDes (tblT.GetType()) ))

   From logdUIRnrrelated: Related items ->
    (1)  type BrijWin = | BrijWin of id:DocUNID * tblId:string * docId:string
    (2)  let getOpenHnd = 
          fun s id ->
            new EventHandler(fun (sender:obj) (e:EventArgs) ->
                match (isOpen tblID s) with
                | Some w -> switchToChild w
                ...
*)

    //Note that Brij.DesDocAux already tells you ty of DesDoc
    //then Dec30 getOpenHnd (above) call becomes ```(isOpen tblID winTy s)```
    type BrijWin_Aug_2_23<'a> = | BrijWin of id:DocUNID * tblId:string * docId:string * dli:list<DesDocAux> *  ty:'a with
        inherit Form(IsMdiChild=true, Text = "...")
        let addDisposeLogic =
           w.Dispose.Add(fun o e -> 
                w.Parent.openWindows <- (w.Parent.openWindows |> List.except w)
                w.Parent.refreshOpenTbls()
              )

    type ટેબલ_પાન_Aug_2_23<'a> (સુપારી,લવલી,ગુલકંદ, ક્વિમામ, પીચાક,બનાવો, ty:'a) as dsk =
        inherit Form(Visible = false, TopMost = true, WindowState = FormWindowState.Maximized, Font = defFont, AutoScroll = true, BackColor = Color.AliceBlue, Text = ("getStr Copy_1"))
        let openTbls = ref (((tblId:string) * DesDocAux) list)
        let openWindows = ref (BrijWin list)
        do printfn "db: ટેબલ_પાન cTor"
        //...
	member d.refreshOpenTbls() =
           let openTblsFromWins = 
              openWindows.Value |> lim (fun win -> 
                 let (tblId, _) = win
                 tblId) |> List.unique
           newOpenTbls <- openTbls.filter(fun tbl -> List.contains tblID openTblsFromWins)
(*
    On the svrSide: 
        UI.ટેબલ_પાન<'a>.openHandler (frm icn) gets payload incl list<DesDocAux> 
          which's added 2 ટેબલ_પાન.openTbls
        @ToDo: For Design Updates, SaveHandler re-sends to cli
    Now dbl-click on dvRow will open local frm.

    BrijSvr has svrReqSvr
    let svrReqSvr = 
        fun cmd categ def ->
            match cmdTy cmd with
            | "chuno" -> chunoHandlerSvr cmd categ def (paanTy cmd)
            | "katho" -> kathoHandlerSvr cmd categ def (paanTy cmd)
            | "supari" -> supariHandlerSvr cmd categ def (paanTy cmd)
            | "lovely" -> lovelyHandlerSvr cmd categ def (paanTy cmd)
            | _ -> 
                tibbie "svr: unknown cmdTy recd"
                //can't really return this, tbfo
                chunoHandlerSvr cmd categ def (paanTy cmd)

     cmdTy = chuno|katho|supari|lovely
     paanTy = banarasi|calcutti|meethoo|saadoo

    (i)   chunoHandlerSvr (tbfo)
    (ii)  kathoHandlerSvr (fo)
    (iii) supariHandlerSvr(tbfo)
    (iv)  lovelyHandlerSvr(tbfo)

For All poss cmd paths below, EXCEPT the kathos(no updates):
       svr req cmpltd successfully ?
          -> cli gets the (single) updated DesDocAux w/same docID
          -> replaced in dsk.openTbls
       Note: might be more efficient to sendAll, consider if another user udpated def...
       chuno|banarasi
       chuno|calcutti
       chuno|meethoo
       chuno|saadoo
       katho|banarasi
       katho|calcutti
       katho|meethoo
       katho|saadoo
       supari|banarasi
       supari|calcutti
       supari|meethoo
       supari|saadoo
       lovely|banarasi 
       lovely|calcutti
       lovely|meethoo
       lovely|saadoo

    So for all DesDoc updates ->
    openConnections.map(fun conn -> match (List.contains updatedTbl conn.getOpenTbls) with
                                       | true -> pushUpdate2User) |> Async

    Current UIAux def ->
    type ફીલ્ડ_પેનલ_Aux (nm, fTy, slg, સુપારી, dzMode, ownr:Form)  as p =
    * The ty maps on dzMode (true will set Ctrl.enabled = false + add hndlrs)
    We nd to convert dzMode to a new DU |Regular|Dz|PriVer and add match cases 4 latter
    which ONLY disable + auto-add a new frm infobox @ top with helpInfo

    Related update to બનારસી_પાન<'t when 't :> ITblMarker> (ctor recieves dzMode)
    (insert in codeBase somewhere near ```let titTxt = new TextBox...```
    match | PriVer -> let priVerInfo = new TextBox...; (l8r) f.Ctrls.Add...
      """You are currently viewing a Prior Document Version.
       Data in this document may not be edited.
       If you wish to use this data, please click the "Make Current" button.
       This will promote this version to the Current Version.
       PLEASE NOTE that all subsequent changes to this document will be replaced.
       We suggest carefully comparing Prior Versions in separate windows before promoting any document."""

   Brij.fs updates ->

     type BrijTy ...
         override this.ContainsMod(Mod) = 
            let (BrijTy(mds, tpl, s, tblTy)) = this
            lico Mod mds
         member this.hasPriorVers:bool = this.ContiainsMod(Mod.DocVersionModule) && lastVerNum > 1
         UPDATE ctor (curr Bld w/o versioning) to take `1` as lastVerNum

     type DocVersionModule = | DocVersionModule of 
OptModID * lastVerNum:int * hasPriorVers:bool with
        REMOVE fld hasPriorVersions (only a coupla refs to this in codeBase; update to this.hasPriorVers)
        ADD fld priorVers:list<(docId * verNum * user * lastMod)>

    let સ_બાઇટ = 
    let સ_સપ્લીટ = 
    let સ_કન્ટેનર્સ = 
    let સ_રિપ્લ = 
    let સ_એક_અજ઼_વાર = 
    let સ_લેન = 
    let સ_ટ્રીમ = 
    let સ_ઇસ_લોઅર = 
    let સ_ઇસ_ડિજીટ = 
    let સ_ટીલ = 
    let સ_આના_થી_ચાલૂ  =
    let સ_આફટર = 
    let સ_આફટર_ટીલ = 
    let સ_એચ_ટી_એમ_એલ_વગર = 
    let સ_ક્વોટ_વગર = 

    let લ_રાન્ડ = 
    let લ_રાન્ડ_આઈ = 
    let લ_રાન્ડ_મનોજ = 
    let લ_રાન્ડ_મનોજ_સ = 
    let લ_રાન્ડ_મનોજ_લ = 
    let લ_આઈ_વગર = 
    let લ_આઈ_બદલો = 
    let લ_આ_છે = 
    let લ_આ_સિવાય = 
    let લ_સિવાય_ચાર_આંગળી_છે_પણ_આ_નઈ = 
    let લ_કોણી_બિલી = 
    let લ_આઈ_જોડે_ટપલ = 
    let લ_છાપો = 
    let લ_આઈ_જોડે_છાપો = 
    let લ_ટૂ_સ = 
    let લ_ટૂ_સ_બાર = 
    let લ_લેન = 
    let લ_ફ્રોમ_ઈ_નમ = 
    let લ_લેનકાસ્ટર_નું_પાઈ = 

    exception Trivedi_ex of string * obj

    let mapRaise = fun x ->
                match x > 5 with
                | true -> Error(Trivedi_ex("no way!", box x))
                | _ -> Ok(x)
    
    let hasErr = fun l -> (l |> List.filter (fun x -> 
                                    match x with
                                    | Error e -> true
                                    | _ -> false)) <> []

    let getErrs = fun l -> (l |> List.map (fun x ->  match x with
                                                        | Error e -> Some(x)
                                                        | _ -> None) |> flat

    let getOks = fun l -> (l |> List.map (fun x ->  match x with
                                                        | Error e -> None
                                                        | _ -> Some(x)) |> flat

    let chkRes = fun l ->
                    let errIO() = printfn "there were errors...(details)...continue?" |> ignore
                    if hasErr l then errIO() else getOks l

    List.map mapRaise <| [1;2;3;4;5;6]
    |> hasErr
    |> printfn "res:%A"

*)

module DnD_ops = 
    open System
    open System.Drawing
    open System.Windows.Forms
    
    ///What:    MVP for impl/testing DnD func + (l8r) Dojo wireframes...
    ///Last updated: Wed Oct 11 2023
    ///Stat:    interimRes3:
    ///[RoTgt -0.5; DzCell ("Cell 1", 1, 1, 0, 0); DzCell ("Cell 2", 1, 1, 1, 0); DzCell ("Cell 3", 1, 1, 2, 0); 
    ///RoTgt 0.5; DzCell ("Cell 4", 1, 1, 0, 1); DzCell ("Cell 5", 3, 1, 1, 1); DzCell ("Cell 6", 2, 1, 4, 1); 
    ///RoTgt 1.5; DzCell ("Cell 7", 2, 1, 0, 2); DzCell ("Cell 8", 2, 1, 2, 2); 
    ///RoTgt 2.5; DzCell ("Cell 9", 2, 1, 0, 3); DzCell ("Cell 10", 3, 1, 2, 3); 
    ///RoTgt 3.5; DzCell ("Cell 11", 2, 1, 0, 4); DzCell ("Cell 12", 2, 1, 2, 4); 
    ///RoTgt 4.5; DzCell ("Cell 13", 2, 1, 0, 5); DzCell ("Cell 14", 2, 1, 2, 5); 
    ///RoTgt 5.5]
    ///
    ///Notes:   - DDnDTgt spans Row (see PostPitch Notes)
    ///         - this curr impl autoCasts to DzCell_v2Struc
    ///         - Xtnd tblPnl with ctor def & add doLayout() to produce struct via member.call()
    ///         - as decided, changes via UI updates def & autoUpdates UI
    ///         - @Add: bld_v1 interleaves dropCells(see output); nd to manually do that
    ///         - @Add: Logic 2 autoPop rows & overflo handling
    
    let printHR() = printfn " - - - - - - - - - - - - - - - - - "
    let tibbie = fun (s:string) -> MessageBox.Show(s, "System Msg") |> ignore
    let defPadding:Padding = new Padding(40)
    let defFont:Font = new Font("Tahoma", 26.0F)
    let defColor:Color = Color.White
    let defForeColor:Color = Color.Black //dCobaltBlue
    let defBackColor:Color = Color.White
    let getCtrlHt() = 
            let g = (new Button()).CreateGraphics()
            ((g.MeasureString("nm", defFont)).ToSize()).Height
    let colN = 3
    let isEven num = (num % 2 = 0)
    let toCellSlug = fun n -> "Cell " + n.ToString()
    let show a = 
            let (n, r, l:list<'t>) = a
            let fixedOrd = List.map (fun innr -> List.rev innr) l
            printfn "fixedOrd: %A" l
            //l.GetType() |> printfn "res:%A ty:%A" (List.splitInto (l.Length / colN) fixedOrd)// |> List.rev)
    let thirdOf3T = fun (x,y,z) -> z
    let getRndLen =
        fun (v:string) -> 
            let r = int (v.Substring 5) //rm cell
            if r%5 = 0 then 3
                elif r < 5 then 1
                else 2


    type DzCell =    | DzCell of string * int * int * int * int
                     | BetwTgt of float * float
                     | RoTgt of float

    type DzRow = | DzRowBlank of DzCell
                 | DzRowFilled of list<DzCell>

    type DzTbl = | DzTable of list<DzRow>

    ///No longer using Random values + State Support + consise 4 doL()
    let tbl = [DzCell ("Cell 1",1,1,0,0); DzCell ("Cell 2",1,1,1,0);
               DzCell ("Cell 3",1,1,2,0); DzCell ("Cell 4",1,1,0,1);
               DzCell ("Cell 5",3,1,1,1); DzCell ("Cell 6",2,1,4,1);
               DzCell ("Cell 7",2,1,0,2); DzCell ("Cell 8",2,1,2,2);
               DzCell ("Cell 9",2,1,0,3); DzCell ("Cell 10",3,1,2,3);
               DzCell ("Cell 11",2,1,0,4); DzCell ("Cell 12",2,1,2,4);
               DzCell ("Cell 13",2,1,0,5); DzCell ("Cell 14",2,1,2,5)]
    let bld_v3 = 
        fun li -> 
            li  |> List.fold (fun s v -> 
                    let (c:int, r:int, inLi:list<'t>) = s
                    let (DzCell(slg, cc, cr, ccI, crI)) = v
                    match inLi with
                    | [] -> 0, 0, [RoTgt((float) 0 - 0.5)]
                    | _ ->
                        match (not(c+1 < colN)) with
                        | true -> 
                            //let CellAndTgt = [RoTgt((float) r + 0.5);DzCell(slg,cc,cr,c,r)] @ inLi
                            //0, r+1, CellAndTgt
                            //reverted to manual pop of RoTgt...
                            0, r+1, DzCell(slg,cc,cr,c,r) :: inLi
                        | _ -> 
                            c+cc, r, DzCell(slg,cc,cr,c,r) :: inLi)  (0,0,[]) 
                |> thirdOf3T |> List.rev
    
    printfn "interimRes3:%A" (bld_v3 tbl)

    let getTblRo =
      fun tbl idx ->
        List.filter (fun c ->
                  let (DzCell(_,_,_,_, cRo)) = c 
                  cRo = idx) tbl

    let getRTPnl() = new Panel(Dock = DockStyle.Fill, AutoSize = true, BorderStyle = BorderStyle.Fixed3D)
    let getCell = 
      fun slg -> new Button(Text = slg, Dock = DockStyle.Fill, AutoSize = true)

    let frm = Form(Text = "DnD ops", Visible = false, TopMost = true, WindowState = FormWindowState.Maximized)
    frm.SuspendLayout()
    let cliP = new TableLayoutPanel(Anchor = AnchorStyles.None, CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset, RowCount = 0, ColumnCount = colN, AutoScroll = true)
    cliP.SuspendLayout()
    cliP.Controls.Clear()
    cliP.ColumnStyles.Clear()
    cliP.RowStyles.Clear()
    List.map (fun c -> cliP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, ((float32) ((1/colN) * 100))))) [1.. colN] |> ignore
    let tblRef = ref tbl
    let tblSButton = new Button(Text = "Table State")
    tblSButton.Click.Add(fun e -> tibbie ((tblRef.Value).ToString()))
    frm.Controls.Add(tblSButton)
    frm.Controls.Add(cliP)
    frm.Layout.AddHandler(new LayoutEventHandler( fun (sender:obj) (e:System.Windows.Forms.LayoutEventArgs) -> 
        //necc? let thisF = sender :?> Form
        let rec procTbl currRo remainder =
          let remLen =
            getTblRo remainder currRo 
            |> List.map (fun currcell -> 
                           let (DzCell(slg,cSp,rSp,cCo, cRo)) = currcell
                           let asCtrl = (getCell slg)
                           cliP.Controls.Add(asCtrl, cCo, cRo)
                           match cSp > 1 with
                           | true -> cliP.SetColumnSpan(asCtrl, cSp) 
                           | _ -> ()
                           match rSp > 1 with
                           | true -> 
                              cliP.SetRowSpan(asCtrl, rSp)
                              cliP.RowStyles.Add(new RowStyle(SizeType.Absolute, ((float32) (((float) (getCtrlHt())) * 1.25 * (float) rSp)))) |> ignore
                           | _ -> ()
                           currcell )
            |> List.length
          //let newRemainder = List.splitAt remLength remainder |> snd
          let newCurrRo = currRo + 1
          //match newRemainder with
          match (newCurrRo > remainder.Length) with
          | true -> () 
          | _ -> procTbl newCurrRo remainder
        procTbl 0 tblRef.Value
        ))


module main = 
    open System
    open System.Diagnostics
    open System.Windows.Forms
    open Trivedi.Core
    open Trivedi.UI
//    open GridTester ModGridTester_RemmedForMonkeyBastas_Jul12_2023
//    open Ide  ModuleIde_RemmedForMonkeyBastas_Jul12_2023
//    open Ext ModuleExt_RemmedForMonkeyBastas_Jul12_2023
//    open deck //ModuleDeck_RemmedForMonkeyBastas_Jul12_2023
//    open perms
//    open FilePanelUpdates
//    open jimmy

    printfn "in mod winFrms..."

    type Bfty = | Bfty of id:string * fty:DocFldType * valu:obj with
        override this.ToString() = 
          let (Bfty(nm, t, v)) = this
          match t with
          | DFldString -> "Bfty(" + nm + ", " + "string)"
          | DFldCurrency -> "Bfty(" + nm + ", $" + v.ToString() + ")"
          | _ -> "Bfty(" + nm + ", " + "unknown)"
        member this.toPer() = 
            "test"
        member this.fromPer() = 
          "test"

#if Camba
    let x = Bfty("test1", DFldCurrency, box 2.22)
    printfn "toString: x:%A x.ToS:%A" x (x.ToString())
//toString: x:Bfty ("test1", DFldCurrency, 2.22) x.ToS:"Bfty(test1, $2.22)"
#endif //Camba

    [<EntryPoint>]
    [<STAThread>]
    let main ag =
        match ag.Length = 0 with 
        | true -> 
            //Application.EnableVisualStyles()
            //Application.SetCompatibleTextRenderingDefault(false)
            try
                hr()
                printfn "winFrms main -> try ->  running main.frm().."
                hr()
                //chkArticleTblDizFile()
                //frm()
                //Application.Run(oldRunner())
                printfn "remmed: Application.Run(frm())"
                //Application.Run(frm())
                //printfn "remmed cardExt..."
                //Application.Run(cardExt())
                //permTests()
            with
                | e -> 
                    printfn "Exc in main: %A" e.Message
                    //remmed 2023.06.28: probably @mbi: Can't find ref 
                    //let st = ((new StackTrace(e, true)).GetFrames()).[0]
                    //printfn "Immed.-> method: %A lineNo:%A col: %A" (st.GetMethod().Name) (st.GetFileLineNumber()) (st.GetFileColumnNumber())
                    //remmed 2023.06: probably @mbi: Can't find ref 
                    //printfn "StackTr -> \r\n%A" (getStTrace e)
            //exeRunnr ag
        | false ->
            System.Console.Write("back in main...pls press any key to continue...")
            let c = System.Console.ReadKey(true)
            match c.Key with
            | ConsoleKey.Escape -> printfn "Esc..."
            | _ -> printfn "..."
        0
