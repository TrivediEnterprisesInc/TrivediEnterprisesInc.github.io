(*  Brij (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 
    TrivediBldHdr->
    |Trivedi SrcCtrlHdr File.  Copyright (c) 2015-2025 M. P. Trivedi.  All rights reserved.|637901455|test.fs|none|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|

    fsc src\pbt\dskCli_DzDv.fs --platform:x64 --standalone --target:exe --out:dzdv.exe -r:lib22\Trivedi.Core.dll -r:lib22\Trivedi.CoreAux.dll -r:lib22\Trivedi.UI.dll -r:lib22\Trivedi.Brij.dll -r:lib22\Trivedi.UIAux.dll -I:C:\Windows\Microsoft.NET\Framework\v4.0.30319

    Provenance: Modified from loggedUIRnrDec20_FrmDz
    to incorp modified  UI els 4 pbt    

    Last updated: Aug 18 2025 (added dsk.TS support)

    Heads up: There is a bogus match somewhere between Categ/DocTitle which will pro'lly fail

    Contains modules:  pbt_main (from dskCli_Desktop)
                                DzDv_Actual
                                DzDv_Ext
                                DzDv_Test
*)

namespace Trivedi

#nowarn "20" "25" "58" "66" "67" "64" "760" "1182" "1558"


[<AutoOpen>]
module pbt_main = 
    open System
    open System.Diagnostics
    open System.Drawing
    open System.Drawing.Imaging
    open System.IO
    open System.Text
    open System.Text.RegularExpressions
    open System.Windows.Forms
    open Trivedi
    open Trivedi.Core
    open Trivedi.Control
    open FSharp.Reflection
    
//Mar 17_25:
//New struct: 
//This mod will contain main mod init stuff + helpers + common
//Next we'll have 1 mod per funct area; with submods 4:
//  1) Desktop_Actual
//  2) Desktop_Exts
//  3) Desktop_Tests (with the models)
//@ btm of the main mod (1) we'll put the runners

    type SOrd = | Asc
                | Dsc

    let RoyalBlue = Color.FromArgb(0, 39, 94)
    let defFont:Font = new Font("Tahoma", 26.0F)
    let tibbie = fun (s:string) -> MessageBox.Show(s, "System Msg") |> ignore
    
    //@ToDo: add to extensions, sig is (fun acc i x)
    let lifoi = 
        fun f (l:list<_>) ->
           List.fold2 f
(*
    let liToString = 
        fun (l:list<_>) ->
           lifoi (fun acc i x -> acc + "\n" + (i.ToString() + " " +  x.ToString())) "" [0..(l.Length - 1)] l
*)
    let liLen =
        fun (l:list<_>) ->
            printfn "res of liLen: %A" ((l).Length)
            l
    let anc = function | "R" -> AnchorStyles.Right | "N" -> AnchorStyles.None | "TL" -> AnchorStyles.Top &&& AnchorStyles.Left | "LR" -> AnchorStyles.Right &&& AnchorStyles.Left | "TB" -> AnchorStyles.Top &&& AnchorStyles.Bottom | "TR" -> AnchorStyles.Top &&& AnchorStyles.Right | "BR" -> AnchorStyles.Bottom &&& AnchorStyles.Right | "BL" -> AnchorStyles.Bottom &&& AnchorStyles.Left
    let doc  = function | "T" -> DockStyle.Top | "B" -> DockStyle.Bottom | "R" -> DockStyle.Right | "L" -> DockStyle.Left | "N" -> DockStyle.None | "F" -> DockStyle.Fill

    let toTSItm = fun o -> o :> ToolStripItem
    let brijLogo = 
        let logo = Bitmap.FromFile("C:\\Users\\inets\\Documents\\mike\\src\\Data\\images\\brij.png")
        Bitmap (logo, new Size(logo.Width/2, logo.Height/2))

    let snapshot = 
        fun ctrl ->
            let no = ref 0
            no.Value <- no.Value + 1
            let c = ctrl :> Control
            let bm = new Bitmap(c.Width, c.Height)
            c.DrawToBitmap(bm, Rectangle(Point(0,0),bm.Size))
            (bm :> Image).Save(("snap" + (no.Value).ToString() + ".gif"), System.Drawing.Imaging.ImageFormat.Gif)


    type UIColorScheme = | UIColorScheme of fore:Color * back:Color 
                            * AccentFore:Color *  AccentBack:Color
                            * TitFore:Color * TitBack:Color * icn:Color with
        member this.Fore() = 
                let (UIColorScheme(Fore, _, _, _, _, _, _)) = this
                Fore
        member this.Back() = 
                let (UIColorScheme(_, Back, _, _, _, _, _)) = this
                Back
        member this.accentFore() = 
                let (UIColorScheme(_, _, AccentFore, _, _, _, _)) = this
                AccentFore
        member this.accentBack() = 
                let (UIColorScheme(_, _, _, AccentBack, _, _, _)) = this
                AccentBack
        member this.titFore() = 
                let (UIColorScheme(_, _, _, _, TitFore, _, _)) = this
                TitFore
        member this.titBack() = 
                let (UIColorScheme(_, _, _, _, _, TitBack, _)) = this
                TitBack
        member this.Icn() = 
                let (UIColorScheme(_, _, _, _, _, _, icn)) = this
                icn


//Recoloring Images: https://learn.microsoft.com/en-us/dotnet/desktop/winforms/advanced/recoloring-images?view=netframeworkdesktop-4.8
//see also: https://learn.microsoft.com/en-us/dotnet/api/system.drawing.imaging.bitmapdata?view=netframework-4.8
    
    let brijScheme = UIColorScheme(Color.Black, Color.FromName("AliceBlue"), 
                                    RoyalBlue,  Color.FromName("DarkGray"), 
                                    RoyalBlue, RoyalBlue, Color.FromName("Red"))
    let darkScheme = UIColorScheme(Color.White, Color.Black, 
                                    Color.FromName("Aqua"), Color.Black, 
                                    Color.FromName("Aqua"), Color.FromName("DarkGray"), Color.FromName("Aqua"))
    let sublime = 
        let sYellow = Color.FromArgb(255, 253, 246, 227)
        let sLtGrn = Color.FromArgb(255, 133,153,0)
        let sBrn = Color.FromArgb(255, 211, 130, 52)
        let sDkGrn = Color.FromArgb(255, 42, 161, 152)
        let sDrkYell = Color.FromArgb(255,238,232,213)
        UIColorScheme(Color.Black, sYellow,sLtGrn, sYellow, sDkGrn, sDrkYell, sBrn)

    //let userScheme = UIColorScheme(Color.White, Color.Black, Color.FromName("Aqua"), Color.FromName("DarkGray"), Color.FromName("SkyBlue"))
    let currentScheme = brijScheme

(*
    let chkArticleTblDizFile() =
        hr()
        //recd: "Microsoft.FSharp.Collections.FSharpList`1[Trivedi.BaseBrijType_NoTpl`1[Trivedi.ArticleTbl]]"
        printfn "now trying to read article.bdf..."
        let body = (File.ReadAllBytes(@"C:\Users\inets\Documents\mike\bin\article.bdf"))
        let newOb = (deSerBA body)
        printfn "recd: %A" (newOb.GetType().ToString())
        hr()
        tibbie "remmed Feb_28_25"
*)

//Oct6 UI updates + dizzy stuff
//File.WriteAllBytes("article.bdf", (serBA dzC))

    let artFldList = [("Custom title",FldString)]

    let tkFldList() =         
       [DocFld(FldString, "unid", true, "Document UNID");
        DocFld(FldString, "title", false, "Title");
        DocFld(FldString, "project", false, "Project");
        DocFld(FldString, "moduleNm", false, "Module");
        DocFld(FldString, "submodule", false, "SubModule");
        DocFld(FldString, "objective", false, "Objective");
        DocFld(FldRange, "importance", false, "Importance");
        DocFld(FldRange, "urgency", false, "Urgency");
        DocFld(FldDate, "completedOn", false, "Completed On");
        DocFld(FldChoiceList, "tgtVer", false, "Target Version");
        DocFld(FldString, "docLinks", false, "DocLinks");
        DocFld(FldLongString, "cont", false, "Content");
        DocFld(FldString, "tags", false, "Tags");
        DocFld(FldBoolean, "flag", false, "Flag")]

#if remmedFeb28_2025
    let setupTestEnv (d:Form) x =
            let ctxtM:ContextMenuStrip = (!!~ "dskCtxtMS" d).Value
            let testItm2 = new ToolStripMenuItem("tkDV")
            testItm2.Click.AddHandler(new EventHandler(fun (sender:obj) (e:EventArgs) ->
                 let tplFLi = baseTkDatAux tDef tkFldLiLocalAux |> fst
                 let tkDt = deSerBA (File.ReadAllBytes("baseTkDatAux.bdf")) :?> list<list<_>>
                 //let tDefLocal = SaadoMasaloAux("Task Tbl", tkColHdrs, brijLogo, TaskTbl())
                    //error FS0030: Value restriction. The value 'tkCalcuttiMasalo' has been inferred to have generic type
                    //val tkCalcuttiMasalo : main.CalcuttiMasalo<'_a,'_b,'_c> when '_c :> ITblMarker
                    //let tkCalcuttiMasalo:CalcuttiMasalo<_,_,_> = deSerBA (File.ReadAllBytes("tkCalcTest1.bdf")) :?> CalcuttiMasalo<_,_,_>
                 let dvD = CalcuttiMasaloAux("Default tkDV", tDef, DesDocInfDeflt(), None, tkColHdrs, 6, tplFLi, None, None, None, None, [], XAll, false, None, Some(pagOptsAux))
                 let tDv = કલકતી_પાન_Aux(Dt, d, dvD, tkDt, "procId", "winTy")
                 (tDv.getFrm()).Show()))

            let tkNov = new ToolStripMenuItem("tkNov")
            tkNov.Click.AddHandler(new EventHandler(fun (sender:obj) (e:EventArgs) ->
                         let mFrm = new Form(Text = "tkDV")
                         let pagOptsDec = Grid2.DVPaginationOpts(200,1,2000, 200, 400, 800000)
                         let tkDt = deSerBA (File.ReadAllBytes("baseTkDatAux.bdf")) :?> list<list<_>>
                         let docInfDec = Grid2.DesDocInf(DateTime.Now, DateTime.Now, "mike@trivedi.com")
                         let tDV = Grid2.DVDef("tkDV", Grid2.TblDef2("task Table", TaskTbl()), docInfDec, None, tkColHdrs, 6,  tkFldLiLocalAux,  None,  None, None, None, [], true, None, Some(pagOptsDec))
                         let musicF = કલકતી_પાન_Nov(Reg, mFrm, tDV, tkDt)
                         mFrm.Show()))


#if remmed_mbi_Dec09            
                         //d.IsMdiContainer <- true
                         let dvD = CalcuttiMasaloAux("Default tkDV", tDef, DesDocInfDeflt(), None, tkColHdrs, 6, tkFldLiLocalAux, None, None, None, None, [], XAll, true, None, Some(pagOptsAux))
                         let tDv = કલકતી_પાન_Aux(Dt, d, dvD, (baseTkDatAux tDef tkFldLiLocalAux), "procId", "winTy")
                         (tDv.getFrm()).Show()))

            let testItm2 = new ToolStripMenuItem("DesignDV")
            testItm2.Click.AddHandler(new EventHandler(fun (sender:obj) (e:EventArgs) ->
                         let dFrm = new Form(Text = "DzDV")
                         let dzDv = કલકતી_પાન_Nov(Dz, dFrm, WDV, HelloDz)
                         dzDv.AutoSizeColumnsMode <- DataGridViewAutoSizeColumnsMode.Fill
                         (dzDv.Columns.[(dzDv.getColNamed("Document Title"))]).FillWeight  <- 200.0f
                         (dFrm.Show())))
#endif //remmed_mbi_Dec09
            let testItm3 = new ToolStripMenuItem("defaultForm")
            testItm3.Click.AddHandler(new EventHandler(fun (sender:obj) (e:EventArgs) ->
                         let flds = tkFldList()
                         //let frmDef = FrmDef((getUNID "^FrmDef"), "AliceBlue Definition Document for: " + (tblNm (tkFldList())), 2, flds, DesDocInfDeflt(), defFont, defFont, Color.Black, Color.White, TaskTbl())
                         tibbie "fake err"))
                         //let frm = (બનારસી_પાન(frmDef, d)) :> Form
                         //frm.Show()))
            let testItm4 = new ToolStripMenuItem("BlueForm")
            testItm4.Click.AddHandler(new EventHandler(fun (sender:obj) (e:EventArgs) ->
                         let frmDef = deSerBA (File.ReadAllBytes("AliceBlue.frm")) //:?> DesDoc<_> //FrmDef<Trivedi.TaskTbl> //DesDoc<_>
                         //...mbi splat...
                         //Unable to cast object of type 'FrmDef[Trivedi.TaskTbl]' to type 'DesDoc`1[Trivedi.ITblMarker]'.
                         tibbie ("type found: " + frmDef.GetType().ToString())
                         //let frm = (બનારસી_પાન(frmDef, d)) :> Form
                         //frm.Show()
                         ))
            let testItm5 = new ToolStripMenuItem("tkPicks")
            testItm5.Click.AddHandler(new EventHandler(fun (sender:obj) (e:EventArgs) ->
                         let w:Mtpl = (!!~ "wld" d).Value
                         let tkP:Mtpl = (Mtpl.GetOne "tkPicks" w).Value
                         tibbie ("tkP ty: " + (tkP.GetType().ToString()))
                         tibbie (tkP.ToString())
                         let tit:string = (Mtpl.GetOne "title" tkP).Value
                         tibbie tit
                         ))
            ctxtM.Items.AddRange([|toTSItm (new ToolStripSeparator()); toTSItm testItm3; toTSItm testItm4|])
            let testItm7 = new ToolStripMenuItem("getImg")
            testItm7.Click.AddHandler(new EventHandler(fun (sender:obj) (e:EventArgs) ->
                         tibbie (x.GetType().ToString())
                         let dat = (Mtpl.GetOne "DatLi" x).Value
                         tibbie ("got ty: " + (dat.GetType().ToString()))
                         let itm = dat |> List.pick (fun itm -> 
                                                tibbie "pick1"
                                                let (BaseBrijType_NoTpl(dt, s, tblTy)) = itm
                                                let (CoreMod(CoreM(DocUNID(unid), crDt, modDt, tit, cont, tags, flag))) = dt.[0]
                                                tibbie "pick2"
                                                if tit = "sort.png" then Some s else None)
                         tibbie ("res was " + itm.GetType().ToString())
                         ))
            let testItm6 = new ToolStripMenuItem(Text="frmSetup")
            testItm6.Click.AddHandler(new EventHandler(fun (sender:obj) (e:EventArgs) ->
                ગપ્પા_પાન (SizeM,Some("Form Configuration Settings"),None , Some(box (tkFldList())), None, d, frmSetupDlg())  |> ignore
            ))

            let datImgTest = new ToolStripMenuItem(Text="datImgTest")
            datImgTest.Click.AddHandler(new EventHandler(fun (sender:obj) (e:EventArgs) ->
                tibbie "launching getDi.."
                let dat = (Mtpl.GetOne "Dat" x).Value
                tibbie ("dat ty: " + dat.GetType().ToString())
                //let (docBase: (string * BaseBrijType_NoTpl<'a>) option) = 
                let li = dat |> Map.filter (fun k v -> કન્ટેનર્સ k "^customImg^Ad") |> Map.toList
                tibbie ("li ty: " + li.GetType().ToString())
(*
                        |> List.tryFind (fun itm -> let (nm, doc:BaseBrijType_NoTpl<_>) = itm
                                                    let (BaseBrijType_NoTpl(dt, s, tblTy)) = doc
                                                    let (CoreMod(CoreM(DocUNID(unid), crDt, modDt, tit, cont, tags, flag))) = dt.[0]
                                                    tit.ToLower().Trim() = nm.ToLower().Trim())
                let (nm, doc:BaseBrijType_NoTpl<'a>) = docBase.Value
                use ms = new MemoryStream(doc.contAsB())
                Image.FromStream(ms)
                let thisTSitm = sender :?> ToolStripItem
                let ts = thisTSitm.Owner
                ts.Items.Add(new ToolStripButton(img))
                tibbie ("stat tibbie " + (img.GetType().ToString())
*)
                ))
            ctxtM.Items.AddRange([| toTSItm testItm7; toTSItm testItm6; toTSItm datImgTest|])
            d

#endif //remmedFeb28_2025

    [<AutoOpen>]
    module DzDv_Actual = 
        open System
        open System.Diagnostics
        open System.Drawing
        open System.Drawing.Imaging
        open System.IO
        open System.Text
        open System.Text.RegularExpressions
        open System.Windows.Forms
        open Trivedi
        open Trivedi.Core
        open Trivedi.Control
        open Trivedi.Brij
        open Trivedi.UIAux
        open FSharp.Reflection

        type expandoState = | XAll
                            | XNone
                            | XUserSel

        type TblDef2<'t when 't :> ITblMarker> = | TblDef2 of tblNm:string * tblTy:'t 

        type DVDef<'g, 's, 'c, 'a when 'a :> ITblMarker > = 
                    | DVDef of nm:string * tblDef:TblDef2<'a> * docInf:DesDocInf
                                        * colCellFont:list<(int * Font)> option
                                        * colHdrs:list<string> * visCols:int
                                        * fldLi:list<string> * fixedSz:(int* int) option
                                        * fltr:('a -> bool) option * grpBy:('a -> 'g) option * categBy:('a -> 'c) option
                                        * sortBy:('a -> 's) option * openCategs:list<string>
                                        * rowTips:bool * Ttips:list<string*list<string>> option
                                        * pOpts:DVPaginationOpts option   

        type brijDGV() =
            inherit DataGridView(SelectionMode = DataGridViewSelectionMode.FullRowSelect,MultiSelect = false, Dock = doc "F", Name = "g", AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders, ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single, CellBorderStyle = DataGridViewCellBorderStyle.Single, GridColor = currentScheme.Fore(), RowHeadersVisible = false, Font = defFont, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells)
            override this.InitLayout() =
                //tibbie "in initlayout"
                base.InitLayout()

(*     June 6: Some notes on the edits:
            - pbtRemmed many sections coz we don;t need em for dzDv
            - Poss already done: multiCateg support via (# of Cats) added prior to Dat (see wobblyDat)
            - We might nd subCategs 4 Log files (see Notes)
            - @TBD: dzDocInf is a stand-in for CoreMod. Any benefits of using the latter?
            - Note also that this may not be the l8st (chk 4 cellPaintHandler; we fiddled w/the icons (smaller, color) etc.
            
            UIAux has the updated DesDoc type which incls the dzEls
               
                
*)
        type કલકતી_પાન_Nov12<'t when 't :> ITblMarker> (own:Form, inDef:DVDef<'c, 's, 't, 'v>, inDat) as g =
            inherit brijDGV()
            let mutable def = inDef
            let mutable dat:obj list list = inDat
            do printfn "in કલકતી_પાન preKatho..."
            let (DVDef(nm, tblDef, docInf, colCellFont, colHdrs, visCols, fldLi, fixedSz, fltr, grpBy, categBy, sortBy, openCategs, rowTips, Ttips, pOpts)) = def
	    let mutable currOpenCategs = openCategs
            do printfn "in કલકતી_પાન preKatho2..."
            let (TblDef2(tblNm, tblTy)) = tblDef
            do printfn "in કલકતી_પાન preKatho3..."
            let (DVPaginationOpts(pgSz, currPg, totPgs,  recFrm , recTo , totRecs)) = pOpts.Value
            do printfn "in કલકતી_પાન preKatho4..."
            //let wld:Mtpl = (!!~ "wld" own).Value
#if pbtRem
            do (!!^ ["wld", box (Mtpl.empty())] own)
#endif //pbtRem
            do printfn "in કલકતી_પાન preKatho5..."
            let categExpando =
                g.CellClick.AddHandler(new DataGridViewCellEventHandler (fun sender (e:DataGridViewCellEventArgs) -> 
                                    match (g.isCategRow(e.RowIndex)) with
				    | true ->
                                        let categ:string = unbox (dat.[e.RowIndex].[g.getColNamed("Parent")])
                                        currOpenCategs <- (categ :: currOpenCategs)
					katho()
                                    | _ -> ()))
            let openDoc =
                g.CellDoubleClick.AddHandler(new DataGridViewCellEventHandler (fun sender (e:DataGridViewCellEventArgs) -> 
                                    match (g.isCategRow(e.RowIndex)) with
				    | false ->
                                        let dzOb = unbox (dat.[e.RowIndex].[g.getColNamed("Parent")])
                                        match dzOb with
					| Saado(unid,dispNm,docInf,SaadoMasaloPbt<'t>) ->
						tibbie("Saado launcher")
                                        | Banarasi(unid,dispNm,docInf,BanarasiMasaloPbt<'t>) ->
						tibbie("Banarasi launcher")
                                        | BanarasiSub(unid,dispNm,docInf,BanarasiSubMasaloPbt<'t>) ->
						tibbie("BanarasiSub launcher")
                                        | Meethoo(unid,dispNm,docInf,MeethooMasaloPbt<'t>) ->
						tibbie("Meethoo launcher")
                                        | Calcutti(unid,dispNm,docInf,CalcuttiMasaloPbt<'t>) ->
						tibbie("Calcutti launcher")
                                        | CalcuttiCard(unid,dispNm,docInf,CalcuttiCardMasaloPbt<'t>) ->
						tibbie("CalcuttiCard launcher")
                                        | ComputedFld(unid,dispNm,docInf,CompFldMasaloPbt<'t>) ->
						tibbie("ComputedFld launcher")
                                        | LookupFld(unid,dispNm,docInf,LookupFldMasaloPbt<'t>) ->
						tibbie("LookupFld launcher")
                                        | AgentDef(unid,dispNm,docInf,AgentMasaloPbt<'t>) ->
						tibbie("AgentDef launcher")
                                        | ConditionExprDoc(unid,dispNm,docInf,CondExpMasaloPbt<'t>) ->
						tibbie("ConditionExprDoc launcher")
                                        | IfThenExprDoc(unid,dispNm,docInf,IfThenExpMasaloPbt<'t>) ->
						tibbie("IfThenExprDoc launcher")
                                        | ActionExprDoc(unid,dispNm,docInf,ActionExpMasaloPbt<'t>) ->
						tibbie("ActionExprDoc launcher")
                                        | LogDoc of(unid,dispNm,docInf,LogDocMasaloPbt<'t>) ->
						tibbie("LogDoc launcher")
                                    | _ -> ()))

            do printfn "in કલકતી_પાન preKatho6..."
            let katho() =
                printfn "in કલકતી_પાન katho..."
                own.Text <- "Brij (TM) Design DataView: Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved."
                g.ColumnCount <- visCols + 1
                g.Columns |> toLi |> lim (fun (c:DataGridViewColumn) -> c.ReadOnly <- true) |> ignore
                g.Rows.Clear()
                limi (fun i (x:list<_>) -> 
			//June10_25 Note: we may nd to locally store/iterate on i; chk
                        let categ:string = unbox (x.[g.getColNamed("Parent")])
			match (List.contains categ currOpenCategs) with
			| true -> 
	                        let ro = Array.ofList x.[0..visCols]
        	                g.Rows.Insert(i, ro)) dat |> ignore
			| _ -> ()
            do katho() //remmed for using datasource instd.
            let chuno =
                printfn "in કલકતી_પાન chuno..."
                ////do procDef df g; runK ...
                //@ToDo: Q: Where is this cfg saved? ini? dvDef?  
                //          Also nd other personalizations
#if pbtRem
                g.ColumnHeadersDefaultCellStyle.BackColor <- currentScheme.GridTit()
                g.ColumnHeadersDefaultCellStyle.ForeColor <- currentScheme.CategFore()
#endif //pbtRem
                g.ColumnHeadersDefaultCellStyle.Font <- (new Font(g.Font, FontStyle.Bold)) //ઓપ્ત_ઓર g.colHdrFont (new Font(g.Font, FontStyle.Bold))
                lim (fun (x:int) ->  (g.Columns.[x]).DefaultCellStyle.BackColor <- currentScheme.Back()
                                     (g.Columns.[x]).DefaultCellStyle.ForeColor <- currentScheme.Fore()) [0..visCols] |> ignore
                match ((colCellFont).IsSome) with
                | true ->   lim (fun x ->   let (i:int), fo = x
                                            if (i < visCols+1) then (g.Columns.[i]).DefaultCellStyle.Font <- fo
                                             else ()) (colCellFont).Value |> ignore
                | _ -> ()
                limi (fun i x -> if (i < visCols+1) then g.Columns.[i].Name <- x else ()) colHdrs |> ignore
            let tumbaaku =
#if pbtRem
                match dvTy with
                | DVType.Reg  ->
                    let pgBar = new ToolStrip(Dock = doc "B", LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow)
                    let gotoFst = new ToolStripButton(Text = "|<")
                    let gotoRR = new ToolStripButton(Text = "<<")
                    let gotoPrev = new ToolStripButton(Text = "<")
                    let gotoNxt = new ToolStripButton(Text = ">")
                    let gotoFF = new ToolStripButton(Text = ">>")
                    let gotoLst = new ToolStripButton(Text = ">|")
                    let pagLbl = new ToolStripLabel(Text = "Records " + recFrm.ToString() + " - " + recTo.ToString() + " of " + totRecs.ToString())
                    pgBar.Items.AddRange([|toTSItm gotoFst; toTSItm gotoRR; toTSItm gotoPrev; toTSItm pagLbl; toTSItm gotoNxt; toTSItm gotoFF; toTSItm gotoLst; toTSItm (getPgSzDropDn g)|])
                    !!^ ["gotoFst", box gotoFst; "gotoRR", box gotoRR; "gotoPrev", box gotoPrev; "gotoNxt", box gotoNxt; "gotoFF", box gotoFF; "gotoLst", box gotoLst; "pagLbl", box pagLbl] g
                    own.Controls.Add(pgBar)

                    //let regTBar = new ToolStrip(Dock = doc "T")
                    //Above updated Aug18_25 for dskTS support
                    let regTBar = own.getDskTS()
                    regTBar.Controls.Clear()

                    //@tbfo deferred for bldCtrl since we may not nd this drpDn anymore
                    //let swDV = new ToolStripDropDownButton(Text = "Switch DataView...", DropDown = (bldCtrl (getDropDnItms (getDVsForTbl "tbl")) None (new ToolStripDropDown())), DropDownDirection = ToolStripDropDownDirection.Left, ShowDropDownArrow = true)
                    regTBar.Items.Add (getTSButton "Add Row" "addImg.jpg" None (Some(new EventHandler (fun sender e -> 
                                                                                //g.Rows.Add() |> ignore 
                                                                                let st:Mtpl = (!!~ "wld" own).Value
                                                                                //tibbie ("ht: " + (g.Height).ToString() + "wd: " + (g.Width).ToString() + "t: " + (g.Top).ToString() + "vis: " + g.Visible.ToString() + "hdrHt: " + g.ColumnHeadersHeight.ToString() + " isvis: " + g.ColumnHeadersVisible.ToString())
                                                                                tibbie (    st.ToString())
                                                                                g.ColumnHeadersHeightSizeMode <- DataGridViewColumnHeadersHeightSizeMode.AutoSize
                                                                                   ))))
                    regTBar.Items.Add (getTSButton "Delete Row" "delImg.jpg" None (Some(new EventHandler (fun sender e -> 
                                            if (g.SelectedRows.Count > 0 && g.SelectedRows.[0].Index <> (g.Rows.Count - 1)) then
                                                g.Rows.RemoveAt(g.SelectedRows.[0].Index)))))
    //                regTBar.Items.Add (getTSButtonH own "Test tgSt" "delImg.jpg" (Some(new EventHandler (fun sender e -> 
                    regTBar.Items.Add (getTSButton "Test tgSt" "delImg.jpg" None (Some(new EventHandler (fun sender e -> 
                                            let thisCtrl = sender :?> ToolStripItem
                                            printfn "******** getting frm..."
                                            let frm = (getTopForm thisCtrl).Value
                                            printfn "******** frm is: %A\n getting state from frm..." frm
                                            let stVal:Mtpl = (!!~ "wld" frm).Value
                                            printfn "******** stVal is: %A " stVal
                                            Option.map (fun (m:obj) -> 
                                                            //let s = (m :?> (Mtpl * _)) |> fst
                                                            let s = m :?> Mtpl
                                                            tibbie (s.ToString())
                                                            //let interim = ((Mtpl.getUNID s) |> snd).ToString()
                                                            //printfn "******** interim is: %A" interim
                                                            //tibbie ("--id rec'd interim:" + interim)
                                                            tibbie "b4 st"
                                                            let updSt =
                                                                s |>
                                                                 ``⍒`` {
                                                                       tibbie "in st"
                                                                       let! f1 = getS
                                                                       printfn "--after 1st get ->: %A" f1
                                                                       let interim = (Mtpl.getUNID f1)
                                                                       tibbie ((interim |> snd).ToString())
                                                                       do! putS (interim |> fst)
                                                                       let! f2 = getS
                                                                       let interim2 = (Mtpl.getUNID f2)
                                                                       do! putS (interim2 |> fst)
                                                                       tibbie ((interim2 |> snd).ToString())
                                                                       } |> fst
                                                            !!^ ["wld", box updSt] frm
                                                            tibbie "after st"
                                                            ) (!!~ "wld" frm) |> ignore ))))
    //                regTBar.Items.Add (getTSButtonH own "Stat" "delImg.jpg" (Some(new EventHandler (fun sender e -> 
                    regTBar.Items.Add (getTSButton "Stat" "delImg.jpg" None (Some(new EventHandler (fun sender e -> 
                                            let img = getImg "expand.gif"
                                            let thisTSitm = sender :?> ToolStripItem
                                            let ts = thisTSitm.Owner
                                            ts.Items.Add(new ToolStripButton(img))
                                            tibbie ("stat tibbie " + (img.GetType().ToString()))
                                            ))))
    //                regTBar.Items.Add (getTSButtonH own "ફોરમ" "delImg.jpg" (Some(new EventHandler (fun sender e -> 
                    regTBar.Items.Add (getTSButton "ફોરમ" "delImg.jpg" None (Some(new EventHandler (fun sender e -> 
                                            tibbie "this call refers to tkFldList which has been @Depr."
                                            //let f = (ફોરમ_પાન(2, tkFldList, own, TaskTbl())) :> Form
                                            //f.Show()
                                            ))))
                    regTBar.Items.Add (getTSButton "ગપ્પા" "delImg.jpg" None (Some(new EventHandler (fun sender e -> 
                        ગપ્પા_પાન (SizeM,Some("લવલી:string option"),None , Some(box ("ક્વિમામ:option<_>")), None,own, txtDlg()) |> ignore ))))
                    regTBar.Items.Add (getTSButton "ટેબલ" "delImg.jpg" None (Some(new EventHandler (fun sender e -> 
                    tibbie "moved to new dsk mod"
                        //ટેબલ_પાન ("સુપારી","લવલી","ગુલકંદ", "ક્વિમામ", "પીચાક",own, AdminTbl()) |> ignore
                        ))))
                    regTBar.Items.Add (new ToolStripSeparator())
                    regTBar.Items.Add (getToolStripDVList)
                    //Aug18: no longer necc. own.Controls.Add(regTBar)
                | _ -> ()
#endif //pbtRem
                let stat = new StatusStrip(SizingGrip = false, Stretch = true, Dock = doc "B", Font = new Font("Tahoma", 18.0f), LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow)
                let statLbl = new ToolStripStatusLabel(Text = "Ready...") :> ToolStripItem
                let cLbl = new ToolStripStatusLabel(Text = "s", Dock = doc "R", Alignment = ToolStripItemAlignment.Right) :> ToolStripItem
#if pbtRem
                cLbl.Click.AddHandler(new EventHandler (fun sender e -> 
                                                            let thisCtrl = sender :?> ToolStripItem
                                                            let frm = (getTopForm thisCtrl).Value
                                                            let stVal:Mtpl = (!!~ "wld" frm).Value
                                                            (ગપ્પા_પાન (SizeM,Some("Current State Value"),None , Some(box (stVal.ToString())), None,own, txtDlg()))
                                                            let statL:list<string> = (Mtpl.GetOne "statLog" stVal).Value
                                                            (ગપ્પા_પાન (SizeM,Some("Current Stat Value"),None , Some(box (liToString statL)), None,own, txtDlg())) |> ignore))
#endif //pbtRem
                stat.Items.AddRange([|statLbl; cLbl|])
                own.Controls.Add(stat)
#if pbtRem
                !!^ ["statLbl", toOb statLbl] g
                !!^ ["grid", toOb g] own
#endif //pbtRem
                g.ColumnHeadersHeight <- g.ColumnHeadersHeight + 135
                own.Controls.Add(g)
#if !Dewey
                g.RowPostPaint.AddHandler(new DataGridViewRowPostPaintEventHandler( fun (sender:obj) (e:DataGridViewRowPostPaintEventArgs) ->
                    if (g.isCategRow(e.RowIndex)) then
                        let CategNm = (dat.[e.RowIndex].[g.getColNamed("Parent")]).ToString()
                        let (DVDef(nm, tblDef, docInf, colCellFont, colHdrs, visCols, fldLi, fixedSz, fltr, gBy, categBy, sortBy, openCategs, rowTips, Ttips, pOpts)) = def
                        let img:Image = 
                            match List.contains CategNm openCategs with
                            | true -> new Bitmap(@"C:\\Users\\inets\\Desktop\\mike\\src\\Data\\images\\collapse.png") :> Image
                            | _ -> new Bitmap(@"C:\\Users\\inets\\Desktop\\mike\\src\\Data\\images\\expand.png") :> Image
                        g.SuspendLayout()
                        let rowBounds = new Rectangle(g.RowHeadersWidth, e.RowBounds.Top, g.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) - g.HorizontalScrollingOffset + 1, e.RowBounds.Height)
                        let rBf = new RectangleF (single (g.DefaultCellStyle.Padding.Horizontal + 2), single (rowBounds.Y), single (rowBounds.Width - 2), single (rowBounds.Height - 2))
                        let topPadding = (rowBounds.Height - img.Height) / 2  //to ctr img in Row
                        let rBi = new Rectangle (g.DefaultCellStyle.Padding.Horizontal + 10, rowBounds.Y + topPadding, rowBounds.Width - 2, rowBounds.Height - 2)
                        let TxtRect = new RectangleF (rBf.X + ((single) (img.Width + 10)), rBf.Y, rBf.Width, rBf.Height)
#if pbtRem
                        e.Graphics.FillRectangle(new SolidBrush(currentScheme.CategBack()), rBf)
#else
                        e.Graphics.FillRectangle(new SolidBrush(Color.White), rBf)
#endif //pbtRem
                        let oldClip = e.Graphics.ClipBounds
                        e.Graphics.SetClip(rBf)
#if pbtRem
                        e.Graphics.FillRectangle(new SolidBrush(currentScheme.CategBack()), rBf)
#else
                        e.Graphics.FillRectangle(new SolidBrush(Color.White), rBf)
#endif //pbtRem
                        let imageAttributes = new ImageAttributes()
                        let width = img.Width
                        let height = img.Height
                        let colorMap = new ColorMap()
                        colorMap.OldColor <- Color.Black
                        colorMap.NewColor <- currentScheme.Icn()
                        let remapTable = [| colorMap |]
                        imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap)
                        let icnR = new Rectangle (rBi.Location, new Size(width, height))
                        e.Graphics.DrawImage(
                           img,
                           icnR,  // destination rectangle
                           0, 0,        // upper-left corner of source rectangle
                           width,       // width of source rectangle
                           height,      // height of source rectangle
                           GraphicsUnit.Pixel,
                           imageAttributes)
#if pbtRem
                        e.Graphics.DrawString(CategNm, g.Font, new SolidBrush(currentScheme.CategFore()), TxtRect)
#else
                        e.Graphics.DrawString(CategNm, g.Font, new SolidBrush(Color.Black), TxtRect)
#endif //pbtRem
                        e.Graphics.SetClip(oldClip)
                        g.ResumeLayout()
                    ))
#endif
                g.CellFormatting.AddHandler(new DataGridViewCellFormattingEventHandler( fun (sender:obj) (e:DataGridViewCellFormattingEventArgs) ->
                    let cell = g.Rows.[e.RowIndex].Cells.[e.ColumnIndex]
                    if rowTips then
#if pbtRem
                        let ttipColNo = colHdrs |> listFindItm "rowTips"
#else
                        let ttipColNo = 4
#endif //pbtRem
                        match ((e.RowIndex < dat.Length) && (e.ColumnIndex < (visCols))) with
                            | true ->
                                cell.ToolTipText <- (dat.[e.RowIndex].[ttipColNo] :?> string)
                            | _ -> () // "ttips: datLen exceeded..." coz it goes beyond
                                //NOTE: nd to update the bottom case too for exceeds
                        else 
                            let tt:list<string*list<string>> = Ttips.Value
                            let currColNm = colHdrs.[e.ColumnIndex]
                            tt |> List.tryFindIndex (fun elm -> 
                                                        let (colNm, ttipLi) = elm
                                                        colNm = currColNm)
                               |> Option.map (fun x -> 
                                                let (colNm, ttipLi) = tt.[x]
                                                cell.ToolTipText <- ttipLi.[e.RowIndex] ) |> ignore
                ))
            let setCols =
#if pbtRem
                g.ColumnHeadersDefaultCellStyle.BackColor <- currentScheme.CategBack()
                g.ColumnHeadersDefaultCellStyle.ForeColor <- currentScheme.CategFore()
#endif //pbtRem
                g.ColumnHeadersDefaultCellStyle.Font <- new Font(g.Font, FontStyle.Bold)
            let wrapup =
                own.WindowState <- FormWindowState.Maximized
                own.TopMost <- true
            do g.AutoResizeColumns()
            do g.AutoResizeColumnHeadersHeight()
            do printfn "કલકતી_પાન ob: Initialize + setup complete..."
            member g.upd(df, d) = 
                def <- df
                dat <- d
                katho()
            member g.getColNamed(nm) = (List.tryFindIndex (fun elm -> elm = nm) colHdrs).Value
            member g.isCategRow(i:int) = dat.[i].[(g.getColNamed("isCateg"))] :?> bool
            member g.rowLi() = g.Rows |> Seq.cast |> List.ofSeq
            member g.lovely(r) = g.isCategRow(r)
            member g.getFrm() = own

        type SaadoMasaloPbt<'t when 't :> ITblMarker> = | SaadoMasaloPbt of string * tblType:'t
        type BanarasiMasaloPbt<'t when 't :> ITblMarker> = | BanarasiMasaloPbt of string * tblType:'t
        type BanarasiSubMasaloPbt<'t when 't :> ITblMarker> = | BanarasiSubMasaloPbt of string * tblType:'t
        type MeethooMasaloPbt<'t when 't :> ITblMarker> = | MeethooMasaloPbt of string * tblType:'t
        type CalcuttiMasaloPbt<'t when 't :> ITblMarker> = | CalcuttiMasaloPbt of string * tblType:'t
        type CalcuttiCardMasaloPbt<'t when 't :> ITblMarker> = | CalcuttiCardMasaloPbt of string * tblType:'t
        type CompFldMasaloPbt<'t when 't :> ITblMarker> = | CompFldMasaloPbt of string * tblType:'t
        type LookupFldMasaloPbt<'t when 't :> ITblMarker> = | LookupFldMasaloPbt of string * tblType:'t
        type AgentMasaloPbt<'t when 't :> ITblMarker> = | AgentMasaloPbt of string * tblType:'t
        type CondExpMasaloPbt<'t when 't :> ITblMarker> = | CondExpMasaloPbt of string * tblType:'t
        type IfThenExpMasaloPbt<'t when 't :> ITblMarker> = | IfThenExpMasaloPbt of string * tblType:'t
        type ActionExpMasaloPbt<'t when 't :> ITblMarker> = | ActionExpMasaloPbt of string * tblType:'t
        type LogDocMasaloPbt<'t when 't :> ITblMarker> = | LogDocMasaloPbt of string * tblType:'t

        let obSaado = SaadoMasaloPbt("forPbt", AdminTbl())
        let obBanarasi = BanarasiMasaloPbt("forPbt", AdminTbl())
        let obBanarasiSub = BanarasiSubMasaloPbt("forPbt", AdminTbl())
        let obMeethoo = MeethooMasaloPbt("forPbt", AdminTbl())
        let obCalcutti = CalcuttiMasaloPbt("forPbt", AdminTbl())
        let obCalcuttiC = CalcuttiCardMasaloPbt("forPbt", AdminTbl())
        let obComp = CompFldMasaloPbt("forPbt", AdminTbl())
        let obLookup = LookupFldMasaloPbt("forPbt", AdminTbl())
        let obAgent = AgentMasaloPbt("forPbt", AdminTbl())
        let obCond = CondExpMasaloPbt("forPbt", AdminTbl())
        let obIfThen = IfThenExpMasaloPbt("forPbt", AdminTbl())
        let obAction = ActionExpMasaloPbt("forPbt", AdminTbl())
        let obLogDoc = LogDocMasaloPbt("forPbt", AdminTbl())

        //from UIAux type type DesDocAux updated for new Masalas
        //for updates the actual dzDoc has been removed (see earlier vers)
        type DesDoc<'t when 't :> ITblMarker> = | Saado of unid:DocUNID * dispNm:string * docInf:DesDocInf *  SaadoMasaloPbt<'t>
                                                | Banarasi of unid:DocUNID * dispNm:string * docInf:DesDocInf * BanarasiMasaloPbt<'t>
                                                | BanarasiSub of unid:DocUNID * dispNm:string * docInf:DesDocInf * BanarasiSubMasaloPbt<'t>
                                                | Meethoo of unid:DocUNID * dispNm:string * docInf:DesDocInf * MeethooMasaloPbt<'t>
                                                | Calcutti  of unid:DocUNID * dispNm:string * docInf:DesDocInf * CalcuttiMasaloPbt<'t>
                                                | CalcuttiCard  of unid:DocUNID * dispNm:string * docInf:DesDocInf * CalcuttiCardMasaloPbt<'t>
                                                | ComputedFld of unid:DocUNID * dispNm:string * docInf:DesDocInf * CompFldMasaloPbt<'t>
                                                | LookupFld of unid:DocUNID * dispNm:string * docInf:DesDocInf * LookupFldMasaloPbt<'t>
                                                | AgentDef of unid:DocUNID * dispNm:string * docInf:DesDocInf * AgentMasaloPbt<'t>
                                                | ConditionExprDoc of unid:DocUNID * dispNm:string * docInf:DesDocInf * CondExpMasaloPbt<'t>
                                                | IfThenExprDoc of unid:DocUNID * dispNm:string * docInf:DesDocInf * IfThenExpMasaloPbt<'t>
                                                | ActionExprDoc of unid:DocUNID * dispNm:string * docInf:DesDocInf * ActionExpMasaloPbt<'t>
                                                | LogDoc of unid:DocUNID * dispNm:string * docInf:DesDocInf * LogDocMasaloPbt<'t> with
            static member CategLi = [("Table Definition Document", 1) ;
            ("Form Definition Document", 1); ("SubForm Definition Document", 1); 
            ("DataView Definition Document", 1); ("Card DataView Definition Document", 1); 
            ("Computed Field Document", 1); ("Lookup Field Document", 1); 
            ("Other Design Documents", 1); 
            ("Agent Definition Document", 2); 
            ("Condition Expression Document", 2); 
            ("IfThen Expression Document", 2); 
            ("Action Expression Document", 2); 
            ("Log Documents", 2)]
            member this.genDefault = 
                fun (ty:'t) ->
                    [(Saado((getUNID "^સાદો_મસાલો"), "Table Definition Document", DesDocInfDeflt(), obSaado));
                     (Banarasi((getUNID "^બનારસી_મસાલો"), "MovieTbl Default Form", DesDocInfDeflt(), obBanarasi));
                     (BanarasiSub((getUNID "^બનારસી_મસાલો_Sub"), "SubForm Definition Document", DesDocInfDeflt(), obBanarasiSub));
                     (Meethoo((getUNID "^Meethoo"), "Table Definition Document", DesDocInfDeflt(), obMeethoo));
                     (Calcutti((getUNID "^DvDef"), "Default DataView Definition Doc", DesDocInfDeflt(), obCalcutti));
                     (CalcuttiCard((getUNID "^CardDvDef"), "Card DataView Definition Document", DesDocInfDeflt(), obCalcuttiC));
                     (ComputedFld((getUNID "^ComputedFld"), "Computed Field Definition Document", DesDocInfDeflt(),obComp));
                     (LookupFld((getUNID "^LookupFld"), "State Lookup Field Definition", DesDocInfDeflt(), obLookup));
                     (AgentDef((getUNID "^AgentDef"), "Gold to Platinum Status", DesDocInfDeflt(), obAgent));
                     (ConditionExprDoc((getUNID "^CondExprDoc"), "ClientWithGoldStatus", DesDocInfDeflt(), obCond));
                     (IfThenExprDoc((getUNID "^IfThenExprDoc"), "IfThen For Platinum", DesDocInfDeflt(), obIfThen));
                     (ActionExprDoc((getUNID "^ActionExprDoc"), "PlatinumAction", DesDocInfDeflt(), obAction));
                     (LogDoc((getUNID "^LogDoc"), "AdminTbl Monthly Usage Stats", DesDocInfDeflt(), obLogDoc))]
                    
(*
            member this.getDefault = 
                fun (docTy:DesDoc<'t>) (tblTy:'t) ->
                    match docTy with
                    //| Banarasi<_> ->
                    | Banarasi ->
                        let hardCodedFrmLi = peru (fun x -> BanarasiFld(x, 1,1, None, tblTy)) (tkFldList())
                        Banarasi((getUNID "^Banarasi"), "Form Definition Document for: " + (genArg this).ToString(), 2, hardCodedFrmLi, defFont, defFont, Color.Black, Color.White,DesDocInfDeflt())
                    | _ -> tibbie "DesDoc.getDefault unimplimented (hard-Coded 4 now)..."
*)
            static member ToSlug = 
                function | Saado _ -> "Table Definition Document" 
                            | Banarasi _ -> "Form Definition Document" 
                            | BanarasiSub _ -> "SubForm Definition Document" 
                            | Calcutti  _ -> "DataView Definition Document" 
                            | CalcuttiCard  _ -> "Card DataView Definition Document" 
                            | ComputedFld _-> "Computed Field Document" 
                            | LookupFld _-> "Lookup Field Document" 
                            | AgentDef _ -> "Agent Definition Document"
                            | ConditionExprDoc _ -> "Condition Expression Document"
                            | IfThenExprDoc _ -> "IfThen Expression Document"
                            | ActionExprDoc _ -> "Action Expression Document"
                            | LogDoc _ -> "Log Document"
            member this.getMod() = 
                //used 4 sorting in bldDat (WD)
                match this with
                    | Saado(_, _, DesDocInf(_, lastMod, _), _)  -> lastMod
                    | Banarasi(_, _, DesDocInf(_, lastMod, _), _)  -> lastMod
                    | BanarasiSub(_, _, DesDocInf(_, lastMod, _), _)  -> lastMod
                    | Calcutti (_, _, DesDocInf(_, lastMod, _), _)  -> lastMod
                    | CalcuttiCard (_, _, DesDocInf(_, lastMod, _), _)  -> lastMod
                    | ComputedFld(_, _, DesDocInf(_, lastMod, _), _)  -> lastMod
                    | LookupFld(_, _, DesDocInf(_, lastMod, _), _)  -> lastMod
                    | AgentDef(_, _, DesDocInf(_, lastMod, _), _)  -> lastMod
                    | ConditionExprDoc(_, _, DesDocInf(_, lastMod, _), _)  -> lastMod
                    | IfThenExprDoc(_, _, DesDocInf(_, lastMod, _), _)  -> lastMod
                    | ActionExprDoc(_, _, DesDocInf(_, lastMod, _), _)  -> lastMod
                    | LogDoc(_, _, DesDocInf(_, lastMod, _), _)  -> lastMod
            member this.ToRow = 
                fun par ->
                    let rnd() = (Random()).Next(30,150)
                    let getNewDt(d) = (d).AddDays(-(float rnd()))

                    match this with
                      | Saado(id, dNm, DesDocInf(_, lastMod, lastModBy), ty)  -> 
                            [box ""; box ""; box dNm; box (getNewDt(lastMod)); box lastModBy; box id; box ("Ttip for " + dNm);box false; box 0; box ty]
                      | Banarasi(id, dNm, DesDocInf(_, lastMod, lastModBy), ty) -> 
                            [box ""; box ""; box dNm; box (getNewDt(lastMod)); box lastModBy; box id; box ("Ttip for " + dNm);box false; box 0; box ty]
                      | BanarasiSub(id, dNm, DesDocInf(_, lastMod, lastModBy), ty) -> 
                            [box ""; box ""; box dNm; box (getNewDt(lastMod)); box lastModBy; box id; box ("Ttip for " + dNm);box false; box 0; box ty]
                      | Calcutti (id, dNm, DesDocInf(_, lastMod, lastModBy), ty) -> 
                            [box ""; box ""; box dNm; box (getNewDt(lastMod)); box lastModBy; box id; box ("Ttip for " + dNm);box false; box 0; box ty]
                      | CalcuttiCard (id, dNm, DesDocInf(_, lastMod, lastModBy), ty) -> 
                            [box ""; box ""; box dNm; box (getNewDt(lastMod)); box lastModBy; box id; box ("Ttip for " + dNm);box false; box 0; box ty]
                      | ComputedFld(id, dNm, DesDocInf(_, lastMod, lastModBy), ty) -> 
                            [box ""; box ""; box dNm; box (getNewDt(lastMod)); box lastModBy; box id; box ("Ttip for " + dNm);box false; box 0; box ty]
                      | LookupFld(id, dNm, DesDocInf(_, lastMod, lastModBy), ty) -> 
                            [box ""; box ""; box dNm; box (getNewDt(lastMod)); box lastModBy; box id; box ("Ttip for " + dNm);box false; box 0; box ty]
                      | AgentDef(id, dNm, DesDocInf(_, lastMod, lastModBy), ty) -> 
                            [box ""; box ""; box dNm; box (getNewDt(lastMod)); box lastModBy; box id; box ("Ttip for " + dNm);box false; box 0; box ty]
                      | ConditionExprDoc(id, dNm, DesDocInf(_, lastMod, lastModBy), ty) -> 
                            [box ""; box ""; box dNm; box (getNewDt(lastMod)); box lastModBy; box id; box ("Ttip for " + dNm);box false; box 0; box ty]
                      | IfThenExprDoc(id, dNm, DesDocInf(_, lastMod, lastModBy), ty) -> 
                            [box ""; box ""; box dNm; box (getNewDt(lastMod)); box lastModBy; box id; box ("Ttip for " + dNm);box false; box 0; box ty]
                      | ActionExprDoc(id, dNm, DesDocInf(_, lastMod, lastModBy), ty) -> 
                            [box ""; box ""; box dNm; box (getNewDt(lastMod)); box lastModBy; box id; box ("Ttip for " + dNm);box false; box 0; box ty]
                      | LogDoc(id, dNm, DesDocInf(_, lastMod, lastModBy), ty) -> 
                            [box ""; box ""; box dNm; box (getNewDt(lastMod)); box lastModBy; box id; box ("Ttip for " + dNm);box false; box 0; box ty]

        let WD =
            fun (dLi:DesDoc<_> list) ->
               DesDoc<_>.CategLi
                |> List.map (fun dzCaseTpl -> 
                               let (dzCase, cL) = dzCaseTpl
                               let itemColl = dLi |> List.filter (fun x -> DesDoc<_>.ToSlug(x) = dzCase) |> List.sortDescending(fun x -> x.getMod())
                               let ct = dzCase +  " (" +  (itemColl.Length).ToString() + 
                                   match itemColl.Length with
                                   | 1 -> "  item)"
                                   | _ -> "  items)"
                               let initCategBlanks = 
                                ([], [1..cL]) 
                                |> lifo (fun s v -> match ((lilen s)+1 = v) with
                                                    | false -> s @ box ""
                                                    | _ ->  s @ box ct)
                               let ctL = initCategBlanks @ [box ""; box ""; box ""; box ""; box ""; box true; box cL; box ct]
//let dzColHdrs = ["";"";"Document Title";"Last Updated";"By";"UNID"; "rowTips"; "isCateg"; "CatLvl"; "Parent"]
                               List.fold (fun s (d:DesDoc<_>) -> s @ ([d.ToRow(ct)])) [ctL] itemColl)  |> List.concat

        let WD_v2 =
            fun (dLi:DesDoc<_> list) ->
                //reflects categCols + shift from isCateg(bool) 2 -1,0,...
               DesDoc<_>.CategLi
                |> List.map (fun dzCaseTpl -> 
                               let (dzCase, cL) = dzCaseTpl
                               let itemColl = dLi |> List.filter (fun x -> DesDoc<_>.ToSlug(x) = dzCase) |> List.sortDescending(fun x -> x.getMod())
                               let ct = dzCase +  " (" +  (itemColl.Length).ToString() + 
                                   match itemColl.Length with
                                   | 1 -> "  item)"
                                   | _ -> "  items)"
                               let ctL = [box ""; box ""; box ""; box ""; box ""; box -1; box cL; box ct]
                               List.fold (fun s (d:DesDoc<_>) -> s @ ([d.ToRow(ct)])) [ctL] itemColl)  |> List.concat

        let HelloDz = (obLogDoc).genDefault(AdminTbl()) |> WD
        let dzColHdrs = ["";"";"Document Title";"Last Updated";"By";"UNID"; "rowTips"; "isCateg"; "CatLvl"; "Parent"]
#if pbtRem
        let WDV = CalcuttiMasalo("DesignDV", tbl, docInf, None, dzColHdrs, 4,  ["fld1";"fld2";"fld3";"fld4";"fld5";"fld6"],  None,   None, None, None, [], expandoState.XUserSel, true, None, Some(pagOpts))
#else
        let mockTbl = TblDef2("MovieTbl", CustID_Trivedi_MockarooTbl())
        let WDV = DVDef("DesignDV", mockTbl, docInf, None, dzColHdrs, 2,  ["fld1";"fld2";"fld3";"fld4";"fld5";"fld6"],  None,   None, None, None, None, [], true, None, Some(pagOpts))
#endif //pbtRem

        let run =
            let dFrm = new Form(Text = "DzDV")
            //let dzDv = કલકતી_પાન(Dz, dFrm, WDV, HelloDz)
            let dzDv = કલકતી_પાન_Nov12(dFrm, WDV, HelloDz)
            //let dzDv = કલકતી_પાન_Aux(Dz, dFrm, WDV, HelloDz)
            dzDv.AutoSizeColumnsMode <- DataGridViewAutoSizeColumnsMode.Fill
            (dzDv.Columns.[(dzDv.getColNamed("Document Title"))]).FillWeight  <- 200.0f
            (dFrm.Show())

(* Fable script:
open System
//gen UsrActivityLog + AgentLog dox 4 Mar-May25

let rnd() = (Random()).Next(30,150)
let getNewDt() = (DateTime.Now).AddDays(-(float (rnd()))).ToShortDateString()

let devNms = ["harish"; "mohandas"; "chloe"; "debra"; "ray"; "uma"]
let agNms = ["OrderFld Update";"CustLevel Gold";"CustLevel Silver";
"EmailOn10kTotal";"WeeklySummary";"CreditCardFailure";"PossibleFraud"]
let agResults = ["harish"; "mohandas"; "chloe"; "debra"; "ray"; "uma"]

[0..20]
|> List.map(fun x -> 
        let agNm = agNms.[((Random()).Next(0,agNms.Length))]
        let dev = devNms.[((Random()).Next(0,devNms.Length))] + "@trivedi.com"
        printfn "Agent '%A' signed by %A ran on %A" (agNm) (dev) (getNewDt()))
|> ignore
*)

module DzDv_Test = 
    open System
    open System.Drawing
    open System.Windows.Forms
    open Trivedi
    open Trivedi.Core


    printfn "...init module ExprBldr_Test"

    let chooseFrmLi_Idx xs = 
        gen{ 
              printfn "...in chooseFrmLi  %A" (List.length xs - 1)
              let! i = Gen.choose(0, List.length xs - 1)
              return (List.item i xs, i) }
          
    let openCatAddsDoxForEaCategTy = 
        fun fld ->

    let docDblClickLaunchesRelevEditor = 
	fun fld -> 
		//(need visualChks 4 this test)
