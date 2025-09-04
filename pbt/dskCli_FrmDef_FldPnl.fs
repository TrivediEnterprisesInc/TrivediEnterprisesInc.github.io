(*  Brij (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 
    TrivediBldHdr->
    |Trivedi SrcCtrlHdr File.  Copyright (c) 2015-2025 M. P. Trivedi.  All rights reserved.|637901455|test.fs|none|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|

    fsc src\pbt\pbt_AI_Dsk.fs src\pbt\pbt_DvDef.fs  --platform:x64 --standalone --out:src\pbt\Trivedi.PbtDvDef.dll -r:lib\Trivedi.Core.dll -r:lib\Trivedi.CoreAux.dll -r:lib\Trivedi.UI.dll -r:lib\Trivedi.UIAux.dll -I:C:\Windows\Microsoft.NET\Framework\v4.0.30319
   
    Notes:
      ****Consider that not covering ALL fldTys will save effort; in that case instd. of partialPatts we just repl w/standard wid (with approp len?) and expect the dev 2 hit Preview...
      
      Purpose: To replace the existing type ફીલ્ડ_પેનલ
      - Instd of pattMatching use ActPatts
      - Ensure that defaultWid exists forEa fldTy; popul8 w/defVals
      - Use a tmp TC (retrofit l8r to propBox) with tabs 4
        Appearance (Fonts/Colors)
        Wids (Offer all choices, default sel)  Ensure that all params r supplied
          This goes into a radioGrp w/Pnls 4 ea choice; 1st col snap 2nd desc
        @TBD: popup forEa indiv wid?  (more space allows better expl.)
              also, a btn phaps to pull up the popup/readOnly dspFld to show?

    Created: Wed Aug 27 2025
    Last updated: 
    
    Contains modules:  FrmDz_Actual
*)

namespace Trivedi

#nowarn "20" "25" "58" "66" "67" "64" "760" "1182" "1558"

[<AutoOpen>]
module FrmDz_Actual = 
  open pbtCore

#if forRef
type BMfld<'t when 't :> ITblMarker> = { unid:DocUNID; title:string ; docF:DocFld; 
                                        colSpan:int ; rowSpan:int ; Pos:(int * int);
                                        lblFont:Font ; dataFont:Font ; foreCol:Color option; backCol:Color option  ; soopari:obj;
                                        vFn:('t -> bool) option ; CanUhear:Thingy ; fldTtip:string option; tblTy:'t } with

//updated Jul_17_25, refactored to rec comme nanoo
type BanarasiMasaloAux<'t when 't :> ITblMarker> = 
{ unid:DocUNID; mutable dispNm:string; mutable સુપારી:int;
  mutable usrFlds:BMdzTbl; mutable usrDefLblFont:Font; 
  mutable usrDefDataFont:Font; mutable usrDefForeColor:Color; 
  mutable usrDefBackColor:Color; mutable docInf:DesDocInf } with
#endif //forRef

//added Aug28th2025
type fldPnlPopup (nds 2 be a gappa) = 
    //@ToDo: We don't nd this tc; to be reconciled w/propBox
    let tc = new TabControl(Dock = doc "F", Width = f.Width - defPadding.Horizontal, Height = f.Height - (defPadding.Horizontal * 8))
    let BMref = //get BMref from the underlying tblDz
    let thisFldRef = //get
    let AppearancePg = new TabPage (Text = "Appearance")
    let UXPg = new TabPage (Text = "UX")
    
    //AppearancePg
    let AppMidP:TableLayoutPanel = new TableLayoutPanel(Margin = new Padding(25), RowCount = 1, ColumnCount = 2, Dock = doc "F", Name="AppMidP", BackColor = Color.White)
    AppMidP.ColumnCount <- 1
    AppMidP.Controls.Clear()

(*
  Sept02: 
  * NOTE that for both of these we nd pickLists pop via fn:
    let getDefaultTblColors/Fonts
  * Note also that the colors nd swatches for user UX.
*)

    let p0 = (ફીલ્ડ_પેનલ  ("Label Font", FldFont, "no Slug", 2)) :> Panel
    let p1 = (ફીલ્ડ_પેનલ  ("Data Font", FldFont, "no Slug", 2)) :> Panel
    let p2 = (ફીલ્ડ_પેનલ  ("Cell ForeColor", FldColor, "no Slug", 2)) :> Panel
    let p3 = (ફીલ્ડ_પેનલ  ("Cell BackColor", FldColor, "no Slug", 2)) :> Panel
    AppMidP.ColumnStyles.Clear()
    AppMidP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.0f))
    AppMidP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.0f))
    AppMidP.Controls.Add(p0, 0, 0)
    AppMidP.Controls.Add(p1, 0, 1)
    AppMidP.Controls.Add(p2, 0, 2)
    AppMidP.Controls.Add(p3, 0, 3)
    AppearancePg.Controls.Add(AppMidP)

    //@ToDo: getting/clearing handlers is compl in winFrms so on dlgDismiss we'll chk if anything has changed, and upd8 the BMref _FOR THIS FLD_ (note that that is ctxt we have access to coz this appears on rtClick) For the UX pg, ensure that currChecked updates the currSelWid

    //UXPg

(*   Sept03: 
  - let getInfo returns a hardCoded tbl containing (per wid):
    | Snapshot+WidNm | Desc | Params |
    INSTD of tbl we can also xtnd the widTy with (getInfoForWid w)
  - let getWidTblPnl = calls getInfo
  - FldTy >> getWids >> toList >> lim (getWidTblPnl)
*)

    let UXMidP:TableLayoutPanel = new TableLayoutPanel(Margin = new Padding(25), RowCount = 1, ColumnCount = 2, Dock = doc "F", Name="UXMidP", BackColor = Color.White)
    UXMidP.ColumnCount <- 1
    UXMidP.Controls.Clear()

    let lbl = new Label(Text = ("Please select the default UX for this field from the available choices:"), Dock = doc "F")
    let widGrpBox = new GroupBox()
    let currSelWid = thisFldRef.CanUhear //cld be Fld.getDefThingy
    let mutable currChecked = currSelWid
    thisFldRef.DocF.getDefThingies()
    |> lim (fun wid -> 
      let (wImg, wNm, wDesc, paramPnl) = getInfo wid
      let radioB = new RadioButton(TextImageRelation = TextImageRelation.ImageBeforeText, Text = wNm, Img = wImg)
      radioB.CheckChanged.Add(new EventHandler(fun o e ->
        //@ToDo: openDlg for this wid to getVals
        //if Cancel -> ignore else upd8 def w/parameterized Wid + proceed
        currChecked <- wid))
      //we're going to bld the whole row here...
      let widPnl = new FlowLayoutPanel(Dock = doc "F", FlowDirection = FlowDirection.LeftToRight, AutoSize = true)
      let descLbl = new TextBox(AcceptsReturn=true, AcceptsTab=true,Dock=doc "F", Multiline=true, Text = wDesc)
      //paramPnl will ret a fully cfg'd pnl for the specific case -- make it so.
      widPnl.Controls.AddItems([|radioB, descLbl, paramPnl|])
      widGrpBox.Controls.Add(widPnl)) |> ignore
    
    UXMidP.ColumnStyles.Clear()
    UXMidP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.0f))
    UXMidP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.0f))
    UXMidP.Controls.Add(lbl, 0, 0)
    UXMidP.Controls.Add(widGrpBox, 0, 1)
    UXPg.Controls.Add(UXMidP)

   





  //common helper fn
  let bldPnl ctrl bossCtrl =
    let fldFP = new TableLayoutPanel(Margin = new Padding(25), RowCount = 1, ColumnCount = 2, Dock = doc "F", Name="TablePanel", BackColor = Color.White)
    fldFP.SuspendLayout()
    let defaultTags = ["fldNm", box slg; "c", box 1; "r", box 1; "o", box 0]
    let lbl = new Label(Text = (nm + ":"), Dock = doc "F")
    fldFP.Controls.Add(lbl, 0, 0)
    fldFP.Controls.Add(ctrl, 1, 0)
    !!^ [nm, box ctrl] p
    fldFP.ColumnStyles.Clear()
    fldFP.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, float32 (getBtnWid lbl.Text)))
    fldFP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f))
    fldFP.ResumeLayout(false)

  let (|PnlInfoBox|_|) params bossCtrl fTy wTy =
    match fTy with
    | FldType.FldInfoBox -> 
      let lbl = new Label(Text = slg, Dock = doc "F")
      bossCtrl.Controls.Add(lbl)
      !!^ [nm, box lbl] bossCtrl
      Some ()
    | _ -> None

  let (|PnlLongString|_|) params fTy wTy =
    match fTy, wTy with
    | FldType.FldLongString, wTy -> 
        let ctrl = 
          tagged ["fldNm", box slg; "c", box સુપારી; "r", box 5; "o",box 0 ] (new TextBox(Multiline=true, Dock = doc "F", Font = defFont, WordWrap = false, ScrollBars = ScrollBars.Both, AcceptsReturn=true) :> Control) //Height = (getCtrlHt() * 5),
        Some (bldPnl ctrl)
    | _ -> None



  let getPnlFor params fTy wTy =
    match (params,fTy,wTy) with
    | PnlInfoBox i -> i
    | Float f -> printfn "%f : Floating point" f
    | _ -> printfn "%s : Not matched." str

  //this ty is extended to supply only the resizing behavior
  //@TBD: shd we move the dnd stuff here l8r? Cleaner?
  type ResizeablePnl<'t when 't :> ITblMarker> (def:docF<'t>, widTy, bossCtrl, સ્તિતિ) as frmP =
    inherit Panel(Dock = doc "F")
    let mutable currDef = def
    do p.SuspendLayout()

  //example call: 
  //let p0 = (ફીલ્ડ_પેનલ  ("Table Display Name", UserInput, "no Slug", 2)) :> Panel
#if forRef
    type ફીલ્ડ_પેનલ (nm, fTy, slg, સુપારી)  as p =
        inherit Panel(Dock = doc "F")
        do p.SuspendLayout()
        let bld =
            hr()
            //printfn "ફીલ્ડ_પેનલ for nm:%A fTy:%A slg:%A" nm fTy slg
            if fTy = FldType.FldInfoBox then
                let lbl = new Label(Text = slg, Dock = doc "F")
                p.Controls.Add(lbl)
                !!^ [nm, box lbl] p
              else
                let fldFP = new TableLayoutPanel(Margin = new Padding(25), RowCount = 1, ColumnCount = 2, Dock = doc "F", Name="TablePanel", BackColor = Color.White)
                fldFP.SuspendLayout()
                let defaultTags = ["fldNm", box slg; "c", box 1; "r", box 1; "o", box 0]
                let lbl = new Label(Text = (nm + ":"), Dock = doc "F")
                let ctrl = 
                    match fTy with
                    | FldType.FldLongString ->
                        tagged ["fldNm", box slg; "c", box સુપારી; "r", box 5; "o",box 0 ] (new TextBox(Multiline=true, Dock = doc "F", Font = defFont, WordWrap = false, ScrollBars = ScrollBars.Both, AcceptsReturn=true) :> Control) //Height = (getCtrlHt() * 5),
                    | FldType.FldBlankRow ->
                        tagged defaultTags (new TextBox(Dock = doc "F") :> Control)
                    | FldType.FldColor ->
                        let colBtn = new Button(Text = "Color", Dock = doc "F", BackColor = defBackColor)
                        let colDlg = new ColorDialog(AllowFullOpen = true, ShowHelp = true, Color = RoyalBlue)
                        colBtn.Click.AddHandler(new EventHandler(fun o e -> 
                                if (colDlg.ShowDialog() = DialogResult.OK) then 
                                    colBtn.BackColor <- colDlg.Color else ()))
                        tagged defaultTags (colBtn :> Control)
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
                    | FldType.FldDate ->                    
                        tagged defaultTags (new DateTimePicker(Dock = doc "F", DropDownAlign = LeftRightAlignment.Right, MinDate = defMinDate, ShowUpDown = false, ShowCheckBox = false, CustomFormat = "MMM dd, yyyy", Format = DateTimePickerFormat.Custom) :> Control)
                    | FldType.FldBoolean ->
                        tagged defaultTags (new CheckBox() :> Control)
                    | FldType.FldRange ->
                        let trP = new FlowLayoutPanel(Dock = doc "F", FlowDirection = FlowDirection.LeftToRight, AutoSize = true)
                        let tBr = new TrackBar(Maximum = 9,Minimum=0,TickFrequency = 1,LargeChange = 3,SmallChange = 1)
                        let tLbl = new Label(Text = (tBr.Value.ToString()), AutoSize = true, Dock = doc "R",  TextAlign = ContentAlignment.MiddleRight)
                        tBr.Scroll.Add(fun e -> tLbl.Text <- tBr.Value.ToString())
                        trP.Controls.Add(tBr)
                        trP.Controls.Add(tLbl)
                        tagged defaultTags trP
                    | FldType.UserInput ->
                        let c = (new TextBox(Dock = doc "F") :> Control)
                        tagged ["fldNm", box slg; "inpt", box c; "c", box સુપારી; "r", box 1; "o",box 0 ] c
                    | _ ->
                        tagged defaultTags (new TextBox(Dock = doc "F") :> Control)
                match fTy = FldType.FldBlankRow with
                | true -> ()
                | _ -> 
                    fldFP.Controls.Add(lbl, 0, 0)
                    fldFP.Controls.Add(ctrl, 1, 0)
                    !!^ [nm, box ctrl] p
                fldFP.ColumnStyles.Clear()
                fldFP.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, float32 (getBtnWid lbl.Text)))
                fldFP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f))
                //fldFP.RowStyles.Clear()
                //fldFP.RowStyles.Add(new RowStyle(SizeType.Absolute, float32 (getCtrlHt())))
                //fldFP.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f))
                p.Controls.Add(fldFP)
                fldFP.ResumeLayout(false)
            p.ResumeLayout(false)

#endif //forRef
