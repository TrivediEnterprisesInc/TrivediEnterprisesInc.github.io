(*  Brij (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 
    TrivediBldHdr->
    |Trivedi SrcCtrlHdr File.  Copyright (c) 2015-2025 M. P. Trivedi.  All rights reserved.|637901455|test.fs|none|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|

    //minus UIAux
    fsc src\pbt\Dnd_ops.fs  --platform:x64 --standalone --target:exe --out:src\pbt\dnd.exe -r:lib\Trivedi.Core.dll -r:lib\Trivedi.UI.dll -I:C:\Windows\Microsoft.NET\Framework\v4.0.30319

    ToDo: 
    - move ops mod type members (member this.ChkHtml() ...) to Dnd_Ext via extensions (just 'with')
    - modify: we no longer nd to ins dropTgts in the html.

    Last updated: Tue Apr 5 2025

*)

namespace Trivedi

#nowarn "20" "25" "58" "66" "67" "64" "760" "1182" "1558"

type BMdzCell = | BMdzCellItm of string * int * int * int * int
                        | B4Tgt of float * float with
    member this.ChkHtml() =
        match this with
        | BMdzCellItm(slg, _, _, _, _)  -> 
            "<td>\n<div class='CellTgt'>" + slg + "</div>\n</td>"
        | B4Tgt(a,b) -> """
            <td>
                <div class='B4Tgt'></div>
            </td>
"""

type BMdzRow = | BMdzRowItm of list<BMdzCell>
                         | RoTgt of float with
    member this.ChkHtml() =
        match this with
        | RoTgt r -> """
            <tr><td>
                <div class='RoTgt'></div>
            </td></tr>
"""
        | BMdzRowItm lc -> 
            (lifo (fun s (v:BMdzCell) -> s + "\n" + v.ChkHtml()) "<tr>\n" lc) + "\n</tr>"


type BMdzTbl = | BMdzTbl of list<BMdzRow> with
    member this.ChkHtml() =
        let (BMdzTbl(l)) = this
        ( lifo (fun s (v:BMdzRow) -> s + "\n" + v.ChkHtml()) "/n<table>" l) + "/n</table>"

//updated Apr'23, from ty BMfld src:UIAux
type BMfld<'t when 't :> ITblMarker> = | BMfld of unid:DocUNID * title:string * docF:DocFld 
                                    * colSpan:int * rowSpan:int * colPos:int * roPos:int 
                                    * lblFont:Font * dataFont:Font * foreCol:Color * backCol:Color * soopari:obj
                                    * vFn:('t -> bool) option * CanUhear:Thingy * fldTtip:string option * tblTy:'t with
    override this.ToString() = 
        let (BMfld(docF, colSpan, rowSpan, vFn, CanUhear, fldTtip, tblTy)) = this
        "BMfld: |DocFld: " + docF.ToString() + "|colSp: " + colSpan.ToString() + 
            "|rowSp: " + rowSpan.ToString() + "|vFn: " + vFn.ToString() + "|tblTy: " + (tblTy.GetType()).ToString()
    static member getDefault(docF:DocFld list, ty:'t) = 
        //@ToDo: We nd to run it initially thro the lifo loop to gen the default cellStruct
        docF |> lim (fun f -> BMfld(f, 1, 1, None, Thingy(""), None, ty))

//updated Apr'23, src:UIAux
type BanarasiMasaloAux<'t when 't :> ITblMarker> = | BanarasiMasaloAux of unid:DocUNID * dispNm:string *
                                                    સુપારી:int * usrFlds:BMfld<'t> list * 
                                                    usrDefLblFont:Font * usrDefDataFont:Font * 
                                                    usrDefForeColor:Color * usrDefBackColor:Color * 
                                                    docInf:DesDocInf with
    static member getDefault(docF:DocFld list, nm, ty:'t) = 
        let bf = BMfld.getDefault(docF, ty)
        let defIt = Font(defFont.FontFamily, defFont.Size, FontStyle.Italic, defFont.Unit)
        BanarasiMasaloAux((getUNID (ty.ToString()), nm, 2, bf, defIt, defFont, currentScheme.Fore(), currentScheme.Back(), DesDocInfDeflt()))


[<AutoOpen>]
module DnDMonad =
    open System
    open Trivedi
    open Trivedi.Core
    open System.Windows.Forms

    type DnDState<'M, 'T> = 'M -> 'M * 'T

    let adder = fun l -> ("adder" + (List.length l).ToString()) :: l

(*

    //essentially cellModel -> (cellModel * UIModel)
    let resetState = 
        fun (li:list<list<BMdzCell>>) (colN) -> 
            //@ToDo: after calling, reset tbl ref in UI + reset (cliP.RowCount <- lilen tbl)
            let cTot, rTot, uiM = 
                li  |> List.fold (fun s v -> 
                        let (c:int, r:int, inLi:list<'t>) = s
                        let ([BMdzCellItm(slg, cc, cr, ccI, crI)]) = v
                        match inLi with
                        | [] -> 0, 0, [RoTgt((float) 0 - 0.5)]
                        | _ ->
                            //lim 4 ro, handle wraparounds here
                            match (not(c+1 < colN)) with
                            | true -> 
                                0, r+1, [[RoTgt((float) r + 0.5)]; [B4Tgt(((float) 0-0.5), ((float) r+0.5)); BMdzCellItm(slg,cc,cr,c,r)]] @ inLi
                            | _ -> 
                                //@ToDo: nd 2 addRo here AFTER to preserve rest?
                                c+cc, r, [[RoTgt((float) r + 0.5)];[B4Tgt(((float) 0-0.5), ((float) r+0.5)); BMdzCellItm(slg,cc,cr,c,r)]] @ inLi)  (0,0,[]) 
                    |> List.rev
            (li, uiM)

    let resetTblLayoutPanel = 
        fun cliP colN rowN tblRef ->
            let getTblRo =
              fun tbl idx ->
                List.filter (fun c ->
                          let (BMdzCellItm(_,_,_,_, cRo)) = c 
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

    let getIdxOfFirstCellForRow =
        fun tbl roIdx cellMod -> 
            printfn "tibbie"
           
    //@TBD: need cellTy?
    let doInsCell =
        fun (srcSlg, Dest, cellSt:list<BMdzCell>) -> 
            printfn "tibbie"

    //@TBD: need cellTy?
    let doDelCell =
        fun (srcSlg, Dest, cellSt:list<BMdzCell>) -> 
            printfn "tibbie"
            
    //@TBD: need cellTy?
    let doInsRo =
        fun (srcSlg, Dest, cellSt:list<BMdzCell>) -> 
            printfn "tibbie"

    //@TBD: need cellTy?
    let doDelRo =
        fun (srcSlg, Dest, cellSt:list<BMdzCell>) -> 
            printfn "tibbie"
            
    //@TBD: need cellTy?
    let doInsCol =
        fun (srcSlg, Dest, cellSt:list<BMdzCell>) -> 
            printfn "tibbie"

    //@TBD: need cellTy?
    let doDelCol =
        fun (srcSlg, Dest, cellSt:list<BMdzCell>) -> 
            printfn "tibbie"

    let doDrop = 
        fun (srcSlg, Dest, colN, cellSt:list<BMdzCell>) -> 
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


    let sHarness() = 
        let f = Form(Visible = false, TopMost = true, WindowState = FormWindowState.Maximized, AutoScroll = true)
        let b = new Button(Text = "ClickMe")
        let initSt = 
            ["ob"] |>
            ``⍾`` { 
                           printfn "initSt"
                           b.Click.AddHandler (fun o e ->
                                    //this inner call doesn't work (@mbi?)
                                    ``⍾`` { 
                                                printfn "in button handler..."
                                                let! b = sRun2()
                                                printfn "after running b: %A" b 
                                                printfn "in button handler b4 putS..."
                                                do! putS b} |> ignore
                                                )
                           let! a = sRun()
                           printfn "after running a: %A" a
                           f.Show()
                        }
        printfn "created Harness..."

    //this handler contains selection logic for cells
    //attach 2 ea cellPnl plus to base/parent form (nds testing) to deselect/empty selItms
    let selHandler =new EventHandler (fun sender e -> 
        //add refCell 4 [selItems] in the host frm/ctrl
        if (e.Shift && (Lico this.unid selItems.Value))
            removeRef
        else 
            this.unid :: selItems
            setBackgroundColor -> red)
*)


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

(*
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

[<AutoOpen>]
module DnD_ops = 
    open System
    open System.Drawing
    open System.Windows.Forms
    open Trivedi
    open Trivedi.Core
    //open Trivedi.UI
    //open Trivedi.UIAux

(*
    What:           MVP for impl/testing DnD func + (l8r) Dojo wireframes...
    Src:            https://github.com/TrivediEnterprisesInc/TrivediEnterprisesInc.github.io/blob/main/winFrms.fs
    Last updated:   Mon Mar 31 2025
    Stat:           interimRes3:
                    [RoTgt -0.5; BMdzCell ("Cell 1", 1, 1, 0, 0); BMdzCell ("Cell 2", 1, 1, 1, 0); BMdzCell ("Cell 3", 1, 1, 2, 0); 
                    RoTgt 0.5; BMdzCell ("Cell 4", 1, 1, 0, 1); BMdzCell ("Cell 5", 3, 1, 1, 1); BMdzCell ("Cell 6", 2, 1, 4, 1); 
                    RoTgt 1.5; BMdzCell ("Cell 7", 2, 1, 0, 2); BMdzCell ("Cell 8", 2, 1, 2, 2); 
                    RoTgt 2.5; BMdzCell ("Cell 9", 2, 1, 0, 3); BMdzCell ("Cell 10", 3, 1, 2, 3); 
                    RoTgt 3.5; BMdzCell ("Cell 11", 2, 1, 0, 4); BMdzCell ("Cell 12", 2, 1, 2, 4); 
                    RoTgt 4.5; BMdzCell ("Cell 13", 2, 1, 0, 5); BMdzCell ("Cell 14", 2, 1, 2, 5); 
                    RoTgt 5.5]
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
    
    let tbl = [[BMdzCellItm ("Cell 1",1,1,0,0); BMdzCellItm ("Cell 2",1,1,1,0)];
               [BMdzCellItm ("Cell 3",1,1,2,0); BMdzCellItm ("Cell 4",1,1,0,1)];
               [BMdzCellItm ("Cell 5",3,1,1,1); BMdzCellItm ("Cell 6",2,1,4,1)];
               [BMdzCellItm ("Cell 7",2,1,0,2); BMdzCellItm ("Cell 8",2,1,2,2)];
               [BMdzCellItm ("Cell 9",2,1,0,3); BMdzCellItm ("Cell 10",3,1,2,3)];
               [BMdzCellItm ("Cell 11",2,1,0,4); BMdzCellItm ("Cell 12",2,1,2,4)];
               [BMdzCellItm ("Cell 13",2,1,0,5); BMdzCellItm ("Cell 14",2,1,2,5)]]


#if newStuff
    printfn "avant l'nouvelle...."
    let docF = tkFldList()
    let nm = "Test tkTable no isCat"
    let ty = (TaskTbl() :> ITblMarker)
    let bf = docF |> lim (fun f -> BMfld(f, 1, 1, None, Thingy(""), None, ty))
    let defIt = Font(defFont.FontFamily, defFont.Size, FontStyle.Italic, defFont.Unit)
    //placeholder
    let dsk = new Form()
    //let bm = BanarasiMasaloAux((getUNID "^બનારસી_મસાલો"), nm, 2, bf, defIt, defFont, (currentScheme ((!!~ "wld" dsk).Value)).Fore(), (currentScheme ((!!~ "wld" dsk).Value)).Back(), DesDocInfDeflt())
    let bm = BanarasiMasaloAux((getUNID "^બનારસી_મસાલો"), nm, 2, bf, defIt, defFont, Color.Black, Color.White, DesDocInfDeflt())
#endif //newStuff

(*
    Note however that the bf above DOES NOT have the positioning; only the spans
    Once that is fixed, the BM by itself is enuff 2 gen the rest
    
    type BanShell(bm...)
        let mutable bmRef = bm
        let initState = 
            ([bm], 0, [], [])  |>
            ``⍾`` { ...
        ...
        member this.getColN() = 
            let (BanarasiMasaloAux(_, _, colN...
            colN
        member this.getRoN() = 
            let (BanarasiMasaloAux(_, _, roN...
            lilen struct
        member this.getCurrBM() = 
            let (BMstate, intPtr, _, _) = stateRef.Value
            matsch intPtr with
            | 0 -> List.last BMstate
            | _ -> BMstate.[intPtr]
        ...


        member this.BMdzCmdHandler = 
            fun cmd params =
(*
        - For _Undo_ we nd refCell 'StateChanged' + member 2 chk/set if necc.
          Add this call to the 1st line of every handler 4 changes 
          + last line of handler saves state 2 BM list last (unless cancelld)
            Note; the BM ref'll now store li<BM> not single, + pointer.
*)
                let updSt =
                    match cmd with
                        | doDrop ->
                        | setRoSp -> 
                        | setColSp -> 
                            let bm = this.getCurrBM()
                            let BanarasiMasaloAux(unid,dispNm,સુપારી,usrFlds,lblFont,dataFont,foreCol, backCol, docInf) = bm
                            let (unid,tit,docF, colSp, roSp,colPos,roPos,lblFont,dataFont,foreCol,backCol,soopari,vFn,CanUhear,fldTtip,tblTy) = usrFlds
                            runStateBinder param
                        | addFld ->
                        | remFld ->
                        | remCell ->
                        | chgCell ->
                        | chgThingy ->
                        | addVFn ->
                        | chgFont ->
                        | chgCol ->
                        | addBlankRo ->
                        | addInfoBox ->
                        | unDo ->
                        | reDo ->
                stateRef <- updatedState
                this.doLayout()
                this.stateChanged()

*)


    printfn "apres l'nouvelle...."


    let onlyCells = fun cell -> match cell with
                                   | BMdzCellItm(_,_,_,_,_) -> true
                                   | _ -> false

    //printfn "interimRes3:%A" (bld_v4 tbl)

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
*)
    printfn "eom Dnd_ops..."

    printfn "...eom..."

