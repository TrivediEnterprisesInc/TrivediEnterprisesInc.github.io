(*  Brij (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 
    TrivediBldHdr->
    |Trivedi SrcCtrlHdr File.  Copyright (c) 2015-2025 M. P. Trivedi.  All rights reserved.|637901455|test.fs|none|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS 

    //minus UIAux
    fsc src\pbt\Dnd_ops.fs  --platform:x64 --standalone --target:exe --out:src\pbt\dnd.exe -r:lib\Trivedi.Core.dll -r:lib\Trivedi.UI.dll -I:C:\Windows\Microsoft.NET\Framework\v4.0.30319

    Last updated: Tue Apr 24 2025

    Contains modules:      FrmDef_Actual
                                    FrmDef_Ext
                                    //tibbie FrmDef_Test

    Outstanding: 
        move ops mod type members (member this.toFsChkModel() ...) to Dnd_Ext via extensions (just 'with')
        modify: we no longer nd to ins dropTgts in the html.
        BMfld.getDefault(docF:DocFld list, ty:'t)
        Instd of matching/chking targets 4 eligibility (canAddRow/Col/etc) it'd be b8r 2 show/hide TbarBtns for ea state
            e.g., if SingleCellSel/AllSelSameRo -> enable AddRoAbove/Belo
            NO selection reqd 4 popupMnu chg cellDetails (props)

    Logic update needed: on AddField OR DndAdd: if overflow addBlankRowNext >> move overflowCell
    
    https://www.meziantou.net/detecting-dark-and-light-themes-in-a-wpf-application.htm

    dojo textBoxes (other w's too?) allow using placeHolder: "type in your name" as part of the wid html

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

    type BMdzCell = | BMdzCellItm of BMFld
                            | BlankCell of (int * int)
                            | B4Tgt of (float * float)

    type BMdzRow = | BMdzRowItm of list<BMdzCell>
                             | BlankRow of int
                             | RoTgt of float

    type BMdzTbl = | BMdzTbl of list<BMdzRow>
    
    //updated Apr'23, src:UIAux
    type BanarasiMasaloAux<'t when 't :> ITblMarker> = | BanarasiMasaloAux of unid:DocUNID * dispNm:string *
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


// type DocFld = | DocFld of FldType*string*bool*string with...
    type FldType = | FldString
                           | FldNumber
                           | FldCurrency
                           | FldLongString
                           | FldAttachment
                           | FldBoolean
                           | FldChoiceList
                           | FldRadioBtn
                           | FldRange
                           | FldNumUpDn  //nixed (range covers this) Apr23: NOT
                           | FldDate 
                           | FldDateTime //nixed (use sep)
                           | FldColor
                           | FldFont
                           | FldInfoBox
                           | FldBlankRow
                           | UserInput  //nixed (??)
                           | FldBtn     //nixed (??)
                           | FldValidBtn //nixed (??)   with
        member this.getDefThingy() = 
            match this with
               | FldString -> wTextBox       //+ SimpleTextarea
               | FldNumber  -> wNumberTxtBox     //NumberTxtBox restricts to #s
               | FldCurrency -> wCurrencyTextBox
               | FldLongString -> wEditor
               | FldAttachment -> 
               | FldBoolean -> wCheckBox
               | FldChoiceList -> wChoiceList  //also has separate wid for multiSelect
               | FldRadioBtn -> wRadioButton
               | FldRange -> wNumberSpinner //+ HorizontalSlider Apr23: NOT; is a sep wid
               | FldDate  -> wDateTextBox
               | FldColor -> existing but modify btn; add fld   //@TBD: we poss don't nd this in webCli; only intl use
               | FldFont -> existing but modify btn; add fld  //@TBD: we poss don't nd this in webCli; only intl use
               | FldInfoBox -> wInfoBox
               | FldBlankRow -> wBlankRow
            printfn "tbfo; ret settable"
    
    type Wid = | wTextBox
                    | wSimpleTextarea
                    | wNumberTxtBox
                    | wCurrencyTextBox
                    | wEditor
                    | wCheckBox
                    | wRadioButton
                    | wNumberSpinner
                    | wHorizontalSlider
                    | wDateTextBox
                    | wTimeTxtBox 
                    | wInfoBox
                    | wBlankRow 
                    | wChoiceList 
                    | wRTEditor 
                    | wCheckedMultiSel 
                    | wRating 
                    | wRangeSlider with
        member this.getHtml(supari) = 
            //move 2 BmFld + add this.defThingy = ...
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
                | wEditor -> """
                | wCheckBox(lblTxt, fldId, fldNm, fldVal) -> 
                    //@TBD: Does this cover multiple opts?
"""<td>
    <label for="{fldId}">{lblTxt}</label>
    <input class="dojoFormValue" type="checkbox" dojoType="dijit/form/CheckBox" 
    id="{fldId}" name="{fldNm}" value={fldVal} data-dojo-observer="window.mShowVals();">
</td>"""
                | wRadioButton -> 
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
                | wInfoBox(colSp, txtVal) -> """
<td class='infoBox cellWid' colspan='{colSp}'>
<span>{txtVal}</span>
</td>"""
                | wBlankRow() -> """
<td class='BlankRow cellWid' colspan='{colN}'>
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
        fun li -> 
            li 
            |> List.fold (fun s v -> 
                    let (c:int, r:int, li:list<'t>) = s
                    match isEven r with
                    | true -> 
                        //printfn "isEven: For c:%A r:%A liLen:%A, returning:%A" c r (li.Length) (DzDnDTgt(v))
                        0, r+1, DzDnDTgt(v,colN,1) :: li
                    | _ -> 
                        match (c+1 = colN) with
                        | true -> 
                            //printfn "nxtRo: For c:%A r:%A liLen:%A, returning:%A" c r (li.Length) (DzCell(v))
                            c+1, r+1, DzCell(v,1,1) :: li
                        | _ -> 
                            //printfn "_: For c:%A r:%A liLen:%A, returning:%A" c r (li.Length) (DzCell(v))
                            c+1, r, DzCell(v,1,1) :: li
                    ) (0,0,[]) 


    //loaded cellGetters: with Dnd hndlrs + sel hndlrs...
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
        dndTBar.Controls.AddItems([DefaultsBtn; BlankRowBtn; InfoBoxBtn; FieldBtn])
        frm.Controls.Add(cliP)
        frm.Controls.Add(lbl)
        frm.ResumeLayout(false)
        let initState = 
            //the 1st run through the monadic flow...
            f.rebldUIlayout()
        member f.bldState() = 
            //(BMdzTbl -> UIModel)
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
(*        Apr7:
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
                    //menuCmds
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

    printfn "...eom..."

