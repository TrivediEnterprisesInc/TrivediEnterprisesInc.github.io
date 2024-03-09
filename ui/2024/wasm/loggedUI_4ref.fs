(*  Wwnn.  HP. QP. (TM)
    Copyright (c) M. P. Trivedi 2016-2022.  All rights reserved. 
    TrivediBldHdr->
    |Trivedi SrcCtrlHdr File.  Copyright (c) 2015-2022 M. P. Trivedi.  All rights reserved.|637901455|test.fs|none|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|

    "src\UI\loggedUIRunner.fs  --platform:x64 --target:exe --out:logdUIRunr.exe -r:bin\Trivedi.Core.dll -r:bin\Trivedi.CoreAux.dll -r:bin\Trivedi.UI.dll -I:C:\Windows\Microsoft.NET\Framework\v4.0.30319"
    "src\UI\loggedUIRunner.fs  --platform:x64 --target:exe --out:load_logdUIRunr.exe -r:bin\Trivedi.Core.dll -r:bin\Trivedi.UI.dll -I:C:\Windows\Microsoft.NET\Framework\v4.0.30319"

*)

namespace Trivedi

#nowarn "20" "25" "58" "66" "67" "64" "760" "1182" "1558"

module main = 

    type ITblMarker = interface end
    type AdminTbl() = interface ITblMarker
    type ArticleTbl() = interface ITblMarker
    type TaskTbl() = interface ITblMarker
    type DocUNID = | DocUNID of string

    let timeStmp = lazy (DateTime.Now)
    let getUNID (cls:string) : DocUNID =
        DocUNID(((timeStmp.Force().Ticks - (DateTime(2001, 1, 1)).Ticks).ToString() + "^" + cls))

    //Nd to decide pros/cons for extending basicTys to custom;
    //e.g.: longStrByteArr vs AttchByteArr.  Poss issues with singleCsDU Constr embedded.
        type FldType = | FldString
                   | FldNumber
                   | FldCurrency
                   | FldLongString
                   | FldAttachment
                   | FldBoolean
                   | FldChoiceList
                   | FldRange
                       | FldDate 
                   | FldDateTime
                   | FldColor

    type DocFld = | DocFld of FldType*string*bool*string with
        override this.ToString() = 
                let (DocFld(ty, intNm, isInt, dispNm)) = this
            "DocFld: |intNm: " + intNm + "|isInt: " + isInt.ToString() + "|dispNm: " + dispNm + "|" + ty.ToString() + "|"


    //@TBD: does isDefault:bool belong here?
    type DesDocInf = | DesDocInf of created:DateTime * lastMod:DateTime * lastModBy:string with
        static member deflt() =
            DesDocInf(DateTime.Now, DateTime.Now, "mike@trivedi.com")
#else


        //printfn "Hello Dz: %A" (DesDoc.genDefault(AdminTbl()))

    type DVDef<'a, 'g, 's, 'c> = | DVDef of nm:string * tblDef:TblDef2<'a> * docInf:DesDocInf
                                    * colCellFont:list<(int * Font)> option
                                    * colHdrs:list<string> * visCols:int
                                    * fldLi:list<string> * fixedSz:(int* int) option
                                    * fltr:('a -> bool) option * grpBy:('a -> 'g) option * categBy:('a -> 'c) option
                                        * sortBy:('a -> 's) option * openCategs:list<string>
                                    * rowTips:bool * Ttips:list<string*list<string>> option
                                    * pOpts:DVPaginationOpts option                                   

    type DzEnum<'a> = | FrmDocE = 0
                      | TblDefE = 1
                          | DVDefE = 2 
                      | LookupFldE = 3 
                      | GiamBE = 4

    let getDefaultDVDef<'a> =
        getDefFlds >> defTitles >> bgColDefaults >> defSort etc.
        
    //the defSort nds 2 match on all poss DzEnums, so 
    let genDefault<'d> =
        match this with | Dizzy -> hardCoded | TblDef -> hardCoded
            | _ -> raise "unknown docTy"

    let dz2TSItm "SetDefault" = tryFind >> unMark >> setThis
    let dz2TSItm "Copy|Paste|Del" = ...
    
    Open DizzyTbl (svr) -> Just one Option (blds WieselD & returns)
    
    Open DV (optId) -> Creates WD, returns w/DizzyDoc -> Changes -> Prompt 4 SaveOnExit
    
        let getDizzyD ty (ob:'t) -> Tbl.'t -> match on DocTy (...|FldDef...)
    
    ALL DizzyCtors get DzDoc; so openFrm (DocID) -> WDPop + open
    
    DizzyDs nd Stubs 4 mAdd/mGet + UNId (Same proc as Dat but w/DzDoc)
    
#endif //ump


    let genCLi li =
       match li with
         | Double d -> printfn "Dbl: %A" d
         | Ty t -> printfn "Ty : %A" t
         | _ -> printfn "Unknown : %A" li
#endif //iffy


#endif //Dewey



    let defFont:Font = new Font("Tahoma", 26.0F)
    let tibbie = fun (s:string) -> MessageBox.Show(s, "System Msg") |> ignore
    let liToString = 
        fun (l:list<_>) ->
           List.fold2 (fun acc i x -> acc + "\n" + (i.ToString() + " " +  x.ToString())) "" [0..(l.Length - 1)] l
    let liLen =
        fun (l:list<_>) ->
            printfn "res of spooProclen: %A" ((l).Length)
            l


    let chkArticleTblDizFile() =
        hr()
        //recd: "Microsoft.FSharp.Collections.FSharpList`1[Trivedi.BaseBrijType_NoTpl`1[Trivedi.ArticleTbl]]"
        printfn "now trying to read article.bdf..."
        let body = (File.ReadAllBytes(@"C:\Users\inets\Documents\mike\bin\article.bdf"))
            let newOb = (deSerBA body)
        printfn "recd: %A" (newOb.GetType().ToString())        
        hr()

//Oct6 UI updates + dizzy stuff
//File.WriteAllBytes("article.bdf", (serBA dzC))

    let artFldList = [("Custom title",FldString)]

    let tkFldList() =         
       [DocFld(FldString, "unid", true, "Document UNID");
        DocFld(FldString, "title", true, "Title");
        DocFld(FldString, "project", true, "Project");
        DocFld(FldString, "moduleNm", true, "Module");
        DocFld(FldString, "submodule", true, "SubModule");
        DocFld(FldString, "objective", true, "Objective");
        DocFld(FldRange, "importance", true, "Importance");
        DocFld(FldRange, "urgency", true, "Urgency");
        DocFld(FldBoolean, "completedOn", true, "completed On");
        DocFld(FldChoiceList, "tgtVer", true, "Target Version");
        DocFld(FldString, "docLinks", true, "DocLinks");
        DocFld(FldLongString, "cont", true, "Content");
        DocFld(FldString, "tags", true, "Tags");
            DocFld(FldBoolean, "flag", true, "Flag")]

    let setupTestEnv =
        fun (d:Form) ->
            let ctxtM:ContextMenuStrip = (ctrlGetTag2 "dskCtxtMS" d).Value
            let testItm1 = new ToolStripMenuItem("MusicDV")
            testItm1.Click.AddHandler(new EventHandler(fun (sender:obj) (e:EventArgs) ->
                         let mFrm = new Form(Text = "MusicDV")
                             let musicF = કલકતી_પાન(Reg, mFrm, musicDV, musicDat
                         mFrm.Show()))
            let testItm2 = new ToolStripMenuItem("DesignDV")
            testItm2.Click.AddHandler(new EventHandler(fun (sender:obj) (e:EventArgs) ->
                         let dFrm = new Form(Text = "DzDV")
                         let dzDv = કલકતી_પાન(Dz, dFrm, WDV, HelloDz)
                             dzDv.AutoSizeColumnsMode <- DataGridViewAutoSizeColumnsMode.Fill
                         (dzDv.Columns.[(dzDv.getColNamed("Document Title"))]).FillWeight  <- 200.0f
                         (dFrm.Show())))
            let testItm3 = new ToolStripMenuItem("Form")
            testItm3.Click.AddHandler(new EventHandler(fun (sender:obj) (e:EventArgs) ->
                         let frm = (ફોરમ_પાન(2, tkFldList(), (new Form(Text = "WidgetTester")), TaskTbl())) :> Form    
                         frm.Show()))
            ctxtM.Items.AddRange([|toTSItm (new ToolStripSeparator()); toTSItm testItm1; toTSItm testItm2; toTSItm testItm3|])
            d    


    let srchFrm m = 
        let (Mtpl(l:list<(string * obj * System.Type)>)) = m
        let fnd = List.tryFindIndex (fun itm -> let (x,_,_) = itm
                                                x = "Form") l
        match fnd with
        | Some idx -> 
            printfn "srchFrm: form found"
            raise (Trivedi_Core_ex("not disposed yet??"))
        | _ -> 
            printfn "srchFrm: from not fnd; ok"
        m
        
    let postDatK s =
        ``⍲`` {
(*        
                 let s2 = prn "*******postDat: assigning parnt FrmVar..." s

                 printfn "postDatK: abt to run ટેબલ_પાન"
                 let f = (new Form(WindowState = FormWindowState.Maximized, Visible = false, Text = "loggedUIRunner Form: Copyright (c) M. P. Trivedi 2016-2022.  All rights reserved.", TopMost=true, Font=defFont))
                 let frm = (ટેબલ_પાન ("સુપારી","લવલી","ગુલકંદ", "ક્વિમામ", "પીચાક", f, AdminTbl())) :> Form
                 //dzDV btns refer to state
                 ctrlAddTags2 ["wld", (box s2)] (frm)
                 Application.Run(setupTestEnv frm)

                 printfn "....................postDatK: RAN Grid2 constr..."
                 return (Mtpl.AddOne "dskFrm" (box frm) s2)   
*)
                  return s
                 }

    let loadDatK m = 
        let (Mtpl(l:list<(string * obj * System.Type)>)) = m
        (AdDatK m spooDatK tkDatK imgDatK postDatK (fun x -> x

    let getInitTpl() = 
        printfn("--getInitTpl():be4 loadDatK... ")
        (loadDatK >!> Mtpl.empty())
    
    let initUISt =
         ``⍒`` {
                   do! prnS "@@@@initUISt 1; launching ChkSt@@@@"
                   let gFrm = (new Form(WindowState = FormWindowState.Maximized, Visible = false, Text = "initUISt Grid Form: Copyright (c) M. P. Trivedi 2016-2022.  All rights reserved.", TopMost=true, Font=defFont))
                   printfn "initUISt: abt to run Grid2 constr"
                   //do કલકતી_પાન(gFrm, musicDV, musicDat)
                   let! a = getS
                   let frm = new Form(Text = "test form")
                   let a2 = getS a |> fst
                   let res = (Mtpl.AddOne "Form" frm a2)
                   do! putS res
                   let! b = getS
                   do! prnS "initUISt:abt to launch srchfrm..."
                   let dd = srchFrm >!> b
                   do! putS (Mtpl.getUNID dd |> fst)
                   let! f = getS
                   do! prnS "--intUISt b4: "
                   printfn ">>%A" (f.ToString())
                   do! putS (Mtpl.getUNID f |> fst)
                   let! f1 = getS
                   do! prnS "--initUISt:after 1st get ->: "
                   printfn ">>%A" (f1.ToString())
                   let interim = (Mtpl.getUNID f1)
                   do! prnS("--initUISt:val rec'd interim: " + (interim |> snd).ToString())
                   do! putS (interim |> fst)
                   let! f2 = getS
                   let interim2 = (Mtpl.getUNID f2)
                   do! putS (interim2 |> fst)
                   do! prnS("--initUISt:val rec'd interim2: " +  (interim2 |> snd).ToString())
                   printfn ">>%A" (f2.ToString())
                   let! g = getS
                   do! putS ((Mtpl.getUNID g) |> fst)
                   do! prnS("--initUISt:after putS getUN- 2nd get ... ")
               }

    //let fn = "bin\Trivedi.UI.exe"
    //let fn = "logged.exe"
    //let fn = "state.exe"
    let fn = "logdUIRunr.exe"

    let exeRunnr =
        fun a ->
            let inf = new ProcessStartInfo(FileName = fn, UseShellExecute = false, RedirectStandardOutput = true)
            let p = new Process(StartInfo = inf)
            p.Start() |> ignore
                Console.WriteLine(p.StandardOutput.ReadToEnd())
            p.WaitForExit()
            System.Console.Write("pls press any key to continue...")
            let c = System.Console.ReadKey(true)
            match c.Key with
            | ConsoleKey.Escape -> printfn "Esc..."
            | _ -> printfn "..."


//@ToDo 10.10 -> Nd to reInstate icnPanels (MBI) + Delta
                //Modify existing getDDocTitl to return [a;b;c]

    [<EntryPoint>]
    [<STAThread>]
    let main ag =
        printfn "main:1"
        match ag.Length = 0 with 
        | true -> 
            printfn "main:2"
            Application.EnableVisualStyles()
            //Application.SetCompatibleTextRenderingDefault(false)
            try
                hr()
                //chkArticleTblDizFile()
                hr()
                    printfn "loggedUIRunner main(): launching runTypeTests().."
                //runTypeTests()
                getInitTpl() |>
                    ``⍒`` {
                            printfn ">>>>>>>>in inner st1..."
                            let! x = getS
                            printfn ">>>>>>main:After getInitTpl(): %A" (x.ToString())
                            hr()
                            
                            let s2 = prn ">>>>>>main: assigning parnt FrmVar..." x
                            printfn ">>>>>>main: abt to run ટેબલ_પાન"
                            let f = (new Form(WindowState = FormWindowState.Maximized, Visible = false, Text = "loggedUIRunner Form: Copyright (c) M. P. Trivedi 2016-2022.  All rights reserved.", TopMost=true, Font=defFont))
                            let frm = (ટેબલ_પાન ("સુપારી","લવલી","ગુલકંદ", "ક્વિમામ", "પીચાક", f, AdminTbl())) :> Form
                            //@ToDo: dzDV btns refer to state from f; nd to update all calls to dsk
                            ctrlAddTags2 ["wld", (box x)] (f)
                            ctrlAddTags2 ["wld", (box x)] (frm)
                            Application.Run(setupTestEnv frm)

                            printfn ">>>>>>main: RAN Dsk constr..."
                            return (Mtpl.AddOne "dskFrm" (box frm) s2)   
                            
                            

                            do! initUISt
                            let! currSt = getS
                            printfn ">>>> main:After initUISt: %A" (currSt.ToString())
#endif


                            printfn "*******in inner st2; gFrm1"
                            let gFrm = (new Form(WindowState = FormWindowState.Maximized, Visible = false, Text = "initUISt Grid Form: Copyright (c) M. P. Trivedi 2016-2022.  All rights reserved.", TopMost=true, Font=defFont))
                            ctrlAddTags2 ["wld", (box currSt)] (gFrm)
                            gFrm.MouseDown.AddHandler(new MouseEventHandler( fun (sender:obj) (e:MouseEventArgs) -> 
                                match e.Button with
                                | MouseButtons.Right -> 
                                        let statL:list<string> = (Mtpl.GetOne "statLog" currSt).Value |> List.rev
                                        let outT = (liToString statL) + "\r\n" + mLTxt
                                        do ગપ્પા_પાન (SizeM,Some("Current Stat State ->"),None , Some(outT), None,gFrm, AdminTbl()) |> ignore
                                        //do કલકતી_પાન(gFrm, musicDV, musicDat)
                                        //do ટેબલ_પાન<'a> ("સુપારી","લવલી","ગુલકંદ", "ક્વિમામ", "પીચાક",gFrm, AdminTbl()) |> ignore
                                | _ -> () ))
                            
                            printfn "*******in inner st2; Mtpl.Has errLog:"
                            printfn "%A" ((Mtpl.Has "errLog" currSt).ToString())
#endif                            
                           
                            

                            //@mbi do કલકતી_પાન(gFrm, musicDV, musicDat)
                                do ટેબલ_પાન<'a> ("સુપારી","લવલી","ગુલકંદ", "ક્વિમામ", "પીચાક",gFrm, AdminTbl()) |> ignore
                            printfn "*******in inner st2; gFrm2"
                            printfn "*******postDatK: abt to run Grid2 constr (remmed, can't find type)"
#endif

                            do કલકતી_પાન(gFrm, musicDV, musicDat)
                            Application.Run(gFrm)
#endif //grid

                            let f = (ફોરમ_પાન_Oct6(2, antiMBI_tkFldList, gFrm, TaskTbl())) :> Form
                            Application.Run(f)
#endif //frmLocal
                            let! z = getS
                            do! prnS "initUISt:abt to launch loadDz..."

                            let dd = loadDz >!> z
                            do! putS dd
#endif //dizzy
                            let! finSt = getS

                            printfn "*******eof St: %A" (finSt.ToString())
                            return ()
                          } |> ignore
            with
                | e -> 
                    printfn "Exc in main: %A" e.Message
                    let st = ((new StackTrace(e, true)).GetFrames()).[0]
                    printfn "Immed.-> method: %A lineNo:%A col: %A" (st.GetMethod().Name) (st.GetFileLineNumber()) (st.GetFileColumnNumber())
                    printfn "StackTr -> \r\n%A" (getStTrace e)
            //exeRunnr ag
        | false ->
            mapArgs ag.[0]  //fsc
            //exeRunnr ag
            System.Console.Write("back in main...pls press any key to continue...")
            let c = System.Console.ReadKey(true)
            match c.Key with
            | ConsoleKey.Escape -> printfn "Esc..."
            | _ -> printfn "..."
        0
