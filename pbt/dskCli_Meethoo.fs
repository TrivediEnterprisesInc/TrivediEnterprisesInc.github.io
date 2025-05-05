(*  Brij (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 
    TrivediBldHdr->
    |Trivedi SrcCtrlHdr File.  Copyright (c) 2015-2025 M. P. Trivedi.  All rights reserved.|637901455|test.fs|none|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|

    fsc src\pbt\pbt_AI_Dsk.fs src\pbt\pbt_Dsk.fs  --platform:x64 --standalone --out:src\pbt\Trivedi.PbtDsk.dll -r:lib\Trivedi.Core.dll -r:lib\Trivedi.CoreAux.dll -r:lib\Trivedi.UI.dll -r:lib\Trivedi.UIAux.dll -I:C:\Windows\Microsoft.NET\Framework\v4.0.30319
   
    Last updated: Fri Mar 28 2025
    
    Contains modules:  Meethoo_Actual
                                Csv_Actual
                                Csv_Ext
                                //tibbie Csv_Test
                                
*)

namespace Trivedi

#nowarn "20" "25" "58" "66" "67" "64" "760" "1182" "1558"

(*

//Mar2025: These are the ones expanded for Form UI usage NOT the DocField stuff...
//src: Addenda Nov22a/z_loggedUI_webCliFrm (part of wobbly upd8s)
//These are imported locally from uiaux 4 ref
//@ToDo: Nov2024: These contain way too much junk, look @ the other one 
//(in this file) Trim them to essentially primitiveTys & use Thingy for the rest
    type FldType = | FldString
                   | FldNumber
                   | FldCurrency
                   | FldLongString
                   | FldAttachment
                   | FldBoolean
                   | FldChoiceList
                   | FldRadioBtn
                   | FldRange
                   | FldNumUpDn
                   | FldDate 
                   | FldDateTime
                   | FldColor
                   | FldFont
                   | FldInfoBox
                   | FldBlankRow
                   | UserInput
                   | FldBtn
                   | FldValidBtn
                   
//src: Brij.fs
type BrijTy<'t when 't :> ITblMarker> = | BrijTy of mods:Mod list * m:Mtpl * string * tblTy:'t with
    override this.ToString() = 
            let (BrijTy(mds, s, tpl, tblTy)) = this
            let (CoreMod(CoreM(DocUNID(unid), crDt, modDt, tit, cont, tags, flag))) = mds.[0]
            let tblT = (genArg this).ToString()
            "BrijTy of " + tblT + "|id:" + unid + "|title:" + tit + "|"
    member this.zipWithID() = 
            let (BrijTy(mds, s, tpl, tblTy)) = this
            let (CoreMod(CoreM(DocUNID(unid), _, _, _, _, _, _))) = mds.[0]
            (unid, mds)
#if tbdb
    static member bld (id:string) (tit:string option) (cont:string option) (tg:string option) = 
        BrijTy([CoreMod(CoreM(DocUNID(id), now(), now(), ઓર  tit "-", બાઇટ |>| ઓર cont "", ઓર tg "", ""))], "", CustID_Trivedi_AdminTbl())
    static member bldSpoo (id:string) (crDt:DateTime )(tit:string option) (cont:string option) (tg:string option) = 
        BrijTy([CoreMod(CoreM(DocUNID(id), crDt, now(), ઓર  tit "-", બાઇટ |>| ઓર cont "", ઓર tg "", ""))], "", CustID_Trivedi_AdminTbl())
#endif
    member this.contAsS() = 
        let (BrijTy(dt, s, tpl, tblTy)) = this
        let (CoreMod(CoreM(DocUNID(unid), crDt, modDt, tit, cont, tags, flag))) = dt.[0]
        bytes2Str cont
    member this.contAsB() =
        let (BrijTy(dt, s, tpl, tblTy)) = this
        let (CoreMod(CoreM(DocUNID(unid), crDt, modDt, tit, cont, tags, flag))) = dt.[0]
        cont
    member this.ToShortStr() =
        let (BrijTy(m, s, tpl, tblTy)) = this
        let (CoreMod(CoreM(DocUNID(unid), crDt, modDt, tit, cont, tags, flag))) = m.[0]
        "BrijTy of " + (genArg this).ToString() + "|id:" + unid + "|title:" + tit + "|"
    member this.getFldDefs() =
        [DocFld(FldString, "unid", true, "Document UNID");
        DocFld(FldDate, "crDt", true, "Created On");
        DocFld(FldDate, "modDt", true, "Last Modified On");
        DocFld(FldString, "title", true, "Title");
        DocFld(FldLongString, "cont", true, "Content");
        DocFld(FldString, "tags", true, "Tags");
        DocFld(FldBoolean, "flag", true, "Flag");
        DocFld(FldString, "optFld", true, "Optional Field")]
    member this.upd(m:Mtpl) =
//      NOTE: this shd pro'lly lie in an upper class (DatDoc?) since it's generic across ty.s
        let (Mtpl(l:list<(string * obj option * System.Type)>)) = m
        let rec updEa li = 
            List.map (fun x -> 
                    let (fldNm, val, ty) = x
                    match (List.tryFindIndex (fun def -> 
                                            let (Ty,intNm,isInt,extNm) = def
                                            intNm = fldNm) (this.getFldDefs()) ) with
                    | Some idx -> 
                        //fst verify types match; if not throw
                        updEa (List.updateAt idx (val :?> ty) li)
                    | _ -> raise SpxErr; []
                    ) li
        updEa l
        
    //src: wobblyDat
    let docF = tkFldList()
    let nm = "Test tkTable no isCat"
    let ty = (TaskTbl() :> ITblMarker)
    let bf = docF |> lim (fun f -> BanarasiFldAux(f, 1, 1, None, Thingy(""), None, ty))
    
    //src: loggedUI_Dec20
    let tkFldList() =         
       [DocFld(FldString, "unid", true, "Document UNID");
        DocFld(FldString, "title", false, "Title");
        DocFld(FldString, "project", false, "Project");
        DocFld(FldString, "moduleNm", false, "Module");
        DocFld(FldString, "submodule", false, "SubModule");
        DocFld(FldString, "objective", false, "Objective");
        DocFld(FldRange, "importance", false, "Importance");
        DocFld(FldRange, "urgency", false, "Urgency");
        DocFld(FldDate, "completedOn", false, "Completed On");
        DocFld(FldChoiceList, "tgtVer", false, "Target Version");
        DocFld(FldString, "docLinks", false, "DocLinks");
        DocFld(FldLongString, "cont", false, "Content");
        DocFld(FldString, "tags", false, "Tags");
        DocFld(FldBoolean, "flag", false, "Flag")]
        
//src: Brij.fs
type DocFld = | DocFld of FldType*string*bool*string with
    override this.ToString() = 
        let (DocFld(fldTy, intNm, isInt, dispNm)) = this
        "DocFld: |intNm: " + intNm + "|isInt: " + isInt.ToString() + "|dispNm: " + dispNm + "|" + fldTy.ToString() + "|"
    member this.getBoxedGenericVal() =
        let (DocFld(fldTy, _, _, _)) = this
        match fldTy with
        | FldString -> box ""
        | FldNumber | FldRange -> box 0     //@ToDo Range params?
        | FldCurrency -> box (0.00)
        | FldLongString | FldAttachment -> box []
        | FldBoolean -> box false
        | FldDate -> box (now())        //these 2 nd to be diff
        | FldDateTime -> box (now())    //these 2 nd to be diff
        | FldColor -> box Color.White
        | FldFont -> box defFont
        | _ -> 
            tibbie ("DocFld.getBlankGenericVal() got an invalid fldType: " + fldTy.ToString())
            box ""
*)


    [<AutoOpen>]
    module Meethoo_Actual = 

        //from UIAux
        type મીઠૂ_પાન_eoy<'t when 't :> ITblMarker> (ctorDef:SaadoMasaloAux<'t>, dsk, સ્તિતિ) as f =
            inherit Form(Text = "Brij (TM) TableDef: Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved.", Visible = false, TopMost = true, WindowState = FormWindowState.Maximized, Font=defFont, ForeColor = (currentScheme ((!!~ "wld" dsk).Value)).Fore(), BackColor = (currentScheme ((!!~ "wld" dsk).Value)).Back())
            let (SaadoMasaloAux(tNm, def, icn, tblTy)) = ctorDef
            let mutable nm = tNm
            let mutable TblFldList = def
            do f.SuspendLayout()
            let setupFrm = 
                //tibbie "db: મીઠૂ_પાન setup..."
                f.Layout.AddHandler(centerFrm)
                    //@TBD: This is from gappa; shd we pull it out? 
                    //      doc diff. so perhaps a 'let getTitleP = fun docVal ->'
                let icnLbl = new Label(Image = brijLogo, Size = (new Size(brijLogo.Width, brijLogo.Height)), Anchor = anc "N", BackColor = Color.Transparent, ForeColor = (currentScheme ((!!~ "wld" dsk).Value)).Icn())
                let titTxt = new TextBox(AutoSize = true, Dock = doc "T", Enabled = false, Text = "Meethoo Def Document for " + nm, ReadOnly = true, Multiline = false, Width = f.Width - 50, TextAlign = HorizontalAlignment.Center, BorderStyle = BorderStyle.None, ForeColor = (currentScheme ((!!~ "wld" dsk).Value)).titFore(), BackColor = (currentScheme ((!!~ "wld" dsk).Value)).titBack())
                let titleP = new TableLayoutPanel(RowCount = 1, ColumnCount = 5, Dock = doc "T", BackColor = (currentScheme ((!!~ "wld" dsk).Value)).titBack(), AutoSize = true, Width = f.Width , Height = ((int) (titTxt.Height * 3)))
                titleP.SuspendLayout()
                titleP.RowStyles.Clear()
                titleP.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f))
                titleP.Controls.Add(icnLbl, 0, 0)
                titleP.Controls.Add(titTxt, 1, 0)
                titleP.SetColumnSpan(titTxt, 4)
                titleP.ResumeLayout(false)
                
                //tibbie "db: મીઠૂ_પાન btnPnl..."
                let btnFP = new FlowLayoutPanel(FlowDirection = FlowDirection.LeftToRight, Anchor = anc "N", AutoSize = true, BackColor = Color.White)
                let btnP = new TableLayoutPanel(Dock = doc "B", Width = f.Width - 100, BackColor = Color.White)
                let okButton = new Button(AutoSize = true, DialogResult = DialogResult.OK, Text = "&OK")
                okButton.Click.AddHandler(new EventHandler(fun o e -> tibbie "no handler yet"))
                let cancelButton = new Button(AutoSize = true, DialogResult = DialogResult.Cancel, Text = "&Cancel")
                cancelButton.Click.AddHandler(new EventHandler(fun o e -> tibbie "no handler yet"))
                btnFP.Controls.AddRange([|okButton; cancelButton|])
                btnP.Controls.Add(btnFP)

                //Added Mar11_25
                let tc = new TabControl(Dock = doc "F", Width = f.Width - defPadding.Horizontal, Height = f.Height - (defPadding.Horizontal * 8))
                let AppearancePg = new TabPage (Text = "Appearance")
                let FldSetupPg = new TabPage (Text = "Field Setup")
                let ImportPg = new TabPage (Text = "Import Data")
                let WebCliPg = new TabPage (Text = "Web Client")
                tc.Controls.Add([AppearancePg; FldSetupPg; ImportPg; WebCliPg])

                //For below, use InfoBox
                //let fldInfoLbl = new Label(Text = "Form Cells:", Dock = doc "F", ToolTipText = "Please select the Cells (data fields, infoboxes, etc) you wish to include in this Form.", BackColor = Color.Transparent, ForeColor = (currentScheme ((!!~ "wld" dsk).Value)).Fore())
                //fldFP.Controls.Add(fldInfoLbl)
                let lV = new ListView(MultiSelect = false, Dock = doc "F", CheckBoxes = false, FullRowSelect = true, HeaderStyle = ColumnHeaderStyle.Nonclickable, LabelEdit = false, View = View.Details, ForeColor = (currentScheme ((!!~ "wld" dsk).Value)).accentFore(), BackColor = (currentScheme ((!!~ "wld" dsk).Value)).accentBack())
                lV.SuspendLayout()
                lV.Columns.Add("FieldName:", -2, HorizontalAlignment.Left)
                lV.Columns.Add("FieldType", -2, HorizontalAlignment.Left)
                match TblFldList with
                | [] -> ()
                | _ ->  lim (fun itm -> 
                                let (DocFld(t, slg, isInt, nm)) = itm
                                let listItm = new ListViewItem(nm,0)
                                listItm.ForeColor <- (currentScheme ((!!~ "wld" dsk).Value)).accentFore()
                                listItm.SubItems.Add(t.ToString())
                                lV.Items.Add(listItm)) TblFldList |> ignore
                                
                //let fldInfoLbl = new Label(Text = "", Dock = doc "F", ToolTipText = "Please select a field to change its settings... ", Backcolor = Color.OldLace)
                //fldFP.Controls.Add(fldInfoLbl)

                lV.ResumeLayout(false)

                //tibbie "db: મીઠૂ_પાન tbar..."
                let tBar = new ToolStrip(Dock = doc "T")
                tBar.Items.Add (getTSButtonAux "Import CSV file..." "table.png" (Some("Auto-Import field definitions and data from a Comma-delimited CSV file")) (Some(new EventHandler (fun sender e -> 
                                let linkLi = "see:" + crlf + "https://stackoverflow.com/questions/53956462/get-column-in-haskell-csv-and-infer-the-column-type" + crlf + "https://deephaven.io/blog/2022/02/23/csv-reader/#:~:text=Type%20inferencing%20intends%20to%20map%20a%20column%20of,short%20if%20a%20memory-constrained%20user%20opted%20for%20that%29." + crlf + "https://stackoverflow.com/questions/60421589/ml-net-type-inference-for-csv-loading" + crlf + "https://github.com/Wittline/csv-schema-inference" + crlf + "** https://itnext.io/building-a-schema-inference-data-pipeline-for-large-csv-files-7a45d41ad4df" + crlf + "https://observablehq.com/@d3/d3-autotype" + crlf + "also srch for 'automatic type inference CSV'"
                            (ગપ્પા_પાન (SizeM,Some("CSV Stuff @ToDo"),None , Some(box (linkLi)), None,dsk, tblDefCSVDlg())) |> ignore 
                            ))))
            tBar.Items.Add (new ToolStripSeparator())
            (getTSButtonAux "Table Setup" "table.png" (Some("Change Table name / icon")) (Some(new EventHandler (fun sender e -> 
                            tibbie "remmed Dec 27"
                            //(ગપ્પા_પાન (SizeM,Some("CSV Stuff @ToDo"),None , Some(box ("tblNm" + crlf + "icon")), None,own, tblDefSetup())) |> ignore 
                            )))) |> tBar.Items.Add
            tBar.Items.Add (getTSButtonAux "Add" "note_add.png" (Some("Add a new Field to this Table")) (Some(new EventHandler (fun sender e -> 
                let quimaam = (Some(box ("Add", TblFldList)))
                tibbie "remmed Dec 27"
                //let res = (ગપ્પા_પાન (SizeM,Some("Add a new Field to this Table"),None , quimaam, None,own, tblDefAddModFld()))
                //tibbie ("res was" + res.ToString()) 
                ))))
            tBar.Items.Add (getTSButtonAux "Modify" "edit_note.png" (Some("Change a Field Type")) (Some(new EventHandler (fun sender e -> 
                            match (lV.SelectedIndices |> Seq.cast |> List.ofSeq) with
                            | [] -> tibbie "Please select a field to modify"
                            | _ as l ->
                                let selItm:ListViewItem = lV.Items.[int (l.[0])]
                                let fNm = selItm.Text
                                let fTy = selItm.SubItems.[1]
                                let quimaam = (Some("Mod", fNm, fTy))
                                //let res = (ગપ્પા_પાન (SizeM,Some("Change a Field Type"),None , Some(box ("fldNm" + crlf + "pickList")), None,own, tblDefAddModFld()))
                                //tibbie ("res was" + res.ToString()) 
                                tibbie "remmed Dec 27"
                                ))))
            tBar.Items.Add (getTSButtonAux "Remove" "delete.png" (Some("Remove a Field from this Table")) (Some(new EventHandler (fun sender e -> 
                            match (lV.SelectedIndices |> Seq.cast |> List.ofSeq) with
                            | [] -> tibbie "Please select a field to remove"
                            | _ as l ->
                                let msg = "Please note that if you proceed with this action, you will not be able to access the data stored in this field in any existing Forms or DataViews...";
                                let caption = "Are you sure?"
                                let res = MessageBox.Show(msg, caption, MessageBoxButtons.YesNo)
                                if (res = System.Windows.Forms.DialogResult.Yes) then
                                    lV.BeginUpdate()
                                    lV.Items.RemoveAt(l.[0])
                                    lV.EndUpdate()
                                 else ()
                            ))))
            !!^ ["lV", box lV; "tBar", box tBar ; "btnP", box btnP] f
            FldSetupPg.Controls.Add(lV)
            f.Controls.Add(titleP)
            f.Controls.Add(tBar)
            f.Controls.Add(tc)
            f.Controls.Add(btnP)
            printfn "db: મીઠૂ_પાન done adding controls..."
            f.ResumeLayout(false)


    [<AutoOpen>]
    module Csv_Actual = 

        //for total coverage of all poss fkdTys 4 the CSV tests...
        //Includes 1st the internalFlds from Core, some repurposed

        let SongFldLi() =
            [DocFld(FldString, "unid", true, "Document UNID");
            DocFld(FldDate, "crDt", true, "Released On");
            DocFld(FldDate, "modDt", true, "Last Modified On");
            DocFld(FldString, "title", true, "Song Title");
            DocFld(FldLongString, "cont", true, "Song Lyrics");
            DocFld(FldString, "tags", true, "Tags");
            DocFld(FldBoolean, "flag", true, "Flag");
            DocFld(FldNumber, "track", false, "Track Number");
            DocFld(FldFloat, "sales", false, "Sales (M)");
            DocFld(FldCurrency, "price", false, "Price");
            DocFld(FldCurrency, "price", false, "Price");
            DocFld(FldAttachment, "cover", false, "Album Cover Art");   //byte[]
            DocFld(FldBoolean, "familiar", false, "Is Familiar?");
            DocFld(FldChoiceList, "subGenre", false, "Sub Genre"); //HardRock|ClassicRock|Metal|Other
            DocFld(FldRange, "rating", false, "Rating");  //1-5
            DocFld(FldDateTime, "rdatetime", false, "DateTime");  //rnd
            DocFld(FldTime, "time", false, "Time");  //rnd
            ]

    //@tbfo
    let getUrl u = u
    let remQuotes s = s

    let mutable ok_res:list<Result> = []
    let mutable err_res:list<Result> = []
    
    let mini_Inp = """
"one","two", 99.9, 90, "three"
"three","four", 199.9, 190, "end"
"""
    let mini_fDef = 
        [DocFld(FldString, "fld_one", true, "Field_One");
        DocFld(FldString, "fld_two", true, "Field_Two");
        DocFld(FldString, "fld_thr", true, "Field_Three");
        DocFld(FldString, "fld_fou", true, "Field_Four")]
        
    let parseAsync roIdx ro =
        async {
            try
                printfn "in parseAsync for idx:%A" roIdx
                let trimd = remQuotes (ro.Trim())
                let roSpl = (સપ્લીટ ro "\"(\"*)(\s*),(\s*)(\"*)"))
                let thisRoTpl, thisRoErrLi, thisRoErrCt, thisRoIdx =
                    ((Mtpl.empty(), [], 0, 0), roSpl)
                    |> lifoi (fun fldS i fldV ->
                                let (fTpl, fldErrLi, fldErrCt, fldIdx) = fldS
                                let thisFld = saaduDef.[i]
                                let (DocFld(t, slg, isInt, nm)) = thisFld
                                match (FldTy.csvTryParse(fldV t slg)) with
                                | Ok ob -> (Mtpl.AddOne (slg, box fldV ob) fTpl), fldErrLi, fldErrCt, fldIdx + 1
                                | Error e ->
                                    let fullErr = roIdx.ToString() + ") " + e + "| Full Row: " + ro.ToString()
                                    (fTpl, fullErr :: fldErrLi, fldErrCt + 1, fldIdx + 1))
                match thisRoErrCt with
                | 0 -> ok_res :: (Mtpl.AddOne(roIdx.ToString(), box thisRoTpl), thisRoErrLi, thisRoErrCt, roIdx + 1)
                | _ -> err_res :: (roTpl, [thisRoErrLi] @ roErrLi, roErrCt, roIdx + 1 

            with
                | ex -> printfn "%s" (ex.Message);
        }
               
    let csvParse optP optU saaduDef = 
        match saaduDef with 
        | [] -> 
            let msg = """
            This table has no fields defined.
            In order to import data into the table you first need to set up the table by defining the fields.  Please choose the tab above (help is available on that page)"""
            //ગપ્પા_પાન (SizeM,Some("csvParse"),None , Some(box (tkFldList())), None, d, frmSetupDlg())  |> ignore
            printfn "csvParse:%A" msg
        | _ -> ()

        let inp = 
            match optP with
            | Some p ->  File.ReadAllText(p, Encoding.UTF8)
            | _ -> match optU with
                    | Some u -> getUrl u //tbfo
                    | _ -> printfn "error in csvParse: both input params None"
                           ""
        let spl = સપ્લીટ inp "\n+"
        match (spl.Length > 1) with
        | true -> 
            printfn "spl ty: %A" (spl.GetType())
            let mapper = parseAsync mini_fDef
            (spl |> List.toSeq)
            |> Seq.mapi mapper
            |> Async.Parallel
            |> Async.RunSynchronously
            |> ignore
        | _ -> 
            printfn "Info: csvParse input len < 1..."
            ()

    

    module Csv_Ext =


        type FldType with
            static member x.csvTryParse(itm expTy slg) =
                match expTy with
                | FldString ->
                | FldNumber -> 
                    match System.Int32.TryParse itm with
                    | true, v -> Ok (intNm, box v, v.GetType())
                    | false, e -> Error ("FieldName: " + slg + " Expected Type: " + expTy + " Got: " + itm)
                | FldFloat -> 
                    match System.Double.TryParse itm with
                    | true, v -> Ok (intNm, box v, v.GetType())
                    | false, e -> Error ("FieldName: " + slg + " Expected Type: " + expTy + " Got: " + itm) 
                | FldCurrency ->
                | FldLongString ->
                | FldAttachment ->
                | FldBoolean ->
                | FldChoiceList ->
                | FldRange ->
                | FldDate ->
                | FldDateTime ->
                | FldTime -> 
                | FldColor ->
