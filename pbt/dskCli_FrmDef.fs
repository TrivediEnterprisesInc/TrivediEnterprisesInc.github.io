(*  Brij (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 
    TrivediBldHdr->
    |Trivedi SrcCtrlHdr File.  Copyright (c) 2015-2025 M. P. Trivedi.  All rights reserved.|637901455|test.fs|none|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS 

    //minus UIAux
    fsc src\pbt\Dnd_ops.fs  --platform:x64 --standalone --target:exe --out:src\pbt\dnd.exe -r:lib\Trivedi.Core.dll -r:lib\Trivedi.UI.dll -I:C:\Windows\Microsoft.NET\Framework\v4.0.30319

    Last updated:   

                    Wed Aug 20_25: let EmbdTblBtn; 
                                   EmbdTblDlg_પીચાક TO BE COMPLETED
                    Tue Aug 19_25: DocFldty Upd8d to support EmbddDVs
                    Thu Jul 17 2025: refactored BM to rec; 
                    other deltas planned 4 toDef/fromD: we currently have bldDefault 4 autoBld; we nd (an alt 4 loadDef) OR DO WE?  Doesn't the logic reload the def?  @Chk
                    Thu May 1 2025

    Contains modules:      FrmDef_Actual | FrmDef_Test

    Outstanding: 
        Instd of matching/chking targets 4 eligibility (canAddRow/Col/etc) it'd be b8r 2 show/hide TbarBtns for ea state
            e.g., if SingleCellSel/AllSelSameRo -> enable AddRoAbove/Belo
            NO selection reqd 4 popupMnu chg cellDetails (props)
    
        dojo textBoxes (other w's too?) allow using placeHolder: "type in your name" as part of the wid html
        Apr28:  (Jun_25: strike stuff below; covered by IDeployable)
            How 2 ensure dev sets/enters widSettings?
            - Cld force input (ty) b4 autoLayout
            - Use opts in recCtors (w); insert in frm/make em reqd flds
            - Poss add btn to 'Chk Frm' highlites red w's nding params?
            - A form deployment status (red/grn)?
            - bldState: Create a new fn w/o b4/roTgts (Freudian slip) 
*)

namespace Trivedi

#nowarn "20" "25" "58" "66" "67" "64" "760" "1182" "1558"


[<AutoOpen>]
module FrmDef_Actual =
    open System
    open System.Drawing
    open System.Windows.Forms
    open Trivedi
    open Trivedi.Core
    //open Trivedi.UI
    //open Trivedi.UIAux
    open System.Windows.Forms

    //moved into mod Apr 15 to use Core
    //updated Apr14 '23, to rec 4 FsChk (+ gen accessibility); src:UIAux
    type BMfld<'t when 't :> ITblMarker> = { unid:DocUNID; title:string ; docF:DocFld; 
                                            colSpan:int ; rowSpan:int ; Pos:(int * int);
                                            lblFont:Font ; dataFont:Font ; foreCol:Color option; backCol:Color option  ; soopari:obj;
                                            vFn:('t -> bool) option ; CanUhear:Thingy ; fldTtip:string option; tblTy:'t } with
        override this.ToString() = 
            "BMfld: |DocFld: " + this.docF.ToString() + "|colSp: " + this.colSpan.ToString() + 
                "|rowSp: " + this.rowSpan.ToString() + "|vFn: " + this.vFn.ToString() + "|tblTy: " + (this.tblTy.GetType()).ToString()
        static member getDefault(docF:DocFld list, ty:'t) = 
            docF 
            |> List.map (fun f -> 
                let (DocFld(ft, intNm, isInt, tit)) = f
                { unid = (getUNID "BmFldTest"); title = tit ; docF = f; 
                colSpan = 1 ; rowSpan = 1 ; Pos = (0, 0); lblFont = defIt ; dataFont = defFont; 
                foreCol = Some(Color.Black); backCol = Some(Color.Black); 
                soopari = box 1; vFn = None ; CanUhear = Thingy("") ; fldTtip = None; tblTy = TaskTbl() } )
        static member getShorty(f, cc, cr, col, ro) = 
                let (DocFld(ft, intNm, isInt, tit)) = f
                { unid = (getUNID "BmFldTest"); title = tit ; docF = f; 
                colSpan = 1 ; rowSpan = 1 ; Pos = (0, 0); lblFont = defIt ; dataFont = defFont; 
                foreCol = Some(Color.Black); backCol = Some(Color.Black); 
                soopari = box 1; vFn = None ; CanUhear = Thingy("") ; fldTtip = None; tblTy = TaskTbl() }
                |> (fun c -> { c with colSpan = cc; rowSpan = cr; Pos = (col,ro) } )



    type BMdzCell = | BMdzCellItm of BMFld
                            | BlankCell of (int * int)
                            | B4Tgt of (float * float)

    type BMdzRow = | BMdzRowItm of list<BMdzCell>
                             | BlankRow of int
                             | RoTgt of float

    type BMdzTbl = | BMdzTbl of list<BMdzRow>
    
    //updated Apr'23, src:UIAux
    type old_BanarasiMasaloAux<'t when 't :> ITblMarker> = | BanarasiMasaloAux of unid:DocUNID * dispNm:string *
                                                        સુપારી:int * usrFlds:BMdzTbl * 
                                                        usrDefLblFont:Font * usrDefDataFont:Font * 
                                                        usrDefForeColor:Color * usrDefBackColor:Color * 
                                                        docInf:DesDocInf with
        static member getDefault(docF:DocFld list, nm, ty:'t) = 
            let bf = BMfld.getDefault(docF, ty)
            let defIt = Font(defFont.FontFamily, defFont.Size, FontStyle.Italic, defFont.Unit)
            BanarasiMasaloAux((getUNID (ty.ToString()), nm, 2, bf, defIt, defFont, currentScheme.Fore(), currentScheme.Back(), DesDocInfDeflt()))
        member getDefaultCellModel = 
            //@ToDo: bf here nds 2 be an auto-layout BMdzTbl
            //@ToDo: i.e., the loop to gen the default cellStruct

    //updated Jul_17_25, refactored to rec comme nanoo
    type BanarasiMasaloAux<'t when 't :> ITblMarker> = 
    { unid:DocUNID; mutable dispNm:string; mutable સુપારી:int;
     mutable usrFlds:BMdzTbl; mutable usrDefLblFont:Font; 
     mutable usrDefDataFont:Font; mutable usrDefForeColor:Color; 
     mutable usrDefBackColor:Color; mutable docInf:DesDocInf } with
        static member getDefault(docF:DocFld list, nm, ty:'t) = 
            let bf = BMfld.getDefault(docF, ty)
            let defIt = Font(defFont.FontFamily, defFont.Size, FontStyle.Italic, defFont.Unit)
            BanarasiMasaloAux((getUNID (ty.ToString()), nm, 2, bf, defIt, defFont, currentScheme.Fore(), currentScheme.Back(), DesDocInfDeflt()))
            { unid = ((getUNID (ty.ToString()); dispNm = nm; 
              સુપારી=2; usrFlds = bf; usrDefLblFont = defIt; 
              usrDefDataFont = defFont; usrDefForeColor = (currentScheme.Fore()); 
              usrDefBackColor = (currentScheme.Back()); docInf:(DesDocInfDeflt()) }
        member getDefaultCellModel = 
            //@ToDo: bf here nds 2 be an auto-layout BMdzTbl
            //@ToDo: i.e., the loop to gen the default cellStruct

    //Aug19_25: Upd8d to support EmbddDVs (becomes a listFld)
    //The new fld is isEmbdd: DocFld(ft, intNm, isInt, tit, isEmbdd)
    type DocFld = | DocFld of FldType*string*bool*string*bool with...

    type FldType =  | FldString
                    | FldNumber
                    | FldRange
                    | FldCurrency
                    | FldHtml
                    | FldAttachment
                    | FldBoolean
                    | FldChoiceList
                    | FldDate 
                    | FldTime
                    | FldColor
                    | FldRating
                    | FldFont with
        member this.getDefThingy() = 
            match this with
               | FldString -> wTextBox
               | FldNumber  -> wNumberTxtBox
               | FldRange -> wRangeSlider
               | FldCurrency -> wCurrencyTextBox
               | FldHtml -> wRTEditor
               | FldAttachment -> 
               | FldBoolean -> wCheckBox
               | FldChoiceList -> wChoiceList
               | FldDate  -> wDateTextBox
               | FldTime -> wTimeTxtBox
               | FldColor -> existing but modify btn; add fld   //@TBD: we poss don't nd this in webCli; only intl use
               | FldFont -> existing but modify btn; add fld  //@TBD: we poss don't nd this in webCli; only intl use
               | FldRating -> wRating
        member this.getDefThingies() = 
            match this with
               | FldString -> [wTextBox;wSimpleTextarea]
               | FldNumber  -> [wNumberTxtBox; wNumberSpinner; wHorizontalSlider]
               | FldRange -> [wRangeSlider]
               | FldCurrency -> [wCurrencyTextBox]
               | FldHtml -> [wRTEditor]
               | FldAttachment -> []
               | FldBoolean -> [wCheckBox]  //nd 2 split to noChoices/wChoices
               | FldChoiceList -> [wRadioButton; wChoiceList; wCheckedMultiSel]
               | FldDate  -> [wDateTextBox]
               | FldTime -> [wTimeTxtBox]
               | FldColor -> [] //existing but modify btn; add fld   //@TBD: we poss don't nd this in webCli; only intl use
               | FldFont -> [] //existing but modify btn; add fld  //@TBD: we poss don't nd this in webCli; only intl use
               | FldRating -> [wRating]

    //added Sep4
    type widParamPnl(widTy) = 
        let p = new FlowLayoutPanel(Dock = doc "F", FlowDirection = FlowDirection.LeftToRight, AutoSize = true)
        match widTy with 
        | wTextBox()
        | wSimpleTextarea(numRows, numCols, widPercent)
            //all params optional; widPercent shd default to "width: auto;"
            let p0 = (ફીલ્ડ_પેનલ  ("Number of Cols", UserInput, "no Slug", 2)) :> Pane
            let coLbl = !!~ "inpt" p0
            coLbl.Text <- "50"
            let p1 = (ફીલ્ડ_પેનલ  ("Number of Rows", UserInput, "no Slug", 2)) :> Pane
            let roLbl = !!~ "inpt" p1
            roLbl.Text <- "4"
            let p2 = (ફીલ્ડ_પેનલ  ("Width in %", UserInput, "no Slug", 2)) :> Pane
            let widLbl = !!~ "inpt" p2
            widLbl.Text <- "auto;"
            p.Controls.AddItems([|p0;p1;p2|])
            p
        | wNumberTxtBox(boolReqd, smallDelta, min, max, places, invalidMsg, rangeMsg)
        | wCurrencyTextBox(boolConstraints, currTy, invMsg, )
        | wRTEditor(boolReqd)
        | wCheckBox()
        | wRadioButton(list<()>)
        | wNumberSpinner(smallDelta, min, max, places)
        | wHorizontalSlider(lblTxt, fldId, fldNm, fldMin, fldMax, fldDiscreteVals, fldVal, decCount, dec1, dec2, dec3)
        | wDateTextBox(valReqd, )
        | wTimeTxtBox(boolReqd)
        | wInfoBox(colSp, txtVal)
        | wBlankRow(colSp)
        | wChoiceList(, StoreLookupFldId, StoreNm, StoreSrchAttr) //poss not last, defaults to 'name' hardcoded
        | wCheckedMultiSel(StoreLookupFldId, StoreNm, StoreSrchAttr) //poss not last, defaults to 'name' hardcoded
        | wRating(max)
        | wRangeSlider(max)
        ... with
        member getParams() = 
            match widTy with 
            | wTextBox()
            | wSimpleTextarea(numRows, numCols, widPercent)
                let coLbl = !!~ "inpt" p0
                let roLbl = !!~ "inpt" p1
                let widLbl = !!~ "inpt" p2
                (roLbl.Text, coLbl.Text, widLbl.Text)
            | wNumberTxtBox(boolReqd, smallDelta, min, max, places, invalidMsg, rangeMsg)
            | wCurrencyTextBox(boolConstraints, currTy, invMsg, )
            | wRTEditor(boolReqd)
            | wCheckBox()
            | wRadioButton(list<()>)
            | wNumberSpinner(smallDelta, min, max, places)
            | wHorizontalSlider(lblTxt, fldId, fldNm, fldMin, fldMax, fldDiscreteVals, fldVal, decCount, dec1, dec2, dec3)
            | wDateTextBox(valReqd, )
            | wTimeTxtBox(boolReqd)
            | wInfoBox(colSp, txtVal)
            | wBlankRow(colSp)
            | wChoiceList(, StoreLookupFldId, StoreNm, StoreSrchAttr) //poss not last, defaults to 'name' hardcoded
            | wCheckedMultiSel(StoreLookupFldId, StoreNm, StoreSrchAttr) //poss not last, defaults to 'name' hardcoded
            | wRating(max)
            | wRangeSlider(max)
        member setParams(params) = 
            match widTy with 
            | wTextBox()
            | wSimpleTextarea(numRows, numCols, widPercent)
                let (ro, co, w) = unbox params
                let coLbl = !!~ "inpt" p0
                coLbl.Text <- ro
                let roLbl = !!~ "inpt" p1
                roLbl.Text <- ro
                let widLbl = !!~ "inpt" p2
                match w.Trim().strIncludes("auto") with
                | _ -> 
                    match isInt(widPercent) with
                    | true -> widLbl.Text <- widPercent + "%;"
                    | _ -> widLbl.Text <- "auto;"
                | _ -> widLbl.Text <- "auto;"
            | wNumberTxtBox(boolReqd, smallDelta, min, max, places, invalidMsg, rangeMsg)
            | wCurrencyTextBox(boolConstraints, currTy, invMsg, )
            | wRTEditor(boolReqd)
            | wCheckBox()
            | wRadioButton(list<()>)
            | wNumberSpinner(smallDelta, min, max, places)
            | wHorizontalSlider(lblTxt, fldId, fldNm, fldMin, fldMax, fldDiscreteVals, fldVal, decCount, dec1, dec2, dec3)
            | wDateTextBox(valReqd, )
            | wTimeTxtBox(boolReqd)
            | wInfoBox(colSp, txtVal)
            | wBlankRow(colSp)
            | wChoiceList(, StoreLookupFldId, StoreNm, StoreSrchAttr) //poss not last, defaults to 'name' hardcoded
            | wCheckedMultiSel(StoreLookupFldId, StoreNm, StoreSrchAttr) //poss not last, defaults to 'name' hardcoded
            | wRating(max)
            | wRangeSlider(max)


    //Aug02: added params _except_ 'lblTxt, fldId, fldNm, fldVal' (these are in the fldDef)
    //widTabPg: table: 1st ro: helpMsg
    //                 2nd ro: 1st col image, 2nd descr, 3rd reqdParams
    //Sep4: added member getParamPnl()
    type Wid =  | wTextBox()
                | wSimpleTextarea(numRows, numCols, widPercent, )
                | wNumberTxtBox(boolReqd, smallDelta, min, max, places, invalidMsg, rangeMsg)
                | wCurrencyTextBox(boolConstraints, currTy, invMsg, )
                | wRTEditor(boolReqd)
                | wCheckBox()
                | wRadioButton(list<()>)
                | wNumberSpinner(smallDelta, min, max, places)
                | wHorizontalSlider(lblTxt, fldId, fldNm, fldMin, fldMax, fldDiscreteVals, fldVal, decCount, dec1, dec2, dec3)
                | wDateTextBox(valReqd, )
                | wTimeTxtBox(boolReqd)
                | wInfoBox(colSp, txtVal)
                | wBlankRow(colSp)
                | wChoiceList(, StoreLookupFldId, StoreNm, StoreSrchAttr) //poss not last, defaults to 'name' hardcoded
                | wCheckedMultiSel(StoreLookupFldId, StoreNm, StoreSrchAttr) //poss not last, defaults to 'name' hardcoded
                | wRating(max)
                | wRangeSlider(max) with
        member getParamPnl() = widParamPnl(this)
        member this.getHtml(supari) = 
            match this with
                | wTextBox(lblTxt, fldId, fldNm, fldVal) -> """
<td>
    <label for="{fldId}">{lblTxt}</label>
    <input class="cellWid dojoFormValue" type="text" dojoType="dijit/form/TextBox" id="{fldId}" name="{fldNm}" value="{fldVal}" observer="showValues">
</td>"""
                | wSimpleTextarea(numRows, numCols, widPercent, lblTxt, fldId, fldNm, fldVal) -> """
<td colspan='{colSp}'>
    <label for="{fldId}">{lblTxt}</label><br>
    <textarea class="cellWid dojoFormValue" dojoType="dijit/form/SimpleTextarea" id="{fldId}" name="{fldNm}" observer="showValues"
    rows="{numRows}" cols="{numCols}" style="width: {widPercent}%;" >{fldVal}</textarea>
</td>"""
                | wNumberTxtBox(lblTxt, fldId, fldNm, fldVal, boolReqd, smallDelta, min, max, places, invalidMsg, rangeMsg) -> """
<td>
    <label for="fldId">{lblTxt}</label>
    <input id="fldId" type="text"
        data-dojo-type="dijit/form/NumberTextBox"
        name= "{fldNm}"
        required="{boolReqd}"
        value="{fldVal}"
        data-dojo-props="constraints:{min:{{min}},max:{{max}},places:{{places}}},
        invalidMessage:'{invalidMsg}',
        rangeMessage:'{rangeMsg}'" />
</td>"""    
                | wNumberSpinner(lblTxt, fldId, fldNm, fldVal, smallDelta, min, max, places) -> """
<td>
    <label for="{fldId}">{lblTxt}</label>
    <input data-dojo-type="dijit/form/NumberSpinner"
        id="{fldId}"
        value="{fldVal}"
        data-dojo-props="smallDelta:{smallDelta}, constraints:{min:{{min}},max:{{max}},places:{{places}}}"
        name="fldNm">
</td>"""
                
                | wCurrencyTextBox(boolConstraints, currTy, invMsg, lblTxt, fldId, fldNm, fldVal) -> """
<td>
        <label for="{fldId}">{lblTxt}</label>
        <input class="cellWid dojoFormValue" type="text" name="{fldNmd}" id="{fldId}" value="{fldVal}" required="true"
                data-dojo-type="dijit/form/CurrencyTextBox"
                data-dojo-props="constraints:{fractional:{boolConstraints}},
                currency:'{currTy}',
                invalidMessage:'{invMsg}'" />
</td>"""
                | wCheckBox(lblTxt, fldId, fldNm, fldVal) -> 
                    //@TBD: Does this cover multiple opts?
"""<td>
    <label for="{fldId}">{lblTxt}</label>
    <input class="dojoFormValue" type="checkbox" dojoType="dijit/form/CheckBox" 
    id="{fldId}" name="{fldNm}" value={fldVal} data-dojo-observer="window.mShowVals();">
</td>"""
                | wRadioButton(list<(lblTxt, fldId, fldNm, fldVal)>) -> 
                    //@ToDo: nd to do a lim on choices here
"""<td>
<input class="cellWid dojoFormValue" type="radio" data-dojo-type="dijit/form/RadioButton" 
    name="{fldNm}" id="{fldChoiceIds[0]}" checked value="{fldVal[0]}"/>
<label for="{fldId}">{lblTxt[0]}</label> <br />
<input class="cellWid dojoFormValue" type="radio" data-dojo-type="dijit/form/RadioButton" 
    name="{fldNm}" id="{fldChoiceIds[1]}" value="{fldVal[0]}"/>
<label for="radioTwo">{lblTxt[1]}</label>
</td>
                | wNumberSpinner -> 
                | wHorizontalSlider (lblTxt, fldId, fldNm, fldMin, fldMax, fldDiscreteVals, fldVal, decCount, dec1, dec2, dec3) -> """
<td>
    <label for="{fldId}">{lblTxt}</label>
    <input class="cellWid dojoFormValue" type="text" id="{fldId}" 
        data-dojo-props="disabled:true, style:'{width:50px;}'" value="0" 
        data-dojo-type="dijit/form/TextBox" />
        <div id="{fldId}"
          class="cellWid dojoFormValue" 
            style="width:300px;"
            name="{fldNm}"
            data-dojo-type="dijit/form/HorizontalSlider"
            data-dojo-props="value:{fldVal},
            minimum: {fldMin},
            maximum:{fldMax},
            discreteValues:{fldDiscreteVals},
            intermediateChanges:true,
            showButtons:true,
            onChange: function(value){
    dom.byId('{fldId}').value = value;
    }">
    <div data-dojo-type="dijit/form/HorizontalRule" container="bottomDecoration"
                count={11} style="height:5px;"></div>
        <ol data-dojo-type="dijit/form/HorizontalRuleLabels" container="bottomDecoration"
                style="height:1em;font-size:75%;color:gray;">
                <li>{dec1}</li>
                <li>{dec2}</li>
                <li>{dec3}</li>
        </ol>
    </div>
</td>"""
                | wDateTextBox(valReqd, lblTxt, fldId, fldNm, fldVal) -> """
<td><label for="{fldId}">{lblTxt}</label>
    <input class="cellWid dojoFormValue" type="text" name="{fldNm}" id="{fldId}" value="{fldVal}"
        data-dojo-type="dijit/form/DateTextBox"
        required="{valReqd}" observer="showValues" />
</td>"""
                | wTimeTxtBox(lblTxt, fldId, fldNm, fldVal, boolReqd) -> """
<td>
	<label for="time1">{lblTxt}</label>
	<input class="cellWid dojoFormValue" type="text" name="{fldNm}" id="{fldId}" value="{fldVal}"
			data-dojo-type="dijit/form/TimeTextBox"
			onChange="require(['dojo/dom'], function(dom){dom.byId('val').value=dom.byId('time1').value.toString().replace(/.*1970\s(\S+).*/,'T$1')})"
			required="{boolReqd}" />
    <--Store in .net friendly fmt-->
	</td><td>(DoWeNdThis???)Time strVal: <input id="val" value="value not changed" readonly="readonly" disabled="disabled" />
</td>"""

                | wChoiceList(lblTxt, fldId, fldNm, fldVal, StoreLookupFldId, StoreNm, StoreSrchAttr) -> 
                    let LookupLi = getLookupFldLi StoreLookupFldId
                    let bldData = 
                        let res = 
                            ("data: [\n", LookupLi) 
                            |> lifo (fun s v -> "\n{name:'" + v + "'},")
                        res(0, res.Length - 1) + "\n]"
"""<td>
	<label for="fldId">{lblTxt}</label>
	<div data-dojo-type="dojo/store/Memory"
			data-dojo-id="fldId"
			data-dojo-props="{bldData}"></div>
	<input class="cellWid dojoFormValue" 
	    data-dojo-type="dijit/form/ComboBox"
			value="fldVal"
			data-dojo-props="store:{StoreNm}, searchAttr:'{StoreSrchAttr}'"
			name="fldNm"
			id="fldId" />
</td>"""
                | wCheckedMultiSel(lblTxt, fldId, fldNm, fldVal, StoreLookupFldId, StoreNm, StoreSrchAttr) -> 
                    let LookupLi = getLookupFldLi StoreLookupFldId
                    let bldData = 
                        ("", LookupLi) 
                        |> lifo (fun s v -> "\n<option value='{v}'>{v}</option>\n")
"""<td>
	<label for="fldId">{lblTxt}</label>
    <select multiple="true" name="{fldNm}" data-dojo-type="dojox/form/CheckedMultiSelect">
          {bldData}
     </select>
</td>"""
                | wRTEditor(lblTxt, fldId, fldNm, fldVal, boolReqd) -> """
<td>
    <label for="fldId">{lblTxt}</label>
    <div class="dojoFormValue" data-dojo-type="dijit/Editor" name="fldNm" id="fldId" data-dojo-props="onChange:function(){console.log('editor1 onChange handler: ' + arguments[0])}, 
    		plugins: ['subscript', 'superscript', 'viewsource', {name:'fullscreen', zIndex:900}, 'createLink', 'unlink', 'insertImage', /*'fontName',*/ 'fontSize', 'formatBlock']">
        <p>This instance is created from a div directly with default toolbar and plugins</p>
    </div>
</td>"""
                | wRating(lblTxt, fldId, fldNm, fldVal, max) -> """
<td>
    <label for="fldId">{lblTxt}</label>
    <div data-dojo-type="dojox/form/Rating" name="fldNm" id="fldId" data-dojo-props="numStars:{max},value:{fldVal}"></div>
</td>"""
                | wRangeSlider(lblTxt, fldId, fldNm, fldVal, max) -> 
            //see plnk/CHmk9G0wYSEBqZPK; val is string rep of Array (TBD)
"""<td>
    <label for="{fldId}">{lblTxt}</label>
    <div id="rangeSlider4{fldId}" data-dojo-type="dojox.form.HorizontalRangeSlider"
        data-dojo-props="value:[{fldVal}], minimum:{min}, maximum:{max}, intermediateChanges:true,
        showButtons:false" style="width:300px;">
        <script type="dojo/method" data-dojo-event="onChange" data-dojo-args="value">
            dojo.byId("{fldId}_Val").value = (parseInt(value[0], 10)).toString() + ", " + (parseInt(value[1], 10)).toString();
        </script>
    </div>
    <p><input type="text" id="{fldId}_Val" /></p>
</td>"""
                | wInfoBox(colSp, txtVal) -> """
<td class='infoBox cellWid' colspan='{colSp}'>
<span>{txtVal}</span>
</td>"""
                | wBlankRow(colSp) -> """
<td class='BlankRow cellWid' colspan='{colSp}'>
</td>"""
    let frmHdr = """
<form class="brijFrm" dojoType="dojox/form/Manager" id="form">
    <table>"""

    let frmFtr = """
    </table>
</form>"""

    let printHR() = printfn " - - - - - - - - - - - - - - - - - "
    let tibbie = fun (s:string) -> MessageBox.Show(s, "System Msg") |> ignore
    let defPadding:Padding = new Padding(40)
    let getCtrlHt() = 100
    let defColor:Color = Color.White
    let defForeColor:Color = Color.Black //dCobaltBlue
    let defBackColor:Color = Color.White


    let mutable timeStmp = DateTime.Now.Ticks
    let getUNID (cls:string) : DocUNID =
        timeStmp <- timeStmp + ((int64) 1)
        DocUNID(((timeStmp - (DateTime(2001, 1, 1)).Ticks).ToString() + "^" + cls))

    //let defFont:Font = new Font("Tahoma", 26.0F)
    //let defIt = Font(defFont.FontFamily, defFont.Size, FontStyle.Italic, defFont.Unit)
    
    BMfld.getDefault(tkFldList()) |> printfn "%A"

    type DnDState<'M, 'T> = 'M -> 'M * 'T

    let adder = fun l -> ("adder" + (List.length l).ToString()) :: l

    let getS = fun s -> (s,s)
    let putS s = fun _ -> (s,())
    let eval m s = m s |> fst
    let exec m s = m s |> snd
    let empty = fun s -> (s,())
    let modif f s = let x = getS in (putS (f x))
    let bind k m = fun s -> 
        let (s', a) = m s
        let s'' = adder s' //bldState s'
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
    let sRun() = 
        ``⍾`` { 
                       printfn "sRun(): run one"
                       return! getS
                    }
        // ["adder1"; "ob"]  i.e., runs thro >>=

    let sRun2() = 
        ``⍾`` { 
                       printfn "sRun2: run two"
                       return! getS
                    }

    let sRunComb() = 
        ["ob"] |>
        ``⍾`` { 
                       printfn "sRunComb: runSt"
                       //do! putS (["ob"])
                       let! a = sRun()
                       printfn "sRunComb: after running a: %A" a
                       let! b = sRun2()
                       printfn "sRunComb: after running b: %A" b
                       let! c = getS
                       return c
                    } |> printfn "sRunComb: final: %A"

(*
        runSt
        run one
        after running a: ["adder2"; "adder1"; "ob"]
        run two
        after running b: ["adder4"; "adder3"; "adder2"; "adder1"; "ob"]
        final: (["adder6"; "adder5"; "adder4"; "adder3"; "adder2"; "adder1"; "ob"],
         ["adder5"; "adder4"; "adder3"; "adder2"; "adder1"; "ob"])
        ...init mod DnD_ops...
        2.1
        eom Dnd_ops...
        ...eom...
*)

    //The launcher below is a harness to test the handler flow thro bind

    [<EntryPoint>]
    [<STAThread>]
    let main ag =
        printfn "main:1"
        match ag.Length = 0 with 
        | true -> 
            printfn "main:2"
            Application.EnableVisualStyles()
            let f = Form(Visible = false, TopMost = true, WindowState = FormWindowState.Maximized, AutoScroll = true)
            let b = new Button(Text = "ClickMe", Dock = DockStyle.Left)
            let b2 = new Button(Text = "ClickMe2", Dock = DockStyle.Right)
            let ww = ref ["ob"]
            let sRun0() = 
                ww.Value |> 
                ``⍾`` {
                       let! a = getS
                       do! putS ("ob00" :: a)
                       let! b = getS
                       printfn "sRun0 res: %A" b
                       ww.Value <- b
                    } |> ignore
            let sRun1() = 
                ww.Value |> 
                ``⍾`` {
                       let! a = getS
                       do! putS ("ob999" :: a)
                       let! b = getS
                       printfn "sRun1 res: %A" b
                       ww.Value <- b
                    } |> ignore
            let initSt = 
                                printfn "initSt"
                                b.Click.AddHandler(new EventHandler (fun sender e -> 
                                        printfn "initSt: in button handler..."
                                        sRun0()
                                        ))
                                printfn "out btnHandler1"
                                b2.Click.AddHandler(new EventHandler (fun sender e -> 
                                        printfn "initSt: in button2 handler..."
                                        sRun1()
                                        ))                                
                                printfn "initSt: ww val @ end: %A" (ww.Value)
            f.Controls.Add(b)
            f.Controls.Add(b2)
            Application.Run(f)
        | _ -> 
            printfn "exit no params"
        0

(*  output:
            out btnHandler1
            initSt: ww val @ end: ["ob"]
            initSt: in button handler...
            sRun0 res: ["adder3"; "ob00"; "adder1"; "ob"]
            initSt: in button2 handler...
            sRun1 res: ["adder6"; "ob999"; "adder4"; "adder3"; "ob00"; "adder1"; "ob"]
            initSt: in button handler...
            sRun0 res: ["adder9"; "ob00"; "adder7"; "adder6"; "ob999"; "adder4"; "adder3"; "ob00";
             "adder1"; "ob"]
            initSt: in button2 handler...
            sRun1 res: ["adder12"; "ob999"; "adder10"; "adder9"; "ob00"; "adder7"; "adder6"; "ob999";
             "adder4"; "adder3"; "ob00"; "adder1"; "ob"]
            *)

#endif //earlier

    let tbl = [[BMdzCellItm ("Cell 1",1,1,0,0); BMdzCellItm ("Cell 2",1,1,1,0)];
               [BMdzCellItm ("Cell 3",1,1,2,0); BMdzCellItm ("Cell 4",1,1,0,1)];
               [BMdzCellItm ("Cell 5",3,1,1,1); BMdzCellItm ("Cell 6",2,1,4,1)];
               [BMdzCellItm ("Cell 7",2,1,0,2); BMdzCellItm ("Cell 8",2,1,2,2)];
               [BMdzCellItm ("Cell 9",2,1,0,3); BMdzCellItm ("Cell 10",3,1,2,3)];
               [BMdzCellItm ("Cell 11",2,1,0,4); BMdzCellItm ("Cell 12",2,1,2,4)];
               [BMdzCellItm ("Cell 13",2,1,0,5); BMdzCellItm ("Cell 14",2,1,2,5)]]
    let cell2btn = 
      fun slg -> new Button(Text = slg, Dock = DockStyle.Fill, AutoSize = true)
    let colN = 3
    let isEven num = (num % 2 = 0)
    let toCellSlug = fun n -> "Cell " + n.ToString()

//The Orig loc8d on Apr 22; 2 be modif.d for initSt...
    let bld = 
        fun inLi -> 
            let toRowLi = 
                fun li -> 
                    li
                    |> List.groupBy (fun n -> 
                                        let (slg, rs, cs, (c, r)) = n
                                        r)
                    |> List.map (fun tpl -> tpl |> snd)
            inLi 
            |> List.fold (fun s v -> 
                    let (c:int, r:int, li:list<'t>) = s
                    match (c+1 = colN) with
                    | true -> 
                        //printfn "nxtRo: For c:%A r:%A liLen:%A, returning:%A" c r (li.Length) (DzCell(v))
                        c+1, r+1, BMfld.getShorty(v, 1, 1, c+1, r+1) :: li
                    | _ -> 
                        //printfn "_: For c:%A r:%A liLen:%A, returning:%A" c r (li.Length) (DzCell(v))
                        c+1, r, BMfld.getShorty(v, 1, 1, c+1, r) :: li
                    ) (0,0,[])
            |> toRowLi

    tkFldList() |> bld |> printfn "res:%A"
(*
    //loaded cellGetters: with Dnd hndlrs + sel hndlrs...

    Mon Apr 28
        see: <a href='https://learn.microsoft.com/en-us/dotnet/desktop/winforms/advanced/walkthrough-performing-a-drag-and-drop-operation-in-windows-forms?view=netframeworkdesktop-4.8#see-also'>Walkthrough: </a>Performing a Drag-and-Drop Operation in Windows Forms
        1) @ DragSrc: Set the data to be dragged + effect
        2) @ DragTgt: AllowDrop <- true; 
        DragEnter: chks if data is acceptable type + sets effect
        DragDrop: retrieve data (e.data.getdata(~)) + custom logic
*)

    let getRowTgt =
        fun f:float cliP:Panel thisRef:Form ->
            let cand = RoTgt(f)
                //DragInitHandler
            cand.MouseDown.AddHandler(new DragEventHandler( fun (sender:obj) (e:System.Windows.Forms.DragEventArgs) -> 
                cliP.DoDragDrop(cand, DragDropEffects.Move))) |> ignore
                //DragEnterHandlers
            cand.AllowDrop <- true
            cand.DragOver.AddHandler(new DragEventHandler( fun (sender:obj) (e:System.Windows.Forms.DragEventArgs) -> 
                    match e.Data.GetDataPresent(typeof<BMdzRow>) with
                    | true -> e.Effect <- DragDropEffects.Move
                    | _ -> e.Effect <- DragDropEffects.None)) |> ignore
                //DragDropHandlers
            cand.DragDrop.AddHandler(new DragEventHandler( fun (sender:obj) (e:System.Windows.Forms.DragEventArgs) -> 
                    match e.Data.GetDataPresent(typeof<BMdzRow>) with
                    | true -> 
                        //row item drop
                        let inItm = e.Data.GetData(typeof<BMdzRow>)
                        thisRef.BMdzCmdHandler "DoDrop" srcTpl Dest
                    | _ -> 
                        //cell item drop
                        let inItm = e.Data.GetData(typeof<BMdzCell>)
                        let srcTpl = //get frm inItm
                        let destTpl = //get frm cand
                        thisRef.BMdzCmdHandler "DoDrop" srcTpl destTpl  )) |> ignore

    let getB4Tgt =
        fun f:float f2:float cliP:Panel thisRef:Form ->
            let cand = B4Tgt(f f2)
                //DragInitHandler
            cand.MouseDown.AddHandler(new DragEventHandler( fun (sender:obj) (e:System.Windows.Forms.DragEventArgs) -> 
                cliP.DoDragDrop(cand, DragDropEffects.Move))) |> ignore
                //DragEnterHandlers
            cand.AllowDrop <- true
            cand.DragOver.AddHandler(new DragEventHandler( fun (sender:obj) (e:System.Windows.Forms.DragEventArgs) -> 
                    match e.Data.GetDataPresent(typeof<BMdzCell>) with
                    | true -> e.Effect <- DragDropEffects.Move
                    | _ -> e.Effect <- DragDropEffects.None)) |> ignore
                //DragDropHandlers
            cand.DragDrop.AddHandler(new DragEventHandler( fun (sender:obj) (e:System.Windows.Forms.DragEventArgs) -> 
                    match e.Data.GetDataPresent(typeof<BMdzCell>) with
                    | true -> 
                        //row item drop
                        let inItm = e.Data.GetData(typeof<BMdzCell>)
                        thisRef.BMdzCmdHandler "DoDrop" srcTpl Dest
                    | _ -> ()  )) |> ignore


       //new pichaak for EmbdTblDlg
       //added Aug20_25
        let EmbdTblDlg_પીચાક =
            fun (ક્વિમામ:option<_>) (dlg:Form) ->
                if ક્વિમામ.IsSome then 
                    //tibbie "EmbdTblDlg_પીચાક isSome"
                    (* Launched via EmbdTblBtn.ButtonClick from type DndWid
                    basically this brings up the dlgBox & ONLY on all validated (correct) choices, e.g. min 1 col added etc., it'll create the tbl AS a new docFld of fldTy FldEmbTbl?? @TBD
                    Note: autofmt; wid=100%, ht=similar 2 rtEd
                    * Add-Rem Cols(flds): cli shd be able 2 chg order 2 / NOTE also that in this case they're editable cells (a la gridxDeflt)  Sort? WidTys? @Chk.

                Aug 21
                EmbDv:
                - Poss winFlds 4 ColHdrs (dispNms) OR use winWid?
                - Add tblPln fldTy for EmbDv

                We'll nd a Masalo 4 this too (say EmbDvM)

                TblPnl to tys (ref Cpy in dskCli_AgentDef.fs)
                    *)
                    let midP:TableLayoutPanel = (!!~ "midP" dlg).Value 
                    midP.Controls.Clear()
                    midP.ColumnStyles.Clear()
                    midP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f))

                    do midP.SuspendLayout()
                    let tc = new TabControl(Dock = doc "F", Width = f.Width - defPadding.Horizontal, Height = f.Height - (defPadding.Horizontal * 8))
                    let AppearPg = new TabPage (Text = "Appearance")
                    let FldsPg = new TabPage (Text = "Columns")
                    tc.Controls.AddItems([|AppearPg; FldsPg|])
                    midP.Controls.Add(tc)
                    do midP.ResumeLayout()

                    //Appearance(ForeCol / BkCol / HdrBkCol / GridLines / etc.)

                    let GridLines = new CheckBox(AutoSize = true, Text = "Show Gridlines", Dock = doc "F", AutoCheck=true)

                    let htP = new FlowLayoutPanel(Dock = doc "F", FlowDirection = FlowDirection.LeftToRight, AutoSize = true)
                    let lbl = new Label(Text = ("Height of the Grid (in px):"), Dock = doc "F")
                    let ht = new TextBox(Dock = doc "F")
                    htP.Controls.AddItems([|lbl,ht|])
                    AppearPg.Controls.AddItems([|GridLines,htP|])


                    //let SummaryRowConfig = @TBD: move this to the cols to be able to config ea col individually ?
                    //Aug23:Oria doesn't support a SummaryRow; so we nd 2 allow users to use calcFlds for EmbdDv Totals ie, !Total(fldNm) <- must only allow EmbFlds

                    //FldsPg will use dskCli_FrmDef_EmbPnl.fs (2ndary plnk)
                    //for the EmbPnl tblPnls with addition/etc.; nd 2 link 2 def
                //- ColWids allowed (%, insist on tot)




    type DndWid(tbl) as f = 
        let bmRef = ref bm
        let selCells = ref []
        let BanarasiMasaloAux(unid,dispNm,સુપારી,usrFlds,lblFont,dataFont,foreCol, backCol, docInf) = bm
        let (unid,tit,docF, colSp, roSp,colPos,roPos,lblFont,dataFont,foreCol,backCol,soopari,vFn,CanUhear,fldTtip,tblTy) = usrFlds
        let initState = 
            ([bm], 0, [], [])  |>
            ``⍾`` { ...}
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
        let dndTBar = new ToolBar()
        let DefaultsBtn = new ToolBarButton(Text = "Form Defaults")
        DefaultsBtn.Click.AddHandler(fun o e -> 
                                                    //gappa)
        //handler pops gappa
        let BlankRowBtn = new ToolBarButton(Text = "Add Blank Row", Style = ToolBarButtonStyle.DropDown)
        let BlankRowMenu = new ContextMenu()
        let BlankRowAbove = new MenuItem ("&Above", (new EventHandler(fun o e -> BMdzCmdHandler("AddBlankRow", true, pos))), Shortcut = Shortcut.A)
        let BlankRowBelow = new MenuItem ("&Below", (new EventHandler(fun o e -> BMdzCmdHandler("AddBlankRow", false, pos))), Shortcut = Shortcut.B)
        
        let InfoBoxBtn = new ToolBarButton(Text = "Add InfoBox", Style = ToolBarButtonStyle.DropDown)
        let InfoBoxMenu = new ContextMenu()
        let InfoBoxAbove = new MenuItem ("&Above", (new EventHandler(fun o e -> )), Shortcut = Shortcut.A)
        let InfoBoxBelow = new MenuItem ("&Below", (new EventHandler(fun o e -> )), Shortcut = Shortcut.B)
        
        let FieldBtn = new ToolBarButton(Text = "Add Field", Style = ToolBarButtonStyle.DropDown)
        let FieldMenu = new ContextMenu()
        let FieldAbove = new MenuItem ("&Before", (new EventHandler(fun o e -> )), Shortcut = Shortcut.B)
        let FieldBelow = new MenuItem ("&After", (new EventHandler(fun o e -> )), Shortcut = Shortcut.A)

        //Added 8/20: AddEmbdTbl
        let EmbdTblBtn = new ToolBarButton(Text = "Add Embedded Table", Style = ToolBarButtonStyle.PushButton)
        EmbdTblBtn.ButtonClick.AddHandler(new ToolBarButtonClickEventHandler(fun o e -> 
                new EmbdTblDlg(thisTblDef)
                if result.OK.... update currDef...
            ))

        dndTBar.Controls.AddItems([DefaultsBtn; BlankRowBtn; InfoBoxBtn; FieldBtn])

        frm.Controls.Add(cliP)
        frm.Controls.Add(lbl)
        frm.ResumeLayout(false)
        let initState = 
            //the 1st run through the monadic flow...
            f.rebldUIlayout()
        member f.bldState() = 
            //(BMdzTbl -> UIModel)
            let cTot, rTot, uiM = 
                li  |> List.fold (fun s v -> 
                        let (c:int, r:int, roLi:list<'t>) = s
                        let ([BMdzCellItm(slg, cc, cr, ccI, crI)]) = v
                            match roLi with
                            | [] -> 0, 0, []
                            | _ ->
                                let thisRoSt = 
                                    roLi 
                                    |> lim (fun thisCl -> [BMdzCellItm(slg,cc,cr,c,r)])
                                    |> lico
                                match (not(c+1 < colN)) with
                                | true -> 0, r+1, (thisRoSt @ roLi)
                                | _ -> c+cc, r, (thisRoSt @ roLi))
                        ) (0,0,[]) 
                    |> List.rev
            (li, uiM)

        member f.bldState_v1() = 
            //(BMdzTbl -> UIModel)
	    //This ver subseq depr in favor of removing b4/roTgts (Apr28)
            //@ToDo: use incoming BM tyDef; remove colN
            //@ToDo: after calling, reset tbl ref in UI + reset (cliP.RowCount <- lilen tbl)
            let cTot, rTot, uiM = 
                li  |> List.fold (fun s v -> 
                        let (c:int, r:int, roLi:list<'t>) = s
                        let ([BMdzCellItm(slg, cc, cr, ccI, crI)]) = v
                            match roLi with
                            | [] -> 0, 0, [getRowTgt(((float) 0 - 0.5), cliP, f)]
                            | _ ->
                                let thisRoSt = 
                                    roLi 
                                    |> lim (fun thisCl -> [getB4Tgt(((float) 0-0.5), ((float) r+0.5), cliP, f); BMdzCellItm(slg,cc,cr,c,r)])
                                    |> lico
                                //@ToDo: Apr26: Add afterTgt @ end of ro 4 completeness
                                let withRoTgt = [getRowTgt(((float) r + 0.5), cliP, f)] @ thisRoSt
                                match (not(c+1 < colN)) with
                                | true -> 0, r+1, (withRoTgt @ roLi)
                                | _ -> c+cc, r, (withRoTgt @ roLi))
                        ) (0,0,[]) 
                    |> List.rev
            (li, uiM)
        member f.rebldUIlayout() = 
            let getTblRo =
                fun tbl idx ->
                List.filter (fun c ->
                            let (BMdzCellItm(_,_,_,_, cRo)) = c 
                            cRo = idx) tbl
            cliP.SuspendLayout()
            cliP.Controls.Clear()
            cliP.ColumnStyles.Clear()
            cliP.RowStyles.Clear()
            //@ToDo: 2 incorp B4Tgts (1/colN) becomes (1/(colN*2))
            [1.. colN] |> lim (fun c -> cliP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, ((float32) ((1/colN) * 100))))) |> ignore
            //@ToDo: clear existing handler?
            cliP.Layout.AddHandler(new LayoutEventHandler( fun (sender:obj) (e:System.Windows.Forms.LayoutEventArgs) -> 
                //necc? let thisF = sender :?> Form
                let rec procTbl currRo remainder =
                    let remLen =
                    getTblRo remainder currRo 
                    |> List.map (fun currcell -> 
                                    let (BMdzCellItm(slg,cSp,rSp,cCo, cRo)) = currcell
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

        member f.cellSelected(title, cellPos) = 
                                        //udpate stateRef
                                        this.unid :: selItems
                                        setBackgroundColor -> red
                                        //enable/disable TBarBtns)
        member f.addDropHandlers() =
            tbl |> lim (fun cel -> 
                            let (pos, _) = cel
                            match cel with 
                            | BlankCell | BlankRow | B4Tgt | RoTgt -> 
                                cel.AddDropHandler(fun o:sender, e:DragEventArgs ->
                                    let tgt = (cell) o
                                    // If the DataObject contains string data, extract it.
                                    match e.Data.GetDataPresent(DataFormats.StringFormat) with
                                    | true -> 
                                        let dataString = (string) e.Data.GetData(DataFormats.StringFormat)
                                        let dropCell = getCellFor(pos)  //from dataString
                                        //ins & rebuild
                                    | _ -> ()))

            // If th
                                //monadicFlo...
                                )
                            | _ -> ())
        member f.getColN() = 
            let (BanarasiMasaloAux(_, _, colN...
            colN
        member f.getRoN() = 
            let (BanarasiMasaloAux(_, _, roN...
            lilen struct
        member f.BMdzCmdHandler = 
            fun cmd params =
(*      Apr25:
            DoDrop covers Pos _but_ we nd coverage for the foll:
            chgCsp/Rsp
            Thingy
            Ttip
            RemFld
        Apr7:
            BM contains ONLY currState (for persist) BUT refCells 4 list<BMTbl, intPtr> for undo/redo

                { unid = (getUNID "BmFldTest"); title = tit ; docF = f; 
                colSpan = 1 ; rowSpan = 1 ; Pos = (0, 0); lblFont = defIt ; dataFont = defFont; 
                foreCol = Some(Color.Black); backCol = Some(Color.Black); 
                soopari = box 1; vFn = None ; CanUhear = Thingy("") ; fldTtip = None; tblTy = TaskTbl() } )
*)
                match cmd with
                    | "chgCol" ->
                        let (fore, newCol) = params
                        match fore with
                        | "fore" ->  f.updCellAt pos tbl (fun c -> { c with foreCol = newCol } )
                        | "back" -> f.updCellAt pos tbl (fun c -> { c with backCol = newCol } )
                    | "chgFont" ->
                        let (ctrl, newFont) = params
                        //@ToDo: nd to tupleize (face,size,type(norm/ital/bold))  //@ToDo: upd8 fldTy
                        match ctrl with
                        | "label" -> f.updCellAt pos tbl (fun c -> { c with lblFont = newFont } )
                        | "data" -> f.updCellAt pos tbl (fun c -> { c with dataFont = newFont } )
                    | "chgColSpan" (pos, newSp) ->
                        f.updCellAt pos tbl (fun c -> { c with colSpan = newSp } )
                    | "chgRowSpan" (pos, newSp) ->
                        f.updCellAt pos tbl (fun c -> { c with rowSpan = newSp } )
                    | "DoDrop" ->
                        let (srcTpl, Dest) = params
                        match Dest with
                        | BlankCell | BlankRow ->
                            let ro = tbl.[c]
                            let oldC = ro.[r]   //- Fetch existing fld
                            let destRo = newTbl.[destC]
                            let insertd = //insert in destRo
                                let front, back = ro |> List.splitAt r
                                [front] @ [oldC] @ [List.tail back]
                            let updRo = 
                                let front, back = destRo |> List.splitAt destR
                                [front] @ [insertd] @ [List.tail back]
                            //we're inserting 1st + removing l8r coz removing will alter idxes, esp.ly sameRow
                            let newRo =       //- Rebld w/o
                                let front, back = updRo |> List.splitAt r
                                front :: (List.tail back)
                            let frontT, backT = tbl |> List.splitAt c
                            let newTbl = [frontT] @ [newRo] @ [List.tail backT]
                            state.insertAt pos >> ``⍾`` { ...}
                        | BMdzCell ->
                            //@ToDo: tbfo
                            //- Fetch existing fld
                            //- Remove from struct
                            //- repl dest cell _before_
                        update BMRef with new struct
                        run thro monadic flow
                    | "AddBlankRow" (above, pos) ->
                        match above with
                        | true -> 
                            let front, back = tbl |> List.splitAt c
                            front @ [BlankRow(c)] @ back
                        | _ -> 
                            let front, back = tbl |> List.splitAt (c+1)
                            front @ [BlankRow(c+1)] @ back
                    | "AddInfoBox" (above, pos) ->
                        //@ToDo: tbfo
                        //@TBD: 1st gappa 2 fetch str?
                        match above with
                        | true -> 
                            let front, back = tbl |> List.splitAt c
                            front @ [InfoBox(c)] @ back
                            state.insertAt pos >> ``⍾`` { ...}
                        | _ -> 
                            let front, back = tbl |> List.splitAt (c+1)
                            front @ [InfoBox(c+1)] @ back
                    | "AddField" (before, pos) ->
                        //@ToDo: tbfo
                        //@ToDo: 1st gappa 2 fetch fld excl pres; intl will be ReadOnly + nm preceded w/'*'
                        let (DocFld(ft, intNm, isInt, tit)) = f
                        match before with
                        | true -> 
                            let ro = tbl.[c]
                            let newRo = 
                                let front, back = ro |> List.splitAt (r)
                                let newCell = 
                                    { unid = (getUNID "CustIdTblId"); title = tit ; docF = f; 
                                    colSpan = 1 ; rowSpan = 1 ; Pos = (c, r); lblFont = defIt ; dataFont = defFont; 
                                    foreCol = Some(Color.Black); backCol = Some(Color.Black); 
                                    soopari = box 1; vFn = None ; CanUhear = Thingy("") ; fldTtip = None; tblTy = TaskTbl() } )
                                [front, newCell, back]                        
                                state.insertAt pos >> ``⍾`` { ...}
                        | _ -> 
                            let ro = tbl.[c]
                            let newRo = 
                                let front, back = ro |> List.splitAt (r+1)
                                let newCell = 
                                    { unid = (getUNID "CustIdTblId"); title = tit ; docF = f; 
                                    colSpan = 1 ; rowSpan = 1 ; Pos = (c, r+1); lblFont = defIt ; dataFont = defFont; 
                                    foreCol = Some(Color.Black); backCol = Some(Color.Black); 
                                    soopari = box 1; vFn = None ; CanUhear = Thingy("") ; fldTtip = None; tblTy = TaskTbl() } )
                                [front, newCell, back]                        
                                state.insertAt pos >> ``⍾`` { ...}
                    | "RemField" (pos) ->
                        let ro = tbl.[r]
                        let newRo = ro.RemoveAt(c)
                        List.InsertAt r newRo tbl
                    | "ChgThingy" (newT) ->
                        f.updCellAt pos tbl (fun c -> { c with CanUhear = newT } )
                    | "ChgThingy" (newTxt) ->
                        f.updCellAt pos tbl (fun c -> { c with CanUhear = Some(newTxt) } )
(*
Run these cmds from cellRightClickMenu:
vFn / Thingy / ForeCol / BkCol / LblFont / DatFont / col-roSpan / removeFld / TtipTxt
//8_20:
FOR EmddTbl: rt-click (CLICK 2?) will allow 'Props' brings up dlgBox w/foll tabs:
* Appearance(ForeCol / BkCol)
* Add-Rem Cols(flds): cli shd be able 2 chg order 2 / NOTE also that in this case they're editable cells (a la gridxDeflt)  Sort? WidTys? @Chk.

These from TBarBtns:
Defaults / AddBlankRo / AddInfoBox / AddFld / AddEmbdTbl

Another option: Instd of all the rt-click cmds; unify em in a DlgBox and menu simply says "Modify Fld"
[Repurpose propBox?]

@chk Move 2 TBarBtns:
-Ins-Del Cell|Ro  -roSpan-colSpan 4 cell|Ro  -InfBox<br>
This is in order to enable/disable upon sel (visual cues)

[Add LN-ty btn "Make Default" 4 fonts etc. (particular scope: lbl/data)]

*)
                    | "vFn " (pos) -> 
                        //@ToDo: tbfo
                       gappa (vFn)
                       getCellAtPos(pos) >>  ``⍾`` { ...}
                    | unDo ->
                        //@ToDo: tbfo
                    | reDo ->
                        //@ToDo: tbfo
            //trigger the monadic stuff; _after_ updating BMref (it'll do the rest)
        member f.getSelCellTitle = 
            //for lilen 1
            let (t, cP, rP) = selCells
            t
        member f.getSelCellPos = 
            //@ToDo: tbfo
            //for lilen 1
            let (t, cP, rP) = selCells
            (cP, rP)
        member f.getCellPosFor(cell) = 
            //for lilen 1
            let (t, cP, rP) = selCells
            (cP, rP)
        member f.updCellAt pos tbl f =
            let ro = tbl.[c]
            let cl = ro.[r]
            let newRo = List.updateAt r (f cl) ro
            List.updateAt c newRo tbl

    printfn "interimRes3:%A" (bldState tbl)

(* interimRes3:
        [RoTgt -0.5; BMdzCell ("Cell 1", 1, 1, 0, 0); BMdzCell ("Cell 2", 1, 1, 1, 0); BMdzCell ("Cell 3", 1, 1, 2, 0); 
        RoTgt 0.5; BMdzCell ("Cell 4", 1, 1, 0, 1); BMdzCell ("Cell 5", 3, 1, 1, 1); BMdzCell ("Cell 6", 2, 1, 4, 1); 
        RoTgt 1.5; BMdzCell ("Cell 7", 2, 1, 0, 2); BMdzCell ("Cell 8", 2, 1, 2, 2); 
        RoTgt 2.5; BMdzCell ("Cell 9", 2, 1, 0, 3); BMdzCell ("Cell 10", 3, 1, 2, 3); 
        RoTgt 3.5; BMdzCell ("Cell 11", 2, 1, 0, 4); BMdzCell ("Cell 12", 2, 1, 2, 4); 
        RoTgt 4.5; BMdzCell ("Cell 13", 2, 1, 0, 5); BMdzCell ("Cell 14", 2, 1, 2, 5); 
        RoTgt 5.5]
*)

    printfn "eom Dnd_ops..."

    printfn "...eom FrmDef_Actual..."

module FrmDef_Test = 
    open System
    open System.Drawing
    open System.Windows.Forms
    open Trivedi
    open Trivedi.Core

    type BMfld<'t when 't :> ITblMarker> with
        member this.toAbstractModel() =
"""| {this.title} | {this.docF}| {this.colSpan} | {this.rowSpan} | {(this.Pos |> fst)} | {(this.Pos |> snd)} | {this.lblFont} | {this.dataFont} | {this.foreCol} | {this.backCol} | {this.CanUhear} | {this.fldTtip} |"""

    type FrmAbstractM(m:list<list<string>>) with
        member this.toModel() = 
            let liJoin li = ("", li) |> lifo (fun s v -> s + "\n" + v)
            m |> lim (fun r -> r |> lim (fun c -> c.toAbstractModel()) |> liJoin) //poss nd to put in []
        member updItm(pos, fldIdx, newV) = 
            //let text = "| title | docF | colSpan | rowSpan | Posfst | Possnd | lblFont | dataFont | foreCol | backCol | CanUhear | fldTtip |"
            let (c, r) = pos
            let ro = m.[c]
            let cl = ro.[r]
            let text = cl.toAbstractModel()
            let rx = new Regex(@"\|\s(?<fld>\w+)", RegexOptions.IgnoreCase)
            let mat = rx.Matches(text) |> List.ofSeq
            let newCl= 
                List.fold (fun s (v:Match) -> 
                        let (i, st) = s
                        let groups = v.Groups
                        match (i=idx) with 
                        | true -> 
                            printfn "%A" (groups.[0].Value)
                            printfn "%A) '%A' fnd at position %A" (i.ToString()) (groups.["fld"].Value) (groups.[1].Index)
                            i + 1, st
                        | _ -> i + 1, st) (0,"") mat
            let newRo = List.updateAt r (newCl) ro
            List.updateAt c newRo m

    let chgColSp pos newS = 
        { new Operation<FrmWrapper,FrmAbstractM>() with
            member __.Run m = m.updItm(pos, 2, newS).toModel()
            member __.Check (f,m) = f.chgColSp(pos, newS)).toModel()
                |> Prop.label (sprintf "chgColSp: model = %A, actual = %A" m res)
            override __.ToString() = "chgColSp"}
            
    let chgRoSp pos newS = 
        { new Operation<FrmWrapper,FrmAbstractM>() with
            member __.Run m = m.updItm(pos, 6, newS).toModel()
            member __.Check (f,m) = f.chgRoSp(pos, newS)).toModel()
                |> Prop.label (sprintf "chgRoSp: model = %A, actual = %A" m res)
            override __.ToString() = "chgRoSp"}

    let chgDataLbl pos newF = 
        { new Operation<FrmWrapper,FrmAbstractM>() with
            member __.Run m = m.updItm(pos, 7, newF).toModel()
            member __.Check (f,m) = f.chgDataLbl(pos, newF)).toModel()
                |> Prop.label (sprintf "chgDataLbl: model = %A, actual = %A" m res)
            override __.ToString() = "chgDataLbl"}

    let chgForeCol pos newC = 
        { new Operation<FrmWrapper,FrmAbstractM>() with
            member __.Run m = m.updItm(pos, 8, newC).toModel()
            member __.Check (f,m) = f.chgForeCol(pos, newC)).toModel()
                |> Prop.label (sprintf "chgForeCol: model = %A, actual = %A" m res)
            override __.ToString() = "chgForeCol"}

    let chgBackCol pos newC = 
        { new Operation<FrmWrapper,FrmAbstractM>() with
            member __.Run m = m.updItm(pos, 9, newC).toModel()
            member __.Check (f,m) = f.chgBackCol(pos, newC)).toModel()
                |> Prop.label (sprintf "chgBackCol: model = %A, actual = %A" m res)
            override __.ToString() = "chgBackCol"}

    let chgThingy pos newT = 
        { new Operation<FrmWrapper,FrmAbstractM>() with
            member __.Run m = m.updItm(pos, 10, newT).toModel()
            member __.Check (f,m) = f.chgThingy(pos, newT)).toModel()
                |> Prop.label (sprintf "chgThingy: model = %A, actual = %A" m res)
            override __.ToString() = "chgThingy"}

    let chgTtip pos newT = 
        { new Operation<FrmWrapper,FrmAbstractM>() with
            member __.Run m = m.updItm(pos, 11, newT).toModel()
            member __.Check (f,m) = f.chgTtip(pos, newT)).toModel()
                |> Prop.label (sprintf "chgTtip: model = %A, actual = %A" m res)
            override __.ToString() = "chgTtip"}

    let doDrop pos newP = 
        { new Operation<FrmWrapper,FrmAbstractM>() with
            member __.Run m = 
                let withNewC = m.updItm(pos, 4, newP |> fst)
                withNewC.updItm(pos, 5, newP |> snd)
            member __.Check (f,m) = f.doDrop(pos, newP)).toModel()
                |> Prop.label (sprintf "doDrop: model = %A, actual = %A" m res)
            override __.ToString() = "doDrop"}
            
    let create initialValue = 
        { new Setup<FrmWrapper,FrmAbstractM>() with
            member __.Actual() = ((FrmWrapper(initialValue)))
            member __.Model() = initialValue }
    
    type TearDownDsk<'Actual>() =
        inherit TearDown<'Actual>()
        override __.Actual actual = 
            pbt.Dsk(( (FrmWrapper) actual).toModel())
    
    //consider switching betw Music+tkTbl?
    let initModel = MusicFldList() |> bld |> tblAbstractMod
    
    let FrmStateMachine =
      { new Machine<FrmWrapper,FrmAbstractM>() with
          member __.Setup = Gen.constant(initModel) |> Gen.map create |> Arb.fromGen
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
    
    Check.Quick (StateMachine.toProperty FrmStateMachine)

    printfn "...eom FrmDef_Test..."

