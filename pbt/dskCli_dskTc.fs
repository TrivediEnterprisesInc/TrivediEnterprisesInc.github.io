(*  Wwnn.  HP. QP. (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 
    TrivediBldHdr->
    |Trivedi SrcCtrlHdr File.  Copyright (c) 2015-2022 M. P. Trivedi.  All rights reserved.|637901455|test.fs|none|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|

    fsc src\dskTc.fs  --platform:x64 --target:exe --out:dskTc.exe -r:lib\Trivedi.Core.dll -r:lib\Trivedi.CoreAux.dll -r:lib\Trivedi.UI.dll -r:lib\Trivedi.UIAux.dll -I:C:\Windows\Microsoft.NET\Framework\v4.0.30319

    Aug 18: Added toolBar to type Dsk
    last upd Jun 23, 2025
    Jul 15: added member wld.get(v) /set
    init work on dsk tc; so far so good...
*)

namespace Trivedi

#nowarn "20" "25" "58" "66" "67" "64" "760" "1182" "1558"

module pbtCore = 
    open System

    type Wrld() as wld = 
        inherit Map<'Key, 'Value>
        do printfn "in Wrld ctor.."
        member wld.svrSync() = 
            tibbie "svrSyncReq recd."
        member wld.reLoad() = 
            tibbie "syncToDisk recd."
        member wld.syncToDisk() = 
            tibbie "syncToDisk recd."
        member wld.get(v) = 
             //If val not present, we nd to get from svr
             //Shd we download the whole thing?  We nd logic for that case.
             //Case1: isFirstLogin Case2: Corruption/missing Case3: newClient + oldSettings
             match wld.TryGetValue(v) with
             | (false, _) -> 
                match svrGetVal(v) with    
                | (false, _) -> criticalErr("SvrKey not found for " + v.ToString())
                | (true, s) -> 
                    wld.Add(v, s)
             | (true, x) -> x
        member wld.set(a,b) = 
            let changeFn x =
                match x with
                | Some s -> Some (b)
                | None -> None
            wld.Change(a, changeFn)

    //chk if BrijEnv = "Dev"
    let wrld = Wrld()
    //defaults
    [("UsrSettingsDarkMode", box false);
    ("UsrSettingsCurrUserName", box "mike@trivedi.com");
    ("UsrSettingsCurrUserLicenseKey", box "");
    ("UsrSettingsCurrUserLicenseType", box "Full");
    ("UsrSettingsCurrBrijVer", box "0.8");
    ("UsrSettingsLocale", box "en-us");
    ("UsrSettingsLocaleStrings", box []);
    ("UsrSettingsDarkMode", box false);
    ("UsrSettingsDefaultFont", box defFont);
    ("UsrSettingsLastSync", box (DateTime(2025, 01, 01)));
    ("UsrSettingsDskSnapshot", box []);
    ("UsrSettingsDskSnapshotNeeded", box false);
    ("UsrSettingsImageSet", box []);
    ("UsrSettingsOpenTables", box []);
    ("UsrSettingsOpenTabs", box []);
    ("UsrSettingsDzDocs", box [])]]


module main = 
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
    
    let tibbie s = MessageBox.Show(s, "...to be implemented...", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
    let defFont = new Font("Tahoma", 32.0f)
    let defMinDate = new DateTime(1985, 6, 20)
    let toTSItm = fun o -> o :> ToolStripItem
    
    //let frm = Form(Visible = false, TopMost = true, WindowState = FormWindowState.Maximized, Font = defFont, AutoScroll = true)
    let getTblNmFor ty = "AdminTbl: "   //stub
    
    MenuItem dskPgClose = new MenuItem("&Close")

    type dskTabC as tc = 
        inherit TabControl()
        member tc.isTabOpen elTy tblId = 
            tc.TabPages 
                |> Seq.cast 
                |> List.ofSeq 
                |> List.tryFindIndex (fun (t:dskPg) -> (t.tblType = tblId) && (t.elType = elTy))
        member tc.isLastItm4 tblId = 
            let tot = tc.TabPages 
                        |> Seq.cast 
                        |> List.ofSeq 
                        |> List.Filter (fun (t:dskPg) -> t.tblType = tblId)
            lilen tot < 2
        member tc.RemTab tblId tb = 
            //@ToDo: nd 2 attach actions (e.g. Save/SaveAs)
            tc.TabPages.Remove(tb)
            match (isLastItm4 tblId) with
            | true -> 
                let openT = this.get("OpenTables")
                this.put("OpenTables", (List.except tb))
                this.put("OpenTabs", (tc.TabPages)
            | _ -> 
                this.put("OpenTabs", (tc.TabPages)

    type dskPg(elTy, tblTy, dskTc, dzDocId) as this = 
        inherit TabPage(Text = "brij", Multiline = true, ControlContextMenu = new ContextMenu(dskPgClose, 
        new EventHandler(fun o e -> 
            let currPg = (dskPg) o
            match currPg.isDesktop with
            | true -> tibbie "Exit this program? (Y/N)"
            | _ -> 
                let tc = (TabControl) this.Parent
                wld.RemTab tc this
                printfn "this page removed -> %A" (currPg.Text))))
        member val elType = elTy
        member val tblType = tblTy
        do printfn "in dskPg ctor.."
        let bld = 
            printfn "in bld..."
            match elTy with
            | "Desktop" -> 
                this.Text <- "Desktop"
                this.ToolTipText <- "Client Desktop"
            | "DataView" -> 
                this.Text <- "DataView"
                this.ToolTipText <- ("DataView:" + (getTblNmFor tblTy))
                let gDef = wrld.get(dzDocId)
                let g = gFromDef(gDef)
                dskTc.Controls.Add(g)
            | "Calcutti" -> 
                this.Text <- "Calcutti"
                this.ToolTipText <- ("Calcutti:" + (getTblNmFor tblTy))
                let gDef = wrld.get(dzDocId)
                let g = gFromDef(gDef)
                dskTc.Controls.Add(g)
            | "Meethoo" -> 
                this.Text <- "Meethoo"
                this.ToolTipText <- ("Meethoo:" + (getTblNmFor tblTy))
                let mDef = wrld.get(dzDocId)
                let m = mFromDef(mDef)
                dskTc.Controls.Add(m)
            | ...
        member val isDz = 
            match elTy with
            | "Dv" -> false
            | _ -> true
        member this.isDesktop = this.Text = "Desktop"

    let addDskDzTabPg = 
        fun tc tblTy -> 
            let tblNm = getTblNmFor tblTy
            let allDDox = wrld.getDzDoxFor tblNm
            tc.Controls.Add(dskPg(DzDv, tblTy, tc))

    let addDskTabPg = 
        fun tc DzDoc -> 
            match DzDoc with 
            | :? DesDoc as d -> 
                match d with
                | Saado as s ->
                    let DesDoc(unid,dispNm,docInf,SaadoMasaloPbt<'t>) = s
                    tc.Controls.Add(dskPg(Saado, 't, dskTc, unid))
                | Banarasi as b ->
                    let DesDoc(unid,dispNm,docInf,BanarasiMasaloPbt<'t>) = b
                    tc.Controls.Add(dskPg(Banarasi, 't, dskTc, unid))
                | BanarasiSub as bs -> 
                    let DesDoc(unid,dispNm,docInf,BanarasiSubMasaloPbt<'t>) = bs
                    tc.Controls.Add(dskPg(BanarasiSub, 't, dskTc, unid))
                | Meethoo as m -> 
                    let DesDoc(unid,dispNm,docInf,MeethooMasaloPbt<'t>) = m
                    tc.Controls.Add(dskPg(Meethoo, 't, dskTc, unid))
                | Calcutti as c -> 
                    let DesDoc(unid,dispNm,docInf,CalcuttiMasaloPbt<'t>) = c
                    tc.Controls.Add(dskPg(Calcutti, 't, dskTc, unid))
                | CalcuttiCard as cc ->
                    let DesDoc(unid,dispNm,docInf,CalcuttiCardMasaloPbt<'t>) = cc
                    tc.Controls.Add(dskPg(CalcuttiCard, 't, dskTc, unid))
                | ComputedFld as cf -> 
                    let DesDoc(unid,dispNm,docInf,CompFldMasaloPbt<'t>) = cf
                    tc.Controls.Add(dskPg(ComputedFld, 't, dskTc, unid))
                | LookupFld as lf -> 
                    let DesDoc(unid,dispNm,docInf,LookupFldMasaloPbt<'t>) = lf
                    tc.Controls.Add(dskPg(LookupFld, 't, dskTc, unid))
                | AgentDef as ad ->
                    let DesDoc(unid,dispNm,docInf,AgentMasaloPbt<'t>) = ad
                    tc.Controls.Add(dskPg(AgentDef, 't, dskTc, unid))
                | ConditionExprDoc as ce ->
                    let DesDoc(unid,dispNm,docInf,CondExpMasaloPbt<'t>) = ce
                    tc.Controls.Add(dskPg(ConditionExprDoc, 't, dskTc, unid))
                | IfThenExprDoc as ifT ->
                    let DesDoc(unid,dispNm,docInf,IfThenExpMasaloPbt<'t>) = ifT
                    tc.Controls.Add(dskPg(IfThenExprDoc, 't, dskTc, unid))
                | ActionExprDoc as aex ->
                    let DesDoc(unid,dispNm,docInf,ActionExpMasaloPbt<'t>) = aex
                    tc.Controls.Add(dskPg(ActionExprDoc, 't, dskTc, unid))
                | LogDoc as log ->
                    let DesDoc(unid,dispNm,docInf,LogDocMasaloPbt<'t>) = log
                    tc.Controls.Add(dskPg(LogDoc, 't, dskTc, unid))
            | _ -> () //isCateg )

    //Add to dzDV.bld(); ensure ref to tc in scope
    DesDoc.DoubleClick.AddHandler(new EventHandler(fun (sender:obj) (e:EventArgs) -> addDskTabPg dskTc sender))

    type Dsk(icnLi:(string * obj * string) list) as dsk =
        inherit Form(Visible = false, TopMost = true, WindowState = FormWindowState.Maximized, Font = defFont, AutoScroll = true) // BackColor = currentScheme.Back())
        do printfn "db: Dsk cTor"
        let mutable currIcnLi = icnLi
        let mutable openWins = []
        let mutable DskStatus = ["Desktop Ready..."]
        let pnl = new FlowLayoutPanel(BorderStyle = BorderStyle.Fixed3D, FlowDirection = FlowDirection.BottomUp, WrapContents = true, Dock = doc "F") //, BackColor = currentScheme.Back())
        let dskTc = new dskTabC()
        let dskTS = new ToolStrip(Dock = doc "T")
        let kathoDskTab = 
            printfn "db: DskTab katho"
            pnl.SuspendLayout()
            dsk.SuspendLayout()
            let stat = new StatusStrip(SizingGrip = false, Stretch = true, Dock = doc "B", Font = new Font("Tahoma", 18.0f), LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow)
            let statLbl = new ToolStripStatusLabel(Text = "Desktop Ready...", AutoToolTip = false) :> ToolStripItem
            let dskCtxtMS = new ContextMenuStrip()
            let AddIconMenuItem = new ToolStripMenuItem("Add Item")
            let RemoveIconMenuItem = new ToolStripMenuItem("Remove Item(s)")
            let tmpMenuItm = new ToolStripMenuItem("tmp")
#if remmedJun2025
            !!^ ["Frm", box dsk ] dskCtxtMS
#endif //remmedJun2025

            tmpMenuItm.Click.AddHandler(new EventHandler(fun (sender:obj) (e:EventArgs) ->
                        let ch:int = pnl.Controls |> Seq.cast |> List.ofSeq |> lilen
                        tibbie ("# of icns -> " + ch.ToString())))
            dskCtxtMS.Items.AddRange([|toTSItm tmpMenuItm;toTSItm AddIconMenuItem;toTSItm RemoveIconMenuItem|])

            let cLbl = new ToolStripStatusLabel(Text = "s", Dock = doc "R", Alignment = ToolStripItemAlignment.Right, AutoToolTip = false) :> ToolStripItem
#if remmedJun2025
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
#endif //remmedJun2025
            stat.Items.AddRange([|statLbl; cLbl|]) |> ignore
            //added Aug18 + member getDskTS() for local TS support
            dsk.Controls.Add( (new ToolStripContainer()).TopToolStripPanel.Controls.Add(dskTS))
            dsk.Controls.Add(ms)
            dsk.Controls.Add(pnl)
            dsk.Controls.Add(stat)
            //This shd be blt from the wrld entries; but we nd to pass the dskTc ref...
            dskTc.Controls.Add(dskPg("Dv", AdminTbl()))
            dskTc.Controls.Add(dskPg("MeethooDef", AdminTbl()))
            dsk.Controls.Add(dskTc)
            dsk.ContextMenuStrip <- dskCtxtMS
#if remmedJun2025
            !!^ ["Status", box stat; "dskCtxtMS", box dskCtxtMS; "ms", box ms; "flowPnl", box pnl] dsk
#endif //remmedJun2025


        let chunoDskTab =
            printfn "db: DskTab chuno"
            //stopgap manual pop
            let userNm = "Env.getUser()"
#if remmedJun2025
            let pnl:FlowLayoutPanel = (!!~ "flowPnl" dsk).Value
#endif //remmedJun2025
            //let (Dsk(userNm, icnLi)) = //(getDesktopFile uNm)
            do printfn "db: Dsk begin proc icnLi"
            lim (fun icnItm -> 
                    let (icnNm, ty, slug) = icnItm
                    printfn "...nm: %A slug: %A ..." icnNm  slug
                    let tblT = unbox ty
                    let icnP = Panel(Margin = new Padding(25), BorderStyle = BorderStyle.None)
                    let img:Image = Image.FromFile(Path.Combine("C:\\Users\\inets\\Documents\\mike\\src\\Data\images\\desktop", (icnNm:string).ToLower().Trim()))
                    let icnLbl = new Label(Dock = doc "T", Image = img, ImageAlign= ContentAlignment.TopCenter) //, BackColor = currentScheme.Back())
                    let txtLbl = new Label(Dock = doc "B", Text = slug, TextAlign = ContentAlignment.BottomCenter) //, ForeColor = currentScheme.accentFore(), BackColor = currentScheme.Back())
                    icnLbl.Paint.Add(fun (e:PaintEventArgs)  ->
                        //base.OnPaint(e)
                        let imageAttributes = new ImageAttributes()
                        let width = img.Width
                        let height = img.Height
                        let colorMap = new ColorMap()
                        colorMap.OldColor <- Color.Black
#if remmedJun2025
                        colorMap.NewColor <- currentScheme.Icn()
#endif //remmedJun2025
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
#if remmedJun2025
                    let tblID = (tblT.GetType().ToString()).Substring((tblT.GetType().ToString()).IndexOf(".")+1)
                    !!^ ["tblID", box tblID] icnP
                    !!^ [tblTy, box icnP] dsk
#endif //remmedJun2025
                    let dskOpenHnd = 
                        new EventHandler(fun (sender:obj) (e:EventArgs) ->
                            printfn "dskOpenHandler"
#if remmedJun2025
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
                                | "TableDesign" -> ()
#endif //remmedJun2025
                                )
                    icnLbl.DoubleClick.AddHandler(dskOpenHnd)
                    txtLbl.DoubleClick.AddHandler(dskOpenHnd)
                    icnP.DoubleClick.AddHandler(dskOpenHnd)
                    let OpenMenuItem = new ToolStripMenuItem("Open")
                    let DesignDVMenuItem = new ToolStripMenuItem("Open in Design Mode")
                    let NewCopyMenuItem = new ToolStripMenuItem("New Copy")
                    let RemoveMenuItem = new ToolStripMenuItem("Remove")
                    icnCtxt.Items.AddItems([|OpenMenuItem;DesignDVMenuItem;NewCopyMenuItem;RemoveMenuItem|]) |> ignore
                    OpenMenuItem.Click.AddHandler(new EventHandler(fun (sender:obj) (e:EventArgs) -> tibbie "OpenMenuItem"))
                    DesignDVMenuItem.Click.AddHandler(new EventHandler(fun (sender:obj) (e:EventArgs) ->
                            //Using the new types...; ONLY for DV/Dz; rest in Dz.click
                            match (dskTc.isTabOpen "DesignView" tblId) with
                            | -1 -> dskTc.Controls.Add(dskPg("DesignView", tblID))
                            | _ as t -> dskTc.SelectTab(t)))
                    NewCopyMenuItem.Click.AddHandler(new EventHandler(fun (sender:obj) (e:EventArgs) -> 
                            DlgFileNewCopy (Some(tblId)) ))
                    RemoveMenuItem.Click.AddHandler(new EventHandler(fun (sender:obj) (e:EventArgs) -> 
                            let dlg = TxtDlg "Are you sure"
                            if dlg.ShowDialog() = DialogResult.OK then
                               removeIcon icnNm))
                    icnP.Controls.Add(icnLbl)
                    icnP.Controls.Add(txtLbl)
                    pnl.Controls.Add(icnP)
                ) currIcnLi |> ignore 
            do printfn "db: ટેબલ_પાન_Nov done proc icnLi"
        let kathoOpenTabs = 
            printfn "db: OpenTabs katho"
            Wrld.get("OpenTabs")
            |> lim (fun ot -> 
                      let (boxedDzDoc) = ot
                      addDskTabPg dskTc boxedDzDoc) |> ignore
        pnl.ResumeLayout(false)
        dsk.ResumeLayout(false)
        dsk.Show()
        member getDskToolStrip() = dskTS
        member removeIcon(nm) = 
            let newState = 
                currIcnLi |> List.filter (fun icn -> 
                                        let (icnNm, ty, slug) = icn
                                        cnNm = nm)
            dsk.currIcnLi <- newState
            dsk.Invalidate()
    
    let getDzEls() = 
        dskTc.TabPages
        |> Seq.cast
        |> List.ofSeq
        |> List.filter(fun (x:dskPg) -> x.isDz)

    let addTabs() = 
        dskTc.Controls.Add(dskPg("Dv", AdminTbl()))
        dskTc.Controls.Add(dskPg("MeethooDef", AdminTbl()))
        //@mbi dskTc.Controls.Add(dskPg("SaadooDef", TaskTbl()))
