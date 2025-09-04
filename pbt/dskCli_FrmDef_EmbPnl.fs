(*  Aug22:
    - type LineItmCtrlPnl: Either xtnd tblP or floP; ALL layout logic incl; callingSite passes all custom ctrls preceding +|x; In a li so that we can set/cng colCnt/roCnt.
    - Move lbls top if necc; Solv 4 other existing ctrls (HlpLbl?) preceding in midP
    - Rewrite as a repl 4 the existing PredBldr 1st; then Impl as necc. 1-by-1
    - The 1st (PredB) will need thorough testing
    - Next port 2: AgentBldr/ExprBldr/IfThen/Acl?/DbLookup?/DvDz/EmbDv
    (AUG28: Note that on chking, alm all of the above already _have_ customized pnlBldrs, so irrelevant.  Also some have unique reqmts e.g. the ExprBldr 1st ro has no connector)
    - Ensure refs/attachable code 4 ea case 4 all reqd fn.ality

    - @TBD: this is pro'lly NOT a good one to adapt: coz Combiner (and/or) reqmt doesn't exist for the other cases
    - Aug23: Decided to NOT implement this coz of varying reqmts; most of the callingsites already have pnls setup...
*)

    //The Original: modified from type એક્સ_પેનલ src:UI_Aux
    //@ToDo: member this.toTxt() updated Aug23_25, merge updates to above.
    //Aug28 @ToDo: currently the 1st ro does a visib=false for the connector; which throws the cols out of whack.  Instd, do a Controls.Clear().  May have issues w/getting hdl, if so use a !!^
    type એક્સ_પેનલ_v0  (dFld, fld, midP:TableLayoutPanel, d) as this =
        inherit TableLayoutPanel(Dock = doc "F", RowCount = 1, ColumnCount = 7)
        //DataSource = ((!!~ "condCB" dlg).Value |> List.toArray))
        do midP.SuspendLayout()
        do this.SuspendLayout()
        let w:Mtpl = (!!~ "wld" d).Value
        let condCB = new ComboBox(CausesValidation = true, Text = "Condition?", Dock = doc "F", DropDownStyle = ComboBoxStyle.DropDownList, AutoCompleteMode = AutoCompleteMode.None, DataSource = ([] |> List.toArray))
        let fldCB = new ComboBox(CausesValidation = true, Text = "Field?", AutoSize = true, Dock = doc "F", DropDownStyle = ComboBoxStyle.DropDownList, AutoCompleteMode = AutoCompleteMode.None, DataSource = (fld |> List.toArray))
        let combiner = new ComboBox(Text = "Connector?", AutoSize = true, Dock = doc "F", DropDownStyle = ComboBoxStyle.DropDownList, AutoCompleteMode = AutoCompleteMode.None, DataSource = (["AND"; "OR"] |> List.toArray))
        let userCond = new TextBox(AutoSize = true, Dock = doc "F")
        let negBox = new CheckBox(AutoSize = true, Text = "Negate", Dock = doc "F", AutoCheck=true)
        let newXBtn = getImgBtn "" "add_to_queue.png" w
        let savedXBtn = getImgBtn "" "pending.png" w
        do savedXBtn.Click.Add(fun e ->
            let (m, id) = (Mtpl.getUNID ((!!~ "wld" d).Value))
            tibbie ("id: " + (id).ToString())
            !!^ ["wld", box m] d)
        let chuno =
            fldCB.SelectedIndexChanged.AddHandler(new EventHandler( fun (sender:obj) (e:EventArgs) ->
                    let cB = sender :?> ComboBox
                    let newItm = List.filter (fun itm -> 
                                                let (DocFld(t, slg, isInt, nm)) = itm
                                                nm = cB.SelectedItem.ToString()) dFld
                    let (DocFld(fTyp, _, _, _)) = newItm.[0]
                    match fTyp with
                     | FldString | FldLongString -> 
                            !!^ ["condCB", box ((Map.find "string" predConds) |> Array.ofList)] d
                            condCB.DataSource <- ((Map.find "string" predConds) |> Array.ofList)
                     | FldNumber | FldCurrency | FldRange -> 
                            !!^ ["condCB", box ((Map.find "number" predConds) |> Array.ofList)] d
                            condCB.DataSource <- ((Map.find "number" predConds) |> Array.ofList)
                     | FldBoolean -> 
                            !!^ ["condCB", box ((Map.find "boolean" predConds) |> Array.ofList)] d
                            condCB.DataSource <- ((Map.find "boolean" predConds) |> Array.ofList)
                     | FldDate | FldDateTime -> 
                            !!^ ["condCB", box ((Map.find "date" predConds) |> Array.ofList)] d
                            condCB.DataSource <- ((Map.find "date" predConds) |> Array.ofList)
                     | _ -> tibbie "unsupported fldType (for pred)"))
                    //ctrlAddTags ["pChoice" (toOb (cB.SelectedIndex,cB.SelectedItem)) ] rPnl
            let exprErr = new ErrorProvider(BlinkRate = 1000, BlinkStyle = ErrorBlinkStyle.AlwaysBlink)
            exprErr.SetIconAlignment (fldCB, ErrorIconAlignment.TopRight)
            exprErr.SetIconPadding (fldCB, 2)
            fldCB.Validated.AddHandler (new EventHandler( fun (sender:obj) (e:EventArgs) -> 
                if (fldCB.SelectedIndex > -1) then
                    exprErr.SetError(fldCB, String.Empty)
                else exprErr.SetError(fldCB, "Please select a field.")))
            exprErr.SetIconAlignment (condCB, ErrorIconAlignment.TopRight)
            exprErr.SetIconPadding (condCB, 2)
            condCB.Validated.AddHandler (new EventHandler( fun (sender:obj) (e:EventArgs) -> 
                if (condCB.SelectedIndex > -1) then
                    exprErr.SetError(condCB, String.Empty)
                else exprErr.SetError(condCB, "Please select a condition.")))

            let layoutThis =
                this.Controls.Add(combiner)
                this.Controls.Add(savedXBtn)
                this.Controls.Add(negBox)
                this.Controls.Add(fldCB)
                this.Controls.Add(condCB)
                this.Controls.Add(userCond)
                this.Controls.Add(newXBtn)
                newXBtn.Click.Add(fun e -> 
                                    match (((string (exprErr.GetError(fldCB))).Length = 0) && ((string (exprErr.GetError(condCB))).Length = 0) ) with
                                    | true ->
                                        this.Render()
                                        એક્સ_પેનલ(dFld, fld, midP, d) |> ignore
                                    | _ -> ())
                this.ColumnStyles.Clear()
                lim (fun num ->  match num with
                                    | 0 | 1 | 6 -> this.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, float32 (getBtnWid "AND ")))
                                    | 2 -> this.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, float32 (getBtnWid " Negate ")))
                                    | _ -> this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f))) [0..6]
                //add self
                let currRow:int = (!!~ "exprRows" d).Value
                match currRow with
                | 0 -> combiner.Visible <- false
                | _ -> ()
                midP.Controls.Add(this)
                midP.SetColumnSpan(this, 6)
                !!^ [("Row" + currRow.ToString()), box (this)] d
                !!^ ["exprRows", box (currRow + 1)] d
                //!!^ [("RowT" + currRow.ToString()), box (currRow.ToTpl())] d
                
                //@ToDo: Currently userCond is a TextBox; nd to autoUpdate it to widget
                //Perhaps extend the FldType panels for these?
                //@ToDo: "oneOf" shd automatically layout multiline txtBox w/tooltip "...one entry per line"
                //@ToDo: For strings nd to add chkbox 4 "ignoreCase"
                this.ResumeLayout(false)
                midP.ResumeLayout(true)
            do printfn "એક્સ_પેનલ layout done..."
        member this.toTxt() =
            (if combiner.Visible then combiner.Text else "") +
                (match negBox.Checked with
                         | true -> " (NOT "
                         | _ -> " (") + qt + fldCB.Text + qt + " " + condCB.Text + " " + qt + userCond.Text + qt + ") "
        member this.Render() =
            let inf:TextBox = (!!~ "predTxt" d).Value
            let rs = ((!!~ "exprRows" d).Value)
            match rs = 0 with
            | true -> 
                let ro:એક્સ_પેનલ = (!!~ "Row0" d).Value
                inf.Text <- ro.toTxt()
            | _ ->
                //@ToDo: Update to Tpl; lope off last connector in fold
                //       (also needed 4 toExpr())
                inf.Text <- (lifo (fun s r ->
                                    let ro:એક્સ_પેનલ = (!!~ ("Row" + r.ToString()) d).Value
                                    s + crlf + ro.toTxt() ) "" [0.. (rs - 1)])
        member this.toExpr() =
            tibbie "this.toExpr():('a -> bool) tibbie"