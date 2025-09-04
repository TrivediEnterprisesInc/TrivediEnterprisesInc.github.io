(*  Brij (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 
    TrivediBldHdr->
    |Trivedi SrcCtrlHdr File.  Copyright (c) 2015-2025 M. P. Trivedi.  All rights reserved.|637901455|test.fs|none|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|

    fsc src\pbt\pbt_AI_Dsk.fs src\pbt\pbt_DvDef.fs  --platform:x64 --standalone --out:src\pbt\Trivedi.PbtDvDef.dll -r:lib\Trivedi.Core.dll -r:lib\Trivedi.CoreAux.dll -r:lib\Trivedi.UI.dll -r:lib\Trivedi.UIAux.dll -I:C:\Windows\Microsoft.NET\Framework\v4.0.30319
   
    Created: Sat Jul 26 2025
    Last updated: Aug 16 2025 (sort)
    
    Contains modules:  DvDef_Actual
*)

namespace Trivedi

#nowarn "20" "25" "58" "66" "67" "64" "760" "1182" "1558"

[<AutoOpen>]
module DvDef_Actual = 
  open pbtCore  //4 wrld; curr in dskTc.fs (mainRepo);@todo:move2pbtCore.fs

  //@ToDo: expand + move to UI Core
  let labelLinkClickHandler = 
    new LinkLabelLinkClickedEventHandler (fun o e ->
      //taken from msftDox; @ToDo: adapt/expand to show intlDlg or webPg
      // Determine which link was clicked within the LinkLabel.
      this.linkLabel1.Links[linkLabel1.Links.IndexOf(e.Link)].Visited = true;
      // Display the appropriate link based on the value of the 
      // LinkData property of the Link object.
      let target:string = e.Link.LinkData

      // If the value looks like a URL, navigate to it.
      // Otherwise, display it in a message box.
      if ((not(IsNull target)) && target.StartsWith("www"))
          System.Diagnostics.Process.Start(target)
      else MessageBox.Show("labelLinkClickHandler: Item clicked: " + target)
    )

  //move 2 UIHelpers
  let getListBoxWithFlds = 
    fun fldLi -> 
      let lB = new ListBox(Dock = doc "F", MultiColumn = false, SelectionMode = SelectionMode.One, BackColor = Color.Transparent, ForeColor = currentScheme.Fore())
      lB.BeginUpdate()
      fldLi |> lim (fun itm -> 
                      let (DocFld(t, slg, isInt, nm)) = itm
                      lB.Items.Add(slg))
      lB.EndUpdate()
      lB

  let setLinkLabelArea = 
    fun lbl -> 
      //we're alw assuming the link is in sqrBrackets coz the text cld contain parantheses
      match ((strContains "[" lbl.Text) , (strContains "]" lbl.Text)) with
      | true, true -> 
          let op = String.FirstIndexOf("[") lbl.Text
          let cl = String.FirstIndexOf("]") lbl.Text
          lbl.LinkArea <- LinkArea(op + 1, cl + 1)
      | _ -> ()

  let autoCreateRecycleVw =
    fun def:cm<'t> ->
      match (doesRecycleViewExistFor (getTblNm 't)) with
      | true -> ()
      | _ ->
        //same cm BUT ensure the tBar btn for "Delete" ren 2 "UnDelete"
        cm.Name <- "Recycle Bin"
        cm.IntlNm <- "$RecycleView" //repl suffix in id 4 DzDocInf?
        gappa( "A default Recycle View has been autoCreated for this Table.  This is just like a regular view, and may be customized (columns, formatting) like the others if you wish.")



  type SortOrderToggleBtn() as sob = 
    inherit Button(ImageAlign = ContentAlignment.MiddleLeft, TextAlign = ContentAlignment.MiddleRight, Text = "Desc")
    sob.Image <- getImage "downArrow"
    member sob.getSOrd() = 
      match sob.Text with 
      | "Desc" -> "D"
      | _ -> "A"
    member sob.reverseSO() = 
      match sob.Text with 
      | "Desc" -> 
        sob.Text <- "Asc"
        sob.Image <- getImage "downArrow"
      | _ -> 
        sob.Text <- "Desc"
        sob.Image <- getImage "upArrow"

  type DvDef<'t when 't :> ITblMarker> (def:cm<'t>, dsk, સ્તિતિ) as dvPg =
    inherit TabPage (Text = "DataView Definition Document")
    let mutable currDef = def
    let mutable currSettings = સ્તિતિ
    //Expand/discomb 
    let [DocFld(_,_)] = TblFldList  //from CM
    do dvPg.SuspendLayout()
    let tc = new TabControl(Dock = doc "F", Width = f.Width - defPadding.Horizontal, Height = f.Height - (defPadding.Horizontal * 8))
    let CategPg = new TabPage (Text = "Categories/Sorting")
    let FldsPg = new TabPage (Text = "Columns")
    tc.Controls.AddItems([|CategPg; FldsPg|])
    dvPg.Controls.Add(tc)
    let setupCategPg =
      let dvDefMidP = new TableLayoutPanel(RowCount = 2, ColumnCount = 2, Dock = doc "F", Name="dvDefMidP", BackColor = Color.White)
      dvDefMidP.Controls.Clear()
      dvDefMidP.SuspendLayout()
 
      //@ToDo: We nd 2 repurpose the "InfoDlg gappa" to this and other TabPgs which will pretty much alw have infoTxt + link to Help/dox
      //Either getPnl >> customize OR pullOut into an indep fn (takes str, outputs midP w this added in row0 + midP in row1)
      let dvDefPgInfoLbl = new LinkLabel(Text = "Please select the Categories and Sorting Order for the DataView (link)", Dock = doc "F")
      setLinkLabelArea dvDefPgInfoLbl
      dvDefPgInfoLbl.LinkClicked.AddHandler(labelLinkClickHandler)
      dvDefMidP.Controls.Add(dvDefPgInfoLbl, 0, 0)

        //MainPnl
        let MainPnl = new TableLayoutPanel(RowCount = 2, ColumnCount = 2, Dock = doc "F", Name="MainPnl")
        let HdrsRow = new TableLayoutPanel(RowCount = 1, ColumnCount = 2, Dock = doc "F", Name="HdrsRow")
        let dvDefCategLbl = new Label(Text = "DataView Categories", Dock = doc "F", TextAlign = ContentAlignment.MiddleCenter
        let dvDefSortLbl = new Label(Text = "DataView Sorting", Dock = doc "F", TextAlign = ContentAlignment.MiddleCenter
        HdrsRow.Controls.AddItems([|dvDefCategLbl, dvDefSortLbl|])
        MainPnl.Controls.Add(HdrsRow, 0, 0)
        let CategPnl = new TableLayoutPanel(RowCount = 1, ColumnCount = 1, Dock = doc "F", Name="CategPnl")
        [1..5] 
        |> limi (fun i r -> 
                  let thisCategBox = getListBoxWithFlds TblFldList
                  thisCategBox.SelectedValueChanged.AddHandler(new EventHandler(fun o e -> 
                  dvPg.addCateg(i, thisCategBox.SelectedValue.ToString())))
                  !!^ [i.ToString() + "Categ", box thisCategBox]
                  CategPnl.Controls.Add(thisCategBox, 0, i))
        MainPnl.Controls.Add(CategPnl, 1, 0)
        let SortPnl = new TableLayoutPanel(RowCount = 1, ColumnCount = 2, Dock = doc "F", Name="SortPnl")
        [1..5] 
        |> limi (fun i r -> 
                  let thisSortBox = getListBoxWithFlds TblFldList
                  thisSortBox.SelectedValueChanged.AddHandler(new EventHandler(fun o e -> 
                  dvPg.addSort(i, thisSortBox.SelectedValue.ToString())))
                  !!^ [i.ToString() + "Sort", box thisSortBox]
                  SortPnl.Controls.Add(thisSortBox, 0, i)
                  let thisSortOrdToggleBtn = SortOrderToggleBtn()
                  !!^ [i.ToString() + "SortOrd", box thisSortOrdToggleBtn]
                  SortPnl.Controls.Add(thisSortOrdToggleBtn, 1, i)
                  thisSortOrdToggleBtn.Click.AddHandler(new EventHandler(fun o e -> 
                  let btn = (SOBtn) o
                  btn.reverseSO()
                  dvPg.updSO(i, btn.getSOrd()))))
        MainPnl.Controls.Add(SortPnl, 1, 1)
        dvDefMidP.ResumeLayout()
        CategPg.Controls.Add(MainPnl)
    let setupFldsPg =
      let fldsPnl = new TableLayoutPanel(RowCount = 2, ColumnCount = 2, Dock = doc "F", Name="dvDefFldsPnl", BackColor = Color.White)
      fldsPnl.Controls.Clear()
      fldsPnl.SuspendLayout()
      let fldsPgInfoLbl = new LinkLabel(Text = "Please select the Columns for this DataView (link)", Dock = doc "F")
      setLinkLabelArea fldsPgInfoLbl
      fldsPgInfoLbl.LinkClicked.AddHandler(labelLinkClickHandler)
      fldsPnl.Controls.Add(fldsPgInfoLbl, 0, 0)
      let ColPnl = new TableLayoutPanel(RowCount = 10, ColumnCount = 1, Dock = doc "F", Name="ColPnl")
      [1..10] 
      |> limi (fun i r -> 
                let thisColBox = getListBoxWithFlds TblFldList
                thisColBox.SelectedValueChanged.AddHandler(new EventHandler(fun o e -> 
                dvPg.addCol(i, thisColBox.SelectedValue.ToString())))
                !!^ [i.ToString() + "Column", box thisColBox]
                CategPnl.Controls.Add(thisColBox, 0, i))
      fldsPnl.Controls.Add(ColPnl, 0, 1)
      FldsPg.Controls.Add(fldsPnl)
    //@ToDo: Attach below to Save/SaveAs btn in tb or tabClose...
    //do autoCreateRecycleVw cm
    member dvPg.addCol(idx, colNm) = 
        //@ToDo: upd8 currDef
    member dvPg.addCateg(idx, categOn) = 
        //@ToDo: upd8 currDef
    member dvPg.addSort(idx, sortOn) = 
        //@ToDo: upd8 currDef
    member dvPg.updSO(idx, so) =
        //@ToDo: upd8 currDef
