(*  Brij (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 
    TrivediBldHdr->
    |Trivedi SrcCtrlHdr File.  Copyright (c) 2015-2025 M. P. Trivedi.  All rights reserved.|637901455|test.fs|none|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|

    fsc src\pbt\dskCli_Desktop_AI.fs src\pbt\dskCli_Desktop.fs  --platform:x64 --standalone --target:exe --out:pbt\dsk.exe -r:lib\Trivedi.Core.dll -r:lib\Trivedi.CoreAux.dll -r:lib\Trivedi.UI.dll -r:lib\Trivedi.UIAux.dll -I:C:\Windows\Microsoft.NET\Framework\v4.0.30319

    Provenance: Modified from loggedUIRnrDec20_FrmDz
    to incorp modified  UI els 4 pbt
    
    This ver uses Trivedi.UI_Nov02_Color+Grids.dll + UIAux + ...

    Last updated: Fri Mar 28 2025

    Contains modules:  pbt_main
                                Desktop_Actual
                                Desktop_Ext
                                Desktop_Test
*)

namespace Trivedi

#nowarn "20" "25" "58" "66" "67" "64" "760" "1182" "1558"

(*

//Mar2025: These are the ones expanded for Form UI usage NOT the DocField stuff...
//src: Addenda Nov22a/z_loggedUI_webCliFrm (part of wobbly upd8s)
//These are imported locally from uiaux 4 ref
//@ToDo: Nov2024: These contain way too much junk, look @ the other one 
//(in this file) Trim them to essentially primitiveTys & use Thingy for the rest
    type FldType = | FldString
                   | FldNumber
                   | FldCurrency
                   | FldLongString
                   | FldAttachment
                   | FldBoolean
                   | FldChoiceList
                   | FldRadioBtn
                   | FldRange
                   | FldNumUpDn
                   | FldDate 
                   | FldDateTime
                   | FldColor
                   | FldFont
                   | FldInfoBox
                   | FldBlankRow
                   | UserInput
                   | FldBtn
                   | FldValidBtn
                   
//src: Brij.fs
type BrijTy<'t when 't :> ITblMarker> = | BrijTy of mods:Mod list * m:Mtpl * string * tblTy:'t with
    override this.ToString() = 
            let (BrijTy(mds, s, tpl, tblTy)) = this
            let (CoreMod(CoreM(DocUNID(unid), crDt, modDt, tit, cont, tags, flag))) = mds.[0]
            let tblT = (genArg this).ToString()
            "BrijTy of " + tblT + "|id:" + unid + "|title:" + tit + "|"
    member this.zipWithID() = 
            let (BrijTy(mds, s, tpl, tblTy)) = this
            let (CoreMod(CoreM(DocUNID(unid), _, _, _, _, _, _))) = mds.[0]
            (unid, mds)
#if tbdb
    static member bld (id:string) (tit:string option) (cont:string option) (tg:string option) = 
        BrijTy([CoreMod(CoreM(DocUNID(id), now(), now(), ઓર  tit "-", બાઇટ |>| ઓર cont "", ઓર tg "", ""))], "", CustID_Trivedi_AdminTbl())
    static member bldSpoo (id:string) (crDt:DateTime )(tit:string option) (cont:string option) (tg:string option) = 
        BrijTy([CoreMod(CoreM(DocUNID(id), crDt, now(), ઓર  tit "-", બાઇટ |>| ઓર cont "", ઓર tg "", ""))], "", CustID_Trivedi_AdminTbl())
#endif
    member this.contAsS() = 
        let (BrijTy(dt, s, tpl, tblTy)) = this
        let (CoreMod(CoreM(DocUNID(unid), crDt, modDt, tit, cont, tags, flag))) = dt.[0]
        bytes2Str cont
    member this.contAsB() =
        let (BrijTy(dt, s, tpl, tblTy)) = this
        let (CoreMod(CoreM(DocUNID(unid), crDt, modDt, tit, cont, tags, flag))) = dt.[0]
        cont
    member this.ToShortStr() =
        let (BrijTy(m, s, tpl, tblTy)) = this
        let (CoreMod(CoreM(DocUNID(unid), crDt, modDt, tit, cont, tags, flag))) = m.[0]
        "BrijTy of " + (genArg this).ToString() + "|id:" + unid + "|title:" + tit + "|"
    member this.getFldDefs() =
        [DocFld(FldString, "unid", true, "Document UNID");
        DocFld(FldDate, "crDt", true, "Created On");
        DocFld(FldDate, "modDt", true, "Last Modified On");
        DocFld(FldString, "title", true, "Title");
        DocFld(FldLongString, "cont", true, "Content");
        DocFld(FldString, "tags", true, "Tags");
        DocFld(FldBoolean, "flag", true, "Flag");
        DocFld(FldString, "optFld", true, "Optional Field")]
    member this.upd(m:Mtpl) =
//      NOTE: this shd pro'lly lie in an upper class (DatDoc?) since it's generic across ty.s
        let (Mtpl(l:list<(string * obj option * System.Type)>)) = m
        let rec updEa li = 
            List.map (fun x -> 
                    let (fldNm, val, ty) = x
                    match (List.tryFindIndex (fun def -> 
                                            let (Ty,intNm,isInt,extNm) = def
                                            intNm = fldNm) (this.getFldDefs()) ) with
                    | Some idx -> 
                        //fst verify types match; if not throw
                        updEa (List.updateAt idx (val :?> ty) li)
                    | _ -> raise SpxErr; []
                    ) li
        updEa l
        
    //src: wobblyDat
    let docF = tkFldList()
    let nm = "Test tkTable no isCat"
    let ty = (TaskTbl() :> ITblMarker)
    let bf = docF |> lim (fun f -> BanarasiFldAux(f, 1, 1, None, Thingy(""), None, ty))
    
    //src: loggedUI_Dec20
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
        
//src: Brij.fs
type DocFld = | DocFld of FldType*string*bool*string with
    override this.ToString() = 
        let (DocFld(fldTy, intNm, isInt, dispNm)) = this
        "DocFld: |intNm: " + intNm + "|isInt: " + isInt.ToString() + "|dispNm: " + dispNm + "|" + fldTy.ToString() + "|"
    member this.getBoxedGenericVal() =
        let (DocFld(fldTy, _, _, _)) = this
        match fldTy with
        | FldString -> box ""
        | FldNumber | FldRange -> box 0     //@ToDo Range params?
        | FldCurrency -> box (0.00)
        | FldLongString | FldAttachment -> box []
        | FldBoolean -> box false
        | FldDate -> box (now())        //these 2 nd to be diff
        | FldDateTime -> box (now())    //these 2 nd to be diff
        | FldColor -> box Color.White
        | FldFont -> box defFont
        | _ -> 
            tibbie ("DocFld.getBlankGenericVal() got an invalid fldType: " + fldTy.ToString())
            box ""
*)


[<AutoOpen>]
module pbt_main = 

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
    let liToString = 
        fun (l:list<_>) ->
           lifoi (fun acc i x -> acc + "\n" + (i.ToString() + " " +  x.ToString())) "" [0..(l.Length - 1)] l
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
    module Desktop_Actual = 
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

        //Altered on Mar_15_25
        //e.g.: longStrByteArr vs AttchByteArr.  Poss issues with singleCsDU Constr embedded.
        type FldType =  | FldString of string
                        | FldNumber of Int32
                        | FldFloat of float
                        | FldCurrency of Decimal //Currency Formatting: Use the :C fmtSpecifier in Console.WriteLine to display val as currency.
                        | FldLongString of htmlStr
                        | FldAttachment of byte[]
                        | FldBoolean of boolean
                        | FldChoiceList of string[]
                        | FldRange of (int * int) //(min:int * max:int)
                        | FldDate of Date
                        | FldDateTime of DateTime
                        | FldTime of Time
                        | FldColor of Color

        type DocFld = | DocFld of FldType*string*bool*string with
            override this.ToString() = 
                let (DocFld(fldTy, intNm, isInt, dispNm)) = this
                "DocFld: |intNm: " + intNm + "|isInt: " + isInt.ToString() + "|dispNm: " + dispNm + "|" + fldTy.ToString() + "|"
            member this.getBoxedGenericVal() =
                let (DocFld(fldTy, _, _, _)) = this
                match fldTy with
                | FldString -> box ""
                | FldNumber | FldRange -> box 0     //@ToDo Range params?
                | FldCurrency -> box (0.00)
                | FldLongString | FldAttachment -> box []
                | FldBoolean -> box false
                | FldDate -> box (now())        //these 2 nd to be diff
                | FldDateTime -> box (now())    //these 2 nd to be diff
                | FldColor -> box Color.White
                | FldFont -> box defFont
                | _ -> 
                    tibbie ("DocFld.getBlankGenericVal() got an invalid fldType: " + fldTy.ToString())
                    box ""


        //Mar 6th '25: These two nd to be completed/impl
    #if tbdb
        type icnPnl<'t> (icnNm, tblT:'t, slug, dsk) as icnP =
            inherit Panel(Margin = new Padding(25), BorderStyle = BorderStyle.None)
            let init =
                let img:Image = Image.FromFile(Path.Combine("C:\\Users\\inets\\Documents\\mike\\src\\Data\images\\desktop", (icnNm:string).ToLower().Trim()))
                let icnLbl = new Label(Margin = new Padding(5), Dock = doc "T", Image = img, ImageAlign= ContentAlignment.TopCenter)
                let txtLbl = new Label(Margin = new Padding(5), Dock = doc "B", Text = slug, TextAlign = ContentAlignment.BottomCenter)
                let g = (icnLbl).CreateGraphics()
                let ht = ((g.MeasureString("test", defFont)).ToSize()).Height
                icnLbl.Height <- ht
                txtLbl.Height <- ht
                icnP.Height <- (ht * 2)
                icnLbl.Width <- icnLbl.PreferredWidth //((g.MeasureString(slug, defFont)).ToSize()).Width
                let icnCtxt:ContextMenuStrip = (!!~ "icnCtxtMS" dsk).Value
                icnLbl.ContextMenuStrip <- icnCtxt
                txtLbl.ContextMenuStrip <- icnCtxt
                icnP.ContextMenuStrip <- icnCtxt
                let openHnd = new EventHandler(fun (sender:obj) (e:EventArgs) ->
                                                tibbie ("icn openDV for dvID -> " + (icnP.GetType().ToString())))
                icnLbl.DoubleClick.AddHandler(openHnd)
                txtLbl.DoubleClick.AddHandler(openHnd)
                icnP.DoubleClick.AddHandler(openHnd)
                icnP.Controls.Add(icnLbl)
                icnP.Controls.Add(txtLbl)
    #endif //tbdb


    #if hardCoded_openWin_Calls
    new EventHandler(fun (sender:obj) (e:EventArgs) ->
            match slug with
            | "MusicDVNov" -> 
                //let musicDVNov = Dકલકતી_મસાલો ("MusicDV", tbl, docInf, None, musicColHdrs, 4,  ["fld1";"fld2";"fld3";"fld4";"fld5";"fld6"],  None,  None, None, None, [], true, None, Some(pagOpts))
                let mFrm = new Form(Text = "musicDVNov")
                //let musicF = કલકતી_પાન_Nov(Reg, mFrm, Grid2.musicDV, musicDat_Nov8)
                //mFrm.Show()
                tibbie "remmed Feb_28_25"
            | "MusicDVAux" -> 
                let musicColHdrsInterned = ["Release Date";"Track";"Title";"Artist"; "Album";"rowTips";"isCateg"; "Parent"]
                (*let dvD = CalcuttiMasaloAux("Default MusicDV", mDef, DesDocInfDeflt(), None, musicColHdrsInterned, 5, musicColHdrsInterned, None, None, None, None, [], XAll, true, None, Some(pagOptsAux))
                //let mDv = કલકતી_પાન_Aux(Dt, dsk, dvD, musicDat_Nov8, "procId", "winTy")
                (mDv.getFrm()).Show()
                *)
                tibbie "remmed Feb_28_25"
            | "TaskDVNov" -> 
                let mFrm = new Form(Text = "tkDV")
                //let tkDt = deSerBA (File.ReadAllBytes("baseTkDatAux.bdf")) :?> list<list<_>>
                //let docInf = Grid2.DesDocInf(DateTime.Now, DateTime.Now, "mike@trivedi.com")
                //let pagOpts = Grid2.DVPaginationOpts(200,1,2000, 200, 400, 800000)
                //let tDV = Grid2.DVDef("tkDV", Grid2.TblDef2("task Table", TaskTbl()), docInf, None, tkColHdrs, 6,  tkFldLiLocalAux,  None,  None, None, None, [], true, None, Some(pagOpts))
                //let musicF = કલકતી_પાન_Nov(Reg, mFrm, tDV, tkDt)
                //mFrm.Show()
                tibbie "remmed Feb_28_25"
            | "TaskDVAux" -> 
                    (*let tplFLi = tkFldLiLocalAux
                    let tkDt = baseTkDatAux tDef tkFldLiLocalAux
                    //let tDefLocal = SaadoMasaloAux("Task Tbl", tplFLi, brijLogo, TaskTbl())
                    //error FS0030: Value restriction. The value 'tkCalcuttiMasalo' has been inferred to have generic type
                    //val tkCalcuttiMasalo : main.CalcuttiMasalo<'_a,'_b,'_c> when '_c :> ITblMarker
                    //let tkCalcuttiMasalo:CalcuttiMasalo<_,_,_> = deSerBA (File.ReadAllBytes("tkCalcTest1.bdf")) :?> CalcuttiMasalo<_,_,_>
                    let dvD = CalcuttiMasaloAux("Default tkDV", tDef, DesDocInfDeflt(), None, tplFLi, 6, tplFLi, None, None, None, None, [], XAll, true, None, Some(pagOptsAux))
                    //let tDv = કલકતી_પાન_Aux(Dt, d, dvD, tkDt, "procId", "winTy")
                    //(tDv.getFrm()).Show()
                    *)
                    tibbie "remmed Feb_28_25"
            | "DesignDV" -> 
                tibbie "#remmed_mbi_Dec09"
    #if remmed_mbi_Dec09
                let dFrm = new Form(Text = "DzDV")
                let dzDv = કલકતી_પાન_Nov(Dz, dFrm, WDV, HelloDz)
                dzDv.AutoSizeColumnsMode <- DataGridViewAutoSizeColumnsMode.Fill
                (dzDv.Columns.[(dzDv.getColNamed("Document Title"))]).FillWeight  <- 200.0f
                (dFrm.Show())
    #endif //remmed_mbi_Dec09
            | "FrmDesigner" -> 
            (*
                let flds = Task.getFldDefs()
                //let frmDef = FrmDef((getUNID "^FrmDef"), "AliceBlue Definition Document for: " + (tblNm (Task.getFldDefs())), 2, flds, DesDocInfDeflt(), defFont, defFont, Color.Black, Color.White, TaskTbl())
                //let frm = (બનારસી_પાન(frmDef, dsk)) :> Form
                //frm.Show()
                *)
                tibbie "mbi"
            | "FrmSetup" -> 
                //ગપ્પા_પાન (SizeM,Some("Form Configuration Settings"),None , Some(box (Task.getFldDefs())), None, dsk, frmSetupDlg())  |> ignore
                tibbie "remmed Feb_28_25"
            | "PredTester" -> 
                tibbie "cant' find ty critDlg()"
                //ગપ્પા_પાન (SizeM,Some("Pred Tester"),None , (Some(box (Task.getFldDefs()))), None, dsk, critDlg()) |> ignore
            | "BlueForm" -> 
                //let frmDef = deSerBA (File.ReadAllBytes("AliceBlue.frm")) //:?> DesDoc<_> //FrmDef<Trivedi.TaskTbl> //DesDoc<_>
                //...mbi splat...
                //Unable to cast object of type 'FrmDef[Trivedi.TaskTbl]' to type 'DesDoc`1[Trivedi.ITblMarker]'.
                //tibbie ("type found: " + frmDef.GetType().ToString())
                //let frm = (બનારસી_પાન(frmDef, d)) :> Form
                //frm.Show()
                tibbie "remmed Feb_28_25"
            | "tkPicks" -> 
                (*
                let w:Mtpl = (!!~ "wld" dsk).Value
                let tkP:Mtpl = (Mtpl.GetOne "tkPicks" w).Value
                tibbie ("tkP ty: " + (tkP.GetType().ToString()))
                tibbie (tkP.ToString())
                let tit:string = (Mtpl.GetOne "title" tkP).Value
                tibbie tit
                *)
                tibbie "remmed Feb_28_25"
            | "getImg" -> 
            (*
                let s:Mtpl = (!!~ "wld" dsk).Value
                let dat = (Mtpl.GetOne "DatLi" s).Value
                tibbie ("got dat ty: " + (dat.GetType().ToString()))
                let itm = dat |> List.pick (fun itm -> 
                                    tibbie "pick1"
                                    let (BaseBrijType_NoTpl(dt, s, tblTy)) = itm
                                    let (CoreMod(CoreM(DocUNID(unid), crDt, modDt, tit, cont, tags, flag))) = dt.[0]
                                    tibbie "pick2"
                                    if tit = "sort.png" then Some s else None)
                tibbie ("res was " + itm.GetType().ToString())
                *)
                tibbie "remmed Feb_28_25"
            | "State" -> 
                (*
                let stVal:Mtpl = (!!~ "wld" dsk).Value
                (ગપ્પા_પાન (SizeM,Some("Current State Value"),None , Some(box (stVal.ToString())), None, dsk, txtDlg()))
                let statL:list<string> = (Mtpl.GetOne "statLog" stVal).Value
                (ગપ્પા_પાન (SizeM,Some("Stat log li:"),None , Some(box (statL)), None, dsk, listDlg()))
                let stVal2:Mtpl = (!!~ "wld" dsk).Value
                tibbie (((Mtpl.GetOne "dlgRes" stVal2).Value).ToString())
                *)
                tibbie "remmed Feb_28_25"
            | _ -> tibbie ("icn openDV for dvID -> " + tblID))
    #endif //hardCoded_openWin_Calls

        type Dsk(icnLi:(string * obj * string) list) as dsk =
            inherit Form(Visible = false, TopMost = true, WindowState = FormWindowState.Maximized, Font = defFont, AutoScroll = true, BackColor = currentScheme.Back())
            do printfn "db: Dsk cTor"
            let mutable openWins = []
            let mutable DskStatus = ["Desktop Ready..."]
            let pnl = new FlowLayoutPanel(BorderStyle = BorderStyle.Fixed3D, FlowDirection = FlowDirection.BottomUp, WrapContents = true, Dock = doc "F", BackColor = currentScheme.Back())
            let katho = 
                printfn "db: Dsk katho"
                pnl.SuspendLayout()
                dsk.SuspendLayout()
                let stat = new StatusStrip(SizingGrip = false, Stretch = true, Dock = doc "B", Font = new Font("Tahoma", 18.0f), LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow)
                let statLbl = new ToolStripStatusLabel(Text = "Desktop Ready...", AutoToolTip = false) :> ToolStripItem
                let dskCtxtMS = new ContextMenuStrip()
                let AddIconMenuItem = new ToolStripMenuItem("Add Item")
                let RemoveIconMenuItem = new ToolStripMenuItem("Remove Item(s)")
                let tmpMenuItm = new ToolStripMenuItem("tmp")
                !!^ ["Frm", box dsk ] dskCtxtMS

                tmpMenuItm.Click.AddHandler(new EventHandler(fun (sender:obj) (e:EventArgs) ->
                            let ch:int = pnl.Controls |> Seq.cast |> List.ofSeq |> lilen
                            tibbie ("# of icns -> " + ch.ToString())))
                dskCtxtMS.Items.AddRange([|toTSItm tmpMenuItm;toTSItm AddIconMenuItem;toTSItm RemoveIconMenuItem|])

                let cLbl = new ToolStripStatusLabel(Text = "s", Dock = doc "R", Alignment = ToolStripItemAlignment.Right, AutoToolTip = false) :> ToolStripItem
                cLbl.Click.AddHandler(new EventHandler (fun sender e -> 
                                                            let thisCtrl = sender :?> ToolStripItem
                                                            let frm = (getTopForm thisCtrl).Value :?> Form
                                                            (ગપ્પા_પાન (SizeM,Some("Current State Value"),None , Some(box (stRef.ToString())), None,frm, txtDlg()))
                                                            let statL:list<string> = (Mtpl.GetOne "statLog" stRef).Value
                                                            (ગપ્પા_પાન (SizeM,Some("Stat log li:"),None , Some(box (liToString statL)), None, frm, listDlg()))
                                                            match (Mtpl.Has "errLog" stRef) with
                                                            | true -> 
                                                                let errL:list<string> = (Mtpl.GetOne "errLog" stRef).Value
                                                                (ગપ્પા_પાન (SizeM,Some("Err li:"),None , Some(box (liToString errL)), None, frm, listDlg())) |> ignore
                                                            | _ -> tibbie "no errLog present!"
                                                            tibbie ("dlgRes: " + ((Mtpl.GetOne "dlgRes" stRef).Value).ToString())
                                                            ))
                stat.Items.AddRange([|statLbl; cLbl|]) |> ignore


                let ms = new MenuStrip(Dock = DockStyle.Top,Font = defFont)
                let filMenu = new ToolStripMenuItem("File")
                let winMenu = new ToolStripMenuItem("Window")

                let fileExit = new ToolStripMenuItem("E&xit", null, 
                    new EventHandler(fun (o:obj) (e:EventArgs) ->
                                    System.Exit(0)
                                        ), Keys.Control ||| Keys.X)
                filMenu.DropDownItems.Add(fileExit) |> ignore

                ms.Items.Add(filMenu) |> ignore
                ms.Items.Add(winMenu) |> ignore
                frm.MainMenuStrip <- ms

                dsk.Controls.Add(ms)
                dsk.Controls.Add(pnl)
                dsk.Controls.Add(stat)
                dsk.ContextMenuStrip <- dskCtxtMS
                !!^ ["Status", box stat; "dskCtxtMS", box dskCtxtMS; "ms", box ms; "flowPnl", box pnl] dsk
            let chuno =
                printfn "db: Dsk chuno"
                //stopgap manual pop
                let userNm = "Env.getUser()"
                let pnl:FlowLayoutPanel = (!!~ "flowPnl" dsk).Value
                //let (Dsk(userNm, icnLi)) = //(getDesktopFile uNm)
                do printfn "db: Dsk begin proc icnLi"
                lim (fun icnItm -> 
                        let (icnNm, ty, slug) = icnItm
                        printfn "...nm: %A slug: %A ..." icnNm  slug
                        let tblT = unbox ty
                        let icnP = Panel(Margin = new Padding(25), BorderStyle = BorderStyle.None)
                        let img:Image = Image.FromFile(Path.Combine("C:\\Users\\inets\\Documents\\mike\\src\\Data\images\\desktop", (icnNm:string).ToLower().Trim()))
                        let icnLbl = new Label(Dock = doc "T", Image = img, ImageAlign= ContentAlignment.TopCenter, BackColor = currentScheme.Back())
                        let txtLbl = new Label(Dock = doc "B", Text = slug, TextAlign = ContentAlignment.BottomCenter, ForeColor = currentScheme.accentFore(), BackColor = currentScheme.Back())
                        icnLbl.Paint.Add(fun (e:PaintEventArgs)  ->
                            //base.OnPaint(e)
                            let imageAttributes = new ImageAttributes()
                            let width = img.Width
                            let height = img.Height
                            let colorMap = new ColorMap()
                            colorMap.OldColor <- Color.Black
                            colorMap.NewColor <- currentScheme.Icn()
                            let remapTable = [| colorMap |]
                            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap)
                            let icnR_First = new Rectangle (new Point(icnLbl.Location.X + (icnP.Padding.Horizontal), (icnLbl.Location.Y + (icnP.Padding.Top))), new Size(width, height))
                            //@ToDo fix this, tmpFix using hard-coded val
                            let icnR = new Rectangle (icnR_First.X + 76, icnR_First.Y + 2, icnR_First.Width, icnR_First.Height)
                            e.Graphics.DrawImage(
                            img,
                            icnR,  // destination rectangle
                            0, 0,        // upper-left corner of source rectangle
                            width,       // width of source rectangle
                            height,      // height of source rectangle
                            GraphicsUnit.Pixel,
                            imageAttributes)
                        )
                        let g = (icnLbl).CreateGraphics()
                        let ht = ((g.MeasureString("test", defFont)).ToSize()).Height
                        let wd = ((g.MeasureString(slug, defFont)).ToSize()).Width
                        icnLbl.Height <- ht
                        txtLbl.Height <- ht
                        icnP.Height <- (ht * 2)
                        icnLbl.Width <- wd
                        let icnCtxt = new ContextMenuStrip()
                        icnLbl.ContextMenuStrip <- icnCtxt
                        txtLbl.ContextMenuStrip <- icnCtxt
                        icnP.ContextMenuStrip <- icnCtxt
                        let tblID = (tblT.GetType().ToString()).Substring((tblT.GetType().ToString()).IndexOf(".")+1)
                        !!^ ["tblID", box tblID] icnP
                        !!^ [tblTy, box icnP] dsk
                        let dskOpenHnd = 
                            new EventHandler(fun (sender:obj) (e:EventArgs) ->
                                match (dsk.isOpen(wT did)) with
                                | Some w -> dsk.switchToChild w
                                | _ -> 
                                    //FIRST add & then launch w pId ***@TBFO***
                                    let winH = brijWin((getUNID.ToString() + "^pId"), tblID, docId)
                                    openWins <- (winH :: openWins)
                                    match s with
                                    | "DataView" -> tibbie ("icn cmd " + s + " for dvID -> " + id + crlf + "launch tbfo")
                                    | "DesignView" -> openDes id
                                    | "Form" -> () //see above (#)  for hardcoded examples...
                                    | "FormDesign" -> ()
                                    | "TableDesign" -> ())
                        icnLbl.DoubleClick.AddHandler(dskOpenHnd)
                        txtLbl.DoubleClick.AddHandler(dskOpenHnd)
                        icnP.DoubleClick.AddHandler(dskOpenHnd)
                        let DesignDVMenuItem = new ToolStripMenuItem("Open in Design Mode")
                        icnCtxt.Items.Add(DesignDVMenuItem) |> ignore
                        DesignDVMenuItem.Click.AddHandler(new EventHandler(fun (sender:obj) (e:EventArgs) ->
                                    //openDes (tblT.GetType()) 
                                    tibbie "remmed Feb_28_25"))
                        icnP.Controls.Add(icnLbl)
                        icnP.Controls.Add(txtLbl)
                        pnl.Controls.Add(icnP)
                    ) icnLi |> ignore 
                do printfn "db: ટેબલ_પાન_Nov done proc icnLi"
                pnl.ResumeLayout(false)
                dsk.ResumeLayout(false)
                snapshot dsk
                //dsk.Show()
            member dsk.getWinTitle wTy tblNm i =
		//@ToDo: cover all cases...
                match (docId isDz) with
                | true -> "&" + i.ToString() + " DesignView: " + wTy.ToString()
                | _ -> "&" + i.ToString() + " DataView: " + wTy.ToString()
            member dsk.isOpen(wT did) = 
                openWins
                |> List.tryFindIndex(fun itm -> 
                                        let (brijWin(_, wTy, docId)) = itm
                                        (wTy = wT) && (docId = did))
            member dsk.activateWin n = 
		let thisIdx = 
			winMenu.DropDownItems 
	                |> seq.ToList
	                |> List.TryFindIndex (fun w -> 
						let idxSlg = (b4 "." w).Trim()
					   	idxSlg = n)
		match thisIdx with
		| Some w -> 
			//setChecked
			((dsk.MdiChildren).[w]).Activate()
		| _ -> ()
	    member dsk.rebuildWins() = 
		//@ToDo: 1st clear the dropdown (or they'll be doubled)
                openWins
                |> peru (fun i itm x y -> 
                            let (brijWin(_, wTy, docId)) = itm
                            let slg = getWinTitle wTy i
                            new ToolStripMenuItem(slg, null, 
                                new EventHandler(fun (o:obj) (e:EventArgs) -> switchToChild i), Keys.Control ||| Keys.i)  
                                |> winMenu.DropDownItems.Add) |> ignore
            member dsk.addWin wTy tblNm = 
		//To create an MDI child form, assign the Form that will be the MDI parent form 
		//to the MdiParent property of the child form.  New win alw appended @ end of openWins
                let nxtHdl = lilen openWins + 1
		let newWinHdl = getWinTitle wTy tblNm nxtHdl
                new ToolStripMenuItem(slg, null, 
                   new EventHandler(fun (o:obj) (e:EventArgs) -> switchToChild i), Keys.Control ||| Keys.i)  
                |> winMenu.DropDownItems.Add) |> ignore
		openWins <- (openWins @ [newWinHdl])
		dsk.rebuildWins()
            member dsk.remWin n = 
		let thisIdx = 
			openWins
	                |> List.TryFindIndex (fun w -> 
						let idxSlg = (b4 "." w).Trim()
					   	idxSlg = n)
		match thisIdx with
		| Some w -> openWins.removeAt thisIdx	//@Chk: sufficient or do we nd 2 reassign mdiParent here? Dispose?
		| _ -> ()
		dsk.rebuildWins()

	    let mutable DskStatus = ["Desktop Ready..."]
	    member dsk.updStatus m = 
		let addMsg ms = 
		  statLbl.Text = ms
		  DskStatus :: ms
		match m with
		| :?> string as s -> addMsg s
		| :?> string list as l -> lim (fun m -> addMsg m) l




    [<AutoOpen>]
    module Desktop_Ext = 

    [<AutoOpen>]
    module Desktop_Test = 

        printfn "Desktop_Test: now loading customizations..."
        
        type ITblMarker = interface end
        type ArticleTbl() = interface ITblMarker
        type AdminTbl() = interface ITblMarker
        type TaskTbl() = interface ITblMarker
        
        
        let baseIconsLi = [("articles.png", box (ArticleTbl()) ,"Article Tbl")]
        
        let _baseIconsLi = [("articles.png", box (ArticleTbl()) ,"Article Tbl");
                      ("ide.png", box (ArticleTbl()) ,"Ide");
                      ("settings.png", box (AdminTbl()) ,"Settings");
                      ("tasklist.png", box (TaskTbl()) ,"TaskList Tbl");
                      ("new.png", box (ArticleTbl()) ,"Add New Icon...");
                      ("music.png", box (ArticleTbl()) ,"MusicTbl")]
        
        let additionalIconsLi = 
          [("database.png", box(ArticleTbl()), "Amazon DynamoDB");("database.png", box(ArticleTbl()), "Microsoft SQL Server");("database.png", box(ArticleTbl()), "MySQL");("database.png", box(ArticleTbl()), "PostgreSQL");("database.png", box(ArticleTbl()), "MongoDB");("database.png", box(ArticleTbl()), "Oracle Database");("database.png", box(ArticleTbl()), "SQLite");("database.png", box(ArticleTbl()), "Cassandra");("database.png", box(ArticleTbl()), "Firebase Realtime Database");("database.png", box(ArticleTbl()), "MariaDB");("database.png", box(ArticleTbl()), "Amazon Redshift");("database.png", box(ArticleTbl()), "Couchbase")]
        
        let philos = 
          [ "Aristotle"; "Socrates"; "Epictetus"; "Seneca"; "Hegel"; "Russell"; "Wittgenstein"; "Rufus"; "Aurelius" ]
        
        let chooseFrmLi xs = 
          gen{ 
              printfn "...in chooseFrmLi  %A" (List.length xs - 1)
              let! i = Gen.choose(0, List.length xs - 1)
              return List.item i xs }
        
        let addl = additionalIconsLi
        
        type DskWrapper(initial:list<string*obj*string>) as d =
            let mutable currState = initial
            member __.AddIcn(itm) = 
                //Console.ForegroundColor <- ConsoleColor.Yellow
                //printfn "AddIcn(%A) actualState b4: %A" r currState
                currState <- currState @ ([itm])
                //printfn "AddIcn(%A) actualState after: %A" r currState
                //Console.ResetColor()
                //hr()
                d
            member __.RemIcn(itm) = 
                //Console.ForegroundColor <- ConsoleColor.Green
                //printfn "RemIcn(%A) actualState b4:\n %A" r currState
                let ll = List.length currState
                match ll <= 0 with
                | true -> failwithf "Precondition fail"
                | _ ->
                currState <- List.except [itm] currState
                //printfn "RemIcn(%A) actualState after:\n%A" r currState
                //Console.ResetColor()
                //hr()
                d
            member __.ChangeLbl(n, p) = 
              let itm = currState.[n]
              let (str, o, str2) = itm
              let u = (str, o, p)
              currState <- List.updateAt n u currState
            member __.OpenWin(mode, tblTy) = 
              let (_, oWins) = currState
              //Note: Actual has the reqd handlers; so for this test,
              //      we're adding the Model stubs instd.
            
            member __.Reset() = 
              currState <- baseIconsLi
              d
            member __.toModel() = 
              currState
            override __.ToString() = "DskWrapper = ...%i n"
            
        let addIcn itm = 
            { new Operation<DskWrapper,list<string*obj*string>>() with
                member __.Run m = 
                    //Console.ForegroundColor <- ConsoleColor.Cyan
                    //printfn "addIcn() modelState b4:\n %A" m
                    let res = m @ ([itm])
                    //printfn "addIcn() modelState after:\n %A" res
                    //Console.ResetColor()
                    //hr()
                    res
                member __.Check (d,m) = 
                    let res = (d.AddIcn(itm)).toModel()
                    //(????_???_Nov<'a>()).dskSnap()
                    m = res
                    |> Prop.label (sprintf "AddIcn: model = %A, actual = %A" m res)
                override __.ToString() = "addIcn"}
        
        let remIcn (itm:string*obj*string) = 
            { new Operation<DskWrapper,list<string*obj*string>>() with
                member __.Run m = 
                    //Console.ForegroundColor <- ConsoleColor.Red
                    //printfn "remIcn() modelState b4:\n %A"  m
                    let res = m |> List.except (Seq.singleton itm) 
                    //printfn "remIcn() modelState after:\n %A"  res
                    //Console.ResetColor()
                    //hr()
                    res
                override __.Pre m = 
                    List.length m > 1
                member __.Check (d,m) = 
                    let res = (d.RemIcn(itm)).toModel()
                    //(????_???_Nov<'a>()).dskSnap()
                    m = res 
                    |> Prop.label (sprintf "RemIcn: model = %A, actual = %A" m res)
                override __.ToString() = "remIcn"}
        
        let openWin w = 
            { new Operation<DskWrapper,(list<string*obj*string>, list<bool*int*dvTy*tblTy>)>() with
                member __.Run m = 
                    match (d.isOpen dvTy tblTy) with
                    | true -> d.activateWin n
                    | _ -> d.addWin dvTy tblTy //rebuilds model
                    d.toModel()
                member __.Check (d,m) = 
                    let res = (d.openWin(w)).toModel()
                    m = res 
                    |> Prop.label (sprintf "openWin: model = %A, actual = %A" m res)
                override __.ToString() = "openWin"}

        let closeWin w = 
            { new Operation<DskWrapper,(list<string*obj*string>, list<bool*int*dvTy*tblTy>)>() with
                member __.Run m = List.except w m
                override __.Pre m = List.contains w m
                member __.Check (d,m) = 
                    let res = (d.remWin(w)).toModel()
                    m = res 
                    |> Prop.label (sprintf "closeWin: model = %A, actual = %A" m res)
                override __.ToString() = "closeWin"}

        let switchToWin w = 
            { new Operation<DskWrapper,(list<string*obj*string>, list<bool*int*dvTy*tblTy>)>() with
                member __.Run m = 
                    let (_, oWin) = m
                    oWin
                    |> lim (fun x -> 
                                let (chk, num, tit, dvT, tbT) = x
                                let (switchTit) = w
                                match (switchTit = tit) with
                                | true -> true, num, dvT, tbT
                                | _ -> false, num, dvT, tbT)
                override __.Pre m = List.contains w m
                member __.Check (d,m) = 
                    let res = (d.activateWin n).toModel()
                    m = res 
                    |> Prop.label (sprintf "switchToWin: model = %A, actual = %A" m res)
                override __.ToString() = "switchToWin"}
        
        let create initialValue = 
            { new Setup<DskWrapper,list<string*obj*string>>() with
                member __.Actual() = ((DskWrapper(initialValue)))
                member __.Model() = initialValue }
        
        //added Mar 1st
        type TearDownDsk<'Actual>() =
            inherit TearDown<'Actual>()
            override __.Actual actual = 
                pbt.Dsk(( (DskWrapper) actual).toModel())
        
        //added Mar 3rd
        let DskStateMachine =
          { new Machine<DskWrapper,list<string*obj*string>>() with
              member __.Setup = Gen.constant(_baseIconsLi) |> Gen.map create |> Arb.fromGen
              member __.Next thisM = 
                Gen.frequency [ (2, gen{  
                                                    let! a = chooseFrmLi addl
                                                    return (addIcn a) } ); 
                                        (1, gen{    
                                                    let! r = chooseFrmLi thisM
                                                    return (remIcn r) } );
                                        (1, gen{    
                                                    let! n = Gen.choose(0, lilen thisM)
                                                    let! p = chooseFrmLi philos
                                                    return (ChangeLbl n p) } )                                
        
                                        ] 
              override __.TearDown = TearDownDsk<'Actual>()
                }
        
        printfn "now runing check..."
        
        Check.Quick (StateMachine.toProperty DskStateMachine)

    
    
