    //@ToDo: This doesn't incl the Parsec stuff; loc8 [Apr '23]
    //(Parsec+Flds) '23 + poss more missing from arcs, saved 2 main repo


[from log23]
mod winFrms/DnDMonad	
    Nov 9: 	refactor, retrofit 2 use existing S monad w/only bind 4 ce
    Nov 7: init work: logic, monadBldr, initial tests OK
mod winFrms/propBox	
    Oct 19: refactored tys
    Oct 17: more Pages, tabCtrl
    Oct 16: init work: logic, TabPages, initial flow
mod winFrms/DnD_ops	
    Nov 6: refactored tys, dragDrop handlers
    Oct 11,12: refactored tys
    Oct 7: further work: blankCell logic, tests
    Oct 6: further work: tests, produces DzCell Struct
    Oct 4: init work: logic, types, init flow
Apr-14	DocFx
Apr-13	Brij/Parser.fs	Some further work on the new predMods
Apr-11	Brij/Parser.fs	Some further work on the new predMods
Apr-10	off/echo	
Apr-01 - 08	Brij/Parser.fs	Contd. work on mod Parser.predNL harness/fleshed out (Apr7 off/cis)
Mar-29 - 31	BrijSvr	Began work on mod Parser.predNL harness

Basically the pic w/FldErr (misspelled) + assoc work (apparently only 2 wks!)


    //Note: the gridx stuff is surprisingly sparse, see: https://github.com/oria/gridx/blob/master/modules/Filter.js
    //https://trivedienterprisesinc.github.io/brijPitch/articles/images/2BM_FldsTab_ValidationBldr.png (Jun 1 '23 docFx)
    //https://trivedienterprisesinc.github.io/brijPitch/articles/images/5PredTester.png
    
    //src:UI_Aux
    let predConds =
        Map [ ("document", ["createdBefore"; "createdBetween"; "createdAfter"; "createdBy"]);
              ("string", ["contains"; "is"; "isOneOf"; "startsWith";"endsWith";"isEmpty"]); 
              ("date", ["lessThan";"largerThan";"isBetween";"isEmpty"]);
              ("number", ["lessThan";"largerThan";"isBetween";"isEmpty"]);
              ("boolean", ["is";"isEmpty"]) ]

    //src:UI_Aux
    type એક્સ_પેનલ  (dFld, fld, midP:TableLayoutPanel, d) as this =
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
                         | _ -> " (") + qt + fldCB.Text + qt + " " + condCB.Text + " " + userCond.Text + ") "
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