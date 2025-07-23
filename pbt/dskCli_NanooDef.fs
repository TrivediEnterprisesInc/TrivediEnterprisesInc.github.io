(*  Brij (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 
    TrivediBldHdr->
    |Trivedi SrcCtrlHdr File.  Copyright (c) 2015-2025 M. P. Trivedi.  All rights reserved.|637901455|test.fs|none|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS 

    //minus UIAux
    fsc src\pbt\Dnd_ops.fs  --platform:x64 --standalone --target:exe --out:src\pbt\dnd.exe -r:lib\Trivedi.Core.dll -r:lib\Trivedi.UI.dll -I:C:\Windows\Microsoft.NET\Framework\v4.0.30319

    Last updated: Fri Jul 11 2025 (added recTys 4 tpls + TabPgTys)
                  Thu Jul 17 2025 (added scatter/fromDef/toDef)

    Contains modules:      NanooDef_Actual | NanooDef_Test

*)

namespace Trivedi

#nowarn "20" "25" "58" "66" "67" "64" "760" "1182" "1558"


[<AutoOpen>]
module NanooDef_Actual =
    open System
    open System.Drawing
    open System.Windows.Forms
    open Trivedi
    open Trivedi.Core
    //open Trivedi.UI
    //open Trivedi.UIAux
    open System.Windows.Forms

    type old_NanooMasalo<'t when 't :> ITblMarker> = | NanooMasalo of unid:DocUNID * dispNm:string * isImgVw:bool * titleFld:DocFld * cntrFld:DocFld * botFld:DocFld * usrDefDataFont:Font * usrDefForeColor:Color * usrDefBackColor:Color * docInf:DesDocInf with
        static member toString() = 
            printfn "placeholder..."

    type NanooMasalo =
        { unid:DocUNID; mutable dispNm:string; mutable isImgVw:bool; mutable titleFld:DocFld; mutable cntrFld:DocFld; mutable botFld:DocFld; mutable usrDefDataFont:Font; mutable usrDefForeColor:Color; mutable usrDefBackColor:Color; mutable docInf:DesDocInf
        }
        static member getDefault(saado:SaadoMasaloAux) =
            let (SaadoMasaloAux(tNm, TblFldList, tblIcn, tblTy)) = saado
            { unid = (getUnid "*" + getTblNm)
              dispNm = "Default CardView: " + getTblNm
              isImgVw = false
              titleFld = TblFldList.[0]
              cntrFld = TblFldList.[1]
              botFld = TblFldList.[2]
              usrDefDataFont = defFont
              usrDefForeColor = defForeColor
              usrDefBackColor = defBackColor
              docInf = DesDocInf.getDefault() }

    //@ToDo: DesDocInf nds members 2 update (consider a record ty there 2?)

    let loadFlds2Combo =
        fun fLi combo ->
            match TblFldList with
            | [] -> ()  //@ToDo: catch this condition in ctor OR disable ctor
            | _ ->  
                fLi
                |> List.filter (fun f ->
                    let (DocFld(t, slg, isInt, nm)) = f 
                    not(isInt))
                |> lim (fun f -> 
                            let (DocFld(_, _, _, nm)) = f
                            combo.Items.Add(nm)) 
                |> ignore

    //These fns extracted from tyFldPnl coz on occasion (we've already encountered this in phaps tblDef) we nd only the button; not a fldPanel
    //This fn nds to be internalized for both
    //@NeedsTesting
    | FldType.FldFont ->
        let fontBtn = new Button(Text = defFont.FontFamily.Name.ToString() + " " + defFont.Size.ToString() + " " + defFont.Style.ToString() , Dock = doc "F", BackColor = Color.White)
        let fontDlg = new FontDialog(ShowColor = true, Font = defFont, Color = lbl.BackColor)
        fontBtn.Click.AddHandler(new EventHandler(fun o e -> 
                if (fontDlg.ShowDialog() = DialogResult.OK) then 
                    fontBtn.Font <- fontDlg.Font
                    fontBtn.BackColor <- fontDlg.Color 
                    fontBtn.Text <- fontDlg.Font.FontFamily.Name.ToString() + " " + fontDlg.Font.Size.ToString()
                    else ()))
        tagged defaultTags (fontBtn :> Control)
        let tagged = 
            fun tL (c:Control) -> 
                !!^ tL c
                c

    let getNanooFontBtn def bossCntrl = 
        let curr = def.usrDefDataFont
        let fontBtn = new Button(Text = deflt.FontFamily.Name.ToString() + " " + deflt.Size.ToString() + " " + deflt.Style.ToString() , Dock = doc "F")
        let fontDlg = new FontDialog(ShowColor = true, Font = defFont, Color = deflt)
        fontBtn.Click.AddHandler(new EventHandler(fun o e -> 
                if (fontDlg.ShowDialog() = DialogResult.OK) then 
                    def.usrDefDataFont <- fontDlg.Font
                    fontBtn.Font <- fontDlg.Font
                    fontBtn.BackColor <- fontDlg.Color 
                    fontBtn.Text <- fontDlg.Font.FontFamily.Name.ToString() + " " + fontDlg.Font.Size.ToString()
                    else ()))
        !!^ ["FontBtn", box fontBtn] bossCntrl
        fontBtn

    let getColBtn def tg bossCntrl = 
        //let ForeBtn = getColBtn usrDefForeC "ForeBtn" NanooPg
        //in handler match on tg


    type NanooAppCvPg(def, bossCtrl) = 
        inherit TabPage(Text = "Appearance")
        let AppMidP:TableLayoutPanel = new TableLayoutPanel(Margin = new Padding(25), RowCount = 1, ColumnCount = 2, Dock = doc "F", Name="AppMidP", BackColor = Color.White)
        let instrText = "If you wish, you may customize the Font and Colors used for the CardDataView on this tab."
        let AppInstrLbl = new Label(Text = instrText, Size = (new Size(DatCvImg.Width, DatCvImg.Height)), Anchor = anc "N")
        this.Controls.Add(AppInstrLbl)

        AppMidP.ColumnCount <- 1 
        AppMidP.RowCount <- lilen styLi
        AppMidP.Controls.Clear()

        //DisplayName
        let nmPnl = new FlowLayoutPanel(Dock = doc "F", FlowDirection = FlowDirection.LeftToRight, AutoSize = true)
        let NmLbl = new Label(Text = "Display Name:", Dock = doc "F")
        let NmFld = new TextBox(Text = def.dispNm)
        NmFld.OnTextChanged.AddHandler(new EventHandler(fun o e -> 
            def.dispNm <- NmFld.Text))
        fPnl.Controls.AddItems([FontLbl, FontBtn])
        AppMidP.Controls.Add(nmPnl)

        //Font
        let fPnl = new FlowLayoutPanel(Dock = doc "F", FlowDirection = FlowDirection.LeftToRight, AutoSize = true)
        let FontLbl = new Label(Text = "Font Face and Size", Dock = doc "F")
        let FontBtn = getNanooFontBtn def "FontBtn" NanooPg
        fPnl.Controls.AddItems([FontLbl, FontBtn])
        AppMidP.Controls.Add(fPnl)
        //ForeCol
        let forePnl = new FlowLayoutPanel(Dock = doc "F", FlowDirection = FlowDirection.LeftToRight, AutoSize = true)
        let ForeLbl = new Label(Text = "Text color for all CardView fields", Dock = doc "F")
        let ForeBtn = getColBtn usrDefForeC "ForeBtn" NanooPg
        forePnl.Controls.AddItems([ForeLbl, ForeBtn])
        AppMidP.Controls.Add(forePnl)
        //backCol
        let backPnl = new FlowLayoutPanel(Dock = doc "F", FlowDirection = FlowDirection.LeftToRight, AutoSize = true)
        let BackLbl = new Label(Text = "CardView Background Color", Dock = doc "F")
        let BackBtn = getColBtn usrDefBackC "BackBtn" NanooPg
        backPnl.Controls.AddItems([BackLbl, BackBtn])
        AppMidP.Controls.Add(backPnl)
        this.Controls.Add(AppMidP)


    let getImgCvPg d =
        let ImgCvPg = new TabPage (Text = "ImageCv")
        let ImgCvTp = new TableLayoutPanel(Dock = DockStyle.Fill, CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset, RowCount = 3, ColumnCount = 2, AutoScroll = true, BackColor = Color.Linen)
        let ImgCvImg = (getImg NanooImgCv)
        let ImgCvLbl = new Label(Image = ImgCvImg, Size = (new Size(ImgCvImg.Width, ImgCvImg.Height)), Anchor = anc "N", BackColor = Color.Transparent, ForeColor = (currentScheme ((!!~ "wld" dsk).Value)).Icn())
        ImgCvTp.Controls.Add(ImgCvLbl, 0, 0)
        ImgCvTp.SetRowSpan(ImgCvLbl, 3)
        let iTitCombo = new ComboBox(Name = "iTitCombo", Dock = doc "F")
        iTitCombo.SelectedIndexChanged.AddHandler(new EventHandler(fun o e ->
            def.isImgVw <- true
            def.titleFld <- (string) iTitCombo.SelectedItem))
        loadFlds2Combo TblFldList iTitCombo
        //@Chk: poss Unnecc.  titCombo.ResetText() //unselects
        ImgCvTp.Controls.Add(iTitCombo, 1, 0)
        let iMidCombo = new ComboBox(Name = "iMidCombo", Dock = doc "F")
        iMidCombo.SelectedIndexChanged.AddHandler(new EventHandler(fun o e ->
            def.isImgVw <- true
            def.cntrFld <- (string) iMidCombo.SelectedItem))
        loadFlds2Combo TblFldList iMidCombo
        ImgCvTp.Controls.Add(iMidCombo, 1, 1)
        let iBotCombo = new ComboBox(Name = "iBotCombo", Dock = doc "F")
        iBotCombo.SelectedIndexChanged.AddHandler(new EventHandler(fun o e ->
            def.isImgVw <- true
            def.botFld <- (string) iMidCombo.SelectedItem))
        loadFlds2Combo TblFldList iBotCombo
        ImgCvTp.Controls.Add(iBotCombo, 1, 2)
        ImgCvPg.Controls.Add(ImgCvTp)

    let getDatCvPg d =
        let DatCvPg = new TabPage (Text = "DataCv")
        let DatCvTp = new TableLayoutPanel(Dock = DockStyle.Fill, CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset, RowCount = 3, ColumnCount = 2, AutoScroll = true, BackColor = Color.Linen)
        let DatCvImg = (getImg NanooDatCv)
        let DatCvLbl = new Label(Image = DatCvImg, Size = (new Size(DatCvImg.Width, DatCvImg.Height)), Anchor = anc "N", BackColor = Color.Transparent, ForeColor = (currentScheme ((!!~ "wld" dsk).Value)).Icn())
        DatCvTp.Controls.Add(DatCvLbl, 0, 0)
        DatCvTp.SetRowSpan(DatCvLbl, 3)
        let dTitCombo = new ComboBox(Name = "dTitCombo", Dock = doc "F")
        dTitCombo.SelectedIndexChanged.AddHandler(new EventHandler(fun o e ->
            def.isImgVw <- false
            def.titleFld <- (string) dTitCombo.SelectedItem))
        loadFlds2Combo TblFldList dTitCombo
        //@Chk: poss Unnecc.  titCombo.ResetText() //unselects
        DatCvTp.Controls.Add(dTitCombo, 1, 0)
        let dMidCombo = new ComboBox(Name = "dMidCombo", Dock = doc "F")
        dMidCombo.SelectedIndexChanged.AddHandler(new EventHandler(fun o e ->
            def.isImgVw <- false
            def.cntrFld <- (string) dTitCombo.SelectedItem))
        loadFlds2Combo TblFldList dMidCombo
        DatCvTp.Controls.Add(dMidCombo, 1, 1)
        let dBotCombo = new ComboBox(Name = "dBotCombo", Dock = doc "F")
        dBotCombo.SelectedIndexChanged.AddHandler(new EventHandler(fun o e ->
            def.isImgVw <- false
            def.botFld <- (string) dTitCombo.SelectedItem))
        loadFlds2Combo TblFldList dBotCombo
        DatCvTp.Controls.Add(dBotCombo, 1, 2)
        DatCvPg.Controls.Add(DatCvTp)

    type NanooPaan<'t when 't :> ITblMarker>(saado<'t>, def, dsk) as n = 
        inherit Form(WindowState = FormWindowState.Maximized, Visible = false, Text = "Nanoo Dz Form: Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved.", TopMost=true)
        do printfn "NanooPaan ctor..."
        let mutable nm = dispNm
        let (SaadoMasaloAux(tNm, TblFldList, tblIcn, tblTy)) = saado
        do n.SuspendLayout()
        let NanooPg = new TabPage (Text = "NanooPaan: " + getCurrTblTy)
        let setupPg = 
            //titleP
            //@TBD: This is from gappa; shd we pull it out? 
            //      doc diff. so perhaps a 'let getTitleP = fun docVal ->'
            let icnLbl = new Label(Image = brijLogo, Size = (new Size(brijLogo.Width, brijLogo.Height)), Anchor = anc "N", BackColor = Color.Transparent, ForeColor = (currentScheme ((!!~ "wld" dsk).Value)).Icn())
            let titTxt = new TextBox(AutoSize = true, Dock = doc "T", Enabled = false, Text = "Meethoo Def Document for " + nm, ReadOnly = true, Multiline = false, Width = f.Width - 50, TextAlign = HorizontalAlignment.Center, BorderStyle = BorderStyle.None, ForeColor = (currentScheme ((!!~ "wld" dsk).Value)).titFore(), BackColor = (currentScheme ((!!~ "wld" dsk).Value)).titBack())
            let ChooseTxt = new TextBox(AutoSize = true, Dock = doc "T", Enabled = false, Text = "You may choose to build a CardView with or without an image by selecting a tab below:", ReadOnly = true, Multiline = true, Width = f.Width - 20, TextAlign = HorizontalAlignment.Center, BorderStyle = BorderStyle.None, ForeColor = (currentScheme ((!!~ "wld" dsk).Value)).titFore(), BackColor = (currentScheme ((!!~ "wld" dsk).Value)).titBack())
            let titleP = new TableLayoutPanel(RowCount = 1, ColumnCount = 5, Dock = doc "T", BackColor = (currentScheme ((!!~ "wld" dsk).Value)).titBack(), AutoSize = true, Width = f.Width , Height = ((int) (titTxt.Height * 3)))
            titleP.SuspendLayout()
            titleP.RowStyles.Clear()
            titleP.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f))
            titleP.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f))
            titleP.Controls.Add(icnLbl, 0, 0)
            titleP.Controls.Add(titTxt, 1, 0)
            titleP.SetColumnSpan(titTxt, 4)
            titleP.Controls.Add(ChooseTxt, 0, 1)
            titleP.SetColumnSpan(ChooseTxt, 5)
            titleP.ResumeLayout(false)
            NanooPg.Controls.Add(titleP)
            let NanooTc = new TabControl(Dock = doc "F", Width = f.Width - defPadding.Horizontal, Height = f.Height - (defPadding.Horizontal * 8))
            NanooTc.SelectTab(0)
            NanooPg.Controls.Add(NanooTc)
            NanooTc.Controls.Add([(getAppCvPg NanooPg); (getImgCvPg NanooPg); (getDatCvPg NanooPg)])
            n.addTB()
        do (dsk.getTc()).Controls.Add(NanooPg)
        member val currDef = d with get, set
        member val dskRef = dsk with get, set
        member addTB() = 
            let dskTB = dsk.getTB()
            dskTB.Controls.Clear()
            addNanooTBItms n //dskTB from n.dskRef
            let SaveBtn = tibbie "same as below"
            let SaveAsBtn = 
                match def.isImgView with
                | _ -> gappa "Must be selected (do we nd 2 chk fldVals here?)"
                | true | false -> tibbie "regular flow persist"

(* Jul 17: No longer necc; fromDef is auto-loaded + toDef is now onChange
    Note: WE DO nd 2 handle the reqd items on Save/SaveAs, which shd occur on tbBtn.click

        interface IDef with
            member this.fromDef() = 
                this.currDef <- fnFromDef this.currDef
            member this.toDef cmdTy = 
                //this.currDef <- fnToDef this.currDef
                let allImgFldsSel = false //tbfo
                let allDatFldsSel = false //tbfo
                match cmdTy with
                | "SaveAs" -> 
                    this.nm <- gappaInf nm "Pls provide the new name"
                | _ -> 
                    match (allImgFldsSel, allDatFldsSel) with
                    | (true, false) || (false, true) -> 
                        //we only want one set selected
                        let cvTy = allImgFldsSel ? "img" : "dat"
                        match cvTy with
                        | "img" -> 
                            currDef <- NanooMasalo(unid, nm.Value, true, iTitCombo.Text, iMidCombo.Text, iBotCombo.Text, usrDefDataF, usrDefForeC, usrDefBackC, docInf)
                        | _ -> 
                            currDef <- NanooMasalo(unid, nm.Value, false, dTitCombo.Text, dMidCombo.Text, dBotCombo.Text, usrDefDataF, usrDefForeC, usrDefBackC, docInf)
                        match cmdTy with
                        | "Preview" -> // launchPreview elTy currDef.Value
                        | _ ->  //@TBFO saveCmd, saveToDsk currDef.Value
                    | _ -> gappa "Please select all required data fields"
*)






