(*  Brij (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 
    TrivediBldHdr->
    |Trivedi SrcCtrlHdr File.  Copyright (c) 2015-2025 M. P. Trivedi.  All rights reserved.|637901455|test.fs|none|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS 

    //minus UIAux
    fsc e:\pbt\dskCli_AgentDef.fs --platform:x64 --standalone --target:exe --out:adef.exe -r:lib\Trivedi.Core.dll -r:lib\Trivedi.UI.dll -I:C:\Windows\Microsoft.NET\Framework\v4.0.30319

    Last updated: Wed May 09 2025

    Contains modules:      AgentDef_Actual
                                    //FrmDef_Ext
                                    //tibbie FrmDef_Test

*)

namespace Trivedi

#nowarn "20" "25" "58" "66" "67" "64" "760" "1182" "1558"


[<AutoOpen>]
module AgentDef_Actual =
    open System
    open System.Diagnostics
    open System.Drawing
    open System.Windows.Forms
    open Trivedi
    open Trivedi.Core
    open Trivedi.UI
    //open Trivedi.UIAux
    open System.Windows.Forms
    
    printfn "AgentDef_Actual init..."
    
    let Agent_પીચાક =
        fun (ક્વિમામ:option<_>) (dlg:Form)  ->
            //if ક્વિમામ.IsSome then 
                //let midP:TableLayoutPanel = (!!~ "midP" dlg).Value
                let midP = new TableLayoutPanel(Dock = doc "F", AutoScroll = true)
                midP.Controls.Clear()
                midP.RowCount <- 0
                midP.ColumnCount <- 3
                midP.SuspendLayout()

                let nmLbl = new Label(Text = "Agent Name:", Dock = doc "F", TextAlign = ContentAlignment.MiddleCenter)
                let nm = new TextBox(Dock = doc "F")
                midP.Controls.Add(nmLbl, 0, 0)
                midP.Controls.Add(nm, 1, 0)

                let schdLbl = new Label(Text = "Agent Trigger:", Dock = doc "F", TextAlign = ContentAlignment.MiddleCenter)
                let schdgB = new GroupBox()
                let manRadio = new RadioButton(Text = "Run Once (manually from Agent List)")
                let schdCombo = new ComboBox(Name = "Run on Schedule", Dock = doc "F")
                schdCombo.Items.AddRange([|"Daily";"Weekly";"Monthly"|]);
                schdgB.Controls.AddRange([|manRadio; schdCombo|])
                midP.Controls.Add(schdLbl, 0, 1)
                midP.Controls.Add(schdgB, 1, 1)

                let tgtLbl = new Label(Text = "Target Documents:", Dock = doc "F", TextAlign = ContentAlignment.MiddleCenter)
                let tgtB = new GroupBox()
                let allRadio = new RadioButton(Text = "All Docs (from Agent List)")
                let newRadio = new RadioButton(Text = "New Docs (Created/Modified since last Agent Run)")
                let condRadio = new RadioButton(Text = "Custom Condition")
                let condCombo = new ComboBox(Name = "condList", Dock = doc "F")
                //pop w/all saved 4 this tbl...
                //condCombo.Items.AddRange([|"Daily";"Weekly";"Monthly"|]) 
                condRadio.CheckedChanged.AddHandler(new EventHandler(fun o e ->
                    //let rb = (RadioButton) o
                    printfn "hey" |> ignore
                    //if (rb.Checked) then insist on sel saved condExpr
                ))
                tgtB.Controls.AddRange([|allRadio; newRadio|])
                midP.Controls.Add(tgtLbl, 0, 2)
                midP.Controls.Add(tgtB, 1, 2)
                midP.Controls.Add(condCombo, 2, 2)

                //IF cond exists we shd insert 'bigBox' ie colspan full
                
                let actLbl = new Label(Text = "Agent Actions:", Dock = doc "F", TextAlign = ContentAlignment.MiddleCenter)
                //(1/more) update fld (choose) to val: box
                //pull frm CondBldrDlg
                
                //also Add Button "Load Saved IfThen Expression"
                //This allows multiple flows + if reqd multiple flds updated
                //Validation: either IfThen or @ least 1 CondExpr nded.
                
                //let ret = if (dlg.ShowDialog() = DialogResult.OK) then Some(befr.SelectedIndex, aftr.SelectedIndex) else None
                //dlg.Dispose()
                //()//ret
              //else ()
                dlg.Controls.Add(midP)
                dlg
                
#if forRef            

    let ટેબલ_ડેફ_સેટપ_પીચાક =
        fun (ક્વિમામ:option<_>) (d:Form) ->
            if ક્વિમામ.IsSome then
                d.WindowState <- FormWindowState.Maximized
                let midP:TableLayoutPanel = (!!~ "midP" d).Value
                midP.ColumnCount <- 1
                midP.Controls.Clear()
                let hdrLbl = new Label(Text = ("Agent Definition Document:"), Dock = doc "F", TextAlign = ContentAlignment.MiddleCenter)
                let p0 = (ફીલ્ડ_પેનલ  ("Table Display Name", UserInput, "no Slug", 2)) :> Panel
                let p1 = (ફીલ્ડ_પેનલ  ("Table Icon", FldFont, "no Slug", 2)) :> Panel
                midP.ColumnStyles.Clear()
                midP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.0f))
                midP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.0f))
                midP.Controls.Add(p0, 0, 1)
                midP.Controls.Add(p1, 1, 1)
            let res = d.ShowDialog()
            d.Dispose()
            None
            
        let p0 = (ફીલ્ડ_પેનલ  ("Number of Columns", FldRange, "Form Column Count", 2)) :> Panel
        let p1 = (ફીલ્ડ_પેનલ  ("Label Font", FldFont, "Form Label Font", 2)) :> Panel
        let p2 = (ફીલ્ડ_પેનલ  ("Data Font", FldFont, "Form Data Font", 2)) :> Panel
        let p3 = (ફીલ્ડ_પેનલ  ("Text Color", FldColor, "Form Text Color", 2)) :> Panel
        let p4 = (ફીલ્ડ_પેનલ  ("Background Color", FldColor, "Form Background Color", 2)) :> Panel
        let p5 = (ફીલ્ડ_પેનલ  ("InfoBox1", FldInfoBox, "Pls note that each of the settings above can be overriden for any particular cell in the Cell Definition Settings (@ToDo)", 2)) :> Panel


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

    printfn "AgentDef_Actual eom..."

    [<EntryPoint>]
    [<STAThread>]
    let main ag =
        db "AgentDef_Actual main():1"
        match ag.Length = 0 with 
        | true -> 
            db "AgentDef_Actual main():2"
            Application.EnableVisualStyles()
            //Application.SetCompatibleTextRenderingDefault(false)
            try
                let f = Agent_પીચાક None (new Form(WindowState = FormWindowState.Maximized, Visible = false, Text = "AgentDef Form: Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved.", TopMost=true, Font=defFont))
                Application.Run(f)
            with
                | e -> 
                    printfn "Exc in main: %A" e.Message
                    let st = ((new StackTrace(e, true)).GetFrames()).[0]
                    printfn "Immed.-> method: %A lineNo:%A col: %A" (st.GetMethod().Name) (st.GetFileLineNumber()) (st.GetFileColumnNumber())
                    printfn "StackTr -> \r\n%A" (getStTrace e)
        | false ->
            System.Console.Write("back in main...pls press any key to continue...")
            let c = System.Console.ReadKey(true)
            match c.Key with
            | ConsoleKey.Escape -> printfn "Esc..."
            | _ -> printfn "..."
        0
    