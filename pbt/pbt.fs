(*  Brij (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 
    TrivediBldHdr->
    |Trivedi SrcCtrlHdr File.  Copyright (c) 2015-2025 M. P. Trivedi.  All rights reserved.|637901455|test.fs|none|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|

    fsc src\pbt\pbt_AI_Dsk.fs src\pbt\pbt_Dsk.fs  --platform:x64 --standalone --out:src\pbt\Trivedi.PbtDsk.dll -r:lib\Trivedi.Core.dll -r:lib\Trivedi.CoreAux.dll -r:lib\Trivedi.UI.dll -r:lib\Trivedi.UIAux.dll -I:C:\Windows\Microsoft.NET\Framework\v4.0.30319

    Provenance: Modified from loggedUIRnrDec20_FrmDz
    to incorp modified  UI els 4 pbt
    
    This ver uses Trivedi.UI_Nov02_Color+Grids.dll + UIAux + ...

    Last updated: Fri Mar 28 2025
module 
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
                                    let winH = brijWin((getUNID.ToString() + ^pId), tblID, docId)
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

    [<AutoOpen>]
    module Meethoo_Actual = 

        //from UIAux
        type મીઠૂ_પાન_eoy<'t when 't :> ITblMarker> (ctorDef:SaadoMasaloAux<'t>, dsk, સ્તિતિ) as f =
            inherit Form(Text = "Brij (TM) TableDef: Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved.", Visible = false, TopMost = true, WindowState = FormWindowState.Maximized, Font=defFont, ForeColor = (currentScheme ((!!~ "wld" dsk).Value)).Fore(), BackColor = (currentScheme ((!!~ "wld" dsk).Value)).Back())
            let (SaadoMasaloAux(tNm, def, icn, tblTy)) = ctorDef
            let mutable nm = tNm
            let mutable TblFldList = def
            do f.SuspendLayout()
            let setupFrm = 
                //tibbie "db: મીઠૂ_પાન setup..."
                f.Layout.AddHandler(centerFrm)
                    //@TBD: This is from gappa; shd we pull it out? 
                    //      doc diff. so perhaps a 'let getTitleP = fun docVal ->'
                let icnLbl = new Label(Image = brijLogo, Size = (new Size(brijLogo.Width, brijLogo.Height)), Anchor = anc "N", BackColor = Color.Transparent, ForeColor = (currentScheme ((!!~ "wld" dsk).Value)).Icn())
                let titTxt = new TextBox(AutoSize = true, Dock = doc "T", Enabled = false, Text = "Meethoo Def Document for " + nm, ReadOnly = true, Multiline = false, Width = f.Width - 50, TextAlign = HorizontalAlignment.Center, BorderStyle = BorderStyle.None, ForeColor = (currentScheme ((!!~ "wld" dsk).Value)).titFore(), BackColor = (currentScheme ((!!~ "wld" dsk).Value)).titBack())
                let titleP = new TableLayoutPanel(RowCount = 1, ColumnCount = 5, Dock = doc "T", BackColor = (currentScheme ((!!~ "wld" dsk).Value)).titBack(), AutoSize = true, Width = f.Width , Height = ((int) (titTxt.Height * 3)))
                titleP.SuspendLayout()
                titleP.RowStyles.Clear()
                titleP.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f))
                titleP.Controls.Add(icnLbl, 0, 0)
                titleP.Controls.Add(titTxt, 1, 0)
                titleP.SetColumnSpan(titTxt, 4)
                titleP.ResumeLayout(false)
                
                //tibbie "db: મીઠૂ_પાન btnPnl..."
                let btnFP = new FlowLayoutPanel(FlowDirection = FlowDirection.LeftToRight, Anchor = anc "N", AutoSize = true, BackColor = Color.White)
                let btnP = new TableLayoutPanel(Dock = doc "B", Width = f.Width - 100, BackColor = Color.White)
                let okButton = new Button(AutoSize = true, DialogResult = DialogResult.OK, Text = "&OK")
                okButton.Click.AddHandler(new EventHandler(fun o e -> tibbie "no handler yet"))
                let cancelButton = new Button(AutoSize = true, DialogResult = DialogResult.Cancel, Text = "&Cancel")
                cancelButton.Click.AddHandler(new EventHandler(fun o e -> tibbie "no handler yet"))
                btnFP.Controls.AddRange([|okButton; cancelButton|])
                btnP.Controls.Add(btnFP)

                //Added Mar11_25
                let tc = new TabControl(Dock = doc "F", Width = f.Width - defPadding.Horizontal, Height = f.Height - (defPadding.Horizontal * 8))
                let AppearancePg = new TabPage (Text = "Appearance")
                let FldSetupPg = new TabPage (Text = "Field Setup")
                let ImportPg = new TabPage (Text = "Import Data")
                let WebCliPg = new TabPage (Text = "Web Client")
                tc.Controls.Add([AppearancePg; FldSetupPg; ImportPg; WebCliPg])

                //For below, use InfoBox
                //let fldInfoLbl = new Label(Text = "Form Cells:", Dock = doc "F", ToolTipText = "Please select the Cells (data fields, infoboxes, etc) you wish to include in this Form.", BackColor = Color.Transparent, ForeColor = (currentScheme ((!!~ "wld" dsk).Value)).Fore())
                //fldFP.Controls.Add(fldInfoLbl)
                let lV = new ListView(MultiSelect = false, Dock = doc "F", CheckBoxes = false, FullRowSelect = true, HeaderStyle = ColumnHeaderStyle.Nonclickable, LabelEdit = false, View = View.Details, ForeColor = (currentScheme ((!!~ "wld" dsk).Value)).accentFore(), BackColor = (currentScheme ((!!~ "wld" dsk).Value)).accentBack())
                lV.SuspendLayout()
                lV.Columns.Add("FieldName:", -2, HorizontalAlignment.Left)
                lV.Columns.Add("FieldType", -2, HorizontalAlignment.Left)
                match TblFldList with
                | [] -> ()
                | _ ->  lim (fun itm -> 
                                let (DocFld(t, slg, isInt, nm)) = itm
                                let listItm = new ListViewItem(nm,0)
                                listItm.ForeColor <- (currentScheme ((!!~ "wld" dsk).Value)).accentFore()
                                listItm.SubItems.Add(t.ToString())
                                lV.Items.Add(listItm)) TblFldList |> ignore
                                
                //let fldInfoLbl = new Label(Text = "", Dock = doc "F", ToolTipText = "Please select a field to change its settings... ", Backcolor = Color.OldLace)
                //fldFP.Controls.Add(fldInfoLbl)

                lV.ResumeLayout(false)

                //tibbie "db: મીઠૂ_પાન tbar..."
                let tBar = new ToolStrip(Dock = doc "T")
                tBar.Items.Add (getTSButtonAux "Import CSV file..." "table.png" (Some("Auto-Import field definitions and data from a Comma-delimited CSV file")) (Some(new EventHandler (fun sender e -> 
                                let linkLi = "see:" + crlf + "https://stackoverflow.com/questions/53956462/get-column-in-haskell-csv-and-infer-the-column-type" + crlf + "https://deephaven.io/blog/2022/02/23/csv-reader/#:~:text=Type%20inferencing%20intends%20to%20map%20a%20column%20of,short%20if%20a%20memory-constrained%20user%20opted%20for%20that%29." + crlf + "https://stackoverflow.com/questions/60421589/ml-net-type-inference-for-csv-loading" + crlf + "https://github.com/Wittline/csv-schema-inference" + crlf + "** https://itnext.io/building-a-schema-inference-data-pipeline-for-large-csv-files-7a45d41ad4df" + crlf + "https://observablehq.com/@d3/d3-autotype" + crlf + "also srch for 'automatic type inference CSV'"
                            (ગપ્પા_પાન (SizeM,Some("CSV Stuff @ToDo"),None , Some(box (linkLi)), None,dsk, tblDefCSVDlg())) |> ignore 
                            ))))
            tBar.Items.Add (new ToolStripSeparator())
            (getTSButtonAux "Table Setup" "table.png" (Some("Change Table name / icon")) (Some(new EventHandler (fun sender e -> 
                            tibbie "remmed Dec 27"
                            //(ગપ્પા_પાન (SizeM,Some("CSV Stuff @ToDo"),None , Some(box ("tblNm" + crlf + "icon")), None,own, tblDefSetup())) |> ignore 
                            )))) |> tBar.Items.Add
            tBar.Items.Add (getTSButtonAux "Add" "note_add.png" (Some("Add a new Field to this Table")) (Some(new EventHandler (fun sender e -> 
                let quimaam = (Some(box ("Add", TblFldList)))
                tibbie "remmed Dec 27"
                //let res = (ગપ્પા_પાન (SizeM,Some("Add a new Field to this Table"),None , quimaam, None,own, tblDefAddModFld()))
                //tibbie ("res was" + res.ToString()) 
                ))))
            tBar.Items.Add (getTSButtonAux "Modify" "edit_note.png" (Some("Change a Field Type")) (Some(new EventHandler (fun sender e -> 
                            match (lV.SelectedIndices |> Seq.cast |> List.ofSeq) with
                            | [] -> tibbie "Please select a field to modify"
                            | _ as l ->
                                let selItm:ListViewItem = lV.Items.[int (l.[0])]
                                let fNm = selItm.Text
                                let fTy = selItm.SubItems.[1]
                                let quimaam = (Some("Mod", fNm, fTy))
                                //let res = (ગપ્પા_પાન (SizeM,Some("Change a Field Type"),None , Some(box ("fldNm" + crlf + "pickList")), None,own, tblDefAddModFld()))
                                //tibbie ("res was" + res.ToString()) 
                                tibbie "remmed Dec 27"
                                ))))
            tBar.Items.Add (getTSButtonAux "Remove" "delete.png" (Some("Remove a Field from this Table")) (Some(new EventHandler (fun sender e -> 
                            match (lV.SelectedIndices |> Seq.cast |> List.ofSeq) with
                            | [] -> tibbie "Please select a field to remove"
                            | _ as l ->
                                let msg = "Please note that if you proceed with this action, you will not be able to access the data stored in this field in any existing Forms or DataViews...";
                                let caption = "Are you sure?"
                                let res = MessageBox.Show(msg, caption, MessageBoxButtons.YesNo)
                                if (res = System.Windows.Forms.DialogResult.Yes) then
                                    lV.BeginUpdate()
                                    lV.Items.RemoveAt(l.[0])
                                    lV.EndUpdate()
                                 else ()
                            ))))
            !!^ ["lV", box lV; "tBar", box tBar ; "btnP", box btnP] f
            FldSetupPg.Controls.Add(lV)
            f.Controls.Add(titleP)
            f.Controls.Add(tBar)
            f.Controls.Add(tc)
            f.Controls.Add(btnP)
            printfn "db: મીઠૂ_પાન done adding controls..."
            f.ResumeLayout(false)


    [<AutoOpen>]
    module Csv_Actual = 

        //for total coverage of all poss fkdTys 4 the CSV tests...
        //Includes 1st the internalFlds from Core, some repurposed

        let SongFldLi() =
            [DocFld(FldString, "unid", true, "Document UNID");
            DocFld(FldDate, "crDt", true, "Released On");
            DocFld(FldDate, "modDt", true, "Last Modified On");
            DocFld(FldString, "title", true, "Song Title");
            DocFld(FldLongString, "cont", true, "Song Lyrics");
            DocFld(FldString, "tags", true, "Tags");
            DocFld(FldBoolean, "flag", true, "Flag");
            DocFld(FldNumber, "track", false, "Track Number");
            DocFld(FldFloat, "sales", false, "Sales (M)");
            DocFld(FldCurrency, "price", false, "Price");
            DocFld(FldCurrency, "price", false, "Price");
            DocFld(FldAttachment, "cover", false, "Album Cover Art");   //byte[]
            DocFld(FldBoolean, "familiar", false, "Is Familiar?");
            DocFld(FldChoiceList, "subGenre", false, "Sub Genre"); //HardRock|ClassicRock|Metal|Other
            DocFld(FldRange, "rating", false, "Rating");  //1-5
            DocFld(FldDateTime, "rdatetime", false, "DateTime");  //rnd
            DocFld(FldTime, "time", false, "Time");  //rnd
            ]

    //@tbfo
    let getUrl u = u
    let remQuotes s = s

    let mutable ok_res:list<Result> = []
    let mutable err_res:list<Result> = []
    
    let mini_Inp = """
"one","two", 99.9, 90, "three"
"three","four", 199.9, 190, "end"
"""
    let mini_fDef = 
        [DocFld(FldString, "fld_one", true, "Field_One");
        DocFld(FldString, "fld_two", true, "Field_Two");
        DocFld(FldString, "fld_thr", true, "Field_Three");
        DocFld(FldString, "fld_fou", true, "Field_Four")]
        
    let parseAsync roIdx ro =
        async {
            try
                printfn "in parseAsync for idx:%A" roIdx
                let trimd = remQuotes (ro.Trim())
                let roSpl = (સપ્લીટ ro "\"(\"*)(\s*),(\s*)(\"*)"))
                let thisRoTpl, thisRoErrLi, thisRoErrCt, thisRoIdx =
                    ((Mtpl.empty(), [], 0, 0), roSpl)
                    |> lifoi (fun fldS i fldV ->
                                let (fTpl, fldErrLi, fldErrCt, fldIdx) = fldS
                                let thisFld = saaduDef.[i]
                                let (DocFld(t, slg, isInt, nm)) = thisFld
                                match (FldTy.csvTryParse(fldV t slg)) with
                                | Ok ob -> (Mtpl.AddOne (slg, box fldV ob) fTpl), fldErrLi, fldErrCt, fldIdx + 1
                                | Error e ->
                                    let fullErr = roIdx.ToString() + ") " + e + "| Full Row: " + ro.ToString()
                                    (fTpl, fullErr :: fldErrLi, fldErrCt + 1, fldIdx + 1))
                match thisRoErrCt with
                | 0 -> ok_res :: (Mtpl.AddOne(roIdx.ToString(), box thisRoTpl), thisRoErrLi, thisRoErrCt, roIdx + 1)
                | _ -> err_res :: (roTpl, [thisRoErrLi] @ roErrLi, roErrCt, roIdx + 1 

            with
                | ex -> printfn "%s" (ex.Message);
        }
               
    let csvParse optP optU saaduDef = 
        match saaduDef with 
        | [] -> 
            let msg = """
            This table has no fields defined.
            In order to import data into the table you first need to set up the table by defining the fields.  Please choose the tab above (help is available on that page)"""
            //ગપ્પા_પાન (SizeM,Some("csvParse"),None , Some(box (tkFldList())), None, d, frmSetupDlg())  |> ignore
            printfn "csvParse:%A" msg
        | _ -> ()

        let inp = 
            match optP with
            | Some p ->  File.ReadAllText(p, Encoding.UTF8)
            | _ -> match optU with
                    | Some u -> getUrl u //tbfo
                    | _ -> printfn "error in csvParse: both input params None"
                           ""
        let spl = સપ્લીટ inp "\n+"
        match (spl.Length > 1) with
        | true -> 
            printfn "spl ty: %A" (spl.GetType())
            let mapper = parseAsync mini_fDef
            (spl |> List.toSeq)
            |> Seq.mapi mapper
            |> Async.Parallel
            |> Async.RunSynchronously
            |> ignore
        | _ -> 
            printfn "Info: csvParse input len < 1..."
            ()

    

    module Csv_Ext =


        type FldType with
            static member x.csvTryParse(itm expTy slg) =
                match expTy with
                | FldString ->
                | FldNumber -> 
                    match System.Int32.TryParse itm with
                    | true, v -> Ok (intNm, box v, v.GetType())
                    | false, e -> Error ("FieldName: " + slg + " Expected Type: " + expTy + " Got: " + itm)
                | FldFloat -> 
                    match System.Double.TryParse itm with
                    | true, v -> Ok (intNm, box v, v.GetType())
                    | false, e -> Error ("FieldName: " + slg + " Expected Type: " + expTy + " Got: " + itm) 
                | FldCurrency ->
                | FldLongString ->
                | FldAttachment ->
                | FldBoolean ->
                | FldChoiceList ->
                | FldRange ->
                | FldDate ->
                | FldDateTime ->
                | FldTime -> 
                | FldColor ->

    [<AutoOpen>]
    module Banarasi_Actual = 


        //from UIAux; called via ગપ્પા_પાન_Jan
        let બનારસી_પીચાક_eoy =
            fun (ક્વિમામ:option<_>) (d:Form) ->
                //NOTE that the getImg handler above nds to go into UI_Current.fs
                //let w = d.getW()
                db "બનારસી_પીચાક_eoy hdr 1..."
                let w:Mtpl = (!!~ "wld" d).Value
                
                //Jan 24 -> bastas are throwing on unbox again @MBI
     
                let mutable def = unbox (ક્વિમામ.Value)
                db "બનારસી_પીચાક_eoy hdr 2..."
                let (BanarasiMasaloAux(unid, dispNm, સુપારી, usrFlds, usrDefLblFont, usrDefDataFont, usrDefForeColor, usrDefBackColor, docInf)) = def
                let mutable BanarasiFldL:BanarasiFldAux<_> list = usrFlds
                let midP:TableLayoutPanel = (!!~ "midP" d).Value
                //@REINSTATE: moved here from katho() in order to have ref avail 4 chubaayHndlr
                let hdrLbl = new Label(Text = ("Form Appearance:"), Dock = doc "F", TextAlign = ContentAlignment.MiddleCenter, BackColor = (currentScheme ((!!~ "wld" d).Value)).accentBack())
                let fldFP = new TableLayoutPanel(RowCount = 2, ColumnCount = 2, Dock = doc "F", Name="TablePanel", BackColor = (currentScheme ((!!~ "wld" d).Value)).accentBack())
                let lV = new ListView(MultiSelect = false, HideSelection = false, Dock = doc "F", CheckBoxes = false, FullRowSelect = true, HeaderStyle = ColumnHeaderStyle.Nonclickable, LabelEdit = false, View = View.Details, ForeColor = (currentScheme ((!!~ "wld" d).Value)).accentFore(), BackColor = (currentScheme ((!!~ "wld" d).Value)).accentBack())
                let btnBarFP = new FlowLayoutPanel(FlowDirection = FlowDirection.LeftToRight, Anchor = anc "N", AutoSize = true, BackColor = Color.White)
                let btnBarP = new TableLayoutPanel(Dock = doc "T", Width = d.Width)
                let btnFP = new FlowLayoutPanel(FlowDirection = FlowDirection.TopDown, Anchor = anc "N", AutoSize = true)
                let p0 = (ફીલ્ડ_પેનલ_Aux  ("Number of Columns", FldRange, "Form Column Count", 2, 0, None)) :> Panel
                let p1 = (ફીલ્ડ_પેનલ_Aux  ("Label Font", FldFont, "Form Label Font", 2, 0, None)) :> Panel
                let p2 = (ફીલ્ડ_પેનલ_Aux  ("Data Font", FldFont, "Form Data Font", 2, 0, None)) :> Panel
                let p3 = (ફીલ્ડ_પેનલ_Aux  ("Text Color", FldColor, "Form Text Color", 2, 0, None)) :> Panel
                let p4 = (ફીલ્ડ_પેનલ_Aux  ("Background Color", FldColor, "Form Background Color", 2, 0, None)) :> Panel
                let p5 = (ફીલ્ડ_પેનલ_Aux  ("InfoBox1", FldInfoBox, "Pls note that each of the settings above can be overriden for any particular cell in the Cell Definition Settings (@ToDo)" + crlf + "Note: IF user tries to attach ttip > 150 chars suggest an InfoBox instd.", 2, 0, None)) :> Panel
                let chuno(s) =
                    fldFP.SuspendLayout()
                    //let defaultTags = ["fldNm", box slg; "c", box 1; "r", box 1; "o", box 0]
                    //let fldInfoLbl = new Label(Text = "Form Cells:", Dock = doc "F", TextAlign = ContentAlignment.MiddleCenter)
                    //ToolTipText = "Please select the Cells (data fields, infoboxes, etc) you wish to include in this Form.")
                    //fldFP.Controls.Add(fldInfoLbl)
                    
                    lV.DoubleClick.AddHandler(new EventHandler(fun o e -> 
                        match (lV.SelectedIndices |> Seq.cast |> List.ofSeq)  with
                        | [] -> ()
                        | _ -> 
                            let selItm:ListViewItem = lV.Items.[(lV.SelectedIndices.[0])]
                            let strIn = ("For fld: " + ((selItm.SubItems.[0]).Text) + 
                                            " ty: " + ((selItm.SubItems.[1]).Text) + 
                                            " colSp: " + ((selItm.SubItems.[2]).Text) + 
                                            " roSp: " + ((selItm.SubItems.[3]).Text) + 
                                            " ttip: " + ((selItm.SubItems.[4]).Text) + 
                                            " vFn: " + ((selItm.SubItems.[5])).Text)
                            let selFldAux = BanarasiFldL.[lV.SelectedIndices.[0]]
                            ગપ્પા_પાન_Banarasi_Fld (SizeM,Some(box "Form Field Definition for field: "), None , Some(box (selFldAux, સુપારી)), None, d, frmSetupDlg())  |> ignore
                    ))
                    lV.SuspendLayout()
                    lV.Columns.Add("FieldName:", -2, HorizontalAlignment.Left)
                    lV.Columns.Add("FieldType", -2, HorizontalAlignment.Left)
                    lV.Columns.Add("ColumnSpan", -2, HorizontalAlignment.Left)
                    lV.Columns.Add("RowSpan", -2, HorizontalAlignment.Left)
                    lV.Columns.Add("CanUhear", -2, HorizontalAlignment.Left)
                    lV.Columns.Add("HelpText", -2, HorizontalAlignment.Left)
                    lV.Columns.Add("Validation Rule(s)", -2, HorizontalAlignment.Center)
                    //lV.SmallImageList <- ((new ImageList()).Images.Add(getIconBmp() :> Image)  )
        (* Move to UI.Common l8r *)
                    let getMinLvColWid(l:string) = 
                            let g = (new Label()).CreateGraphics()
                            ((g.MeasureString(l, defFont)).ToSize()).Width
                    lV.Columns.[2].Width <- getMinLvColWid("ColumnSpan")
                    lV.Columns.[3].Width <- getMinLvColWid("RowSpan")
                    
                    lV.Layout.AddHandler(new LayoutEventHandler( fun (sender:obj) (e:System.Windows.Forms.LayoutEventArgs) -> 
                        match ((getTopForm (sender :?> Control)).Value) with
                        | :? Form as f ->
                            match ((lV.Items).Count < 1) with
                            | true -> ()
                            | _ -> lV.Items.Clear()
                            BanarasiFldL
                            |> lim (fun itm -> 
                                        let (BanarasiFldAux(fldInf, colSp, rowSp, vFn, CanUHr, ttip, _)) = itm
                                        let (DocFld(t, slg, isInt, nm)) = fldInf
                                        let listItm = new ListViewItem(nm,0)
                                        listItm.ForeColor <- dCobaltBlue
                                        listItm.SubItems.Add(t.ToString())
                                        listItm.SubItems.Add(colSp.ToString())
                                        listItm.SubItems.Add(rowSp.ToString())
                                        listItm.SubItems.Add(CanUHr.ToString())
                                        match ttip with
                                        | Some t -> listItm.SubItems.Add(t.ToString())
                                        | _ -> listItm.SubItems.Add("None")
                                        //listItm.SubItems.Add(ttip.ToString())
                                        //listItm.SubItems.Add("ttip") //ttip.ToString())
                                        match vFn with
                                        | Some f -> listItm.SubItems.Add(vFn.ToString())
                                        | _ -> listItm.SubItems.Add("None")
                                        lV.Items.Add(listItm)) |> ignore 
                        | _ -> 
                            printfn "listView layout: couldn't find form ref..."
                            ()
                        ))
                let katho() =
                    d.WindowState <- FormWindowState.Maximized
                    midP.ColumnCount <- 3
                    midP.Controls.Clear()
                    let fBtn:Button = (!!~ "Label Font" p1).Value
                    fBtn.Text <- usrDefLblFont.FontFamily.Name.ToString() + " " + usrDefLblFont.Size.ToString()  + " " + (usrDefLblFont.Style).ToString()
                    let fBtn2:Button = (!!~ "Data Font" p2).Value
                    fBtn2.Text <- usrDefDataFont.FontFamily.Name.ToString() + " " + usrDefDataFont.Size.ToString()  + " " + (usrDefDataFont.Style).ToString()
                    let colBtn:Button = (!!~ "Text Color" p3).Value
                    colBtn.BackColor <- usrDefForeColor
                    let colBtn2:Button = (!!~ "Background Color" p4).Value
                    colBtn2.BackColor <- usrDefBackColor
                    //!!^ [nm, box ctrl] midP
                    midP.ColumnStyles.Clear()
                    midP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.0f))
                    midP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.0f))
                    midP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.0f))
                    midP.Controls.Add(hdrLbl, 0, 1)
                    //midP.SetColumnSpan(hdrLbl, 3)
                    midP.Controls.Add(p1, 1, 1)
                    midP.Controls.Add(p2, 2, 1)
                    midP.Controls.Add(p0, 0, 2)
                    midP.Controls.Add(p3, 1, 2)
                    midP.Controls.Add(p4, 2, 2)
                    //midP.Controls.Add(p5, 0, 3)   //InfoBox1
                    //midP.SetColumnSpan(p5, 3)     //InfoBox1
                    def
                let lovely() =
                    let widButton = getImgBtn "&Wider" "swap_horiz.png" w
                    widButton.Click.AddHandler(getListVwH2 lV "ColumnSpan" "add")
                    let narButton = getImgBtn "&Narrower" "compare_arrows.png" w
                    narButton.Click.AddHandler(getListVwH2 lV "ColumnSpan" "rem")
                    let talButton = getImgBtn "&Taller" "import_export.png" w
                    talButton.Click.AddHandler(getListVwH2 lV "RowSpan" "add")
                    let shtButton = getImgBtn "&Shorter" "unfold_less.png" w
                    shtButton.Click.AddHandler(getListVwH2 lV "RowSpan" "rem")
                    let moveUpButton = getImgBtn "Move &up" "arrow_upward.png" w
                    moveUpButton.Click.AddHandler(getListVwH lV "moveUp")
                    let moveDnButton = getImgBtn "Move &down" "arrow_downward.png" w
                    moveDnButton.Click.AddHandler(getListVwH lV "moveDn")
                    let AddFldButton = getImgBtn "&Add Field" "playlist_add.png" w
                    AddFldButton.Click.AddHandler(new EventHandler(fun o e -> 
                            //shim: Instd of tg we can bld from listVw
                            let currLVItems = lV.Items |> Seq.cast |> List.ofSeq |> List.map (fun (item:ListViewItem) -> item.Text) |> Seq.ofList
                            let allFlds = 
                                BanarasiFldL
                                |> lim (fun itm -> 
                                            let (BanarasiFldAux(fldInf, colSp, rowSp, vFn, CanUHr, _, _)) = itm
                                            let (DocFld(t, slg, isInt, nm)) = fldInf
                                            nm)
                            let choices = liExc currLVItems allFlds
                            tibbie ((choices.ToString()) + "\r\n @mbi with listDlg (splat)")
                            //(ગપ્પા_પાન (SizeM,Some("Please make your selections:"),None , Some(box (choices)), None, frm, listDlg())) |> ignore
                            // <- BanarasiFldL
                    ))
                    let RemFldButton = getImgBtn "&Remove Field" "playlist_remove.png" w
                    RemFldButton.Click.AddHandler(getListVwH lV "remove")
                    btnBarFP.Controls.AddRange([|widButton; narButton;talButton; shtButton; moveUpButton; moveDnButton; AddFldButton; RemFldButton |])
                    btnBarP.Controls.Add(btnBarFP)
                    def
                let cutkaa(s) =
                    //IboxButton.Click.AddHandler(new EventHandler(fun o e -> Application.Exit()))
                    //moved from lovely(); dupes nd 2 be removed from there...
                    let moveUpButton = getImgBtn "Move &up" "arrow_upward.png" w
                    moveUpButton.Click.AddHandler(getListVwH lV "moveUp")
                    let moveDnButton = getImgBtn "Move &down" "arrow_downward.png" w
                    moveDnButton.Click.AddHandler(getListVwH lV "moveDn")
                    let AddFldButton = getImgBtn "&Add Field" "playlist_add.png" w
                    AddFldButton.Click.AddHandler(new EventHandler(fun o e -> 
                            //shim: Instd of tg we can bld from listVw
                            let currLVItems = lV.Items |> Seq.cast |> List.ofSeq |> List.map (fun (item:ListViewItem) -> item.Text) |> Seq.ofList
                            let allFlds = 
                                BanarasiFldL
                                |> lim (fun itm -> 
                                            let (BanarasiFldAux(fldInf, colSp, rowSp, vFn, CanUHr, _, _)) = itm
                                            let (DocFld(t, slg, isInt, nm)) = fldInf
                                            nm)
                            let choices = liExc currLVItems allFlds
                            tibbie ((choices.ToString()) + "\r\n @mbi with listDlg (splat)")
                            //(ગપ્પા_પાન (SizeM,Some("Please make your selections:"),None , Some(box (choices)), None, frm, listDlg())) |> ignore
                            // <- BanarasiFldL
                    ))
                    let RemFldButton = getImgBtn "&Remove Field" "playlist_remove.png" w
                    RemFldButton.Click.AddHandler(getListVwH lV "remove")
                    let IboxButton = getImgBtn "Add &InfoBox" "info.png" w
                    let BlankButton = getImgBtn "Add &Blank Row" "space_bar" w
                    let setThingyBtn = getImgBtn "Set &Thingy" "space_bar" w
                    setThingyBtn.Click.AddHandler(new EventHandler(fun (sender:obj) (e:EventArgs) ->
                        //let res = dlg(Thingy.getOptionsFor "2Col")
                        tibbie "//follow proc for AddFldBtn (poss commonHndlr?)"
                    ))
                    let valButton = getImgBtn "&Validation Rule" "rule.png" w
                    valButton.Click.AddHandler(new EventHandler(fun (sender:obj) (e:EventArgs) ->
                        lV.SelectedIndices |> Seq.cast |> List.ofSeq 
                            |> lim (fun idx -> 
                                       match idx > -1 with
                                       | true -> 
                                           let selItm = lV.Items.[idx]
                                           tibbie ("For fld: " + ((selItm.SubItems.[0]).Text) + 
                                                    " ty: " + ((selItm.SubItems.[1]).Text) + 
                                                    " colSp: " + ((selItm.SubItems.[2]).Text) + 
                                                    " roSp: " + ((selItm.SubItems.[3]).Text) + 
                                                    " ttip: " + ((selItm.SubItems.[4]).Text) + 
                                                    " vFn: " + ((selItm.SubItems.[5])).Text)
                                       | _ -> tibbie "unknwn rec selected..."
                            ) |> ignore))
                    // //; setThingyBtn; valButton
                    [moveUpButton; moveDnButton; AddFldButton; RemFldButton; IboxButton; BlankButton]
                    |> lim (fun (b:Button) -> b.BackColor <- Color.White
                                              b :> Control)
                    |> Array.ofList |> btnFP.Controls.AddRange
                    //fldFP.Controls.Add(btnBarP, 0, 0)
                    //midP.SetColumnSpan(btnBarP, 2)
                    fldFP.Controls.Add(lV, 0, 1)
                    fldFP.Controls.Add(btnFP, 1,1)
                    fldFP.ColumnStyles.Clear()
                    fldFP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f))
                    fldFP.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, float32 (getBtnWid "Remove Field")))
                    //let fldInfoLbl = new Label(Text = "", Dock = doc "F", ToolTipText = "Please select a field to change its settings... ", Backcolor = Color.OldLace)
                    //fldFP.Controls.Add(fldInfoLbl)

                    fldFP.ResumeLayout(false)
                    lV.ResumeLayout(false)
                    midP.Controls.Add(fldFP, 0, 4)
                    midP.SetColumnSpan(fldFP, 3)
                    def
                let goolkand() =
                    let tBar = new ToolStrip(Dock = doc "T")
                    let saveHndlr = new EventHandler(fun o e -> 
                        let flt = """Brij files (*.bsd)|*.bsd|All files (*.*)|*.*"""
                        let saveDlg = new SaveFileDialog(Filter = flt, FilterIndex = 1)
                        if saveDlg.ShowDialog() = DialogResult.OK then
                            tibbie "Code to write the stream goes here."
                           else ()
                    )
                    tBar.Items.Add (getTSButtonAux "Save" "save.png" (Some("Save to Disk")) (Some(saveHndlr)))
                    d.Controls.Add(tBar)
                    //chubaay() inserted here (Jan 23) for normal flow
                    hdrLbl.Click.AddHandler(new EventHandler(fun (sender:obj) (e:EventArgs) ->
                        let origFlds = Task.getFldDefs()
                        let currUsrFlds = 
                            lV.Items |> Seq.cast |> List.ofSeq 
                            |> limi (fun i itm -> 
                                       let selItm = lV.Items.[i]
                                       let docFldForThisItm = 
                                          origFlds 
                                            |> List.filter(fun f -> 
                                                                let (DocFld(fldTy, intNm, isInt, dispNm)) = f
                                                                dispNm = (selItm.SubItems.[0]).Text)
                                            |> List.head
                                       let cS = (int (selItm.SubItems.[2]).Text)
                                       let rS = (int (selItm.SubItems.[3]).Text)
                                       let tt = if (len ((selItm.SubItems.[4]).Text)) > 0 then Some ((selItm.SubItems.[4]).Text) else None
                                       let vFn = None //selItm.SubItems.[5])).Text
                                       let CanUhear = Thingy("")
                                       //error FS0193: Type constraint mismatch. The type       <- probably @mbi (splat!)
                                       //'Type' is not compatible with type 'ITblMarker'
                                       let workaround = TaskTbl()
                                       //BanarasiFld(docFldForThisItm, cS, rS, vFn, CanUhear, tt, ((genArg def) :> ITblMarker ))
                                       BanarasiFldAux(docFldForThisItm, cS, rS, vFn, CanUhear, tt, workaround)
                            )
                        //@ToDo: font btn also records color change; return tpl?
                        let currSoopC:TrackBar = (!!~ "Number of Columns" p0).Value
                        let currDefLblFontBtn:Button = (!!~ "Label Font" p1).Value
                        let currDefDataFontBtn:Button = (!!~ "Data Font" p2).Value
                        let currusrDefForeColorBtn:Button = (!!~ "Text Color" p3).Value
                        let currusrDefBackColorBtn:Button = (!!~ "Background Color" p4).Value
                        let currSoop = currSoopC.Value
                        let currDefLblFont:Font = currDefLblFontBtn.Font
                        let currDefDataFont:Font = currDefLblFontBtn.Font
                        let currusrDefForeColor:Color = currusrDefForeColorBtn.BackColor
                        let currusrDefBackColor:Color = currusrDefBackColorBtn.BackColor
                        def <- BanarasiMasaloAux(unid, dispNm, currSoop, currUsrFlds, currDefLblFont, currDefDataFont, currusrDefForeColor, currusrDefBackColor, docInf)
                    ))
                    def
                let bld =
                    match ક્વિમામ.IsSome with
                    | true ->
                        ટેન્ગ (chuno) (katho) (lovely())
                        T2 (goolkand()) (cutkaa) |> ignore
                        let res = d.ShowDialog()
                        d.Dispose()
                        None
                    | _ -> None
                do db "બનારસી_પીચાક_eoy done..."



        type DzCell =    | DzCellItm of string * int * int * int * int
                               | B4Tgt of float * float with
            member this.ChkHtml() =
                match this with
                | DzCellItm(slg, _, _, _, _)  -> 
                    "<td>\n<div class='CellTgt'>" + slg + "</div>\n</td>"
                | B4Tgt(a,b) -> """
                    <td>
                        <div class='B4Tgt'></div>
                    </td>
        """

        type DzRow = | DzRowItm of list<DzCell>
                             | RoTgt of float with
            member this.ChkHtml() =
                match this with
                | RoTgt r -> """
                    <tr><td>
                        <div class='RoTgt'></div>
                    </td></tr>
        """
                | DzRowItm lc -> 
                    (lifo (fun s (v:DzCell) -> s + "\n" + v.ChkHtml()) "<tr>\n" lc) + "\n</tr>"


        type DzTbl = | DzTbl of list<DzRow> with
            member this.ChkHtml() =
                let (DzTbl(l)) = this
                ( lifo (fun s (v:DzRow) -> s + "\n" + v.ChkHtml()) "/n<table>" l) + "/n</table>"

    [<AutoOpen>]
    module DnDMonad =
        open System
        open Trivedi
        open Trivedi.Core

        type DnDState<'M, 'T> = 'M -> 'M * 'T

        let adder = fun l -> ("adder" + (List.length l).ToString()) :: l

    (*

        //essentially cellModel -> (cellModel * UIModel)
        let resetState = 
            fun (li:list<list<DzCell>>) (colN) -> 
                //@ToDo: after calling, reset tbl ref in UI + reset (cliP.RowCount <- lilen tbl)
                let cTot, rTot, uiM = 
                    li  |> List.fold (fun s v -> 
                            let (c:int, r:int, inLi:list<'t>) = s
                            let ([DzCellItm(slg, cc, cr, ccI, crI)]) = v
                            match inLi with
                            | [] -> 0, 0, [RoTgt((float) 0 - 0.5)]
                            | _ ->
                                //lim 4 ro, handle wraparounds here
                                match (not(c+1 < colN)) with
                                | true -> 
                                    0, r+1, [[RoTgt((float) r + 0.5)]; [B4Tgt(((float) 0-0.5), ((float) r+0.5)); DzCellItm(slg,cc,cr,c,r)]] @ inLi
                                | _ -> 
                                    //@ToDo: nd 2 addRo here AFTER to preserve rest?
                                    c+cc, r, [[RoTgt((float) r + 0.5)];[B4Tgt(((float) 0-0.5), ((float) r+0.5)); DzCellItm(slg,cc,cr,c,r)]] @ inLi)  (0,0,[]) 
                        |> List.rev
                (li, uiM)

        let resetTblLayoutPanel = 
            fun cliP colN rowN tblRef ->
                let getTblRo =
                  fun tbl idx ->
                    List.filter (fun c ->
                              let (DzCellItm(_,_,_,_, cRo)) = c 
                              cRo = idx) tbl
                cliP.SuspendLayout()
                cliP.Controls.Clear()
                cliP.ColumnStyles.Clear()
                cliP.RowStyles.Clear()
                [1.. colN] |> lim (fun c -> cliP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, ((float32) ((1/colN) * 100))))) |> ignore
                cliP.Layout.AddHandler(new LayoutEventHandler( fun (sender:obj) (e:System.Windows.Forms.LayoutEventArgs) -> 
                    //necc? let thisF = sender :?> Form
                    let rec procTbl currRo remainder =
                      let remLen =
                        getTblRo remainder currRo 
                        |> List.map (fun currcell -> 
                                       let (DzCellItm(slg,cSp,rSp,cCo, cRo)) = currcell
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
                    procTbl 0 tblRef ))
                cliP.ResumeLayout(false)

        let getIdxOfFirstCellForRow =
            fun tbl roIdx cellMod -> 
                printfn "tibbie"
               
        //@TBD: need cellTy?
        let doInsCell =
            fun (srcSlg, Dest, cellSt:list<DzCell>) -> 
                printfn "tibbie"

        //@TBD: need cellTy?
        let doDelCell =
            fun (srcSlg, Dest, cellSt:list<DzCell>) -> 
                printfn "tibbie"
                
        //@TBD: need cellTy?
        let doInsRo =
            fun (srcSlg, Dest, cellSt:list<DzCell>) -> 
                printfn "tibbie"

        //@TBD: need cellTy?
        let doDelRo =
            fun (srcSlg, Dest, cellSt:list<DzCell>) -> 
                printfn "tibbie"
                
        //@TBD: need cellTy?
        let doInsCol =
            fun (srcSlg, Dest, cellSt:list<DzCell>) -> 
                printfn "tibbie"

        //@TBD: need cellTy?
        let doDelCol =
            fun (srcSlg, Dest, cellSt:list<DzCell>) -> 
                printfn "tibbie"

        let doDrop = 
            fun (srcSlg, Dest, colN, cellSt:list<DzCell>) -> 
                let newCellSt = doDropCellSt srcSlg Dest cellSt
                resetState newCellSt colN
                put newSt
                resetTblLayoutPanel cliP colN rowN newCellSt
                reset clp.RowCount 2 lilen, colCount 2 newColN
    *)

        let getS = fun s -> (s,s)
        let putS s = fun _ -> (s,())
        let eval m s = m s |> fst
        let exec m s = m s |> snd
        let empty = fun s -> (s,())
        let modif f s = let x = getS in (putS (f x))
        
(*  Mar30'25: 
        initState with BanMasAux
        bind: do a = f x; b = getUiModel(); putS(a,b)
        After putS run the chain of fns incl doLayout() based on refCell
        Ea delta produces updated Mas/Models and shd upd8 layout etc.
        Keep ref 2 list<cellModel, intPtr> (2nd param is 0 or undelIdx for Clobber)
        
*)
       
        
        let bind k m = fun s -> 
            let (s', a) = m s
            let s'' = adder s' //resetState s'
            (k a) s''

        type DnDStateBuilder() =
            member this.Return(a) : DnDState<'M,'T> = fun s -> (s,a)
            member this.ReturnFrom(m:DnDState<'M, 'T>) = m
            member this.Bind(m:DnDState<'M,'T>, k:'T -> DnDState<'M,'U>) : DnDState<'M,'U> =  bind k m
            member this.Zero() = this.Return()
            member this.Delay(f) = this.Bind(this.Return (), f)
        let ``⍾`` = new DnDStateBuilder()

#if earlier
        let sRun = 
            ["ob"] |>
            ``⍾`` {
                   let! a = getS
                   do! putS ("ob2" :: a)
                   let! b = getS
                   return b
                } |> snd |> printfn "sRun res:\n %A"
                // ["adder3"; "ob2"; "adder1"; "ob"]
    #else
        let sRun = 
            ["ob"] |>
            ``⍾`` { 
                           printfn "run one"
                           return! getS
                        } |> ignore
            // ["adder1"; "ob"]  i.e., runs thro >>=

        let sRun2 = 
            ["adder1"; "ob"] |>
            ``⍾`` { 
                           printfn "run two"
                           return! getS
                        } |> snd |> printfn "sRun2 res:\n %A"
           // ["adder2"; "adder1"; "ob"]

#endif //earlier

#if tmpRem

    [<AutoOpen>]
    module DnD_ops = 
        open System
        open System.Drawing
        open System.Windows.Forms
        open Trivedi
        open Trivedi.Core

    (*
        What:           MVP for impl/testing DnD func + (l8r) Dojo wireframes...
        Src:            https://github.com/TrivediEnterprisesInc/TrivediEnterprisesInc.github.io/blob/main/winFrms.fs
        Last updated:   Mon Mar 31 2025
        Stat:           interimRes3:
                        [RoTgt -0.5; DzCell ("Cell 1", 1, 1, 0, 0); DzCell ("Cell 2", 1, 1, 1, 0); DzCell ("Cell 3", 1, 1, 2, 0); 
                        RoTgt 0.5; DzCell ("Cell 4", 1, 1, 0, 1); DzCell ("Cell 5", 3, 1, 1, 1); DzCell ("Cell 6", 2, 1, 4, 1); 
                        RoTgt 1.5; DzCell ("Cell 7", 2, 1, 0, 2); DzCell ("Cell 8", 2, 1, 2, 2); 
                        RoTgt 2.5; DzCell ("Cell 9", 2, 1, 0, 3); DzCell ("Cell 10", 3, 1, 2, 3); 
                        RoTgt 3.5; DzCell ("Cell 11", 2, 1, 0, 4); DzCell ("Cell 12", 2, 1, 2, 4); 
                        RoTgt 4.5; DzCell ("Cell 13", 2, 1, 0, 5); DzCell ("Cell 14", 2, 1, 2, 5); 
                        RoTgt 5.5]
        
        Notes:           - Prior vers of this mod exist, chk past commits (bld_v1 interleaves BlankRows)
                         - DDnDTgt spans Row (see PostPitch Notes)
                         - this curr impl autoCasts to DzCell_v2Struc
                         - as decided, changes via UI updates def & autoUpdates UI
                         - @Add: bld_v1 interleaves dropCells(see output); nd to manually do that
        Notes Mar '25:  - The curr sys on drop will relayout subseq rows/sections; this is _not_ desired:
                          it may spoil completed layouts.  Instd: if drop causes overflo/wrap, do an li.inserAt
                          & leave rest of struct unchanged (this means the tbl ros subseq have 2 be incr.d)
                        - Also beauc. deltas for FsChk modelling purps.
                        
                        - The Dnd mods don't use DocFld/FldTy but the foll.: (uiaux)
            type BanarasiFldAux<'t when 't :> ITblMarker> = 
                    | BanarasiFldAux of docF:DocFld * colSpan:int * rowSpan:int * 
                        vFn:('t -> bool) option * CanUhear:Thingy * fldTtip:string option * tblTy:'t with
                override this.ToString() = ...
                static member getDefault(docF:DocFld list, ty:'t) = ...

            type BanarasiMasaloAux<'t when 't :> ITblMarker> = 
                    | BanarasiMasaloAux of unid:DocUNID * dispNm:string *
                                    સુપારી:int * usrFlds:BanarasiFldAux<'t> list * 
                            usrDefLblFont:Font * usrDefDataFont:Font * 
                            usrDefForeColor:Color * usrDefBackColor:Color * docInf:DesDocInf with
    *)
        
        
        printfn "...init mod DnD_ops..."
        
        let printHR() = printfn " - - - - - - - - - - - - - - - - - "
        let tibbie = fun (s:string) -> MessageBox.Show(s, "System Msg") |> ignore
        let defPadding:Padding = new Padding(40)
        let getCtrlHt() = 100
        let defColor:Color = Color.White
        let defForeColor:Color = Color.Black //dCobaltBlue
        let defBackColor:Color = Color.White

        let getCell = 
          fun slg -> new Button(Text = slg, Dock = DockStyle.Fill, AutoSize = true)
        let colN = 3
        let isEven num = (num % 2 = 0)
        let toCellSlug = fun n -> "Cell " + n.ToString()

        printfn "2.1"
        
        let tbl = [[DzCellItm ("Cell 1",1,1,0,0); DzCellItm ("Cell 2",1,1,1,0)];
                   [DzCellItm ("Cell 3",1,1,2,0); DzCellItm ("Cell 4",1,1,0,1)];
                   [DzCellItm ("Cell 5",3,1,1,1); DzCellItm ("Cell 6",2,1,4,1)];
                   [DzCellItm ("Cell 7",2,1,0,2); DzCellItm ("Cell 8",2,1,2,2)];
                   [DzCellItm ("Cell 9",2,1,0,3); DzCellItm ("Cell 10",3,1,2,3)];
                   [DzCellItm ("Cell 11",2,1,0,4); DzCellItm ("Cell 12",2,1,2,4)];
                   [DzCellItm ("Cell 13",2,1,0,5); DzCellItm ("Cell 14",2,1,2,5)]]

        let onlyCells = fun cell -> match cell with
                                       | DzCellItm(_,_,_,_,_) -> true
                                       | _ -> false

        printfn "interimRes3:%A" (bld_v4 tbl)

    (*
        //*  Dnd handlers   */

        ///Add to src.MouseDown
        let getDragInitHandler = 
            fun src -> 
                new DragEventHandler( fun (sender:obj) (e:System.Windows.Forms.DragEventArgs) -> 
                    let button1 = (Button) obj
                    button1.DoDragDrop(src.Text, DragDropEffects.Copy) |> ignore)

        let getTgtDragEnterHandler =
            fun src -> 
                new DragEventHandler( fun (sender:obj) (e:System.Windows.Forms.DragEventArgs) -> 
                    match e.Data.GetDataPresent(DataFormats.Text) with
                    | true -> e.Effect <- DragDropEffects.Copy
                    | _ -> e.Effect <- DragDropEffects.None)

        let getRowTgtDragDropHandler =
            fun src -> 
                new DragEventHandler( fun (sender:obj) (e:System.Windows.Forms.DragEventArgs) -> 
                    //update the struct directly
                    textBox1.Text <- e.Data.GetData(DataFormats.Text).ToString())
                
        let getB4TgtDragDropHandler =
            fun src -> 
                new DragEventHandler( fun (sender:obj) (e:System.Windows.Forms.DragEventArgs) -> 
                    //update the struct directly
                    textBox1.Text <- e.Data.GetData(DataFormats.Text).ToString())

    *)

        type DndWid(tbl) as f = 
            let getRTPnl() = new Panel(Dock = DockStyle.Fill, AutoSize = true, BorderStyle = BorderStyle.Fixed3D)
            let frm = new Form(Text = "DnD ops", Visible = true, TopMost = true, WindowState = FormWindowState.Maximized, BackColor = Color.SkyBlue)
            frm.SuspendLayout()
            let lbl = new Label(Text = "Dnd Tester", AutoSize = true, Dock = DockStyle.Top)
            let cliP = new TableLayoutPanel(Dock = DockStyle.Fill, CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset, RowCount = 5, ColumnCount = colN, AutoScroll = true, BackColor = Color.Linen)
            lbl.DoubleClick.Add(fun e -> 
                printfn "tibbie tblRef:\n %A" (tblRef.Value)
                printfn "cliP ctrlCount: %A" (cliP.Controls).Count
                printfn "li:\n"
                List.mapi (fun i cl -> printfn "%A) %A" i cl) (cliP.Controls |> Seq.cast |> List.ofSeq ) |> ignore)
            frm.Controls.Add(cliP)
            frm.Controls.Add(lbl)
            frm.ResumeLayout(false)

        printfn "eom Dnd_ops..."
        
        
        
