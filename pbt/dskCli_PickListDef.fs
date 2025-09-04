(*  Brij (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 
    TrivediBldHdr->
    |Trivedi SrcCtrlHdr File.  Copyright (c) 2015-2025 M. P. Trivedi.  All rights reserved.|637901455|test.fs|none|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|

    fsc src\pbt\pbt_AI_Dsk.fs src\pbt\pbt_PickListDef.fs  --platform:x64 --standalone --out:src\pbt\Trivedi.PbtPickListDef.dll -r:lib\Trivedi.Core.dll -r:lib\Trivedi.CoreAux.dll -r:lib\Trivedi.UI.dll -r:lib\Trivedi.UIAux.dll -I:C:\Windows\Microsoft.NET\Framework\v4.0.30319
   
    Created: Jul 31 2025
    Last updated: Aug 14 2025 (TblLookupDef.createAutoFillFlds())
    
    Contains modules:  PickListDef_Actual

Mar24
- Form (dz) will have LookupFldNm [  ]; Values (Pls enter ea on a newLine) + btns 2 sort asc/dec + Chkbox "Allow itms not in list"  
- OnSave creates Masalo
- @TBD: Didn't ln offer bar-seperated Disp/Save vals? (CA|California) Any util?

*)

namespace Trivedi

#nowarn "20" "25" "58" "66" "67" "64" "760" "1182" "1558"

[<AutoOpen>]
module PickListDef_Actual = 
  open pbtCore  //4 wrld; curr in dskTc.fs (mainRepo);@todo:move2pbtCore.fs


  type PickListDef<'t when 't :> ITblMarker> (def:pm<'t>, dsk, સ્તિતિ) as PLPg =
    inherit TabPage (Text = "PickList Definition Document")
    let mutable currDef = def
    let mutable currSettings = સ્તિતિ
    do PLPg.SuspendLayout()
    let setup =
      let midPnl = new TableLayoutPanel(RowCount = 5, ColumnCount = 2, Dock = doc "F", Name="PickListDefMidPnl", BackColor = Color.White)
      midPnl.Controls.Clear()
      midPnl.SuspendLayout()

      let nmPnl =  new FlowLayoutPanel(Dock = doc "F", FlowDirection = FlowDirection.LeftToRight, AutoSize = true)
      let nmLbl = new Label(Text = ("PickList FieldName:"), Dock = doc "F")
      let nmBox = new TextBox(Dock = doc "F")
      nmBox.TextChanged.AddHandler(new EventHandler(fun o e ->
        currDef.Name <- nmBox.Text))
      nmPnl.Controls.AddItems([|nmLbl;nmBox|])
      midPnl.Controls.Add(PLPgInfoLbl, 0, 0)

      let PLPgInfoLbl = new LinkLabel(Text = "Please enter the choices for your picklist below (each value on a new line): [more info]", Dock = doc "F")
      setLinkLabelArea PLPgInfoLbl
      PLPgInfoLbl.LinkClicked.AddHandler(labelLinkClickHandler)
      midPnl.Controls.Add(PLPgInfoLbl, 0, 1)

      let sortBtnPnl =  new FlowLayoutPanel(Dock = doc "F", FlowDirection = FlowDirection.LeftToRight, AutoSize = true)
      let sortInfoLbl = new Label(Text = "Please click one of following  buttons to sort all the entries below:", Anchor = anc "L")
      let descSortBtn = Button(ImageAlign = ContentAlignment.MiddleLeft, TextAlign = ContentAlignment.MiddleRight, Text = "Desc")
      descSortBtn.Image <- getImage "downArrow"      
      descSortBtn.Click.AddHandler(new EventHandler(fun o e -> 
        pL.Text <- (PlPg.getPickList() |> List.SortDescending |> PlPg.liToTxt)))
      let ascSortBtn = Button(ImageAlign = ContentAlignment.MiddleLeft, TextAlign = ContentAlignment.MiddleRight, Text = "Asc")
      ascSortBtn.Image <- getImage "upArrow"
      ascSortBtn.Click.AddHandler(new EventHandler(fun o e -> 
        pL.Text <- (PlPg.getPickList() |> List.Sort |> PlPg.liToTxt)))
      sortBtnPnl.Controls.AddItems([|sortInfoLbl; descSortBtn; ascSortBtn|])
      midPnl.Controls.Add(sortBtnPnl, 0, 2)

      let pL = new TextBox(Multiline=true, Dock = doc "F", Font = defFont, WordWrap = false, ScrollBars = ScrollBars.Horizontal, AcceptsReturn=true)
      midPnl.Controls.Add(pL, 0, 3)
      PLPg.Controls.Add(midPnl)
    do PLPg.loadDef()
    member PLPg.getPickList() = 
        match ((pL.Text).Trim()) with 
        | "" -> ()
        | _ as t -> t.Split "\n" |> List.ofArray |> lim (fun e -> e.Trim())
    member PLPg.liToTxt(l) = 
        ("", l) |> lifo (fun s v -> s + "\n" + v)
    member PlPg.loadDef() = 
        //@ToDo: nds scatter
    member PLPg.saveDef() = 
        //triggers: tb.Save | tb.SaveAs | tabClose
        {Name = nmBox.Text, Choices=PlPg.getPickList...}

  type TblLookupDef<'t when 't :> ITblMarker> (def:pm<'t>, dsk, સ્તિતિ) as TblLPg =
    inherit TabPage (Text = "Table Lookup Definition Document")
    let mutable currDef = def
    let mutable currSettings = સ્તિતિ
    do TblLPg.SuspendLayout()
    let setup =
      let midPnl = new TableLayoutPanel(RowCount = 5, ColumnCount = 2, Dock = doc "F", Name="TblLookupDefMidPnl", BackColor = Color.White)
      midPnl.Controls.Clear()
      midPnl.SuspendLayout()

      let nmPnl =  new FlowLayoutPanel(Dock = doc "F", FlowDirection = FlowDirection.LeftToRight, AutoSize = true)
      let nmLbl = new Label(Text = ("Table Lookup FieldName:"), Dock = doc "F")
      let nmBox = new TextBox(Dock = doc "F")
      nmBox.TextChanged.AddHandler(new EventHandler(fun o e ->
        currDef.Name <- nmBox.Text))
      nmPnl.Controls.AddItems([|nmLbl;nmBox|])
      midPnl.Controls.Add(TblLPgInfoLbl, 0, 0)

      let tblPnl =  new FlowLayoutPanel(Dock = doc "F", FlowDirection = FlowDirection.LeftToRight, AutoSize = true)
      let tblLbl = new Label(Text = ("Source Table:"), Dock = doc "F")
      let tblCBox = new ListBox(Dock = doc "F", MultiColumn = false,  BackColor = Color.Transparent, ForeColor = currentScheme.Fore())
      toolTip.SetToolTip(tblCBox, "Please select the source Table containing the data you need");
      tblCBox.BeginUpdate()
      tblCBox.Items.AddRange(getAllTblsForCurrOrg() |> Array.ofList)
      tblCBox.EndUpdate()
      tblPnl.Controls.AddItems([|nmLbl;tblCBox|])
      midPnl.Controls.Add(tblPnl, 0, 1)

      let tblColPnl =  new FlowLayoutPanel(Dock = doc "F", FlowDirection = FlowDirection.LeftToRight, AutoSize = true)
      let tblColLbl = new Label(Text = ("Source Column:"), Dock = doc "F")
      let tblColCBox = new ListBox(Dock = doc "F", MultiColumn = false,  BackColor = Color.Transparent, ForeColor = currentScheme.Fore())
      toolTip.SetToolTip(tblCBox, "Please select the source Column containing the data you need");
      tblColCBox.BeginUpdate()
      tblColCBox.Items.AddRange(getAllColsForTbl(tblCBox.Text) |> Array.ofList)
      tblColCBox.EndUpdate()
      tblColPnl.Controls.AddItems([|tblColLbl;tblColCBox|])
      midPnl.Controls.Add(tblColPnl, 0, 2)

      let autoFillPnl =  new FlowLayoutPanel(Dock = doc "F", FlowDirection = FlowDirection.LeftToRight, AutoSize = true)
      let autoFillLbl = new Label(Text = ("AutoFill Fields:"), Dock = doc "F")
      let autoFillCBox = new ListBox(Dock = doc "F", MultiColumn = false,  BackColor = Color.Transparent, ForeColor = currentScheme.Fore(), SelectionMode = SelectionMode.MultiExtended)
      toolTip.SetToolTip(autoFillCBox, "Please select additional fields you wish to load when a PickList value is chosen (Calculated Fields for each column will be automatically created by the system)");
      autoFillCBox.BeginUpdate()
      autoFillCBox.Items.AddRange(getAllColsForAutoFillTbl()) |> Array.ofList)
      autoFillCBox.EndUpdate()
      autoFillPnl.Controls.AddItems([|autoFillLbl;autoFillCBox|])
      midPnl.Controls.Add(autoFillPnl, 0, 3)

      TblLPg.Controls.Add(midPnl)
    do TblLPg.loadDef()
    member TblLPg.createAutoFillFlds() = 
      //chk if they exist, warn.  @TBD: Is there a flag we can set/chk to determine whether this is a NEW pickList or an Edit?  How to improve this flow?
      let state, fNames = 
        [], TblLPg.getAutoFillList()
        |> lifo (fun s fNm -> 
            match (FieldExistsThisTblWithNm ("AutoFill_" + fNm)) with
            | true -> s @ [("AutoFill_" + fNm)]
            | _ -> ("AutoFill_" + fNm)
      match state with
      | [] -> ()
      | _ -> gappa "The following fieldNms already exist in the table and will not be recreated: " + (liToTxt state)
      fNames 
      |> lim (fun f -> createCalcFld4 f)
      |> ignore)
    member TblLPg.getAutoFillList() = 
        match (autoFillCBox.SelectedItems.Count) with 
        | 0 -> ()
        | _ -> 
          autoFillCBox.SelectedItems
          |> Seq.cast 
          |> List.ofSeq
          |> lim (fun i -> i.Text.Trim())
    member TblLPg.liToTxt(l) = 
        ("", l) |> lifo (fun s v -> s + "\n" + v)
    member TblLPg.loadDef() = 
        //@ToDo: nds scatter
    member TblLPg.saveDef() = 
        //triggers: tb.Save | tb.SaveAs | tabClose
        {Name = nmBox.Text, Choices=TblLPg.getPickList...}
        

