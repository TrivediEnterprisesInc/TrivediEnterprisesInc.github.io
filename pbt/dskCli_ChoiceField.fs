(*  Brij (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 
    TrivediBldHdr->
    |Trivedi SrcCtrlHdr File.  Copyright (c) 2015-2025 M. P. Trivedi.  All rights reserved.|637901455|test.fs|none|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS 

    Created:        Tue Jul 29 1 2025
    Last updated:   
    
    Contains modules:      ChoiceFld_Actual //| ChoiceFld_Test

    ****SEE ALSO dskCli_PickListDef.fs in the meethoo plnk
*)

namespace Trivedi

#nowarn "20" "25" "58" "66" "67" "64" "760" "1182" "1558"


[<AutoOpen>]
module ChoiceFld_Actual =
    open System
    open System.Drawing
    open System.Windows.Forms
    open Trivedi
    open Trivedi.Core
    //open Trivedi.UI
    //open Trivedi.UIAux
    open System.Windows.Forms

//(1) We don't need to offer users a chkbox "Allow value(s) not in list" ->
//dojoDox: Like an INPUT text field, the user can type whatever text they want, regardless of whether or not it’s in the drop down menu.

//use bossCtrl ref to assign 'fldId' 4 form purps

//src: dskCli_FrmDef.fs; has been edited/updated to incorp deltas
type FldType = ...
member this.getDefThingy() = 
| FldChoiceList -> wChoiceList
//...
member this.getDefThingies() = 
| FldChoiceList -> [wRadioButton; wChoiceList; wCheckedMultiSel]
//...
  | wChoiceList(dispNm, fldId, fldNm, initVal, StoreChoiceFldId, StoreNm) -> 
      let LookupLi = 
        match (Env.isDevelopment()) with
        | true -> 
            ["Alabama";"Alaska";"Arizona";"Arkansas";"California";"Colorado";"Connecticut";"Delaware";"Florida";"Georgia";"Hawaii";"Idaho";"IllinoisIndiana";"Iowa";"Kansas";"Kentucky";"Louisiana";"Maine";"Maryland";"Massachusetts";"Michigan";"Minnesota";"Mississippi";"Missouri";"MontanaNebraska";"Nevada";"New Hampshire";"New Jersey";"New Mexico";"New York";"North Carolina";"North Dakota";"Ohio";"Oklahoma";"Oregon";"PennsylvaniaRhode Island";"South Carolina";"South Dakota";"Tennessee";"Texas";"Utah";"Vermont";"Virginia";"Washington";"West Virginia";"Wisconsin";"Wyoming"]
        | _ -> getChoiceFldLi StoreChoiceFldId
      let bldData = 
          let res = 
              ("data: [\n", LookupLi) 
              |> lifo (fun s v -> "\n{name:'" + v + "'},")
          res(0, res.Length - 1) + "\n]"
"""<td>
	<label for="{fldId}">{lblTxt}</label>
	<div data-dojo-type="dojo/store/Memory"
			data-dojo-id="{fldId}_store"
			data-dojo-props="{bldData}"></div>
	<input class="cellWid dojoFormValue" 
	    data-dojo-type="dijit/form/ComboBox"
			value="{initVal}"
			data-dojo-props="store:{fldId}_store, searchAttr:'name'"
			name="fldNm"
			id="{fldId}_inp" />
</td>"""

    type ChoiceFldMasalo<'t when 't :> ITblMarker> = 
    { unid:DocUNID; mutable dispNm:string; mutable initVal:string; mutable StoreNm:string } with
        static member getDefault(docF:DocFld list, nm, ty:'t) = 
            { unid = getUNID (ty.ToString()) + "^ChoiceM"; dispNm = nm; 
              initVal = ""; StoreNm = ""}
    
    //repurp ui stuff from below
    //Aug01: _OR_ follow the dlg setup used in svr_gg.fs
      type DvDef<'t when 't :> ITblMarker> (def:cm<'t>, dsk, સ્તિતિ) as dvPg =
        inherit TabPage (Text = "DataView Definition Document")
        let mutable currDef = def
        let mutable currSettings = સ્તિતિ
        //Expand/discomb 
        let [DocFld(_,_)] = TblFldList  //from CM
        do dvPg.SuspendLayout()
        let setupPg =
          let dvDefMidP = new TableLayoutPanel(RowCount = 2, ColumnCount = 2, Dock = doc "F", Name="dvDefMidP", BackColor = Color.White)
          dvDefMidP.Controls.Clear()
          do dvDefMidP.SuspendLayout()
    
          //@ToDo: We nd 2 repurpose the "InfoDlg gappa" to this and other TabPgs which will pretty much alw have infoTxt + link to Help/dox
          //Either getPnl >> customize OR pullOut into an indep fn (takes str, outputs midP w this added in row0 + midP in row1)
          let dvDefPgInfoLbl = new LinkLabel(Text = "Please select the Categories and Sorting Order for the DataView (link)", Dock = doc "F", LinkArea = LinkArea(65,68))
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
                      CategPnl.Controls.Add(thisCategBox, i, 0))

