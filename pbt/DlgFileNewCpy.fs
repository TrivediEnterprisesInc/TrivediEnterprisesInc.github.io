//Last updated: Aug 16 '25

//@ToDo: add triggers to icons to invoke/pass tblTy
let DlgFileNewCopy =
  fun (ક્વિમામ:option<_>) (dlg:Form) ->
    if ક્વિમામ.IsSome then 
        //tibbie "લીસ્ટ_પીચાક ક્વિમામ isSome"
        let tblTy = unbox (ક્વિમામ.Value)
        let mutable currDispNm = (getTblDispNameFor tblTy) + " - Copy"
        let midP:TableLayoutPanel = (!!~ "midP" dlg).Value 
        midP.Controls.Clear()
    
        let nmLbl = new Label(Text = ("Table Name:"), Dock = doc "F")
        let nmBox = new TextBox(Dock = doc "F", Text = currDispNm)
        nmBox.TextChanged.AddHandler(new EventHandler(fun o e ->
          currDispNm <- nmBox.Text))
        nmPnl.Controls.AddItems([|nmLbl;nmBox|])
        midPnl.Controls.Add(nmLbl, 0, 0)
        midPnl.Controls.Add(nmBox, 1, 0)
        !!^ ["nmBox", box nmBox] dlg

        let CopyDz = new CheckBox(AutoSize = true, Text = "Copy Design", Dock = doc "F", AutoCheck=true)
        let CopyDat = new CheckBox(AutoSize = true, Text = "Copy Data", Dock = doc "F", AutoCheck=true)
        midPnl.Controls.Add(nmLbl, 0, 1)
        midPnl.Controls.Add(nmBox, 1, 1)
        !!^ [("CopyDz", box CopyDz); ("CopyDat", box CopyDat)] dlg

        dlg.ResumeLayout(true)
        let okBtn:Button = (!!~ "okBtn" dlg).Value 
        if (dlg.ShowDialog() = DialogResult.OK) then
            match (CopyDz.Checked, CopyDat.Checked) with
            | (true, true) -> svrHandler FileNewCopy currDispNm Both
            | (false, true) -> svrHandler FileNewCopy currDispNm DatOnly
            | (true, false) -> svrHandler FileNewCopy currDispNm DzOnly
            | _ -> ()
            dlg.Dispose()
            None
        else 
            dlg.Dispose()
            None
      else None