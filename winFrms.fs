(*  brij (TM)
    Copyright (c) M. P. Trivedi 2016-2023.  All rights reserved. 
    TrivediBldHdr->
    |Trivedi SrcCtrlHdr File.  Copyright (c) 2015-2023 M. P. Trivedi.  All rights reserved.|637901455|winFrms.fs|none|- - - - - - -
>* rockdale * Beckham * YOAKUM * jimwells * FLOYD * Pushmataha * baltimore * 
<* SanMateo * CONWAY * frontier * CONECUH * Bingham * chittenden * Valley * 
<* GILLIAM * nacogdoches * BROOKS * Broomfield,CityandCountyof * montana * Houghton * GOLDENVALLEY * <* california * IZARD * Haywood * broome * Cache * JOHNSON * centre * 
<* PRINCEGEORGE * Wilcox * massachusetts * LasAnimas * BEXAR * dakota * PROVIDENCE * <

  Contains the foll modules:

	GridTester		Ide
	Ext (bookmarkPainter/addTreeVwZip/bldPSlideTester)
	Cambattable
	deck		
Note: Somewhere in Aug '23 the monkeyBastas removed all above mods; reinserted Nov15 in all locs.
	perms (combines earlier Base + Red)
	Bhojpuri (replaces cPnl, rPnl, etc.; incl new tys)
	jimmy			Tokenizer
	Folding			FilePanelUpdates
	Outliner		frmDelta (ty DeltaTracker...)
	clientInit (UI/UIAux/Brij updates) <- !!contains new Keywds!!
	Dnd_ops		dndState
	main
	
New reqmt:  Zeep :  user auth/registration/assign API keys.  Is this doable via webhooks/gitEmail?

VisualDb.com ('advanced filtering and/or'|100k recs/vw|Unlim|BYOD|SQL)

github.com/refinedev/refine|MIT| 15k devs 'headlessUI incl material|ProjCreationWizrd
github.com/marmelab/react-admin
"Refine & react-admin are the same noco solutions & are going after retool..."

retool: dashbrdMaker/React/'MSAcc on steroids'
junjat.com: loco uses Refine as a UI frmwrk 2 rndr data models

github.com/BudiBase
github.com/appsmithorg
github.com/ToolJet
github.com/lowdefy
github.com/windmill-labs/windmill
Onu
frappeframework.com : OSS loco
Flask AppBldr competes w/ MS PowerApps
superblocks.com

sql.ophir.dev: bld entire apps & intl tools
Hansura|Supabase


marmelab.com/blog/2023/07/04/react-admin-...
*)

namespace Trivedi

#nowarn "20" "25" "58" "66" "67" "64" "760" "1182" "1558"

open System
open System.Drawing
open System.Windows.Forms
open Trivedi.Core
open Trivedi.UI

type tyRtb() as r = 
       inherit System.Windows.Forms.RichTextBox()
       //the next line fires WindProc PLUS paint, w/o it no firing....
       do r.SetStyle(ControlStyles.OptimizedDoubleBuffer ||| ControlStyles.DoubleBuffer ||| ControlStyles.AllPaintingInWmPaint ||| ControlStyles.UserPaint, true)
       let WM_PAINT = 15
       member r.WindProc(m:System.Windows.Forms.Message) =
          tibbie "in tyRtb() WindProc..."
          if (m.Msg = WM_PAINT) then
            r.Invalidate()
            base.WndProc(ref m)
            use g = Graphics.FromHwnd(r.Handle)
            tibbie "tyRtb() graphics if"
            g.DrawLine(Pens.Red, Point.Empty, new Point(r.ClientSize.Width - 1, r.ClientSize.Height - 1))
          else 
            tibbie "tyRtb() graphics else"
            base.WndProc(ref m)       




///General Grid/UI skeleton for testing; to be extended per use case
///Currently has Flash ability; Stable
///Note that this is NOT the latest version; 
///    no Flash/autosize hdrs; chk commits (lk 2 log 4 dt)
#if ModGridTester_RemmedForMonkeyBastas_Jul12_2023
module GridTester = 
    open System
    open System.Collections
    open FSharp.Collections
    open System.Drawing
    open System.IO
    open System.Text
    open System.Runtime.Serialization
    open System.Runtime.Serialization.Formatters.Binary
    open System.Windows.Forms
    open Trivedi
    open Trivedi.Core
    open Trivedi.UI
    open Trivedi.UI.Helpers

    printfn "----Initializing module winFrms.GridTester----"

    let getTSButtonAux txt imgNm tt optF wRef :ToolStripItem =
        printfn "getTSButtonAux0..."
        //let itm:Option<Image> = (((Mtpl.GetOne "BrijDat" wRef), imgNm) |> uncurry getImageFromDat)
        let itm:Option<Image> = None
        let i =
            match (itm.IsSome) with
            | true -> itm.Value
            | _ -> (new Icon(SystemIcons.Information, 40, 40)).ToBitmap() :> Image
        let newBtn = new ToolStripButton(Image = i, Text = txt, Font = defFont, DisplayStyle = ToolStripItemDisplayStyle.Text, TextAlign = System.Drawing.ContentAlignment.MiddleRight)
        printfn "getTSButtonAux1..."
(* remmed 2023
        match tt with
        | Some t -> 
            newBtn.AutoToolTip <- false
            //newBtn.ToolTipText <- t
            newBtn.MouseEnter.AddHandler(new EventHandler(fun o e -> 
                let itm = o :?> ToolStripButton
                mTtip.SetToolTip(itm.Owner, t)))
        | None -> ()
*)
        printfn "getTSButtonAux2..."
        match optF with
        | Some f -> 
            newBtn.Click.AddHandler f
        | None -> newBtn.Click.Add (fun ev -> stati "ToolStripButton click; no deflt handler->" imgNm)
        printfn "getTSButtonAux3..."
        newBtn :> ToolStripItem

#if NotNecc_RemmedForMonkeyBastas_Jul12_2023
//Stateless @tbfo
    let getToolStripDVList =
        printfn "getToolStripDVList (stateLess)..."
        let getDropDnItms dvList = List.map (fun dvNm -> getTSButton dvNm "imgNm" (Some ( fun e -> tibbie "getDropDnItms" ))) dvList |> List.toArray
        let getDVsForTbl tblName = ["DataView one"; "DataView two"; "DataView three"] //@tibbie; hardCoded for now
        //toolStrip.items.add dropdownBtn
        let drpDn = new ToolStripDropDown()
        drpDn.Items.AddRange(getDropDnItms (getDVsForTbl "tbl"))
        new ToolStripDropDownButton(Text = "Switch DataView...", DropDown = drpDn, DropDownDirection = ToolStripDropDownDirection.Left, ShowDropDownArrow = true)
#endif //NotNecc_RemmedForMonkeyBastas_Jul12_2023

    let getCol (g:DataGridView) (n:int):DataGridViewColumn = g.Columns.[n]

    let AddRows (g:DataGridView) (rows:DataGridViewRow[]) =
        //g.InsertRows(g.RowCount - 1, rows)
        g.Rows.Add(rows) |> ignore
        g

    let setColHdrStyle  (g: DataGridView) = 
        g.ColumnHeadersDefaultCellStyle <- new DataGridViewCellStyle(BackColor = Color.Aqua, Font = defFont)
        g

    let freezeBand (g: DataGridView) (num:int) rowOrcol = 
        //bands also have a Tag property if nded 2 store propVals
        let band:DataGridViewBand = 
            match rowOrcol with 
            | true -> g.Rows.[num] :> DataGridViewBand
            | _ -> g.Columns.[num] :> DataGridViewBand
        band.Frozen <- true
        let style = new DataGridViewCellStyle()
        match band.Tag with 
        | :? string as s ->
            if s = "xxx" then
                style.BackColor <- Color.WhiteSmoke
        | _ -> tibbie "in Freezeband: band.Tag expected string but got something else"
        band.DefaultCellStyle <- style
        g

    let addTTip (g: DataGridView) (colNum:int) (str:string) : DataGridView = 
        (getCol g colNum).ToolTipText <- str
        g


    let origMusicDat() =
        [[ box "11/22/1968"; box "29"; box "Revolution 9"; box "Beatles"; box "The Beatles [White Album]"];
        [ box "02/02/1960"; box "6"; box "Fools Rush In"; box "Frank Sinatra"; box "Nice 'N' Easy"];
        [ box "11/11/1971"; box "1"; box "One of These Days"; box "Pink Floyd"; box "Meddle"];
        [ box "1/1/1988"; box "7"; box "Where Is My Mind?"; box "Pixies"; box "Surfer Rosa"];
        [ box "5/5/1981"; box "9"; box "Can't Find My Mind"; box "Cramps"; box "Psychedelic Jungle"];
        [ box "6/10/2003"; box "13"; box "Scatterbrain. (As Dead As Leaves.)"; box "Radiohead"; box "Hail to the Thief"];
        [ box "6/30/1992"; box "3"; box "Dress"; box "P J Harvey"; box "Dry"]]
        |> lim (fun r -> Array.ofList r)
        |> Array.ofList

    let PopulateDV (g: DataGridView) = 
        //g.Rows.Insert(int, Object[])
        g.Rows.Insert(0, "11/22/1968", "29", "Revolution 9", "Beatles", "The Beatles [White Album]")
        g.Rows.Insert(1,"02/02/1960", "6", "Fools Rush In", "Frank Sinatra", "Nice 'N' Easy")
        g.Rows.Insert(2,"11/11/1971", "1", "One of These Days", "Pink Floyd", "Meddle")
        g.Rows.Insert(3,"1/1/1988", "7", "Where Is My Mind?", "Pixies", "Surfer Rosa")
        g.Rows.Insert(4,"5/5/1981", "9", "Can't Find My Mind", "Cramps", "Psychedelic Jungle")
        g.Rows.Insert(5,"6/10/2003", "13", "Scatterbrain. (As Dead As Leaves.)", "Radiohead", "Hail to the Thief")
        g.Rows.Insert(6,"6/30/1992", "3", "Dress", "P J Harvey", "Dry")
        g

    let getToolStrip = 
          fun (own: Form) ->
                printfn "getToolStrip0..."
                let wld = Mtpl.empty()
                let regTBar = new ToolStrip(Dock = doc "T")
                printfn "getToolStrip1..."
                regTBar.Items.Add (getTSButtonAux "initFlash" "delImg.jpg"  None (Some(new EventHandler (fun sender e -> 
                    tibbie ("initFlash: recUps len: ")
                    ))) wld)
                printfn "getToolStrip2..."
                regTBar.Items.Add (new ToolStripSeparator())
                printfn "getToolStrip3..."
                regTBar

    let addGridToolBar (f: Form) (g:DataGridView) = 
        printfn "addGridToolBar..."
        getToolStrip f
        f

    let SetupDV = 
        fun (f:Form) (g:DataGridView) ->
            printfn "setupDV0... reset colWids"
            g.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader) //.AllCells  //throws on .Fill
            printfn "setupDV1..."
            let g_CellFormatting = new DataGridViewCellFormattingEventHandler(fun (sender:obj) (e:DataGridViewCellFormattingEventArgs) -> 
                if ( (e <> null) && (g.Columns.[e.ColumnIndex].Name = "Release Date") && (e.Value <> null)) then
                    //tibbie ("trying to parse: " + (e.Value.ToString()))
                    try
                        e.Value <- (DateTime.Parse(e.Value.ToString())).ToLongDateString()
                        e.FormattingApplied <- true
                    with 
                    | ex -> 
                        tibbie ("setupDV:CellFormatting: Couldn't parse dateTimeVal for: " + e.Value.ToString()))
            //set the colNames...
            List.fold2 (fun acc (x:int) (y:string) -> g.Columns.[x].Name <- y; "") "" [0..4] ["Release Date";"Track";"Title";"Artist";"Album"] |> ignore
            g.Columns.[4].DefaultCellStyle.Font <- new Font(g.DefaultCellStyle.Font, FontStyle.Italic)
            g.CellFormatting.AddHandler(g_CellFormatting)
            ctrlAddRange [|g;(getToolStrip f)|] f |> ignore
            Application.Run(f)

    let addImgColAt =
        fun (i:int) (g:DataGridView) -> 
            printfn "addImgColAt... %A ..." i
            let bitmapPadding = 6
            let b = new Bitmap("filNm")
            //Add twice the padding for the left & right sides of the cell.
            let colWidth = b.Width + 2 * bitmapPadding + 1
            g.Columns.RemoveAt(i)
            g.Columns.Insert(i, 
                new DataGridViewImageColumn(Width = colWidth, Image = b))
            g

    let paintCustomImgCell =
        fun (g:DataGridView) -> 
            g.CellPainting.Add(fun (e:DataGridViewCellPaintingEventArgs) -> 
                e.Graphics.DrawImage(Image.FromFile("SampImag.jpg"), new Rectangle(100, 100, 450, 150))
            )
            g

    let getDefDV1 = 
        printfn "getDefDV1"
        new DataGridView(
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            Font = defFont,
            Dock = DockStyle.Fill,
            Name = "g",
            Location = new Point(8, 8),
            AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders,
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single,
            CellBorderStyle = DataGridViewCellBorderStyle.Single,
            GridColor = Color.Black,
            RowHeadersVisible = false,
            ColumnCount = 5)

    let setColHdrs (g:DataGridView) = 
            printfn "setColHdrs"
            g.ColumnHeadersDefaultCellStyle.BackColor <- Color.Navy
            g.ColumnHeadersDefaultCellStyle.ForeColor <- Color.White
            g.ColumnHeadersDefaultCellStyle.Font <- new Font(g.Font, FontStyle.Bold)
            g

    let getDefDV = setColHdrs (getDefDV1)

(*
    //note that this mod currently incl 2 dotNet examples:
    //(1) songCollection tbl (https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.datagridviewrowcollection.addrange?view=net-5.0)
    //(2) boundDataSrc tbl (https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.datagridview?view=netcore-3.1#examples)
    // see for obj binding -> https://docs.microsoft.com/en-us/dotnet/desktop/winforms/controls/how-to-bind-objects-to-windows-forms-datagridview-controls?view=netframeworkdesktop-4.8
    // see for customizing cells/rows/cols -> https://docs.microsoft.com/en-us/dotnet/desktop/winforms/controls/customizing-the-windows-forms-datagridview-control?view=netframeworkdesktop-4.8
    // a multi-col row w/custom painting => https://docs.microsoft.com/en-us/dotnet/desktop/winforms/controls/customize-the-appearance-of-rows-in-the-datagrid?view=netframeworkdesktop-4.8
    let bld (owner:Form) = 
        state {
            PopulateDV |>| SetupDV (new Form(Owner=owner)) (getDefDV)
        }
*)

    let bldGrid (owner:Form) = 
        printfn "bldGrid"
        getDefDV |> PopulateDV |> SetupDV owner

#endif //ModGridTester_RemmedForMonkeyBastas_Jul12_2023

#if ModuleIde_RemmedForMonkeyBastas_Jul12_2023
module Ide = 
    open System
    open System.IO
    open System.IO.Compression
    open System.Drawing
    open System.Windows.Forms
    open System.Text.RegularExpressions
    open System.Diagnostics
    open DiffMatchPatch
    open Trivedi
    open Trivedi.Core
    open Trivedi.UI
    open GridTester 

    printfn "----Initializing module winFrms.Ide----"

#if fayette

    let diffOb = new diff_match_patch()

    //extracted from ui.dll where it is remmed for backward compat.
    let getChoiceDlg_Old msg (i:Form) (l1:list<string>) (l2:list<string>) =
        let frm = getDefaultDlgFrm_Old i
        let mutable retPtr = IntPtr.Zero
        let lbl = new Label(Size = new Size(400, 50), Location=Point(25, 10), Text = msg)
        let befr = new ComboBox(Size = new Size(200, 50), Location=Point(25, 100), Name = "beforeList")
        let aftr = new ComboBox(Size = new Size(200, 50), Location=Point(250, 100), Name = "afterList")
        List.map (fun x -> befr.Items.Add(x) |> ignore
                           aftr.Items.Add(x) |> ignore) l1 |> ignore
        let okButton = new Button(DialogResult = DialogResult.OK, Name = "okButton", Size = new Size(125, 50), Location = new Point(100, 150), Text = "&OK")
        let cancelButton = new Button(DialogResult = DialogResult.Cancel, Name = "cancelButton", Size = new Size(125, 50), Location = new Point(275, 150), Text = "&Cancel")
        frm.AcceptButton <- okButton
        frm.CancelButton <- cancelButton
        frm.Visible <- false
        frm.Controls.AddRange([|lbl; befr; aftr; okButton; cancelButton|])
        let result = frm.ShowDialog()
        result, befr.SelectedIndex, aftr.SelectedIndex

#if PointeCoupeeParish
    let getDiffsLineMode (bef:string) (aft:string) =
        let fstD = diffOb.diff_linesToChars(bef, aft)
        let lineTxt1 = fstD.[0]
        let lineTxt2 = fstD.[1]
        let lineArray = fstD.[2]
        let sndD = diffOb.diff_main(lineTxt1, lineTxt2, false)
        stati ">>>>>>>>Diff textualRepresentation ->\r\n" (diffOb.diff_charsToLines(sndD, lineArray))
#endif //PointeCoupeeParish

    let getDiffs (bef:string) (aft:string) =
        stati ">>>>>>>>Diff textualRepresentation ->\r\n" (diffOb.diff_main(bef, aft))

    let getDiffsHtml (bef:string) (aft:string) =
        stati ">>>>>>>>Diff HtmlRepresentation ->\r\n" (diffOb.diff_prettyHtml(diffOb.diff_main(bef, aft)))

    let getPatch (bef:string) (aft:string) (benc:bool) =
        let patches = diffOb.patch_make(bef, aft)
        match benc with
         | true -> 
              tibbie "remmed for b64 ref"
              //stati ">>>>>>>>Patch textualRepresentation ->\r\n" (b64Enc (diffOb.patch_toText(patches)))
         | _ -> 
              //stati ">>>>>>>>Patch textualRepresentation ->\r\n" (diffOb.patch_toText(patches))
              tibbie "remmed for b64 ref"

    let applyPatch patch =
      fun (bef:string) ->
        let res = diffOb.patch_apply(patch, bef)
        stati ">>>>>>>>Patch applied ->\r\n" (res.[0])

    let testDiff = 
      fun (bef:string) (aft:string) -> tibbie (">>>>>>>>Patch textualRepresentation ->\r\n" + (diffOb.patch_toText(diffOb.patch_make(bef, aft))))

#if lee
    let addTreeVw =
      fun (frm:Form) ->
          let rec createDirNode =
              fun dirInf -> 
                  let dirNode = new TreeNode(dirInf.Name)
                  dirInf.GetDirectories() |> List.ofArray |> lim (fun sd -> dirNode.Nodes.Add(createDirNode(sd)))
                  dirInf.GetFiles() |> List.ofArray |> lim (fun fl -> dirNode.Nodes.Add(new TreeNode(fl.Name)))
                  dirNode
          let treeVw = new TreeView()
          treeVw.Nodes.Clear()
          let rootDirInf = new DirectoryInfo(arcPath)
          treeVw.Nodes.Add(createDirNode(rootDirectoryInf))
#endif //lee

#if princeedward
//https://stackoverflow.com/questions/8624071/save-and-load-memorystream-to-from-a-file
    let getTreeVwZip() =
        let zipBA = File.ReadAllBytes(@"E:\tmp\zipTest.zip")
        use memStream = new MemoryStream(zipBA)
        let zipArc = new ZipArchive(memStream, ZipArchiveMode.Update)
        zipArc.Entries |> Seq.cast |> List.ofSeq 
           |> lifo (fun s en -> 
                       let (tv, dirLi) = s
                       match (en.FullName.EndsWith('\')) with
                       | true -> 
                           //create dirNode
                           dirNode.ImageIndex <- 0 //flderIco
                           (tv, dirLi)
                       | _ -> 
                           //create filNode
                           (tv, dirLi)
                   ) (tv,[]) |> ignore
          let flderImgLi = new ImageList()
          flderImgLi.Images.Add(Image.FromFile(@"E:\src\Data\images\google\folder.png"))
          let treeVw = new TreeView(ImageList = flderImgLi)
          treeVw.Nodes.Clear()
          let zipArc = new ZipArchive(memStream, ZipArchiveMode.Update)
          zipArc.Entries |> Seq.cast |> List.ofSeq |> lim (fun en -> createNode(en []))
          let rootDirInf = new DirectoryInfo(arcPath)
          treeVw.Nodes.Add(createDirNode(rootDirectoryInf))
          treeVw
#endif //princeedward

    let setupMainFrm =
      fun (frm:Form) ->
        let getLines() = List.fold (fun acc itm -> acc + itm.ToString() + "\r\n") "" [1..100]
        let tabCtrl = new TabControl(Dock = DockStyle.Fill, Name = "tabControl")
        ctrlAddTags2 ["tabControl", box tabCtrl] frm

#if princeedward
//06.29 this works as-is in the other file; the bastas are absolutely loco today throwing 5+ fake errs for methd below
//BASTAS probably have issues with tags
        let getCurrentWB = 
          fun (p:TabPage option) ->
            let pg:TabPage = Option.ઓર  p tabCtrl.SelectedTab
            (ctrlGetTag "www" pg).Value

        tabCtrl.DoubleClick.Add(fun e ->
                       if tabCtrl.SelectedTab.Controls.ContainsKey("www") then 
                           let nTxt = getInpDlg_Old "Pls enter address:" frm |> snd
                           let siteAdd = if ( nTxt.StartsWith("http://") || nTxt.StartsWith("https://") ) then nTxt else "http://" + nTxt
                           match getCurrentWB() with
                           | Some w -> w.Navigate(siteAdd)
                           | _ -> ()
                       else 
                           let nTxt = getInpDlg_Old "Pls enter new name:" frm |> snd
                           tabCtrl.SelectedTab.Text <- nTxt )
#endif //princeedward

        let ideTS = new ToolStrip(Name = "ideToolStrip")

        let getRTB = 
          fun (p:TabPage option) ->
//#if princeedward
            let pg:TabPage = Option.ઓર  p tabCtrl.SelectedTab
            (ctrlGetTag2 "rtb" pg).Value  :?> RichTextBox
//#else
//            new RichTextBox()
//#endif //princeedward

        let getTxtForPage =
          fun (p:TabPage option) ->
            let pg = 
                match p.IsSome with
                | true -> p
                | _ -> Some(tabCtrl.SelectedTab)
            (getRTB pg :?> RichTextBox).Text
        let getCurrentRTB() = getRTB None
        let getCurrentRTBTxt() = getTxtForPage None
        let addWebPg  = 
            fun (pgTxt: string) (tabCtrl:TabControl) ->
#if princeedward
                let pg = new TabPage( TabIndex = 1, Dock = DockStyle.Fill, Text = pgTxt, Font = defFont, Name = "wwwTabPg")
                let w = new WebBrowser(TabIndex = 1, Dock = DockStyle.Fill, Font = defFont, Name = "www")
                pg.Controls.Add(w)
                pg
#else
                printfn "remmed for the monkeyBastas"
#endif  //princeedward


        let addPg (pgTxt: string) (tabCtrl:TabControl) = 
            let pg = new TabPage( TabIndex = 1, Dock = DockStyle.Fill, Text = pgTxt ,Font = defFont, Name = "TabPg")
            let spl = new SplitContainer( Anchor = (AnchorStyles.Top ||| AnchorStyles.Left), Dock = DockStyle.Fill, IsSplitterFixed = true, SplitterDistance = 100, SplitterWidth = 1, TabIndex = 2 , FixedPanel = FixedPanel.Panel1, Name = "spl")
            ctrlAddTags2 ["pg", toOb pg; "spl", toOb spl] tabCtrl
            spl.SuspendLayout()
            spl.Panel1.BackColor <- Color.LightGray
            let rtb = new RichTextBox(ScrollBars = RichTextBoxScrollBars.ForcedBoth, RightMargin = Int32.MaxValue, Anchor = (AnchorStyles.Top ||| AnchorStyles.Left), BorderStyle = BorderStyle.None, Dock = DockStyle.Fill,TabIndex = 0, Font = defFont, AcceptsTab = true, Text = "Text", Name = "rtb" )
            !!^ ["clipBuffer", box []; "bookmarkBuffer", box []] rtb
            !!^ ["rtb", box rtb] pg
            rtb.ContentsResized.Add(fun e -> rtb.ZoomFactor = 0.0f |> ignore)
            rtb.LinkClicked.Add(fun e -> tibbie ("click recd on txt: " + e.LinkText))
#if lee
            //rtb.OnLinkClicked.Add(fun sender e -> tibbie ("click recd on txt: " + e.LinkText))
            //raises: error FS0491: The member or object constructor 'OnLinkClicked' is not accessible. Private members may only be accessed from within the declaring type. Protected members may only be accessed from an extending type and cannot be accessed from inner lambda expressions.
            let lineNo = new Label( Text = getLines(), Font = defFont, TabIndex = 1, AutoSize = true, Name = "lineNoLbl" )
#else
            let lineNo = new TextBox(Dock = doc "F", Multiline = true, Text = "1", Font = defFont, TextAlign = HorizontalAlignment.Right, Enabled=false)
            lineNo.Paint.Add(bookmarkPainter())
#endif //lee
            let updatelineNo() = 
#if lee
                lineNo.SuspendLayout()
                //we get index of first visible char and number of first visible line
                let firstIndex = rtb.GetCharIndexFromPosition(new Point(0, 0))
                let firstLine = rtb.GetLineFromCharIndex(firstIndex) + 1
                //now we get index of last visible char and number of last visible line
                let lastIndex = rtb.GetCharIndexFromPosition(new Point( (rtb.ClientRectangle).Width, (rtb.ClientRectangle).Height))
                let lastLine = rtb.GetLineFromCharIndex(lastIndex)
                //this is point position of last visible char, we'll use its Y value for calculating numberLabel size
                let pos = rtb.GetPositionFromCharIndex(lastIndex)
                let genLineNums (li:list<int>) =
                    List.fold (fun acc itm -> acc + itm.ToString() + "\r\n") "" li 
                lineNo.Text <- ( genLineNums [ firstLine .. lastLine ] )
                lineNo.ResumeLayout()
                //frm.Invalidate()
#else
                let charIdx = rtb.GetCharIndexFromPosition(new Point(0,0))
                let topLn = rtb.GetLineFromCharIndex(charIdx)
                lineNo.Text <- List.fold(fun s l -> s + l.ToString() + "\r\n") "" [topLn .. topLn + 40]
#endif //lee

            let hiliteCurrLn() =
                let charIdx = rtb.GetCharIndexFromPosition(new Point(0,0))
                let selStart = rtb.SelectionStart
                let selLen = rtb.SelectionLength
                rtb.SelectAll()
                rtb.SelectionBackColor <- Color.White
                rtb.Select(selStart, 0)
                let currLn = rtb.GetLineFromCharIndex(selStart)
                let firstCharPos = rtb.GetFirstCharIndexOfCurrentLine()
                let lastCharPos = rtb.GetFirstCharIndexFromLine(currLn + 1)
                //rtb.SelectionStart = firstCharPos
                //rtb.SelectionLength = lastCharPos - firstCharPos
                rtb.Select(firstCharPos, lastCharPos - firstCharPos)
                rtb.SelectionBackColor <- Color.Yellow
                rtb.Select(charIdx, 0)
                rtb.ScrollToCaret() //to ensure same pos
                rtb.Select(selStart, selLen)
                printfn "%A" ("rtb curr pos @ line #1: " + currLn.ToString())

            let bookmarkPainter() = 
                fun (e:PaintEventArgs) -> 
                    let firstIndex = rtb.GetCharIndexFromPosition(new Point(0, 0))
                    let firstLine = rtb.GetLineFromCharIndex(firstIndex) + 1
                    let lastIndex = rtb.GetCharIndexFromPosition(new Point( (rtb.ClientRectangle).Width, (rtb.ClientRectangle).Height))
                    let lastLine = rtb.GetLineFromCharIndex(lastIndex)
                    bkmarksLi |> List.filter (fun lineN -> (lineN < lastLine + 1) && (lineN > firstLine - 1)) 
                      |> lim (fun markable ->
                            //Retrieves the index of the first character of a given line
                            let charIdx = rtb.GetFirstCharIndexFromLine markable
                            //Retrieves the location within the control at the specified character index.
                            let loc = rtb.GetPositionFromCharIndex charIdx
                            //(i)Add staticRef to icn in ui.dll (ii)nd to move r
                            e.Graphics.DrawIcon(new Icon(SystemIcons.Information, 40, 40), 0, 0)) |> ignore

            rtb.MouseDown
                |> Event.add ( fun e -> hiliteCurrLn())

            rtb.KeyDown
                |> Event.filter ( fun e -> ( ( (e.Control && e.KeyCode = Keys.X) || (e.Shift && e.KeyCode = Keys.Delete) ) ) || ((e.Control && e.KeyCode = Keys.C)  || (e.Control && e.KeyCode = Keys.Insert)) )
                |> Event.add ( fun e -> MessageBox.Show("The Cut/Copy Options have been disabled ") |> ignore
                                        e.Handled <- true )

            rtb.KeyDown
                |> Event.filter ( fun e -> e.KeyCode = Keys.Tab )
                |> Event.add ( fun e -> e.Handled <- true
                                        rtb.SelectedText = new string(' ', 4) )

            //@mpt06.28.23 handler 4 pasteToIntlC
            rtb.KeyDown
                |> Event.filter ( fun e -> e.Alt && e.KeyCode = Keys.Delete )
                |> Event.add ( fun e -> 
                                 let buffer = !!~ "clipBuffer" (getCurrentRTB())
                                 !!^ ["clipBuffer", box ([(getCurrentRTB()).SelectedText] @ buffer)] (getCurrentRTB())
                                 //update StatusBar 'added to clipbd...'
                                 e.Handled <- true )

            //@mpt06.28.23 handler 4 pasteFromIntlC
            rtb.KeyDown
                |> Event.filter ( fun e -> e.Alt && e.KeyCode = Keys.Insert )
                |> Event.add ( fun e -> 
                                 let buffer = !!~ "clipBuffer" (getCurrentRTB())
                                 let ret = (ગપ્પા_પાન (SizeM,Some("Please make your selection (dbl-click):"), None , Some(box (buffer)), None, frm, listDlg())) |> ignore
                                 match ret with
                                 | Some r -> (getCurrentRTB()).SelectedText <- r
                                 | _ -> ()
                                 e.Handled <- true )

            //@mpt06.28.23 handler 4 //toggleBookmark
            rtb.KeyDown
                |> Event.filter ( fun e -> e.Control && e.KeyCode = Keys.B)
                |> Event.add ( fun e -> 
                                 let buffer = !!~ "bookmarkBuffer" (getCurrentRTB())
                                 let rtb = getCurrentRTB()
                                 let currLnNum = rtb.GetLineFromCharIndex(rtb.SelectionStart)
                                 match List.contains currLnNum buffer with
                                 | true -> 
                                     !!^ ["bookmarkBuffer", (box (List.except ([currLnNum] |> Seq.ofList) buffer))] rtb
                                     //update StatusBar 'bkmrk removed...'
                                 | _ -> 
                                     !!^ ["bookmarkBuffer", (box ([currLnNum] @ buffer))] rtb
                                     //update StatusBar 'bkmrk added...'
                                 e.Handled <- true )

#if remmedMBI
            //@mpt06.28.23 handler 4 gotoNxtBookmark
            rtb.KeyDown
                |> Event.filter ( fun e -> e.KeyCode = Keys.F2)
                |> Event.add ( fun e -> 
                                 let buffer = !!~ "bookmarkBuffer" (getCurrentRTB())
                                 let rtb = getCurrentRTB()
                                 let currLnStart = rtb.SelectionStart
                                 let currLnNum = rtb.GetLineFromCharIndex(currLnStart)
                                 match (buffer |> List.sort |> List.tryPick(fun en -> if en > currLnNum then Some(en) else None) with
                                 | Some n -> rtb.Select(rtb.GetFirstCharIndexFromLine(n), 0)
                                 | _ -> ()
                                 e.Handled <- true )

            //@mpt06.28.23 handler 4 gotoPrevBookmark
            rtb.KeyDown
                |> Event.filter ( fun e -> e.Shift && e.KeyCode = Keys.F2)
                |> Event.add ( fun e -> 
                                 let buffer = !!~ "bookmarkBuffer" (getCurrentRTB())
                                 let rtb = getCurrentRTB()
                                 let currLnStart = rtb.SelectionStart
                                 let currLnNum = rtb.GetLineFromCharIndex(currLnStart)
                                 match (buffer |> List.sortDescending |> List.tryPick(fun en -> if currLnNum > en then Some(en) else None)) with
                                 | Some n -> rtb.Select(rtb.GetFirstCharIndexFromLine(n), 0)
                                 | _ -> ()
                                 e.Handled <- true )
#endif //remmedMBI

            rtb.VScroll
                |> Event.add ( fun e ->
                    //move location of numberLabel for amount of pixels caused by scrollbar
                    lineNo.Location <- new Point(0, rtb.GetPositionFromCharIndex(0).Y % (rtb.Font.Height + 1))
                    updatelineNo() )

            rtb.TextChanged
                |> Event.add ( fun e -> updatelineNo() )

            rtb.Resize
                |> Event.add ( fun e ->
                        //rtb.VScroll(null, null)
                    lineNo.Location <- new Point(0, rtb.GetPositionFromCharIndex(0).Y % (rtb.Font.Height + 1))
                    updatelineNo() ) 

            rtb.FontChanged 
                |> Event.add ( fun e ->
                    //lineNo.Location <- new Point(0, rtb.GetPositionFromCharIndex(0).Y % (rtb.Font.Height + 1))
                    printfn "FontChanged event"
                    updatelineNo() ) 
            spl.Panel1.Controls.Add(lineNo)
            spl.Panel2.Controls.Add(rtb)
            spl.ResumeLayout()
#if lee
            //treeVw outerSpl, resizeable, fixed is spl
            let outerSpl = new SplitContainer( Anchor = (AnchorStyles.Top ||| AnchorStyles.Left), Dock = DockStyle.Fill, IsSplitterFixed = false, SplitterDistance = 100, SplitterWidth = 1, TabIndex = 2 , FixedPanel = FixedPanel.Panel2, Name = "outerSpl")
            outerSpl.Panel1.Controls.Add(getTreeVwZip())
            outerSpl.Panel2.Controls.Add(spl)
            pg.Controls.Add(outerSpl)

            //add to btnBar in Pg
            hideProjPnlBtn.Click.Add(fun e -> 
             outerSpl.Panel1Collapsed <- True
             outerSpl.Panel1.Hide()
            )
            //plus IIIlar 4 viewProjPnlBtn ...
#else
            pg.Controls.Add(spl)
#endif //lee 4 treeVw outerSpl
            pg

        let newP:Process = new Process()

        let bldCmd (p: Process) (exe:string) : Process = 
            match exe with
            | "fsc.exe" -> 
                 p.StartInfo <- new ProcessStartInfo (FileName = exe,
                                 Arguments = "latest.fs --target:winexe --platform:x64 -r:googleDiff.dll -r:System.Runtime -r:System.Windows.Forms -r:System.IO -I:C:\\Windows\\Microsoft.NET\\Framework64\\v4.0.30319", 
                                 UseShellExecute = false, RedirectStandardOutput = false, RedirectStandardError = false,
                                 WindowStyle = ProcessWindowStyle.Maximized )
            | _ -> 
                 p.StartInfo <- new ProcessStartInfo (FileName = exe,
                                 UseShellExecute = false, RedirectStandardOutput = false, RedirectStandardError = false,
                                 WindowStyle = ProcessWindowStyle.Maximized )
            p

        let CompilerProc = bldCmd newP "fsc.exe"
        let CompiledProc = bldCmd newP "latest.exe"

        let runProcess (cmd:Process) = 
            //let sb = new StringBuilder()
            //cmd.OutputDataReceived.Add(fun x -> appendString sb (x.Data + Environment.NewLine) |> ignore)
            //cmd.ErrorDataReceived.Add(fun x -> appendString sb (x.Data + Environment.NewLine) |> ignore)
            cmd.Start() |> ignore
            //cmd.BeginOutputReadLine()
            cmd.WaitForExit()
            //cmd.BeginErrorReadLine()
            //cmd.WaitForExit()
            //cmd.StandardOutput.ReadToEnd()  |> ignore
            //cmd.StandardError.ReadToEnd() |> ignore
            cmd.Close()
            //tibbie (getString sb)
            tibbie "runProc finished w/successful cmd.close()"


        //This is the source for the test file (compile fine manually)
        //The bastas are still playing with the File System but we got em on the run now
        //soon their stealing days will be over... (they will return to lying & cheating)
        let srcFileTxt = """
    open System
    open System.Windows.Forms
    printfn "----Initializing module main----"
    [<EntryPoint>]
    [<STAThread>]
    let main argv =
        printfn "----In main1; formshow -------"
        Application.EnableVisualStyles()
        Application.Run(new Form(Text = "testing"))
        printfn "----In main2: before readLine -------"
        let unused = Console.ReadLine()
        printfn "----In main3: after readLine -------"
        0
"""

        //https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.process.outputdatareceived?view=net-5.0
        let waitProcess =
          fun (proc: Process) ->
             proc.WaitForExit()
             stati "proc output:" "->"
             while (not proc.StandardOutput.EndOfStream) do
                printfn "%A" (proc.StandardOutput.ReadLine())
                //printfn $"{proc.StandardOutput.ReadLine()}"

        let runFSC() = 
            //latest.fs
            use outFl = new FileStream("latest.exe", FileMode.Create, FileAccess.ReadWrite, FileShare.None)
            use src = new FileStream("latest.fs", FileMode.Create, FileAccess.ReadWrite, FileShare.None)
            let srcTxt = getCurrentRTBTxt()
            let fmted =  બાઇટ (રિપ્લ  srcTxt "\t" "    ")
            let dummy = બાઇટ srcFileTxt
            src.Write(fmted, 0, fmted.Length)
            tibbie "finished writing, running fsc..."
//  -> debug procErr
            let cmd = CompilerProc
            tibbie "1.  got cmd, about to start()..."
            cmd.Start() |> ignore
            tibbie "2.  waitForExit..."
            cmd.WaitForExit()
            tibbie "2.  close()..."
            cmd.Close()
//  <- debug procErr

            //runProcess (CompilerProc)

            tibbie "after fsc..."
//            src.Seek(0, SeekOrigin.Begin) |> ignore
            src.SetLength(1L)
            src.Write(dummy, 0, Convert.ToInt32(dummy.Length))
            tibbie "after writing over src; running new file..."
            runProcess (CompiledProc)

        let setupMenu = 
          fun (frm:Form) (tc:TabControl) ->
            let filMenu = new ToolStripMenuItem("File")
            let difMenu = new ToolStripMenuItem("Diff")
            let tstMenu = new ToolStripMenuItem("Test")
            let toolMenu = new ToolStripMenuItem("Tools")

            let fileNew = new ToolStripMenuItem("New Tab", null, 
                new EventHandler(fun (o:obj) (e:EventArgs) -> tc.Controls.Add(addPg ("Tab " + (tabCtrl.TabCount + 1).ToString()) tc)), Keys.Control ||| Keys.N)

            let fileNew2 = new ToolStripMenuItem("New Tab2", null, new EventHandler(fun (o:obj) (e:EventArgs) -> 
#if Suffolk
                                                                                     match (ctrlGetTag "rtabControl" frm) with
                                                                                     | Some v -> let tc = v :?> TabControl
                                                                                                 tc.Controls.Add(addWebPg ("wwwTab " + (tabCtrl.TabCount + 1).ToString()) tc)
                                                                                     | None ->   ()
#else 
                                                                                     tibbie "remmed for mbi (stable otherwise)"
#endif //Suffolk
                                                                                      ))

#if losangeles
            let fileNew = new ToolStripMenuItem(Text = "New",Font = defFont)
            fileNew.Click.Add (fun evArgs -> 
                                 match (ctrlGetTag "tabControl" frm) with
                                  | Some v -> let tc = v :?> TabControl
                                              tc.Controls.Add(addPg ("Tab " + (tabCtrl.TabCount + 1).ToString()) tc)
                                  | None ->   () )

#endif //losangeles

            let fileGrid = new ToolStripMenuItem(Text = "Grd tester",Font = defFont)
            fileGrid.Click.Add (fun evArgs -> bldGrid frm )

#if UIDEBUGGED
            //refers to module Dlg
            let fileDlg = new ToolStripMenuItem(Text = "txtDlg",Font = defFont)
            fileDlg.Click.Add(fun (e:EventArgs) ->
                                   printfn "db: fileDlg"
                                   let d = પલંગ_તોડ_પાન frm
                                   printfn "db: fileDlg2"
                                   d.ShowDialog() |> ignore )
                                   //ટેક્ષ્ટ_પીચાક mLTxt (પલંગ_તોડ_પાન mLTxt frm) |> ignore )
                                           //ટેક્ષ્ટ_પીચાક mLTxt ((પલંગ_તોડ_પાન mLTxt frm).સુપારી(1).લવલી("A test; only (merely?) a test.")) |> ignore ))
#endif //UIDEBUGGED

            let topFrmMenuItm = fun (m:ToolStripMenuItem) ->
                                      let cont = m.Container :?> Control
                                      cont.TopLevelControl :?> Form
            let getHalfScreenSz = 
               fun (own:Form) -> 
                   let newWd = (Screen.GetWorkingArea(own)).Width / 2 
                   let newHt = (Screen.GetWorkingArea(own)).Height / 2 
                   new Size(newWd,newHt)
            let fileDlg2 = new ToolStripMenuItem(Text = "txtDlg noTy",Font = defFont)
            fileDlg2.Click.Add(fun (e:EventArgs) ->

(*
	Fake monkeyBastard err: 
        UI.fs(1470,55): error FS0491: The member or object constructor 'Parent' is not accessible. Private members may only be accessed from within the declaring type. Protected members may only be accessed from an extending type and cannot be accessed from inner lambda expressions.


                                            let p =  ((o :?> ToolStripItem).Parent) :?> Control
                                            let બનાવો =  p.TopLevelControl :?> Form
*)
                                            printfn "db: Dlg setupDlg0"
                                            let બનાવો =  frm
                                            //nd to test autoscroll behav. ~~~~
                                            let d = Form(Visible = false, TopMost = true, Size = getHalfScreenSz બનાવો, Owner = બનાવો, WindowState = FormWindowState.Normal, Font = defFont)

#if !setupChk

                                            let innrPnl() = new Panel(Dock = doc "F", Width = d.Width - 20, BackColor = getRandomLightColor() )
                                            let btnP = new FlowLayoutPanel(FlowDirection = FlowDirection.LeftToRight, Dock = doc "B", AutoSize = true, Width = d.Width, BackColor = Color.DarkGray)
                                            let okButton = new Button(AutoSize = true, DialogResult = DialogResult.OK, Size = new Size(125, 50), Location = new Point(100, 150), Text = "&OK")
                                            let cancelButton = new Button(AutoSize = true, DialogResult = DialogResult.Cancel, Size = new Size(125, 50), Text = "&Cancel")
                                            btnP.Controls.Add(okButton)
                                            btnP.Controls.Add(cancelButton)
                                            let midHt = d.Height - btnP.Height - defPadding.Vertical
                                            let midP = new TableLayoutPanel(GrowStyle = TableLayoutPanelGrowStyle.AddRows, Anchor = anc "N", Width = d.Width - 20, Height = midHt, BackColor = Color.OldLace, AutoScroll = true)
                                            midP.Controls.Add(innrPnl())
                                            midP.SetAutoScrollMargin(5, 5)
                                            d.Controls.Add(midP)
                                            d.Controls.Add(btnP)
                                            btnP.Click.Add(fun evA ->
                                                      let res = MessageBox.Show("Add another pnl?", "System msg", MessageBoxButtons.AbortRetryIgnore)
                                                      if res = DialogResult.Retry then 
                                                         let newP = innrPnl()
                                                         newP.BackColor <- getRandomLightColor()
                                                         midP.Controls.Add(newP))

#else 
//                                            setupDlg f
                                            printfn "db: Dlg setupDlg1"
                                            let b = (new Icon(SystemIcons.Information, 40, 40)).ToBitmap()
                                            let icnLbl = new Label(Image = b, Size = (new Size(b.Width, b.Height)), Anchor = anc "N")
                                            let titTxt = new TextBox(Dock = doc "F", AcceptsReturn = true, Text = "default Title txt", ForeColor = Color.DarkBlue, ReadOnly = true, Multiline = true)
                                            let titleP = new TableLayoutPanel(RowCount = 1, ColumnCount = 5, Dock = doc "T", BackColor = Color.OldLace, AutoSize = true, Width = d.Width ) //Height = 125
                                            titleP.SuspendLayout()
                                            titleP.Controls.Add(icnLbl, 0, 0)
                                            titleP.Controls.Add(titTxt, 1, 0)
                                            titleP.SetColumnSpan(titTxt, 4)
                                            titleP.ResumeLayout(false)
                                            printfn "db: Dlg setupDlg2"
                                            let btnP = new FlowLayoutPanel(FlowDirection = FlowDirection.LeftToRight, Dock = doc "B", AutoSize = true, Width = d.Width )
                                            let okButton = new Button(AutoSize = true, DialogResult = DialogResult.OK, Size = new Size(125, 50), Location = new Point(100, 150), Text = "&OK")
                                            let cancelButton = new Button(AutoSize = true, DialogResult = DialogResult.Cancel, Size = new Size(125, 50), Text = "&Cancel")
                                            btnP.Controls.Add(okButton)
                                            btnP.Controls.Add(cancelButton)
                                            printfn "db: Dlg setupDlg3"
                                            let midHt = d.Height - ((max (titleP.Height) (btnP.Height)) * 2) - (defPadding.Vertical)
                                            let midP = new TableLayoutPanel(Anchor = anc "N", Width = d.Width, Height = midHt)
                                            let midTxt = new TextBox(Width = d.Width, Height = midHt, Text = mLTxt, Multiline=true, Dock = doc "F", Enabled = false)
                                            d.Controls.Add(titleP)
                                            d.Controls.Add(btnP)
                                            d.Controls.Add(midP)
                                            ctrlAddTags ["titleP", toOb titleP; "titTxt", toOb titTxt; "icnLbl", toOb icnLbl; "btnP", toOb btnP; "okBtn", toOb okButton;"cancelBtn", toOb cancelButton; "midTxt", toOb midTxt;"midP", toOb midP] d
                                            printfn "db: Dlg setupDlg4"

#endif //!setupChk
                                            d.ShowDialog() |> ignore)


//refers to module Dlg
#if UIDEBUGGED
            let fileDlg3 = new ToolStripMenuItem(Text = "txtDlg rDlgTy",Font = defFont)
            fileDlg3.Click.Add(fun (e:EventArgs) ->

                                            printfn "db: Dlg setupDlg0"
                                            let બનાવો =  frm
                                            let d = Form(Visible = false, TopMost = true, Size = getHalfScreenSz બનાવો, Owner = બનાવો, WindowState = FormWindowState.Normal, Font = defFont)

#if !setupChk

                                            let btnP = new FlowLayoutPanel(FlowDirection = FlowDirection.LeftToRight, Dock = doc "B", AutoSize = true, Width = d.Width, BackColor = Color.DarkGray)
                                            let okButton = new Button(AutoSize = true, DialogResult = DialogResult.OK, Size = new Size(125, 50), Location = new Point(100, 150), Text = "&OK")
                                            let cancelButton = new Button(AutoSize = true, DialogResult = DialogResult.Cancel, Size = new Size(125, 50), Text = "&Cancel")
                                            ctrlsAdd [okButton;cancelButton] btnP
                                            let midHt = d.Height - btnP.Height - defPadding.Vertical
                                            let midWd = d.Width - 20
                                            let midP = rPnl(midHt, midWd)
                                            d.Controls.Add(midP)
                                            d.Controls.Add(btnP)
                                            btnP.Click.Add(fun evA ->
                                                              let res = MessageBox.Show("Add another pnl?", "System msg", MessageBoxButtons.YesNo)
                                                              if res = DialogResult.Yes then midP.addPnl())

#else 
//                                            setupDlg f
                                            printfn "db: Dlg setupDlg1"
                                            let b = (new Icon(SystemIcons.Information, 40, 40)).ToBitmap()
                                            let icnLbl = new Label(Image = b, Size = (new Size(b.Width, b.Height)), Anchor = anc "N")
                                            let titTxt = new TextBox(Dock = doc "F", AcceptsReturn = true, Text = "default Title txt", ForeColor = Color.DarkBlue, ReadOnly = true, Multiline = true)
                                            let titleP = new TableLayoutPanel(RowCount = 1, ColumnCount = 5, Dock = doc "T", BackColor = Color.OldLace, AutoSize = true, Width = d.Width ) //Height = 125
                                            titleP.SuspendLayout()
                                            titleP.Controls.Add(icnLbl, 0, 0)
                                            titleP.Controls.Add(titTxt, 1, 0)
                                            titleP.SetColumnSpan(titTxt, 4)
                                            titleP.ResumeLayout(false)
                                            printfn "db: Dlg setupDlg2"
                                            let btnP = new FlowLayoutPanel(FlowDirection = FlowDirection.LeftToRight, Dock = doc "B", AutoSize = true, Width = d.Width )
                                            let okButton = new Button(AutoSize = true, DialogResult = DialogResult.OK, Size = new Size(125, 50), Location = new Point(100, 150), Text = "&OK")
                                            let cancelButton = new Button(AutoSize = true, DialogResult = DialogResult.Cancel, Size = new Size(125, 50), Text = "&Cancel")
                                            btnP.Controls.Add(okButton)
                                            btnP.Controls.Add(cancelButton)
                                            printfn "db: Dlg setupDlg3"
                                            let midHt = d.Height - ((max (titleP.Height) (btnP.Height)) * 2) - (defPadding.Vertical)
                                            let midP = new TableLayoutPanel(Anchor = anc "N", Width = d.Width, Height = midHt)
                                            let midTxt = new TextBox(Width = d.Width, Height = midHt, Text = mLTxt, Multiline=true, Dock = doc "F", Enabled = false)
                                            d.Controls.Add(titleP)
                                            d.Controls.Add(btnP)
                                            d.Controls.Add(midP)
                                            ctrlAddTags ["titleP", toOb titleP; "titTxt", toOb titTxt; "icnLbl", toOb icnLbl; "btnP", toOb btnP; "okBtn", toOb okButton;"cancelBtn", toOb cancelButton; "midTxt", toOb midTxt;"midP", toOb midP] d
                                            printfn "db: Dlg setupDlg4"

#endif //!setupChk

                                            d.ShowDialog() |> ignore)
#endif  //UIDEBUGGED

#if losangeles
            let fileListDlg = new ToolStripMenuItem(Text = "ListDlg",Font = defFont)
            fileListDlg.Click.Add 
             (fun evArgs -> 
                  //Pick a (random seed val) -> toL() -> bcw
                  let choiceL = [["John"; "Paul"; "George"];
                                    ["Ceasar"; "Napoleon"; "Tokugawa"];
                                    ["Bruce"; "Jackie"; "Chuck"];
                                    ["Hemingway"; "Fitzgerald"; "Wharton"];
                                    ["Trump"; "Obama"; "Biden"];
                                    ["Bezos"; "Musk"; "Gates"];
                                    ["Morgan"; "Astor"; "Vanderbilt"];
                                    ["Lemon"; "Orange"; "Lime"];
                                    ["Scotch"; "Bourbon"; "Vodka"];
                                    ["Nabokov"; "Mann"; "Brecht"];
                                    ["Marathon"; "Triathlon"; "Biathlon"];
                                    ["DeNero"; "Pacino"; "Eastwood"]]

                  let dlg = getListDlg frm (Some(choiceL))
                                   (Some (fun e -> 
                                             //scoping issues (new type?)
                                             let txt = "(lB.SelectedItems).ToString()"
                                             MessageBox.Show( txt , "chosen items") |> ignore )) 0
                  dlg.Show())
#endif //losangeles
 
            let fileTest2 = new ToolStripMenuItem(Text = "TestTblDlg",Font = defFont)
#if craven
            //remmed 08.27 : too many @MBI errs (bogus)
            fileTest2.Click.Add (fun evArgs -> 
                                    let li = (List.fold (fun x -> Riddle("Riddle Qn " + x.ToString(),"Riddle Key " + x.ToString()) ) [1..9] )
                                    getTblDlg frm (li)  None 
                                    )
#endif //craven

            let fileTest3 = new ToolStripMenuItem(Text = "TestGetConrol3",Font = defFont)

            //remmed 8/27: monkeyBastaInteference because they use Components (WPF), the effers
#if craven
            fileTest3.Click.Add (fun evArgs -> 
                                     getControlByNm3 frm "rtb"
                                     |> List.fold (fun acc x -> acc + "\r\n" + x.ToString()) ""
                                     |> logToIde )
#endif //craven

            let cDlgTest = new ToolStripMenuItem(Text = "cDlgTest",Font = defFont)
            cDlgTest.Click.Add (fun evArgs -> 
                                     tibbie "tibbie")
(*
            cDlgTest.Click.Add (fun evArgs -> 
                                     let res, outp = getCInputDlg "enter inpTxt pls." frm 
                                     tibbie ("res: " + res.ToString() + "\r\nOutput: " + outp.ToString()))
*)

            let diffTest = new ToolStripMenuItem(Text = "diffTest",Font = defFont)
            diffTest.Click.Add (fun evArgs -> 
#if !Suffolk
                                  let pgs:list<TabPage> = (Seq.cast tc.TabPages) |> Seq.toList
                                  let pgTitles:list<String> = List.map (fun (x:TabPage) -> x.Text) pgs
                                  let res, befIdx, aftIdx = getChoiceDlg_Old "Pls select before/after" frm pgTitles pgTitles
                                  testDiff (getTxtForPage (Some(pgs.[befIdx]))) (getTxtForPage (Some(pgs.[aftIdx])))
#else 
                                  tibbie "remmed coz it calls ChoiceDlg; which the monkeyBastas have issues with impl.ing (still slow as eff)"
#endif //!Suffolk
                                )

            let fileGoTo =  new ToolStripMenuItem("&GoTo", null, new EventHandler(fun (o:obj) (e:EventArgs) -> 
                                                                                    let nTxt = getInpDlg_Old "Pls enter lineNum:" frm |> snd
                                                                                    match (tryParseInt nTxt) with
                                                                                    | Some i -> 
                                                                                        let rt = getCurrentRTB() :?> RichTextBox
                                                                                        let goToPos = rt.GetFirstCharIndexFromLine(i)
                                                                                        rt.Select(goToPos, 0)
                                                                                        rt.ScrollToCaret()
                                                                                    | None ->  ()), Keys.Control ||| Keys.G)

            let fileTest3 = new ToolStripMenuItem(Text = "Test3",Font = defFont)
            fileTest3.Click.Add (fun evArgs -> 
                                     tibbie "tibbie" )

            let fileCompile = new ToolStripMenuItem(Text = "Compile",Font = defFont)
            fileCompile.Click.Add (fun evArgs -> runFSC() )

            let fileExit = new ToolStripMenuItem("E&xit", null, new EventHandler(fun (o:obj) (e:EventArgs) -> Application.Exit()), Keys.Control ||| Keys.X)

            filMenu.DropDownItems.Add(fileNew) |> ignore
            //filMenu.DropDownItems.Add(fileFetch) |> ignore
            //filMenu.DropDownItems.Add(fileTest1) |> ignore
            filMenu.DropDownItems.Add(fileTest2) |> ignore
            filMenu.DropDownItems.Add(fileGoTo) |> ignore
            filMenu.DropDownItems.Add(cDlgTest) |> ignore
            filMenu.DropDownItems.Add(fileTest3) |> ignore
            filMenu.DropDownItems.Add(fileCompile) |> ignore
            filMenu.DropDownItems.Add(fileExit) |> ignore

#if !Suffolk
//connected to getChoiceDlg_Old fakeErrs...
            let applyPatch2 (bef:string) (pat:string) = 
                (diffOb.patch_apply(diffOb.patch_fromText(pat), bef)).[0] :?> string

            let applyPatch = new ToolStripMenuItem(Text = "applyPatch",Font = defFont)
            applyPatch.Click.Add (fun evArgs -> 
                                  let pgs:list<TabPage> = (Seq.cast tc.TabPages) |> Seq.toList
                                  let pgTitles:list<String> = List.map (fun (x:TabPage) -> x.Text) pgs
                                  let res, befIdx, aftIdx = getChoiceDlg_Old "Pls select before/patch" frm pgTitles pgTitles
                                  let tgtPage = pgs.[befIdx]
                                  let patchedTxt = (applyPatch2 (getTxtForPage (Some(tgtPage))) (getTxtForPage (Some(pgs.[aftIdx]))))
                                  (getRTB (Some(tgtPage))).Text <- patchedTxt )
            difMenu.DropDownItems.Add(diffTest) |> ignore
            difMenu.DropDownItems.Add(applyPatch) |> ignore
#endif //!Suffolk
            tstMenu.DropDownItems.Add(fileGrid) |> ignore

#if UIDEBUGGED
            //refers to module Dlg
            tstMenu.DropDownItems.Add(fileDlg) |> ignore
#endif

            tstMenu.DropDownItems.Add(fileDlg2) |> ignore

#if UIDEBUGGED
            //refers to module Dlg
            tstMenu.DropDownItems.Add(fileDlg3) |> ignore
#endif

            //tstMenu.DropDownItems.Add(fileListDlg) |> ignore

            let ms = new MenuStrip(Dock = DockStyle.Top,Font = defFont)
            ms.Items.Add(filMenu) |> ignore
            ms.Items.Add(difMenu) |> ignore
            ms.Items.Add(tstMenu) |> ignore
            ms.Items.Add(toolMenu) |> ignore
            frm.MainMenuStrip <- ms
            frm.Controls.Add(ms)
            frm

        let findHandlerOLD = (fun e -> 
                            let findTxt (r:RichTextBox) (txt:string) = 
                               let currPos = r.GetPositionFromCharIndex(0)
                               let currChar = r.GetFirstCharIndexOfCurrentLine()
                               let start = r.SelectionStart
                               let length = r.SelectionLength

                               let regex = new Regex(txt, RegexOptions.Singleline)
                               let matches = regex.Matches((r.Text).Substring(currChar, (r.Text).Length - currChar))
                               if (matches.Count > 0) then
                                  //tibbie ("found " + matches.Count.ToString() + " matches")
                                  //for m in matches do
                                     r.Select((matches.[0]).Index, (matches.[0]).Length)
                                     r.SelectionBackColor <- Color.Yellow
                                     r.ScrollToCaret()
                                  // put selection back to the way it was
                                     r.Select(start, length)
                               else
                                  tibbie "No matches found"
                            tibbie "old handler")

        let findHandler = (fun e -> 
                            let findTxt = 
                                fun (r:RichTextBox) (txt:string) ->
                                    let rg = new Regex(txt, RegexOptions.Singleline)
                                    let li = rg.Matches (getCurrentRTBTxt()) |> Seq.cast |> List.ofSeq
                                    let mLi:list<_> = List.map(fun (m:Match) -> 
                                                        let i = m.Groups.[0].Index
                                                        let len = m.Groups.[0].Value.Length
                                                        i, len) li
                                    ctrlAddTags2 ["matches", box mLi; "currIdx" box 0] r
                                    let (idx, l) = mLi.[0]
                                    r.Select(idx, l)
                            let txtToFind() = 
                                 match (!!~ "findTxtBox" ideTS) with
                                  | Some v -> let tb:ToolStripTextBox = v :?> ToolStripTextBox
                                              tb.Text
                                  | None ->   "" 
                            if txtToFind() = "" then 
                                tibbie "findHandler couldn't get text to find" else
                                    match getCurrentRTB() with
                                        | Some r -> 
                                            let rt = r :?> RichTextBox
                                            findTxt rt txtToFind
                                        | None ->   
                                            tibbie "couldn't get handle on rtb"
                                            ()
                            )


//        let findNxtHandler = (fun e -> 
//                                let matchLi = ctrlGetTag ...


        let TSAddRange (ts:ToolStrip) = 
            let findBox = getTSTxtBox "findTxtBox"
            let replBox = getTSTxtBox "replTxtBox"
            ts.Items.AddRange  
                (List.toArray 
                  [
                    //getTSButton "Find:" "addImg.jpg" (Some( fun e -> tibbie "tbdb (recent changes)...")) ;
                    getTSButton "Find:" "addImg.jpg" (Some(findHandler)) ;
                    findBox;
                    getTSButton "Replace With:" "delImg.jpg" (Some( fun e -> tibbie "no handler yet...")) ;
                    replBox;
                    new ToolStripSeparator() :> ToolStripItem;
                    getTSButton "Add Something" "addImg.jpg" (Some(fun e -> tibbie "no handler yet..." )) ])
            !!^ ["findTxtBox", box findBox; "replTxtBox", box replBox] ts
            ts
        let getToolStrip:ToolStrip = 
            ideTS |> TSAddRange
        let TSPanelAddToolStrips (tsPanl:ToolStripPanel) = 
            tsPanl.Join( getToolStrip )
            tsPanl
        let getTSPanel = 
            (new ToolStripPanel(Dock = DockStyle.Top)) |> TSPanelAddToolStrips

        tabCtrl.Controls.Add(addPg "Tab 1" tabCtrl)
        let stat = new StatusStrip(SizingGrip = false, Stretch = true, Dock = doc "B", Font = new Font("Tahoma", 18.0f))
        let statLbl = new ToolStripStatusLabel(Text = "Ready...") :> ToolStripItem
        stat.Items.AddRange([|statLbl|])
        frm.Controls.Add(tabCtrl)
        frm.Controls.Add(getTSPanel)
        frm.Controls.Add(stat)
        setupMenu frm tabCtrl

    let oldRunner() = setupMainFrm (new Form(WindowState = FormWindowState.Maximized, Visible = true, Text = "Earlier Spx Form: Copyright (c) M. P. Trivedi 2016-2023.  All rights reserved.", TopMost=true, Font=defFont))

#endif //fayette


#endif //ModuleIde_RemmedForMonkeyBastas_Jul12_2023

#if ModuleExt_RemmedForMonkeyBastas_Jul12_2023
module Ext =
    open System
    open System.Drawing
    open System.IO
    open System.IO.Compression
    open System.Text
    //open System.Buffers.Text
    open System.Windows.Forms
    open System.Text.RegularExpressions
    open System.Diagnostics
    open DiffMatchPatch
    open Trivedi
    open Trivedi.Core
    open Trivedi.UI
    open GridTester 

    printfn "in mod winFrms.Ext..."

#if modExt

#if princeedward
    let ext() = 
        let f = (new Form(WindowState = FormWindowState.Maximized, Visible = false, Text = "winFrms Test Form: Copyright (c) M. P. Trivedi 2016-2023.  All rights reserved.", TopMost=true, Font=defFont))
        f.Width <- 800
        f.Height <- 400
        let txtEdCtrl = new ICSharpCode.TextEditor.TextEditorControlEx(ContextMenuEnabled = true,
            ContextMenuShowDefaultIcons = true,
            ContextMenuShowShortCutKeys = true, FoldingStrategy = "XML", 
            HideVScrollBarIfPossible = true, Location = new Point(15, 46), Margin = new Padding(4, 4, 4, 4),
            Name = "textEditorControl1", ShowVRuler = false,
            Size = new System.Drawing.Size(695, 462),
            SyntaxHighlighting = "XML", TabIndex = 0,
            Text = "resources.GetString(textEditorControl1.Text)", VRulerRow = 999)
        txtEdCtrl.TextChanged.AddHandler(new EventHandler(fun o e ->
            txtEdCtrl.Document.FoldingManager.UpdateFoldings(null, null)
            //textBox1.Text = string.Join("\r\n", textEditorControl1.GetFoldingErrors())
          ))
        //@mbi, fix l8r
        //txtEdCtrl.Font = defFont,
        txtEdCtrl.SetHighlighting("XML")
        txtEdCtrl.SetFoldingStrategy("XML")
        txtEdCtrl.Document.FoldingManager.UpdateFoldings(null, null)
        f.Controls.Add(txtEdCtrl)
        f

(*
richTextBox1.Text = "Hello";
richTextBox1.Select(0,2);
//Gets or sets the text color of the current text selection or insertion point.
richTextBox1.SelectionColor = Color.Red 
*)
!#else

    let testArchive_v2 =
        //basically we nd 2 do much LESS work; the archive is sorted & ordered; shd NOT throw
        //poa is: don't chk for existence of nodes (if the archive contains a 'subdir/file' then
        //it automatically has to have the subdir already defined; just get the node & attach...
        hr()
        printfn "running testArchive..."
        let zipBA = File.ReadAllBytes(@"./zipTest.zip")
        use memStream = new MemoryStream(zipBA)
        let zipArc = new ZipArchive(memStream, ZipArchiveMode.Update)
        zipArc.Entries |> Seq.cast |> List.ofSeq 
           |> lifo (fun s (en:ZipArchiveEntry) -> 
                       let ((tv:TreeView), dirLi) = s
                       match (en.FullName.EndsWith(dirSep)) with
                       | true -> 
                           printfn "testArchive: //create dirNode %A\n" en.FullName 
#if !tbdb
                           let dirLi = સપ્લીટ en.FullName dirSep
                           lifo (fun st (dEn:string) -> 
                                match (((dEn).Trim()).Length > 0 ) with
                                | true -> 
                                  match  (tv.Nodes.ContainsKey dEn) with
                                  | true -> 
                                        printfn "TV already contains dir node: %A" dEn
                                        ()
                                  | _ ->
                                        printfn "TV doesn't contain dir node, adding: %A" dEn
                                        let newDirNd = new Node(dEn, dEn, 0)
                                        st.Add(newDirNd)
                                        //get parnt nd, add children
                                        //let parNdId = tv.GetNode(dirLi.[i - 1])
                                        //let parNd = tv.Nodes.Find(parNdId, true)
                                        //parNd.Nodes.Add(dEn, dEn, 0)
                                        newDirNd.Nodes
                                | _ -> ()) tv.Nodes dirLi
                           |> ignore
                           //dirNode.ImageIndex <- 0 //flderIco
#endif
                           (tv, dirLi)
                       | _ -> 
                           printfn "testArchive: //create filNode %A" en.FullName
#if tbdb
                           let dirLi = સપ્લીટ 
 en.FullName dirSep |> clearBlanks
                           let parId = 
                            lifo (fun s v -> s + dirSep + v) "" (dirLi.[(dirLi.Length) - 1])
                           let parNd = tv.Nodes.Find(parId, true)
                           parNd.Nodes.Add(dirLi, dEn, 0)
                                | _ -> ()) dirLi
#endif
                           (tv, dirLi)
                   ) (new TreeView(),[]) |> ignore
(*
    let addTreeVwZip =
      fun (frm:Form) ->
          let rec createNode =
              fun zEnt dirLi -> 
                  let path = Path.GetFullPath(zEnt.FullName)

                  dirNode
          let treeVw = new TreeView()
          treeVw.Nodes.Clear()
          let zipArc = new ZipArchive(memStream, ZipArchiveMode.Update)
          zipArc.Entries |> Seq.cast |> List.ofSeq |> lim (fun en -> createNode(en []))
          let rootDirInf = new DirectoryInfo(arcPath)
          treeVw.Nodes.Add(createDirNode(rootDirectoryInf))
*)
#endif

    let bookmarkPainter (rtb:RichTextBox) = 
        new PaintEventHandler(fun o (e:PaintEventArgs) -> 
            let bkmarksLi = [2;4;6;8]
            let firstIndex = rtb.GetCharIndexFromPosition(new Point(0, 0))
            let firstLine = rtb.GetLineFromCharIndex(firstIndex) + 1
            let lastIndex = rtb.GetCharIndexFromPosition(new Point( (rtb.ClientRectangle).Width, (rtb.ClientRectangle).Height))
            let lastLine = rtb.GetLineFromCharIndex(lastIndex)
            bkmarksLi |> List.filter (fun lineN -> (lineN < lastLine + 1) && (lineN > firstLine - 1)) 
                |> lim (fun markable ->
                    //Retrieves the index of the first character of a given line
                    let charIdx = rtb.GetFirstCharIndexFromLine markable
                    //Retrieves the location within the control at the specified character index.
                    let loc = rtb.GetPositionFromCharIndex charIdx
                    //(i)Add staticRef to icn in ui.dll (ii)nd to move r
                    //e.Graphics.DrawIcon(new Icon(SystemIcons.Information, 40, 40), 0, 0)) |> ignore
                    let exp = new Icon("E:\\tmp\\expand.ico", new Size(10, 10))
                    let col = new Icon("E:\\tmp\\collapse.ico", new Size(10, 10))
                    e.Graphics.DrawIcon(exp, 100, 100) ) |> ignore)

#if mbiWithPaint
    let ext() = 
        let f = (new Form(WindowState = FormWindowState.Maximized, Visible = false, Text = "winFrms Test Form: Copyright (c) M. P. Trivedi 2016-2023.  All rights reserved.", TopMost=true, Font=defFont))
        f.Width <- 800
        f.Height <- 400
        let spl = new SplitContainer( Anchor = (AnchorStyles.Top ||| AnchorStyles.Left), Dock = DockStyle.Fill, IsSplitterFixed = true, SplitterDistance = 100, SplitterWidth = 1, TabIndex = 2 , FixedPanel = FixedPanel.Panel1, Name = "spl")
        spl.SuspendLayout()
        let rtb = new tyRtb(ScrollBars = RichTextBoxScrollBars.ForcedBoth, RightMargin = Int32.MaxValue, Anchor = (AnchorStyles.Top ||| AnchorStyles.Left), BorderStyle = BorderStyle.None, Dock = DockStyle.Fill,TabIndex = 0, Font = defFont, AcceptsTab = true, Text = "Text", Name = "rtb" )
        rtb.ContentsResized.Add(fun e -> rtb.ZoomFactor = 0.0f |> ignore)
        rtb.LinkClicked.Add(fun e -> tibbie ("click recd on txt: " + e.LinkText))
        let lineNo = new TextBox(Dock = doc "F", Multiline = true, Text = "1", Font = defFont, TextAlign = HorizontalAlignment.Right, Enabled=false)
        //lineNo.Paint.AddHandler(bookmarkPainter rtb)
        rtb.Text <- """
let rtb = new tyRtb(ScrollBars = RichTextBoxScrollBars.ForcedBoth, RightMargin = Int32.MaxValue, Anchor = (AnchorStyles.Top ||| AnchorStyles.Left), BorderStyle = BorderStyle.None, Dock = DockStyle.Fill,TabIndex = 0, Font = defFont, AcceptsTab = true, Text = "Text", Name = "rtb" )
        rtb.ContentsResized.Add(fun e -> rtb.ZoomFactor = 0.0f |> ignore)
        rtb.LinkClicked.Add(fun e -> tibbie ("click recd on txt: " + e.LinkText))
        let lineNo = new TextBox(Dock = doc "F", Multiline = true, Text = "1", Font = defFont, TextAlign = HorizontalAlignment.Right, Enabled=false)
        //lineNo.Paint.AddHandler(bookmarkPainter rtb)
"""
        rtb.Paint.Add(fun (e:PaintEventArgs) ->
            //(rtb :> RichTextBox).Paint() //base.WndProc(ref m)
            (rtb :> RichTextBox).WndProc(WM_PAINT)
            let exp = new Icon("E:\\tmp\\expand.ico", new Size(10, 10))
            let col = new Icon("E:\\tmp\\collapse.ico", new Size(10, 10))
            let rectF = e.Graphics.ClipBounds
            let charIdx = rtb.GetCharIndexFromPosition(new Point(int rectF.X, int rectF.Y))
            let thisLn = rtb.GetLineFromCharIndex(charIdx)
            if thisLn = 5 then
               printfn "line # is 5"
               e.Graphics.DrawIcon(exp, 10, 10)
               e.Graphics.DrawIcon(col, 200, 10)
            else printfn "line # %A" thisLn)
        let charIdx = rtb.GetCharIndexFromPosition(new Point(0,0))
        let topLn = rtb.GetLineFromCharIndex(charIdx)
        lineNo.Text <- List.fold(fun s l -> s + l.ToString() + "\r\n") "" [topLn .. topLn + 40]
        spl.Panel1.Controls.Add(lineNo)
        spl.Panel2.Controls.Add(rtb)
        spl.ResumeLayout()
        f.Controls.Add(spl)
        f
#endif //mbiWithPaint

#endif //modExt

    let getListBxHUpDn = 
        fun (lB:ListBox) opt ->
            //Qn: alter only selTxt or move ob around here??
            new EventHandler(fun o e -> 
                    printfn "tibbie")
#if tbfo
                    lV.SelectedIndices |> Seq.cast |> List.ofSeq 
                        |> lim (fun idx -> 
                               if idx = 0 && opt = "remove" then
                                    lB.BeginUpdate()
                                    let selItm = lV.Items.[idx]
                                    lB.Items.RemoveAt(idx)
                                    lB.EndUpdate()
                                  elif opt = "moveDn" && idx = (lB.Items.Count-1) then ()
                                  else
                                       match idx > 0 with
                                       | true -> 
                                           lB.BeginUpdate()
                                           let selItm = lB.Items.[idx]
                                           lB.Items.RemoveAt(idx)
                                           match opt with
                                           | "moveUp" -> lB.Items.Insert(idx - 1, selItm) |> ignore
                                           | "moveDn" -> lB.Items.Insert(idx + 1, selItm) |> ignore
                                           | _ -> ()
                                           lV.EndUpdate()
                                       | _ -> ()) |> ignore
#endif


    let bldPSlideTester =
        fun bef aft li f ->
            let fldFP = new TableLayoutPanel(Margin = new Padding(25), RowCount = 1, ColumnCount = 2, Dock = doc "F", Name="TablePanel", ForeColor = Color.Black, BackColor = Color.White)
            let FldMidP = new TableLayoutPanel(RowCount = 2, ColumnCount = 1, Dock = doc "F", AutoScroll = true)
            let btnBarFP = new FlowLayoutPanel(FlowDirection = FlowDirection.LeftToRight, Anchor = anc "N", AutoSize = true, BackColor = Color.White)
            let btnBarP = new TableLayoutPanel(Dock = doc "T", Width = f.Width)
            let btnFP = new FlowLayoutPanel(FlowDirection = FlowDirection.TopDown, Anchor = anc "N", AutoSize = true)
            let lB = new ListBox(Dock = doc "F", ForeColor = Color.Black, BackColor = Color.White)
            //@mbi !!^ ["lV", box lV] f
            fldFP.SuspendLayout()
            lB.SuspendLayout()
            lB.Items.Add(ComboBox(Name = "BeginBox", DataSource = ([bef] |> Array.ofList), DropDownStyle = ComboBoxStyle.DropDownList))
            let midChoices = li |> Array.ofList
            lim (fun n -> lB.Items.Add(ComboBox(Name = "MidBox" + n.ToString(), DataSource = midChoices, DropDownStyle = ComboBoxStyle.DropDownList))) [0..2]
            lB.Items.Add(ComboBox(Name = "EndBox", DataSource = ([aft] |> Array.ofList), DropDownStyle = ComboBoxStyle.DropDownList))
            //Qn: alter only selTxt or move ob around here??
            let moveUpButton = Button(Text = "^") //getImgBtn "Move &up" "arrow_upward.png" w
            moveUpButton.Click.AddHandler(getListBxHUpDn lB "moveUp")
            let moveDnButton = Button(Text = "v") //getImgBtn "Move &down" "arrow_downward.png" w
            moveDnButton.Click.AddHandler(getListBxHUpDn lB "moveDn")
            [moveUpButton; moveDnButton]
            |> lim (fun (b:Button) -> b.BackColor <- Color.White
                                      b :> Control)
            |> Array.ofList |> btnBarFP.Controls.AddRange
            fldFP.Controls.Add(btnBarP, 0, 0)
            fldFP.Controls.Add(lB, 0, 1)
            fldFP.ResumeLayout(false)
            lV.ResumeLayout(false)
            FldMidP.Controls.Add(btnBarFP, 0, 0)
            FldMidP.Controls.Add(fldFP, 0, 1)
            FldMidP.SetColumnSpan(fldFP, 3)
            btnBarP.Controls.Add(btnBarFP)
            let okButton = new Button(AutoSize = true, DialogResult = DialogResult.OK, Text = "&OK")
            let cancelButton = new Button(AutoSize = true, DialogResult = DialogResult.Cancel, Text = "&Cancel")
            btnFP.Controls.Add(okButton)
            btnFP.Controls.Add(cancelButton)
            f.Controls.Add(fldFP)
            f.Controls.Add(btnFP)
            f

    let multiLineTester = ["This is choice # 0";"This is choice # 1";"This is choice # 2
 which has two lines";"This is choice # 3";"This is choice # 4";"This is choice # 5
 which has two lines";"This is choice # 6";"This is choice # 7";"This is choice # 8";"This is choice # 9
 which has two lines";"This is choice # 10";"This is choice # 11";"This is choice # 12";"This is choice # 13
 which has two lines";"This is choice # 14";"This is choice # 15
 which has two lines";"This is choice # 16";"This is choice # 17";"This is choice # 18
 which has two lines";"This is choice # 19";"This is choice # 20";"This is choice # 21";"This is choice # 22
 which has two lines";"This is choice # 23";"This is choice # 24";"This is choice # 25
 which has two lines";"This is choice # 26";"This is choice # 27
 which has two lines";"This is choice # 28";"This is choice # 29"]

    let getPB = 
        fun nm -> 
           let fullNm = nm + ".png"
           let img = (Image) (Bitmap(Paths.Combine("E:\\src\\Data\\images\\google\\", nm)))
           new PictureBox(Name = nm, Image = img, CanSelect = true, SizeMode = PictureBoxSizeMode.StretchImage)

    let multiImgTester = lim (fun nm -> getPB nm) ["folder";"info";"insert";"mouse";"rule";"save";"space_Bar";"swap_horiz"]

    let ext() = 
        let f = (new Form(WindowState = FormWindowState.Maximized, Visible = false, Text = "winFrms Test Form: Copyright (c) M. P. Trivedi 2016-2023.  All rights reserved.", TopMost=true, Font=defFont))
        f.Width <- 800
        f.Height <- 400
        bldPSlideTester "Begin" "End" multiLineTester f
        bldPSlideTester (getPB "add_box") (getPB "add_circle") multiImgTester f

#endif //ModuleExt_RemmedForMonkeyBastas_Jul12_2023

module Cambattable =
    open System
    open System.Drawing
    open System.IO
    open System.IO.Compression
    open System.Text
    open System.Windows.Forms
    open System.Text.RegularExpressions
    open System.Diagnostics
    open Trivedi
    open Trivedi.Core
    open Trivedi.UI

    printfn "in mod winFrms.Cambattable..."

(*
Aug 8 work ->
type mWith = interface end
type mWithout = interface end

type tyA() = interface mWith
type tyB() = interface mWithout

type TstOne<'t when 't :> mWith> = 
            | Tsti of int * 't
            | Tsts of string * 't
            interface mWith

type TstTwo<'t when 't :> mWithout> = 
            | Tstj of int * 't
            | Tstk of string * 't
            interface mWithout

let t1 = (Tsti(1, tyA()))   //t1 is TstOne<tyA>
let t2 = (Tstj(1, tyB()))  //t2 is TstTwo<tyB>

*)

    type Bfty = | Bfty of id:string * fty:DocFldType * valu:obj with
        override this.ToString() = 
          let (Bfty(nm, t, v)) = this
          match t with
          | DFldString -> "Bfty(" + nm + ", " + "string)"
          | DFldCurrency -> "Bfty(" + nm + ", $" + v.ToString() + ")"
          | _ -> "Bfty(" + nm + ", " + "unknown)"
        member this.toPer() = 
            "test"
        member this.fromPer() = 
          "test"
    
    let x = Bfty("test1", DFldCurrency, box 2.22)
    printfn "toString: x:%A x.ToS:%A" x (x.ToString())
//toString: x:Bfty ("test1", DFldCurrency, 2.22) x.ToS:"Bfty(test1, $2.22)"

    type Btpl = | Btpl of l:list<Bfty> with

type Mtpl = | Mtpl of l:list<(string * obj * System.Type)> with
    override this.ToString() = 
        "tibbie"
    static member empty() = Mtpl([])
    static member AddOne s o (m:Mtpl) =
                        match s = "Dat" with
                        | true -> 
                            let (Mtpl(l:list<(string * obj * System.Type)>)) = m
                            let fnd = List.tryFindIndex (fun (x,_,_) -> x = s) l
                            match fnd.IsSome with
                            | true -> Mtpl(liUpdAt (fnd.Value) (s, (toOb o), (o.GetType())) l)
                            | _ -> Mtpl((s, (toOb o), (o.GetType())) :: l)
                        | _ -> 
                            let (Mtpl(l:list<(string * obj * System.Type)>)) = m
                            let fnd = List.tryFindIndex (fun (x,_,_) -> x = s) l
                            match fnd.IsSome with
                            | true -> Mtpl(liUpdAt (fnd.Value) (s, (toOb o), (o.GetType())) l)
                            | _ -> Mtpl((s, (toOb o), (o.GetType())) :: l)
    static member AddLi li (m:Mtpl) =
                    let (Mtpl(l:list<(string * obj * System.Type)>)) = m
                    List.fold (fun s o -> 
                                let (str, ob) = o
                                Mtpl.AddOne str ob s) m li
    static member RemLi li (m:Mtpl) =
                    let (Mtpl(l)) = m
                    let remSingle tg li = 
                        let fnd = List.tryFindIndex (fun (x,_,_) -> x = tg) li
                        match fnd.IsSome with
                        | true -> liRemAt (fnd.Value) li
                        | _ -> li
                    Mtpl(List.fold (fun s remTag -> remSingle remTag l) [] li)
    static member GetOne tg (m:Mtpl) =
                    //printfn "==================================getOne 1"
                    let (Mtpl(l:list<(string * obj * System.Type)>)) = m
                    //printfn "==================================getOne 2"
                    let fnd = List.tryFindIndex (fun itm -> let (x,_,_) = itm
                                                            x = tg) l
                    //printfn "==================================getOne 3"
                    match fnd with
                    | Some idx -> 
                        //printfn "==================================getOne 4"
                        let (tg, ob, ty) = l.[idx]
                        //printfn "==================================getOne 5"
                        //run typeChk here to ensure correct expected ty
                        //printfn "==================================GetOne: found idx:%A tag:%A ty:%A inTy:%A" (idx.ToString()) (tg) (ob.GetType()) (ty.ToString())
                        Some(unbox ob)
                    | _ -> None
    static member Has tg (m:Mtpl) =
                    let (Mtpl(l:list<(string * obj * System.Type)>)) = m
                    let fnd = List.tryFindIndex (fun itm -> let (x,_,_) = itm
                                                            x = tg) l
                    match fnd with
                    | Some idx -> true
                    | _ -> false
    static member getUNID (m:Mtpl) =
                    let (Mtpl(l:list<(string * obj * System.Type)>)) = m
                    let fnd = List.tryFindIndex (fun itm -> let (x,_,_) = itm
                                                            x = "env_Tick") l
                    match fnd with
                    | Some idx -> 
                        let (tg, ob, ty) = l.[idx]
                        let newTick = (ob :?> int64) + (1L)
                        ((Mtpl.AddOne "env_Tick" (box newTick) m), newTick)
                    | _ -> 
                        let newTick = idTicks()
                        Mtpl.AddOne "env_Tick" (box newTick) m
                        ((Mtpl.AddOne "env_Tick" (box newTick) m), newTick)


type BrijTy<'t when 't :> ITblMarker> = | BrijTy of mods:Mod list * m:Mtpl * string * tblTy:'t with
    override this.ToString() = 
            let (BrijTy(mds, tpl, s, tblTy)) = this
            let (CoreMod(CoreM(DocUNID(unid), crDt, modDt, tit, cont, tags, flag))) = mds.[0]
            let tblT = (genArg this).ToString()
            "BrijTy of " + tblT + "|id:" + unid + "|title:" + tit + "|"
    member this.zipWithID() = 
            let (BrijTy(mds, tpl, s, tblTy)) = this
            let (CoreMod(CoreM(DocUNID(unid), _, _, _, _, _, _))) = mds.[0]
            (unid, mds)

#if ref
    //the Following members exist in BrijTy<'t>
    static member bld (id:string) (tit:string option) (cont:string option) (tg:string option) = 
    static member bldSpoo (id:string) (crDt:DateTime )(tit:string option) (cont:string option) (tg:string option) = 
    member this.contAsS() = 
    member this.contAsB() =
    member this.ToShortStr() =
    member this.getFldDefs() =
    member this.upd(m:Mtpl) =
#endif



#if ModuleDeck_RemmedForMonkeyBastas_Jul18_2023
module deck =
    open System
    open System.IO
    open System.Diagnostics
    open System.Windows.Forms
    open FSharp.Reflection //for dutype
    open Trivedi.Core
    open Trivedi.UI
    //open GridTester
    //open Ide
    //open Ext

    printfn "...init module deck..."

#if !deck
    type Rank = | Two
                | Three
                | Four
                | Five
                | Six
                | Seven
                | Eight
                | Nine
                | Ten
                | Jack 
                | Queen 
                | King 
                | Ace with
        static member fromStr =
            function | "Two" -> Two 
                     | "Three" -> Three
                     | "Four" -> Four
                     | "Five" -> Five
                     | "Six" -> Six
                     | "Seven" -> Seven
                     | "Eight" -> Eight
                     | "Nine" -> Nine
                     | "Ten" -> Ten
                     | "Jack" -> Jack
                     | "Queen" -> Queen
                     | "King" -> King
                     | "Ace" -> Ace
                     | _ -> raise (Trivedi_Core_ex "cards: Invalid Rank supplied to r.fromStr...")

    type Suit = | Spades | Hearts | Diamonds | Clubs with
        static member fromStr =
            function | "Clubs" -> Clubs
                     | "Diamonds" -> Diamonds
                     | "Hearts" -> Hearts
                     | "Spades" -> Spades
                     | _ -> raise (Trivedi_Core_ex "cards: Invalid Suit supplied to s.fromStr...")

    type Card = | Card of Suit * Rank with
        member c.rnkG() =
            let (Card(s, r)) = c
            match r with
                  | Two -> "2"
                  | Three -> "3"
                  | Four -> "4"
                  | Five -> "5"
                  | Six -> "6"
                  | Seven -> "7"
                  | Eight -> "8"
                  | Nine -> "9"
                  | Ten -> "10"
                  | Jack  -> "jack"
                  | Queen  -> "queen"
                  | King  -> "king"
                  | Ace -> "ace"
        member c.intVal() =
            let (Card(s, r)) = c
            match r with
                  | Two -> 2
                  | Three -> 3
                  | Four -> 4
                  | Five -> 5
                  | Six -> 6
                  | Seven -> 7
                  | Eight -> 8
                  | Nine -> 9
                  | Ten -> 10
                  | Jack  -> 11
                  | Queen  -> 12
                  | King  -> 13
                  | Ace -> 14
        member c.suitG() =
            let (Card(s, r)) = c
            match s with
                   | Spades -> "spades"
                   | Hearts -> "hearts"
                   | Diamonds -> "diamonds"
                   | Clubs -> "clubs"
        member c.suitG_unicode() =
            let (Card(s, r)) = c
            match s with
                   | Spades -> "♠"
                   | Hearts -> "♥"
                   | Diamonds -> "♦"
                   | Clubs -> "♣"
        member c.isFace() =
            let (Card(s, r)) = c
            match r with
            | Jack | Queen | King  -> true
            | _ -> false
        member c.isOneEyed() =
            let (Card(s, r)) = c
            match c.isFace() with
            | true ->
                match (r, c.suitG_unicode()) with
                | (J,♠) | (J,♥) | (K,♦) -> true
                | _ -> false
            | _ -> false
        override c.ToString() =
           let (Card(s, r)) = c
           //printfn $"{rnkG} of {suitG}" //suitG_unicode()
           "err: This token is reserved for future use"

#if tbdb
    type CardHand = | CardHand of Card list with
        override this.ToString() = 
           this |> lifo (fun s cd -> 
                             match (len s) with
                             | 0 -> cd.ToString()
                             | _ -> " " + cd.ToString()) ""
        member this.getRndHand(n) =
           let rRnk = (typeof<Rank> |> FSharpType.GetUnionCases |> List.ofArray).[getRnum 0 13]
           let rSt = (typeof<Suit> |> FSharpType.GetUnionCases |> List.ofArray).[getRnum 0 3]
           [0..n] |> List.collect(fun x -> [Card((Suit.fromStr rSt), (Rank.fromStr rRnk))]) |> liShuffle (getRand()) |> CardHand
        member this.getRndHand(n s) =
           let rRnk = (typeof<Rank> |> FSharpType.GetUnionCases |> List.ofArray).[getRnum 0 13]
           let fst = Card(rRnk, s)
           [fst] @ c.getRndHand(n - 1) |> liShuffle (getRand()) |> CardHand
        member this.getRndHand(n r) =
           let rSt = (typeof<Suit> |> FSharpType.GetUnionCases |> List.ofArray).[getRnum 0 3]
           let fst = Card(r, rSt)
           [fst] @ c.getRndHand(n - 1) |> liShuffle (getRand()) |> CardHand
        member this.runTestsWithPred(n prd) =
           //if perf necc l8r port to seq
           (List.filter prd (getRndHand(n)) |> lilen |> string) + " out of " + n.ToString() + " hands passed the test"
        member this.hasFaceCards() = List.exists (fun c -> c.isFace()) this
        member this.hasOneEyedCards() = List.exists (fun c -> c.isOneEyed()) this
        member this.chkSelBySuit(s, selL) =
           //nds suitFromBase
           let allowd = List.filter (fun c -> c.suitG() = s) this
           match not (len selL > len allowd) with
           | true -> Set.isSubset Set.ofList selL  Set.ofList allowd
           | _ -> true

        member c.toImgNmG() = 
           let (Card(s, r)) = c
           //c.rnkG() + "_of_" + c.suitG() + ".png"
           printfn "%A of %A.png" c.rnkG c.suitG
           //rtb.Text <- rtb.Text + (c.rnkG() + " of " + c.suitG() + ".png\n")


    let getBDFTpl c =
                        let file = Path.Combine(@"E:\tmp\PNG-cards-1.3\", c.toImgNmG())
                        let localEx = if File.Exists(file) then true else false
                        match localEx with
                        | true -> 
                            let localBA = File.ReadAllBytes(file) |> b64EncB_SingleLine
                            printfn "getBDFTpl Success: Card img exists for Card: %A" (file)
                            Some(c.rnkG(), c.suitG(), localBA)
                        | _ -> 
                            printfn "getBDFTpl ERR: Card img does not exist for Card: %A" (file)
                            None

    let bldOut rtb =
      let flatLocal l = l
      let resDat =
        typeof<Rank> |> FSharpType.GetUnionCases |> List.ofArray 
         |> lim (fun rInf -> 
                     let r = Rank.fromStr(rInf.Name)
                     typeof<Suit> 
                     |> FSharpType.GetUnionCases 
                     |> List.ofArray 
                     |> lim (fun stInf -> 
                               let st = stInf.Name
                               let crd = Card(Suit.fromStr st, r)
                               getBDFTpl crd )) |> List.concat |> flatLocal
      printfn "................postProc..."
      //File.WriteAllBytes("E:\src\Brij\mongo\Cards.bdf", (serBA resDat))


    let cardExt() = 
        let f = (new Form(WindowState = FormWindowState.Maximized, Visible = false, Text = "winFrms Test Form: Copyright (c) M. P. Trivedi 2016-2023.  All rights reserved.", TopMost=true, Font=defFont))
        f.Width <- 800
        f.Height <- 400
        let rtb = new RichTextBox(ScrollBars = RichTextBoxScrollBars.ForcedBoth, RightMargin = Int32.MaxValue, Anchor = (AnchorStyles.Top ||| AnchorStyles.Left), BorderStyle = BorderStyle.None, Dock = DockStyle.Fill,TabIndex = 0, Font = defFont, AcceptsTab = true, Text = "Text", Name = "rtb" )
        f.Controls.Add(rtb)
        bldOut rtb
        f
#endif //tbdb

#if tbfo
    let hardCodedBridgeSOrdSupplier(h) =
        //chkBridgeSOrd h
        true
    let hardCodedFCrdSupplier(h) =
         //One-eyed: J♠, J♥, K♦
        //chkFCrd h
        true
    let hardCodedFiveBaseSupplier(h) = 
        //getCurrBase() |> chkBase h
        true
    let hardCodedStrat4Supplier(h) =
        //chkStrat4 h
        true
    //mnemonic: "Bridge Co_nt_ract - WA_SH_INGTON _D.C._"
    type G_Strategy = | BridgeSOrd = 1
                      | FCrd = 2
                      | FiveBase = 3
                      | Strat4 with = 4

    let G_Strategy_From_Int(i:int):G_Strategy = enum(i)

    let genDeckTpl() =
        let h =
              match (getRnum 1 11) > 5 with
              | true -> CardHand.getRndHand(5)
              | _ -> CardHand.getRndHand(3)
        let strat = G_Strategy_From_Int(getRnum 1 4)
            match strat with
            | BridgeSOrd -> (if hardCodedBridgeSOrdSupplier(h)) then true else false
            | FCrd -> (if hardCodedFCrdSupplier(h)) then true else false
            | FiveBase -> (if hardCodedFiveBaseSupplier(h)) then true else false
            | Strat4 -> (if hardCodedStrat4Supplier(h)) then true else false
            | _ -> false
        (strat, h)

    let cardExt() = 
        tibbie "launching rpnl..."
        RPnl()
        tibbie "after launching rpnl..."
#endif //tbfo

#endif //deck

#endif //ModuleDeck_RemmedForMonkeyBastas_Jul18_2023

#if !modPerms
module perms =
    open System
    open System.Diagnostics
    //open System.Diagnostics.PerformanceCounter
    //open System.Windows.Forms
    open Trivedi.Core

    printfn "...init module perms..."
    System.Console.ReadKey(true) |> ignore

#if Notes
    PermsBase returned a result where res/20 = 2.3M
    we nd to optimize for (Vlen + Plen)

        ///UsrCycleInitCounter
        type UCIC = | UCIC of int with
            member this.getNext() = usrCfg.UCIC <- Rnd(0, (vGenLi.Len - 1))
              
        ///UsrCurrCounter
        type UCC = | UCC of int with
            member this.getNext(UCIC) = 
                match (UCC < UCIC - 50) with
                | true -> sendSystemMsg(Level.Critical, "User " + usrCfg.UserNm + " about to reach vCycle completion!")
                | _ -> ()
                match (not (UCIC < (vGenLi.len - 1)))
                | true -> usrCfg.UCC <- 0; ret vGenLi.head
                | _ -> usrCfg.UCC <- =+ 1; ret vGenLi.[usrCfg.UCC]

    Formula 2 manually calc perms ->
        https://www.mathsisfun.com/combinatorics/combinations-permutations.html
        n! / (n − r)!
        where n is the number of things to choose from,
            and we choose r of them,
            no repetitions, order matters.

#endif //Notes


#if failsOnMono
    let memStat() =
        //see: https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.process.privatememorysize64?view=net-7.0
        use proc = Process.GetCurrentProcess()
        (proc.PrivateMemorySize64 / ((1024*1024) |> int64) |> string) + "\n" + 
        "CPU usage: " + (PerformanceCounter("Processor", "% Processor Time", "_Total").NextValue()) + "%\n" + 
        "RAM usage: " + (PerformanceCounter("Memory", "Available MBytes").NextValue()) + " MB"
#endif //failsOnMono

    type PermCfg = | PermCfg of p:int * v:int with
        member this.pLen() = 
            let (PermCfg(p, v)) = this
            p
        member this.vLen() = 
            let (PermCfg(p, v)) = this
            v

    let BaseCfg = PermCfg(30,60)           //1,171,800
    let Reduced_vLen_Cfg = PermCfg(30,30)  //585,900
    let Reduced_pLen_Cfg = PermCfg(15,60)  //502,200
    let Optimized_Cfg = PermCfg(20,60)     //725,400
    let min_Cfg = PermCfg(15,30)           //243 k <- produces 4,860 k tot 
    let chosenCfg = min_Cfg

    let flattenOb = fun (o:option<_>) -> o.Value
    let flatLocal = fun (li:option<_> list) -> li |> List.filter (fun (x:option<_>) -> x.IsSome) |> List.map (fun x -> flattenOb x)
    let getPermutations li =
        //see TomP @ https://stackoverflow.com/questions/286427/calculating-permutations-in-f
        let rec perms inli takn = 
            seq { if Set.count takn = List.length inli then yield [] else
                    for l in inli do
                    if not (Set.contains l takn) then 
                        for perm in perms inli (Set.add l takn)  do
                        yield l::perm }
        perms li Set.empty

    let liT = [1;2;3]
    let eaTy = ["Cd";"P*";"Tr";"V1";"V2"]
    let P = lim (fun el -> "P_" + (string el)) [0..(chosenCfg.pLen() - 1)]
    let Pr = lim (fun el -> "Pr_" + (string el)) [0..(chosenCfg.pLen() - 1)]
    let Pi = lim (fun el -> "Pi_" + (string el)) [0..(chosenCfg.pLen() - 1)]
    let Pri = lim (fun el -> "Pri_" + (string el)) [0..(chosenCfg.pLen() - 1)]

    let r1 = (List.windowed 5 P)

    //len:26
    //printfn "r1:%A len: %A" r1 (r1.Length)

    let cyclicalOrder =
        fun (l:list<_>) ->
            let cyc = [0;5;10;15;20;25]
            List.map (fun x ->
                List.map(fun cy -> 
                        match ((x + cy) < l.Length) with
                        | true -> Some(l.[x + cy])
                        | _ -> None
                        ) cyc) [0..4]
          //|> @ this pt we nd to randomize `in situ`, so:  
          //   unzip >> shuffle >> zip >> carry on...
            |> List.concat |> flatLocal

    let getWins() =
        ((List.windowed 5 P) |> cyclicalOrder) @ ((List.windowed 5 Pr) |> cyclicalOrder)
        @ ((List.windowed 5 Pi) |> cyclicalOrder) @ ((List.windowed 5 Pri) |> cyclicalOrder)

    printfn "getWins() res:%A len:%A" (getWins()) ((getWins()).Length)
    //len:104 min_Cfg: 44
    hr()
    printfn "cyclicalOrder res:%A len:%A" (cyclicalOrder r1) ((cyclicalOrder r1).Length)
    //len:26 min_Cfg: 11
    hr()

    let procAndReplEl elm li replLi =
        lim (fun eaL ->
                match List.contains elm eaL with
                | true -> 
                    let idx = (List.tryFindIndex (fun el -> el = elm) eaL).Value
                    lim (fun repl -> List.updateAt idx repl eaL) (replLi)
                | _ -> [eaL] ) li


    //printfn "below the windowed are represented by W0..W104" //all poss win combos
    let wins() = lifo (fun s x -> s @ ["W" + x.ToString()]) [] [0..44]
        //res_Ps_Replaced_With_All_Variables
    let res2 = procAndReplEl "P*" ((getPermutations eaTy) |> List.ofSeq) (wins())
    let AllPossV1s() = lifo (fun s x -> s @ ["V_1" + x.ToString()]) [] [0..(chosenCfg.vLen() - 1)]
    let res3 = procAndReplEl "V1" (res2 |> List.concat) (AllPossV1s()) |> List.concat
    printfn "Replaced_V1s_#3: %A len.0:%A len:%A" (res3.[0]) ((res3.[0]).Length) (res3.Length)
    //For min_Cfg: ["Cd"; "W0"; "Tr"; "V_10"; "V2"] len.0:5 len:162000
    hr()

    let res4Alt (inL:list<_>) =
       printfn "...in res4Alt; fed li.0:%A len: %A uniqueLen:%A" (inL.[0])(inL.Length) ((inL |> Seq.distinct |> List.ofSeq).Length)
       //minCfg: 8100
       let innrRes:list<_> = 
        inL |>
          limi (fun i eaL ->
                  match List.contains "V2" eaL with
                  | true -> 
                    let idx = (List.tryFindIndex (fun el -> el = "V2") eaL)
                    let res = 
                        lifo (fun s x -> s @ ["V_2" + x.ToString()]) [] [0..29]
                        |> lim (fun repl -> eaL.[0..3] @ [repl])
                    if i = 0 then 
                      printfn "for i=0 res of fold/mapi is:%A" res
                    match (i % 10000 = 0) with
                    | true ->  printfn "i:%A resL:%A" i (lilen res)
                    | _ -> ()
                    res
                  | _ -> [eaL] ) |> List.concat
       //printfn "Replaced_V1s_#4.0: %A len:%A uniqueLen:%A" innrRes.[0] (innrRes.Length) ((innrRes |> Seq.distinct |> List.ofSeq).Length)
       //printfn "concLen: %A 1st:%A last:%A" (concL.Length) (concL.[0]) ((List.last concL))
       printfn "concLen: %A 1st:%A last:%A" (innrRes.Length) (innrRes.[0]) ((List.last innrRes))
       //for min_Cfg 
       //concLen: 243000 1st:["Cd"; "W0"; "Tr"; "V_10"; "V_20"] last:["Cd"; "W44"; "V2"; "V_129"; "V_229"]
       //serToFile
       hr()

    let run() = List.splitInto 20 res3 |> List.head |> res4Alt

    //below work done on Aug_14_23 for lazy ver to fetch only bckt
    
    let res4Alt2 start (inL:list<_>) =
        (inL |>
          limi (fun i eaL ->
                  match List.contains "V2" eaL with
                  | true -> 
                    let idx = (List.tryFindIndex (fun el -> el = "V2") eaL)
                    let res = 
                        lifo (fun s x -> s @ ["V_2" + x.ToString()]) [] [0..29]
                        |> lim (fun repl -> eaL.[0..3] @ [repl])
(*
                    if i = 0 then 
                      printfn "for i=0 res of fold/mapi is:%A" res
                    match (i % 10000 = 0) with
                    | true ->  printfn "i:%A resL:%A" i (lilen res)
                    | _ -> ()
*)
                    res
                  | _ -> [eaL] ) |> List.concat).[start..(start+11)]

    //slightly buggy but will do in a pinch (just a standin, anyway)
    let rec findWin num winSz optChk =
        match optChk with
        | Some o -> 
            if num > (winSz * o) then o 
             elif num =  (winSz * o) then o - 1
             else findWin num winSz (Some(o-1))
        | _ -> if num > (winSz * 19) then 19
                elif num = (winSz * 19) then 18
                else findWin num winSz (Some(18))

    let getBuck forNum =
        let w = findWin forNum 243000 None
        let start = forNum % 243000
        printfn "running alt 4 win:%A start:%A" w start
        res4Alt2 start (List.splitInto 20 res3).[w]

    let newSeed() = getRnum 0 4860000
    let runAlt42() =
    [0..5]
    |> limi (fun i ctr -> 
              let res = getBuck (newSeed() + (12*i))
              printfn "res %A for %A: %A" i newSeed res) |> ignore
(*
Output:
    running alt 4 win:6 start:185025
res 0 for <fun:Pipe #5 stage #1 at line 218@220-4>: 
[["W25"; "V_117"; "V2"; "Cd"; "V_215"]; ["W25"; "V_117"; "V2"; "Cd"; "V_216"];
 ["W25"; "V_117"; "V2"; "Cd"; "V_217"]; ["W25"; "V_117"; "V2"; "Cd"; "V_218"];
 ["W25"; "V_117"; "V2"; "Cd"; "V_219"]; ["W25"; "V_117"; "V2"; "Cd"; "V_220"];
 ["W25"; "V_117"; "V2"; "Cd"; "V_221"]; ["W25"; "V_117"; "V2"; "Cd"; "V_222"];
 ["W25"; "V_117"; "V2"; "Cd"; "V_223"]; ["W25"; "V_117"; "V2"; "Cd"; "V_224"];
 ["W25"; "V_117"; "V2"; "Cd"; "V_225"]; ["W25"; "V_117"; "V2"; "Cd"; "V_226"];
 ["W25"; "V_117"; "V2"; "Cd"; "V_227"]]
    running alt 4 win:6 start:185037
res 1 for <fun:Pipe #5 stage #1 at line 218@220-4>: 
[["W25"; "V_117"; "V2"; "Cd"; "V_227"]; ["W25"; "V_117"; "V2"; "Cd"; "V_228"];
 ["W25"; "V_117"; "V2"; "Cd"; "V_229"]; ["W25"; "V_118"; "V2"; "Cd"; "V_20"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_21"]; ["W25"; "V_118"; "V2"; "Cd"; "V_22"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_23"]; ["W25"; "V_118"; "V2"; "Cd"; "V_24"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_25"]; ["W25"; "V_118"; "V2"; "Cd"; "V_26"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_27"]; ["W25"; "V_118"; "V2"; "Cd"; "V_28"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_29"]]
    running alt 4 win:6 start:185049
res 2 for <fun:Pipe #5 stage #1 at line 218@220-4>: 
[["W25"; "V_118"; "V2"; "Cd"; "V_29"]; ["W25"; "V_118"; "V2"; "Cd"; "V_210"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_211"]; ["W25"; "V_118"; "V2"; "Cd"; "V_212"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_213"]; ["W25"; "V_118"; "V2"; "Cd"; "V_214"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_215"]; ["W25"; "V_118"; "V2"; "Cd"; "V_216"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_217"]; ["W25"; "V_118"; "V2"; "Cd"; "V_218"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_219"]; ["W25"; "V_118"; "V2"; "Cd"; "V_220"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_221"]]
    running alt 4 win:6 start:185061
res 3 for <fun:Pipe #5 stage #1 at line 218@220-4>: 
[["W25"; "V_118"; "V2"; "Cd"; "V_221"]; ["W25"; "V_118"; "V2"; "Cd"; "V_222"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_223"]; ["W25"; "V_118"; "V2"; "Cd"; "V_224"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_225"]; ["W25"; "V_118"; "V2"; "Cd"; "V_226"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_227"]; ["W25"; "V_118"; "V2"; "Cd"; "V_228"];
 ["W25"; "V_118"; "V2"; "Cd"; "V_229"]; ["W25"; "V_119"; "V2"; "Cd"; "V_20"];
 ["W25"; "V_119"; "V2"; "Cd"; "V_21"]; ["W25"; "V_119"; "V2"; "Cd"; "V_22"];
 ["W25"; "V_119"; "V2"; "Cd"; "V_23"]]
    running alt 4 win:6 start:185073
res 4 for <fun:Pipe #5 stage #1 at line 218@220-4>: 
[["W25"; "V_119"; "V2"; "Cd"; "V_23"]; ["W25"; "V_119"; "V2"; "Cd"; "V_24"];
 ["W25"; "V_119"; "V2"; "Cd"; "V_25"]; ["W25"; "V_119"; "V2"; "Cd"; "V_26"];
 ["W25"; "V_119"; "V2"; "Cd"; "V_27"]; ["W25"; "V_119"; "V2"; "Cd"; "V_28"];
 ["W25"; "V_119"; "V2"; "Cd"; "V_29"]; ["W25"; "V_119"; "V2"; "Cd"; "V_210"];
 ["W25"; "V_119"; "V2"; "Cd"; "V_211"]; ["W25"; "V_119"; "V2"; "Cd"; "V_212"];
 ["W25"; "V_119"; "V2"; "Cd"; "V_213"]; ["W25"; "V_119"; "V2"; "Cd"; "V_214"];
 ["W25"; "V_119"; "V2"; "Cd"; "V_215"]]
    running alt 4 win:6 start:185085
res 5 for <fun:Pipe #5 stage #1 at line 218@220-4>: 
[["W25"; "V_119"; "V2"; "Cd"; "V_215"]; ["W25"; "V_119"; "V2"; "Cd"; "V_216"];
 ["W25"; "V_119"; "V2"; "Cd"; "V_217"]; ["W25"; "V_119"; "V2"; "Cd"; "V_218"];
 ["W25"; "V_119"; "V2"; "Cd"; "V_219"]; ["W25"; "V_119"; "V2"; "Cd"; "V_220"];
 ["W25"; "V_119"; "V2"; "Cd"; "V_221"]; ["W25"; "V_119"; "V2"; "Cd"; "V_222"];
 ["W25"; "V_119"; "V2"; "Cd"; "V_223"]; ["W25"; "V_119"; "V2"; "Cd"; "V_224"];
 ["W25"; "V_119"; "V2"; "Cd"; "V_225"]; ["W25"; "V_119"; "V2"; "Cd"; "V_226"];
 ["W25"; "V_119"; "V2"; "Cd"; "V_227"]]
 *)

#endif //modPerms

#if Remmed_Aug14_23_mbi
module mockADO = 
    open System
    open System.IO
    open Trivedi.Core
    open System.Diagnostics
    
    printfn "in mod mockADO..."
    System.Console.ReadKey(true) |> ignore

    let getBucket i =
        async {
            try
              printfn "1"
              let buckNo = 
                match i < 251100 with
                | true -> 1
                | _ -> (i/251100) + 1
              //@hardCoded :fable workaround: last choice shd be throw "impossible err"
              printfn "2"
              //let buck = function | 1 -> l1 | 2 -> l2 | 3 -> l3 | 4 -> l4 | 5 -> l5 | _ -> l1
              printfn "3 for %A" i
              let path = buckNo.ToString() + ".out"
              printfn "4"
              let! bytes = File.ReadAllBytesAsync(path) |> Async.AwaitTask
              printfn "5"
              let tB = deSerBA (bytes) :?> list<list<string>>
              printfn "6"
              let rr = i % 251100 //remainder
              let tI = tB.[rr]
              printfn "7"
              return ( match ((rr + 12) > (List.length tB)) with
                        | true -> List.skip ((tB).Length - 12) (tB)
                        | _ -> 
                            //@hardCoded: in prod nd to open nxt (hehe, mgo)
                            lim (fun ctr -> tB.[ctr]) [rr..(rr+11)])
            with
                | ex -> 
                    do printfn "Exc in getBucket for %A -> msg:%A" i (ex.Message)
                    return []
            }

    let runWith =
        fun params ->
          let (stopW, l) = params
          stopW.Start()
          l 
          |> Seq.ofList
          |> Seq.map getBucket
          |> Async.Parallel
          |> Async.RunSynchronously
          |> List.ofSeq
          |> printfn "result of getBucket:\n%A" |> ignore

    let postProc =
      fun params ->
          let (stopW, l) = params
          stopW.Stop()
          let ts = stopW.Elapsed
          printfn $"getBucket took: {ts.Minutes} min: {ts.Seconds} secs: {ts.Milliseconds / 10} millis"

    hr()
    printfn "running getBucket..."
    //[4;94;64;45;80;88;22]
    //[251200;951100]
    મિંગ runWith ([251200;951100], (new Stopwatch())) postProc
#endif //Remmed_Aug14_23_mbi

module permsHarness = 
    open System
    open System.IO
    open System.Data
    open System.Diagnostics
    open Trivedi.Core
    open Trivedi.UI
    open Trivedi.Brij
    open FsCheck
    open FsCheck.FSharp

    printfn "in mod permsHarness..."
    System.Console.ReadKey(true) |> ignore
    
    let getStats() =
      [0;10;19] |>
      lim (fun n ->
            let thisFn = n.ToString() + ".out"
            let li = deSerBA (File.ReadAllBytes(thisFn)) :?> list<list<string>>
            printfn " ***  For n=%A, len:%A" n (li.Length))
      (* Output:
         ***  For n=0, len:251100
         ***  For n=10, len:251100
         ***  For n=19, len:251100
         min_Cfg; exact tot: 5,022,000
         Ea Test Case:
            needs a batch: say, dz
            [0..5,022,000] -> get batch -> run test
        *)

    let chkFsCheckSetup() =
      let revRevIsOrig (xs:list<int>) = List.rev(List.rev xs) = xs
  
      let revIsOrig (xs:list<int>) = List.rev xs = xs
  
      printfn "running FsChk on revRevIsOrig..."
      Check.Quick revRevIsOrig
      hr()
      printfn "running FsChk verbose on revRevIsOrig..."
      Check.Verbose revRevIsOrig
      hr()
      printfn "running FsChk on revIsOrig..."
      Check.Quick revIsOrig
      hr()
      printfn "running FsChk verbose on revIsOrig..."
      Check.Verbose revIsOrig
      hr()

    let genConst() = 
      Gen.constant (1, "Foo") |> Gen.sampleWithSize 0 10 |> printfn "gen.const: %A"
      //gen.const: [|(1, "Foo"); (1, "Foo"); (1, "Foo"); (1, "Foo"); (1, "Foo"); (1, "Foo");
                    //(1, "Foo"); (1, "Foo"); (1, "Foo"); (1, "Foo")|]

    printfn "module arbs..."

    type Slotd = |Slotd of Cd:string * P:string * Tr:string * Vi:string * Vii:string
    //let eaTy = ["Cd";"P*";"Tr";"V1";"V2"]
    //["Cd"; "W0"; "Tr"; "V_10"; "V_230"]
    
    //let myprop (i:int) = i >= 0
    let mygen = ArbMap.defaults |> ArbMap.arbitrary<int> |> Arb.mapFilter (fun i -> Math.Abs i) (fun i -> i >= 0) //|> Arb.toGen |> Gen.resize 20 |> Arb.fromGen

#if tbfo
    let myGen = 
      let fetchBatch =
        fun i ->
          ...
      gen { let! i = Gen.choose (0, 5021999) 
            return fetchBatch i }
#endif //tbfo

    let helper = "a string"
    let private helper' = true
    
    type permHrnssMrkr = class end
    let cfg = Config.Verbose.WithArbitrary([typeof<permHrnssMrkr>.DeclaringType])
    Check.All(cfg, typeof<permHrnssMrkr>.DeclaringType)

    chkFsCheckSetup()


#if ModuleBhojpuri_Remmed_Jul21_2023
module Bhojpuri =
    open System
    open System.Drawing
    open System.IO
    open System.IO.Compression
    open System.Windows.Forms
    open System.Text.RegularExpressions
    open System.Diagnostics
    open DiffMatchPatch
    open Trivedi
    open Trivedi.Core
    open Trivedi.UI
//    open GridTester ModGridTester_RemmedForMonkeyBastas_Jul12_2023

    ///Jul 21 '23: Combined all prior vers & removed redundancies...
    ///            cPnl, rPnl et al were v. rudimentary, all removed

    printfn "in mod winFrms.Bhojpuri..."


    type BhojPuriM =  | SevardhanM of string list
                      | KutkaaM of string * string * string list
                      | CheepsM of C
                      | MeetheeM of T
                      | DilKhushM of Prize //, tbfo... with

    type BhojPuri_Supaari =  | Sevardhan of SevardhanM
                             | Kutkaa of KutkaaM
                             | Cheeps of CheepsM
                             | Meethee of string
                             | DilKhush of int with
        static member New(s:string) = Meethee(s)
        static member New(i:int) = DilKhush(i)
        static member New(c:SevardhanM, bp) = 
           let pwd = new SecureString()
           let retPtr = ref IntPtr.Zero
           let SupaariPnl = new TableLayoutPanel(RowCount = 5, ColumnCount = 1, Dock = doc "F", AutoScroll = true, BackColor = Color.Azure)
           match (len inLi) with
           | 3 ->
               lim (fun i v -> 
                        let pl = if i < 3 then " +" else ""
                        SupaariPnl.Add(new Label(Text = v + pl, AutoSize = true, Anchor = anc "F"),0,(i+1))
                     ) inLi
           | _ -> raise "BhojPuri_Supaari.New(c:SevardhanM, bp): invalid input encountered"
           let inpBox = new TextBox(AutoSize = true, Anchor = anc "F", PasswordChar = '*')
           inpBox.KeyDown.Add(fun o (e:KeyEventArgs) -> 
               match e.KeyCode with
               | Keys.Enter -> 
                   assignPtr [Marshal.SecureStringToGlobalAllocUnicode(pwd)]
                   pwd.Dispose()
                   //auto-Adv + trigger submit if necc
                   bp.updPanel()
               | Keys.Back -> //chk Back vs BackSpc
                   pwd.RemoveAt(pwd.Length - 1)
               | _ -> 
                   let k = e.KeyValue
                   if ( not e.Shift && k >= (int) Keys.A && k <= (int) Keys.Z ) then
                      pwd.AppendChar((char)(k + 32))
                   textBox.Text <- textBox.Text + "*" )
           SupaariPnl.Add(inpBox,0,4)
        static member New(c:KutkaaM, bp) = 
           let SupaariPnl = new TableLayoutPanel(RowCount = 5, ColumnCount = 1, Dock = doc "F", AutoScroll = true, BackColor = Color.Azure)
           let bgBox = new ComboBox(AllowSelection = false, Text = bg, AutoSize = true, Dock = doc "F", DropDownStyle = ComboBoxStyle.DropDownList, AutoCompleteMode = AutoCompleteMode.None, DataSource = [|bg|])
           let enBox = new ComboBox(AllowSelection = false, Text = en, AutoSize = true, Dock = doc "F", DropDownStyle = ComboBoxStyle.DropDownList, AutoCompleteMode = AutoCompleteMode.None, DataSource = [|en|])
           let cb1 = new ComboBox(AutoSize = true, Dock = doc "F", DropDownStyle = ComboBoxStyle.DropDownList, AutoCompleteMode = AutoCompleteMode.None, DataSource = [|cbli|])
           let cb2 = new ComboBox(AutoSize = true, Dock = doc "F", DropDownStyle = ComboBoxStyle.DropDownList, AutoCompleteMode = AutoCompleteMode.None, DataSource = [|cbli|])
           let cb3 = new ComboBox(AutoSize = true, Dock = doc "F", DropDownStyle = ComboBoxStyle.DropDownList, AutoCompleteMode = AutoCompleteMode.None, DataSource = [|cbli|])
           let updateH = new EventHandler(fun o e -> 
                if ((not (cb1.SelectedItem = null)) && (not (cb2.SelectedItem = null)) && (not (cb3.SelectedItem = null))) then
                     assignPtr [bg; (cb1.SelectedItem); (cb2.SelectedItem); (cb3.SelectedItem); en]
                     //auto-Adv + trigger submit if necc
                     bp.updPanel()
                  )
           cb1.SelectedIndexChanged.AddHandler(updateH)
           cb2.SelectedIndexChanged.AddHandler(updateH)
           cb3.SelectedIndexChanged.AddHandler(updateH)
           SupaariPnl.Add(bgBox,0,0)
           SupaariPnl.Add(cb1,0,1)
           SupaariPnl.Add(cb2,0,2)
           SupaariPnl.Add(cb3,0,3)
           SupaariPnl.Add(enBox,0,4)
           //if up/dn btns deemed necc code exists in prior version of this file
        static member New(c:CheepsM, bp) = 
           let SupaariPnl = new TableLayoutPanel(RowCount = 2, ColumnCount = 5, Dock = doc "F", AutoScroll = true, BackColor = Color.Azure)
           let TitlLbl = 
              match CheepsM with
              | 3ty -> new Label(Text = "3ty Title Lbl", AutoSize = true, Anchor = anc "F")
              | _ -> new Label(Text = "Other Title Lbl", AutoSize = true, Anchor = anc "F")
           SupaariPnl.Add(TitlLbl, 0,0)
           SupaariPnl.SetColumnSpan(TitlLbl, 4)
           match CheepsM with
           | 3ty -> SupaariPnl.ColumnCount <- 3
           | _ -> SupaariPnl.ColumnCount <- 5
           lim (fun i c -> 
                   let pb = new PictureBox(BorderStyle = BorderStyle.None, Image = ...)
                   pb.Click.AddHandler(fun o e -> 
						match isThisLastItm with
                                                | true -> 
                                                    let allSelects = thisItm :: (!!~ "priorSelects" SupaariPnl)
                                                    assignPtr allSelects
                                                    //auto-Adv + trigger submit if necc
                                                    bp.updPanel()
                                                | _ -> 
                                                    //if necc. add code to disable via greying out
                                                    thisItm.BorderStyle = BorderStyle.Fixed3D
                                                    let updSelects = thisItm :: (!!~ "priorSelects" SupaariPnl) )
                                                    !!^ ["priorSelects", box updSelects] SupaariPnl) )
                   SupaariPnl.Add(pb, i,1)) [0..ct]
           


    type BhojPuri(t, w) as bp = 
        inherit System.Windows.Forms.Form(StartPosition = FormStartPosition.CenterScreen, WindowState = FormWindowState.Normal, Visible = false, Text = "winFrms Test Form: Copyright (c) M. P. Trivedi 2016-2023.  All rights reserved.", TopMost=true, Font=defFont, Width = 900, Height = 600)
        do printfn "in BhojPuri ctor..."
        let cfgTpl:list<BhojPuri_Supaari> = t
        //these are moved 2 be in scope, just retrofit to gappa
        //Note Jul 21: reconsider...
        let btnP = new TableLayoutPanel(Dock = doc "B", Width = rp.Width)
        let midP = new TableLayoutPanel(RowCount = 1, ColumnCount = 1,Dock = doc "F", AutoScroll = true, BackColor = Color.Azure)
        let okButton = new Button(AutoSize = true, DialogResult = DialogResult.OK, Text = "&OK")
        let cancelButton = new Button(AutoSize = true, DialogResult = DialogResult.Cancel, Text = "&Cancel")
        let titTxt = new TextBox(AutoSize = true, Dock = doc "F", Enabled = false, Text = "Step", ForeColor = dCobaltBlue, ReadOnly = true, Multiline = false, TextAlign = HorizontalAlignment.Center, BorderStyle = BorderStyle.None, BackColor = Color.OldLace)
        let icnLbl = new Label(Image = Bitmap (Bitmap.FromFile("E:\\src\\Data\\images\\brij.png"), new Size(200, 86)), Size = Size(200, 86), Anchor = anc "N")
        let init =
            !!^ ["currFrame", box 0] bp
            !!^ ["totFrames", box 5] bp  //nds to come in via state
        let setupBtnP =
            let btnFP = new FlowLayoutPanel(FlowDirection = FlowDirection.LeftToRight, Anchor = anc "N", AutoSize = true, BackColor = Color.White)
            btnFP.Controls.Add(okButton)
            btnFP.Controls.Add(cancelButton)
            btnP.Controls.Add(btnFP)
            bp.Controls.Add(btnP)
        let setupTitleP =
            let titleP = new TableLayoutPanel(RowCount = 1, ColumnCount = 5, Dock = doc "T", BackColor = Color.OldLace, AutoSize = true, Width = rp.Width , Height = ((int) (titTxt.Height * 3)))
            titleP.SuspendLayout()
            titleP.RowStyles.Clear()
            let icnLbl = new Label(Image = brijLogo, Size = (new Size(brijLogo.Width, brijLogo.Height)), Anchor = anc "N", BackColor = Color.Transparent, ForeColor = (currentScheme ((wRef dsk).Value)).Icn())
            let titTxt = new TextBox(AutoSize = true, Dock = doc "T", Enabled = false, Text = "Meethoo Def Document for " + nm, ReadOnly = true, Multiline = false, Width = f.Width - 50, TextAlign = HorizontalAlignment.Center, BorderStyle = BorderStyle.None, ForeColor = (currentScheme ((wRef dsk).Value)).titFore(), BackColor = (currentScheme ((wRef dsk).Value)).titBack())
            let titleP = new TableLayoutPanel(RowCount = 1, ColumnCount = 6, Dock = doc "T", BackColor = (currentScheme ((wRef dsk).Value)).titBack(), AutoSize = true, Width = f.Width , Height = ((int) (titTxt.Height * 3)))
            let statLbl = new Label(Text = "", Anchor = anc "N", BackColor = Color.Transparent, ForeColor = (currentScheme ((wRef dsk).Value)).Icn())
            titleP.RowStyles.Clear()
            titleP.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f))
            titleP.Controls.Add(icnLbl, 0, 0)
            titleP.Controls.Add(titTxt, 1, 0)
            titleP.Controls.Add(statLbl, 5, 0)
            titleP.SetColumnSpan(titTxt, 4)
            titleP.ResumeLayout(false)
            bp.Controls.Add(titleP)
        let updStatLbl() = 
            statLbl.Text <- ((!!~ "currFrame" bp) :?> string) + " of " + ((!!~ "totFrames" bp) :?> string)
            let currIcn = 
                match (cfgTpl.[!!^ "currFrame" bp] with
                | Sevardhan -> Bitmap (Bitmap.FromFile("E:\\src\\Data\\images\\Sevardhan.png")
                | Kutkaa -> Bitmap (Bitmap.FromFile("E:\\src\\Data\\images\\Kutkaa.png")
                | Cheeps -> Bitmap (Bitmap.FromFile("E:\\src\\Data\\images\\Cheeps.png")
                | Meethee -> Bitmap (Bitmap.FromFile("E:\\src\\Data\\images\\Meethee.png")
                | DilKhush -> Bitmap (Bitmap.FromFile("E:\\src\\Data\\images\\DilKhush.png")
                | _ -> getEmptyIcon()
            statLbl.Image <- currIcn
        let updPanel() =
            midP.SuspendLayout()
            let currF = !!~ "currFrame" bp
            match ((currf + 1) = (!!~ "totFrames" bp)) with
            | true -> bp.submit()
            | _ -> 
                midP.Controls.Clear()
                let SupaariPnl =
                    let currM = cfgTpl.[!!^ "currFrame" bp]            
                    match currM with
                    | SevarthanM -> BhojPuri_Supaari.New(currM, bp)
                    | KutkaaM -> BhojPuri_Supaari.New(currM, bp)
                    | CheepsM -> BhojPuri_Supaari.New(currM, bp)
                    | MeetheeM -> BhojPuri_Supaari.New(currM, bp)
                    | DilKhushM -> BhojPuri_Supaari.New(currM, bp)
                    | _ -> raise "Bhojpuri: invalid Supaari supplied..."
                midP.Controls.Add(SupaariPnl)
                bp.Controls.Add(midP)
                bp.updPanel()
        member bp.submit() =
           do printfn "request to submit recd..."
        do printfn "Bhojpuri ctor init updPanel()..."
        do bp.updStatLbl()
        do bp.updPanel()
        bp.Show()
#endif //ModuleBhojpuri_Remmed_Jul21_2023

#if Jimmy_Remmed_Jul21_2023
module jimmy =
    open System
    open System.Diagnostics
    open System.Windows.Forms
    open Trivedi.Core
    open ikvm
    open com.google.common.jimfs

    printfn "...init mod jimmy..."

    //Add references to all jar files that you use not directly
    //ikvm.runtime.Startup.addBootClassPathAssemby(Assembly.Load("second"))
//    ikvm.runtime.Startup.addBootClassPathAssembly(Assembly.Load("guava.jar"))
    //java.lang.Class clazz = typeof(hello.HelloWorld)
    printfn "init 0..."
    let clazz = typeof<Jimfs>
    printfn "init 1..."
//    java.lang.Thread.currentThread().setContextClassLoader(clazz.getClassLoader())

    let obj = (Jimfs).newFileSystem(Configuration.windows())
    printfn "newfs..."

    let fs = Jimfs.newFileSystem(Configuration.windows())
    printfn "init 2..."
    let foo = fs.getPath("/foo")
    printfn "init 3..."
    java.nio.file.Files.createDirectory(foo)
    printfn "init 4..."
    let hello = foo.resolve("hello.txt") // /foo/hello.txt


    printfn "...Jimmy: init completed..."

#endif //Jimmy_Remmed_Jul21_2023


#if Tokenizer_Remmed_Jul22_2023
module Tokenizer =
    open System
    open System.IO 
    open System.Diagnostics
    open System.Text
    open System.Text.RegularExpressions
    open Trivedi.Core
    open Trivedi.UI
    //open FSharp.Compiler.SourceCodeServices
    //open FSharp.Compiler.Tokenization
    
    printfn "in mod winForms_Tester..."


https://github.com/dotnet/fsharp/blob/2c6344dd627f05c69dfca8a8e0419bb4b440324f/docs/fcs/tokenizer.fsx#L33

    //https://github.com/dotnet/fsharp/blob/main/fcs-samples/Tokenizer/Program.fs
    //https://fsharp.github.io/fsharp-compiler-docs/fcs/tokenizer.html
    
    let sourceTok = FSharpSourceTokenizer([], Some ".\main.fs")
    
    let tokenizeLines (lines:string[]) =
      [ let state = ref FSharpTokenizerLexState.Initial
        for n, line in lines |> Seq.zip [ 0 .. lines.Length ] do
          let tokenizer = sourceTok.CreateLineTokenizer(line)
          let rec parseLine() = seq {
            match tokenizer.ScanToken(!state) with
            | Some(tok), nstate ->
                let str = line.Substring(tok.LeftColumn, tok.RightColumn - tok.LeftColumn + 1)
                yield str, tok
                state := nstate
                yield! parseLine()
            | None, nstate -> state := nstate }
          yield n, parseLine() |> List.ofSeq ]
    
    let tokenizedLinesHardCoded = 
    tokenizeLines
        [| "// Sets the hello wrold variable"
           "(* Multi-line comment #1"
           "Multi-line comment #2"
           "*)"
           "let a = 123f"
           "let hello = \"Hello world\" " |]

    let tokenizedLines = tokenizeLines (File.ReadAllLines("main.fs"))
    
    let runTokenizer() =
        printfn "...running Tokenizer..."
        for lineNo, lineToks in tokenizedLines do
          printfn "%d:  " lineNo
          for str, info in lineToks do printfn "       [nm:%s|str:'%s'|clr:''%A'|char:''%A'|locL:''%A'|locR:''%A']" info.TokenName str (info.ColorClass) (info.CharClass) (info.LeftColumn) (info.RightColumn) 


//output: (partly)
249:  
       [nm:WHITESPACE|str:'                '|clr:''Default'|char:''WhiteSpace'|locL:''0'|locR:''15']
       [nm:IDENT|str:'printfn'|clr:''Identifier'|char:''Identifier'|locL:''16'|locR:''22']
       [nm:WHITESPACE|str:' '|clr:''Default'|char:''WhiteSpace'|locL:''23'|locR:''23']
       [nm:STRING_TEXT|str:'"'|clr:''String'|char:''String'|locL:''24'|locR:''24']
       [nm:STRING_TEXT|str:'Esc'|clr:''String'|char:''String'|locL:''25'|locR:''27']
       [nm:STRING_TEXT|str:'.'|clr:''String'|char:''String'|locL:''28'|locR:''28']
       [nm:STRING_TEXT|str:'.'|clr:''String'|char:''String'|locL:''29'|locR:''29']
       [nm:STRING_TEXT|str:'.'|clr:''String'|char:''String'|locL:''30'|locR:''30']
       [nm:STRING|str:'"'|clr:''String'|char:''String'|locL:''31'|locR:''31']
250:  
       [nm:WHITESPACE|str:'            '|clr:''Default'|char:''WhiteSpace'|locL:''0'|locR:''11']
       [nm:BAR|str:'|'|clr:''Punctuation'|char:''Delimiter'|locL:''12'|locR:''12']
       [nm:WHITESPACE|str:' '|clr:''Default'|char:''WhiteSpace'|locL:''13'|locR:''13']
       [nm:UNDERSCORE|str:'_'|clr:''Identifier'|char:''Identifier'|locL:''14'|locR:''14']
       [nm:WHITESPACE|str:' '|clr:''Default'|char:''WhiteSpace'|locL:''15'|locR:''15']
       [nm:RARROW|str:'->'|clr:''Keyword'|char:''Keyword'|locL:''16'|locR:''17']
       [nm:WHITESPACE|str:' '|clr:''Default'|char:''WhiteSpace'|locL:''18'|locR:''18']
251:  
       [nm:WHITESPACE|str:'                '|clr:''Default'|char:''WhiteSpace'|locL:''0'|locR:''15']
       [nm:IDENT|str:'printfn'|clr:''Identifier'|char:''Identifier'|locL:''16'|locR:''22']
       [nm:WHITESPACE|str:' '|clr:''Default'|char:''WhiteSpace'|locL:''23'|locR:''23']
       [nm:STRING_TEXT|str:'"'|clr:''String'|char:''String'|locL:''24'|locR:''24']
       [nm:STRING_TEXT|str:'.'|clr:''String'|char:''String'|locL:''25'|locR:''25']
       [nm:STRING_TEXT|str:'.'|clr:''String'|char:''String'|locL:''26'|locR:''26']
       [nm:STRING_TEXT|str:'.'|clr:''String'|char:''String'|locL:''27'|locR:''27']
       [nm:STRING|str:'"'|clr:''String'|char:''String'|locL:''28'|locR:''28']

/// Gives an indication of the color class to assign to the token an IDE
/// src/Compiler/Service/ServiceLexing.fsi#L42
type FSharpTokenColorKind =
    | Default = 0           Glot/Eclipse
    | Text = 0 
    | Keyword = 1           red
    | Comment = 2           green
    | Identifier = 3        brown
    | String = 4            brightBlue / drkBlue(Chrome)
    | UpperIdentifier = 5
    | InactiveCode = 7
    | PreprocessorKeyword = 8   sameAsKeywd
    | Number = 9
    | Operator = 10             sameAsKeywd
    | Punctuation = 11          grey(Chrome)

#endif //Tokenizer_Remmed_Jul22_2023

#if Folding_Remmed_Jul22_2023
module Folding =

    printfn "...init module Folding..."

    //repurposed from utils/mapper.fs
    let parseLn =
        fun i str ->
            match str with
             | ParseRegex "\A\s{0,4}type\s(.*?)=" [tyNm] ->
                  Some( i, " type " + tyNm )
             | ParseRegex "\Amodule\s(.*?)=" [modNm]
                  -> Some( i, "module " + modNm)
             | ParseRegex "\A\s{4}let\s(.*?)=" [fnNm]
                  -> Some( i, " let " + fnNm)
             | ParseRegex "\A\#(.*?)\Z" [dirNm]
                  -> Some( i, " #" + dirNm)
             | _ -> None

#if Remmed_Jul20_mbi_stable

    let balancedMatches =
        //see: https://www.codeproject.com/Articles/21080/In-Depth-with-RegEx-Matching-Nested-Constructions
        //Allows push/pop to-from stacks, v. useful BUT we probably nd more control (granular)
        //Shd suffice if we're only looking for top-level lets.  Tinker & determine.
        let pattern = @"(?>#if(?<DEPTH>)|#endif(?<QUOTE-DEPTH>)|.?)*(?(DEPTH)(?!))"
        let source =  """
#if !Wadena
#if Sibley
The result of squaring the integer 4573 and adding 3 is 20912332
#else
The result of applying the 2nd sample function to (7 + 4) is 243
#endif
The result of applying the 3rd sample function to (6.5 + 4.5) is 242.800000
'otherNumber' is 2
#endif
'otherNumber' changed to be 3
#if Barbour
processing [1; 2; 3; 4; 5] through 'squareOddValuesAndAddOne' produces: [2; 10; 26]
processing [1; 2; 3; 4; 5] through 'squareOddValuesAndAddOneNested' produces: [2; 10; 26]
processing [1; 2; 3; 4; 5] through 'squareOddValuesAndAddOnePipeline' produces: [2; 10; 26]
processing [1; 2; 3; 4; 5] through 'squareOddValuesAndAddOneShorterPipeline' produces: [2; 10; 26]
#else
#if Jeffersona
Factorial of 6 is: 720
The Greatest Common Factor of 300 and 620 is 20
#endif
The sum 1-10 is 55
#endif
    """
        let mtch = Regex.Match(source, pattern, RegexOptions.IgnorePatternWhitespace)
        printfn "res:%A" (mtch.Success.ToString()) 
        mtch.Groups |> Seq.cast |> List.ofSeq |> printfn "res grps:%A"

    let parseTxt =
        fun str ->
            match str with
             | ParseRegex2 "\n\s{0,4}type\s(.*?)\n" [tyNm, i, l] ->
                  Some( " type " + tyNm + " " + i.ToString() + " " + l.ToString())
             | ParseRegex2 "\Amodule\s(.*?)=" [modNm, i, l]
                  -> Some( "module " + modNm)
             | ParseRegex2 "\A\s{4}let\s(.*?)=" [fnNm, i, l]
                  -> Some( " let " + fnNm)
             | ParseRegex2 "\A\#(.*?)\Z" [dirNm, i, l]
                  -> Some( " #" + dirNm)
             | _ -> None


#if tbdb
    let parseTxt2 =
        fun str ->
          //The last module may not end w/\n, so for ea file add \n @ end
          //Manually bld mod foldables coz we don't match entire mod, just hdrs
          //This won't do rec via indentation, just straight 4 now
          let isFoldedLi = !!~ "isFoldedli" rtb
          let isFoldableLi = !!~ "isFoldedli" rtb
          let expAll = !!^ ("isFoldedLi", box [])
          let collAll = !!^ ("isFoldedLi", box (!!~ "isFoldableLi" rtb))
          
          let modMatches = Regex.Matches("\s{0,8}module\s(.*?)=",str).Groups |> Seq.cast |> List.ofSeq
          limi (fun i m -> 
                    match (i = len mods - 1) with
                    | true -> 
                      //last mod in file
                      let fstLn = rtb.GetLineFromCharIndex(m.Index)
                      let lstLn = rtb.GetLineFromCharIndex((len rtb.Text) - 1)
                      match (fstLn < lstLn) with
                      | true -> ()
                      | _ -> (fstLn, lstLn) :: [isFoldableLi]
                    | _ -> 
                      let fstLn = rtb.GetLineFromCharIndex(m.Index)
                      let lstLn = rtb.GetLineFromCharIndex((mods.[i+1]).Index - 1)
                      match (fstLn < lstLn) with
                      | true -> ()
                      | _ -> (fstLn, lstLn) :: [isFoldableLi]
                    ) 
          
          let typeMatches = Regex("\s{0,8}type\s(.*?)\n").Match(str)
          if typeMatches.Success then
            m.Groups |> Seq.cast |> List.ofSeq
            |> lifo (fun s x -> 
                      printfn "$A) mat:%A i:%A l:%A" x.Value x.Index x.Length
                      let fstLn = rtb.GetLineFromCharIndex(Index)
                      let lstLn = rtb.GetLineFromCharIndex(Index + Length)
                      match (fstLn < lstLn) with
                      | true -> ()
                      | _ -> (fstLn, lstLn) :: [isFoldableLi]
                      ) [isFoldableLi]

      let dirMatches = Regex("(?>#if(?<DEPTH>)|#endif(?<QUOTE-DEPTH>)|.?)*(?(DEPTH)(?!))").Match(str)
          if typeMatches.Success then
            m.Groups |> Seq.cast |> List.ofSeq
            |> lifo (fun s x -> 
                      printfn "$A) mat:%A i:%A l:%A" x.Value x.Index x.Length
                      ...add to FoldableLi...

      let letMatches = Regex("([^\S\r\n]{0,8}let\s(.*?)\n\n)").Match(str)
      ...add to FoldableLi...
      //(multi-line only) 
      let commentMatches = Regex("([^\S\r\n]{0,8}\(\*(.*?)\*\)\n)")
        ...add to FoldableLi...
        let fstLn = rtb.GetLineFromCharIndex(Index)
        let lstLn = rtb.GetLineFromCharIndex(Index + Length)...
      
      
#endif //tbdb


    let runParser() = 
      hr()
      parseTxt (File.ReadAllText("UI_Jan18.fs")) |> printfn "res:\n%A"

#endif //Remmed_Jul20_mbi_stable

#endif //Folding_Remmed_Jul22_2023

#if FilePanelUpdates_Remmed_Jul21_2023
module FilePanelUpdates =
    open System
    open System.IO
    open System.IO.Compression
    open System.Drawing
    open System.Windows.Forms
    open System.Text.RegularExpressions
    open System.Diagnostics

    printfn "...init module FilePanelUpdates..."


(*
	This version uses a flat fileSystem BUT we'll have to rever to using a tree because
          (i) lV.SubItems have no icons & we nd em
          (ii) Creating new items via lV.Insert(idx...) still adds to the root so to 
               distinguish we'd have to label it dir1\dir2\file.txt which won't do.
        The added ben of using a TV is it manages itself.
*)
    let getFilePnlDepr() =
        let zipBA = File.ReadAllBytes(@"E:\tmp\zipTest.zip")
        let pnlImgLi = new ImageList()
        pnlImgLi.Images.Add(Image.FromFile(@"E:\src\Data\images\google\folder.png"))
        pnlImgLi.Images.Add(Image.FromFile(@"E:\src\Data\images\google\folder_open.png"))
        pnlImgLi.Images.Add(Image.FromFile(@"E:\src\Data\images\google\doc.png"))
        let lV = new ListView(MultiSelect = false, Dock = doc "F", CheckBoxes = false, FullRowSelect = true, HeaderStyle = ColumnHeaderStyle.Nonclickable, LabelEdit = false, View = View.Details, SmallImageList=pnlImgLi, LargeImageList=pnlImgLi)
        lV.SuspendLayout()
        use memStream = new MemoryStream(zipBA)
        let zipArc = new ZipArchive(memStream, ZipArchiveMode.Update)
        lV.Layout.AddHandler(new LayoutEventHandler( fun (sender:obj) (e:System.Windows.Forms.LayoutEventArgs) -> 
            lV.Items.Clear()
#if remmed
            //1st pass: no dirs scanned
            zipArc.Entries |> Seq.cast |> List.ofSeq 
            |> lim (fun en -> 
                       match (en.FullName.EndsWith("\")) with
                       | true -> 
                           //create dirNode
                           let dirNode = new ListViewItem(en,0)
                           dir.ForeColor <- dCobaltBlue
                           lV.Items.Add(dirNode)
                       | _ -> 
                           //create filNode ie, in root
                           lV.Items.Add(new ListViewItem(en,2))
                    ) |> ignore
#endif //remmed
            printfn "...eof..."  ))


        lV.Click.AddHandler(new EventHandler(fun o e -> 
                 let selItm = lV.SelectedItems |> Seq.cast |> List.ofSeq |> List.head
                 "click recd on " + (selItm |> string) + " img idx " + ( (selItm.ImageIndex) |> string)
                 |> tibbie 
                 match selItm.ImageIndex with
                 | 0 -> 
                     //currState:folder(closed)
                     zipArc.Entries |> Seq.cast |> List.ofSeq |> List.filter(fun en -> (en.FullName.Contains(selItm)) && not (en.FullName.EndsWith("\")))
                     |> lifo (fun s en -> 
                       match (en.FullName.EndsWith("\")) with
                       | true -> 
                           //create dirNode
                           let dirNode = new ListViewItem(en,0)
                           dir.ForeColor <- dCobaltBlue
                           lV.Items.Add(dirNode)
                       | _ -> 
                           //create filNode ie, in root
                           lV.Items.Add(new ListViewItem(en,2))
                               ) selItm

                 | 1 -> 
                     //currState:folder_open
                 | 2 -> 
                     //doc
                     ()
                 //lim (fn x -> selItm.SubItems.Add("SubItem# " + x) [0..3] |> 
                 ))
        lV

    let getFilePnl() =
        let zipBA = File.ReadAllBytes(@"E:\tmp\zipTest.zip")
        let pnlImgLi = new ImageList()
        pnlImgLi.Images.Add(Image.FromFile(@"E:\src\Data\images\google\folder.png"))
        pnlImgLi.Images.Add(Image.FromFile(@"E:\src\Data\images\google\folder_open.png"))
        pnlImgLi.Images.Add(Image.FromFile(@"E:\src\Data\images\google\doc.png"))
        let treeVw = new TreeView(ImageList = pnlImgLi, ShowRootLines = true, ShowPlusMinus=true)
        treeVw.SuspendLayout()
        use memStream = new MemoryStream(zipBA)
        let zipArc = new ZipArchive(memStream, ZipArchiveMode.Update)
        treeVw.Layout.AddHandler(new LayoutEventHandler( fun (sender:obj) (e:System.Windows.Forms.LayoutEventArgs) -> 
            treeVw.Nodes.Clear()
            zipArc.Entries |> Seq.cast |> List.ofSeq 
            |> lim (fun en -> 
                       match (en.FullName.EndsWith("\")) with
                       | true -> 
                           let dirNode = new TreeNode(en,0)
                           dir.ForeColor <- dCobaltBlue
                           treeVw.Nodes.Add(dirNode)
                       | _ -> 
                           let (parentDir, fileNm) = getSlugs en
                           let filNode = new TreeNode(en,2)
                           treeVw.Nodes.Add(filNode)
                    ) |> ignore
            printfn "...eof..."  ))
        treeVw.Click.AddHandler(new EventHandler(fun o e -> 
                 let selItm = treeVw.SelectedNode
                 match debugMode with
                 | true -> 
	                 "click recd on " + (selItm |> string) + " img idx " + ( (selItm.ImageIndex) |> string)
        	         |> tibbie 
                 | _ -> ()
                 match selItm.ImageIndex with
                 | 0 -> 
			//closed foldr; chng icn + expnd
                        selItm.ImageIndex <- 1
                        selItm.Expand()
                 | 1 -> 
			//open foldr; chng icn + collpse
                        selItm.ImageIndex <- 0
                        selItm.Collapse()
                 | 2 -> ()  //doc; do nothing (dbl-Click to process it)
                 //lim (fn x -> selItm.SubItems.Add("SubItem# " + x) [0..3] |> 
                 ))
        treeVw.DoubleClick.AddHandler(new EventHandler(fun o e -> 
                 let selItm = treeVw.SelectedNode
                 match (len rtb.Text) > 0 with
                 | true -> 
			match ("(@ToDo: internStr) currTxt will be replaced.  Continue?", choiceDlg()) with
                        | true -> 
                              //Load on overloaded ty also Parses
                              rtb.Load(File.ReadAllText(selItm.FullPath), selItm.FullPath)
                        | _ -> ()
                 | _ -> ()
                 ))
        lV
#endif //FilePanelUpdates_Remmed_Jul21_2023

#if Outliner_Remmed_Jul22_2023
module Outliner =
    This is a new version offering a multi-col dlg

    //repurposed from mod Folding which repurposed utils/mapper
    //Earlier vers had spl handling for Brij ass, amongst other deltas
    let parseLn =
        fun i str ->
            match str with
             | ParseRegex "\A\s{0,4}type\s(.*?)=" [tyNm] ->
                  Some( i, " type " , tyNm )
             | ParseRegex "\Amodule\s(.*?)=" [modNm]
                  -> Some( i, "module " , modNm)
             | ParseRegex "\A\s{4}let\s(.*?)=" [fnNm]
                  -> Some( i, " let " , fnNm)
             | ParseRegex "\A\#(.*?)\Z" [dirNm]
                  -> Some( i, " #" , dirNm)
             | _ -> None

    type outLineLbl(outlineTpl) =
        override Label()
        let init() =
           !!^ ["lineNum", box lineNo] this
        member this.onPaint(args ->
            //multi-color logic in drawstring
            let (lineN, slg, ident) = outlineTpl
            match slg with
            | itmTy -> DrawString( lineNo)  slg)
                       DrawString( Color, ident)
         )

    let run =
	limi (fun i l -> parseLn i l કન્ટેનર્સ ) spl
	|> flatLocal
	|> lim (fun itm ->
  		   let (lineN, slg, ident) = itm
                   new outLineLbl(itm))
        |> feed to lV
        |> get listDlg
        |> attach 
             lbl.Click.AddHandler(new Handler(args ->
               let ln = !!~ "lineNum" this
		//note: if Folding impl will also nd auto-expansion of target
               match (કન્ટેનર્સ  ln isFolded) with
               | true -> let topOfFold = getTopOfFold(ln)
 			 expandFold(topOfFold)
               | _ -> ()
	       rtb.GotoLine(ln)  //ensure it's top of scn by bott() if necc
           )


#endif //Outliner_Remmed_Jul22_2023


#if frmDelta_Remmed_Jul29_2023
module frmDelta =
    open System
    open System.Diagnostics
    open System.Windows.Forms
    open Trivedi.Core
    open Trivedi.UI

    printfn "...init module frmDelta..."

    //mbis: no nd 2 get frustrated...
    //https://ocaml.org/docs/ocaml-on-windows
    //https://stackoverflow.com/questions/40336452/recommended-way-to-add-typescript-to-an-existing-asp-net-4-webforms-project#:~:text=Yes%2C%20Web%20forms%20can%20do%20TypeScript%20and%20Gulp.,Gulp-Watch%20A%20complete%20solution%20can%20be%20found%20here.
    //java: https://sourceforge.net/p/ikvm/wiki/Tutorial/
    //(plus: you can always ask some high-schl kids to teach you coding...)

    printfn "in mod frmDelta..."

    //current workflow (partly impl.) ->
    //કલકતી_પાન >> (get list<DesDocAux<'t>>) >> (pick 1st_or_Default બનારસી_મસાલો) >>  bld&show(no_tpl_passed)

    type DeltaTracker = | DeltaTracker of (fldNm:string * fldTy:FrmFieldType * initVal:obj * deltaVal:option<obj>) list

    //this nds 2 be in ALL frms; plus store a ref to the DT
    //create an overloaded brijForm ty with these methds
        member setDelta(f d) =
            this.DT <- lim (fun x ->
                   discombob...
                   match (initVal = d) with
                   | _ -> Some(d)
                   val)
        member getDeltas() = 
           lim (all items -> flat) |> Option
           |> OMap |> if isSome then getList() 
             -> push to docFld (currNonExisting in CoreMod) (deltas * userNm * dtTime)
             -> attach to list above (w/fldNm 4 CoreMod.delta fld)
        member submit() = oMap(fun x -> getDeltas() -> svr.request(x))

    //eventually we nd ->
    //tpl_passed into બનારસી_મસાલો generates a DeltaTracker >> (!!^ "deltaTracker")


    //this fn nds to be triggered on ea wid upd 
    //(trigs'll vary; chk/ins into widBlder or wherever appropriate)
    //wids will take cTor fldNms (probably already do so) which're passed on...
    let registerDelta = fun fldNm:string newVal:obj -> thisFrm.deltaTracker.setDelta(newVal)
     
//WidTys from ફીલ્ડ_પેનલ_Aux are given below
type FrmFldType =  | FldString | tibbie >> shd be TextBox >> TextChanged event
                   | FldNumber | tibbie >> shd be TextBox >> TextChanged event
                   | FldCurrency | tibbie >> shd be TextBox >> TextChanged event
                   | FldLongString | TextBox >> TextChanged event
                   | FldAttachment | tibbie >> shd be Btn >> attach event to FilePickerDlg code
                   | FldBoolean | CheckBox >> CheckedChanged event
                   | FldChoiceList | ComboBox >> SelectedIndexChanged event
                   | FldRadioBtn | tibbie
                   | FldRange | Label (autoUpd from TrackBar) >> attach event to btn.lblUpdate code
                   | FldNumUpDn | NumericUpDown	>> TextChanged event (chk ValueChngd 2)
                   | FldDate | DateTimePicker >> TextChanged event
                   | FldDateTime | tibbie
                   | FldColor | colBtn >> attach event to colPickerDlg code
                   | FldFont | fontBtn >> attach event to fntPickerDlg code
                   | FldInfoBox | tibbie >> ()
                   | FldBlankRow | TextBox >> ()
                   | UserInput | TextBox >> ()
                   | FldBtn | Button >> ()
                   | FldValidBtn | Button >> ()

//in the wrkflow 4 wids we nd two sep. wids 4 FldChoiceList:
//a ComboBox w/w-o ability to add|edit entries.
//| FldRWComboBox -> ComboBoxStyle=DropDown (allows entering new text/editing list)
//| FldROComboBox -> ComboBoxStyle=DropDownList (allows only picking frm li)

#endif //frmDelta_Remmed_Jul29_2023

#if Aug31
Notes Aug 28: DeltaTracker: 
	- if currFldVal.trim() <> origVal push to Deltas
	- Add to DeltaInf: @user : dtMod : fldsMod [1;2;3...]
	- onDocOpen (NOT pushed with orig payload) if hasPriorVers then req(PriorversFor id);
	  asynch.await(x -> populate to docVersProp; enable viewPVButton)
	- onDocOpen(d -> if doc.isPriorVer then (getModFldLi >> hiliteToRed)
	- note also that we need an easy/efficient way (poss via !!^ [] pnl) to get fldNm|Val tpl)

Aug_31_23 addenda: 
1.  If we embed array w/PriorVers in doc; we nd a hook to update on changes/MakeCurrent
2.  ACLs: Nd to ensure that svrSide applies ACL on qry so that flds are not sent at all in the mTpl;
          on bld gets dfltVal if user hasn't hidden
3.  Grouping ability for fldPs?  Might cause havoc w/rearrangement; make bkgrnd-only rendered? 
    (cells move on surface in-out of groupBox?)
4.  Validation rules on compose: see if we can use the existing forms/err via passing in fns for vdn.

Sept_1_23 notes:
    //Bottom line 4 promotePriorVers:
    //we do not amend any existing docs merely
    //- set PriorVerInf.fldsMod to ALL flds (get from fldDef)
    //- promote prior to curr
    //- (regular route) save curr as latest Ver
    //This retains immutablity + reduces overhead related to tracking deltas


///PriorVers CarryFwd logic for deltas 2. & 3. above

    //note:instd of storing ea UNID (easier) we cld merely store ver#
    //  (svr can deduce the id) @TBD: is this useful?
    type PriorVerInf = | PriorVerInf of userNm:string * dtMod:DateTime * fldsMod:list<string> * docUNID:string

    type AdditionalDocFld = | AdditionalDocFld of PriorVers:list<PriorVerInf>

    //Bottom line 4 promotePriorVers:
    //we do not amend any existing docs merely
    //- set PriorVerInf.fldsMod to ALL flds (get from fldDef)
    //- promote prior to curr
    //- (regular route) save curr as latest Ver
    //This retains immutablity + reduces overhead related to tracking deltas
    let promoteBtn = getBtn "Promote" "Make this Prior Version the Current Document"
    promoteBtn.Click.Add(fun e -> 
        svr.request("promote" Unid None) //ROMode = None 
    )
    let svrPromoteHandler = 
        fun unid ->
            let currDoc = getCurrDocFor unid
            let (userNm,dtMod,fldsMod,docUNID) = currDoc.PriorVerInf
            currDoc.PriorVerInf <- (userNm,dtMod,(getAllFlds SaadoM),docUNID)
            //reconcile currTpl w/priorTpl for vals
            svrSaveHandler(currDoc)

    ViewPriorVersions.click.Add(fun e -> 
           let currVerNo = brijDoc.getCurrVerNo thisDoc.unid
           match (currVerNo - 1) = PriorVers.len with
           | true ->
               લ_રાન્ડ_આઈ (fun _ pv _ ->
                        discomb
                        //what about col titles? does that gappa accept in ctor?
                        (ind, userNm, lastMod)) <| PriorVers
               gappa.itms.DblClick.Add(fun e -> 
                            let tgtItm = PriorVers.[(getIndex targt)]
                            discomb
                            server.req("docOpen " (tgtDocUNID ROMode)))
                let dlg = gappa list
                dlg.Show()
           | _ -> error("corrupt data")
           )

///4. - onDocOpen(d -> if doc.isPriorVer then (getModFldLi >> hiliteToRed)
        SaadoM nds new ctor param for isPrV
        match isPrV with
        | true -> 
              ctor called w/isPrV=true; 
              within SaadoP
              match isPrV w/
              | true -> 
                (dynamic) add txtBox below titlPn: "You are curr vwing a Prior Revision Document.  This document was last edited by user@x.com on DateTime": this info comes from PriorVerInf
                ea fldP gets enabled <- false
                ToolBar's MakeCurrVer btn gets auto-enabled
                ALSO, within ea fld add logic
                if fldNm in PriorVerInf.fldsMod; foreColor <- PVHiLite (Red (assignable))
                taking this back needs a ctor param for fldP
        | _ -> normal flow

#endif //Aug31

module dizCopy =
    open System
    open System.Diagnostics
    open System.Windows.Forms
    open Trivedi.Core
    open Trivedi.UI

    printfn "...init module dizCopy..."

(*
    logdRnr:

    let getOpenHnd = 
///        fun s id ->
ins:        fun s cmd id -> (to match on cmd)
            new EventHandler(fun (sender:obj) (e:EventArgs) ->
                match (isOpen tblID s) with
                | Some w -> switchToChild w
                | _ -> 
                    //FIRST add & then launch w pId 
                    let winH = BrijWin((getUNID.ToString() + ^pId), tblID, docId)
                    openWins <- (winH :: openWins)
                    match s with
                    | "DataView" -> tibbie ("icn cmd " + s + " for dvID -> " + id + crlf + "launch tbfo")
                    | _ -> openDes id )
**Note: this calls Brij.Gullo.openDes (tblty:'a)
**Note also:
     This nds 2 be reconciled with logd/dskOpenHnd which currently matches on dTy; nds 2 be generic

    type સાદુ_પાન_Jan (સુપારી,લવલી,ગુલકંદ, ક્વિમામ, પીચાક,બનાવો, સ્તિતિ) as dsk =
        ...
                    let DesignDVMenuItem = new ToolStripMenuItem("Open in Design Mode")
                    icnCtxt.Items.Add(DesignDVMenuItem) |> ignore
///                    DesignDVMenuItem.Click.AddHandler(new EventHandler(fun (sender:obj) (e:EventArgs) ->
///                                openDes (tblT.GetType()) ))
ins:                  DesignDVMenuItem.Click.AddHandler(getOpenHnd s openDes (tblT.GetType()) )


ADDENDA:
                    let CopyDesignMenuItem = new ToolStripMenuItem("New Table (Copy Design)")
                    icnCtxt.Items.Add(CopyDesignMenuItem) |> ignore
                    CopyDesignMenuItem.Click.AddHandler(new EventHandler(fun (sender:obj) (e:EventArgs) ->
                                copyDes (tblT.GetType()) ))

//extension 2 Brij.Gullo //follows openDes (tblty:'a)
    let copyDes (tblty:'a) =
       let newNm = ref (ગપ્પા_પાન (SizeM,Some("Please enter..."),None , Some(box ("nm")), None, frm, inputDlg()))
       match chkNm with
       | false -> newNm <- ગપ્પા_પાન ("taken, please offer alt...")
       | _ -> ()
       //now the logic for logd/dskOpenHnd (see notes above)
       match cfg with
       | Case1_AllDzInAdmin -> 
            let dox:list<DesDoc<_>> = getDat dz tblty
            lim (fun d -> d.bldCpyFor(tblty)) ** (see below)
       | Case2_SepFIles -> () //...

//addenda 4 Brij.type DesDocAux<'t>
      member this.bldCpyFor(tblty) =
           chk member.genDefault; we'll nd 2 rebuild docInfs/slugs while retaining els

moreover, each dzDoc (e.g. Calcutti) will nd to have a `bldCopyFor('a)` member to return a new inst
Test 2 ensure no refs to earlier ty remain.
*)


module clientInit =
    open System
    open System.Diagnostics
    open System.Windows.Forms
    open Trivedi.Core
    open Trivedi.UI

    printfn "...init module clientInit..."

(*
   From UI.ટેબલ_પાન<'a>: Two entry pts into svrReq (attached 2 icns) ->
      (1) let openHnd = new EventHandler(fun (sender:obj) (e:EventArgs) ->
        tibbie ("icn openDV for dvID -> " + tblID))
      (2) DesignDVMenuItem.Click.AddHandler(new EventHandler(fun (sender:obj) (e:EventArgs) ->
        openDes (tblT.GetType()) ))

   From logdUIRnrrelated: Related items ->
    (1)  type BrijWin = | BrijWin of id:DocUNID * tblId:string * docId:string
    (2)  let getOpenHnd = 
          fun s id ->
            new EventHandler(fun (sender:obj) (e:EventArgs) ->
                match (isOpen tblID s) with
                | Some w -> switchToChild w
                ...
*)

    //Note that Brij.DesDocAux already tells you ty of DesDoc
    //then Dec30 getOpenHnd (above) call becomes ```(isOpen tblID winTy s)```
    type BrijWin_Aug_2_23<'a> = | BrijWin of id:DocUNID * tblId:string * docId:string * dli:list<DesDocAux> *  ty:'a with
        inherit Form(IsMdiChild=true, Text = "...")
        let addDisposeLogic =
           w.Dispose.Add(fun o e -> 
                w.Parent.openWindows <- (w.Parent.openWindows |> List.except w)
                w.Parent.refreshOpenTbls()
              )

    type ટેબલ_પાન_Aug_2_23<'a> (સુપારી,લવલી,ગુલકંદ, ક્વિમામ, પીચાક,બનાવો, ty:'a) as dsk =
        inherit Form(Visible = false, TopMost = true, WindowState = FormWindowState.Maximized, Font = defFont, AutoScroll = true, BackColor = Color.AliceBlue, Text = ("getStr Copy_1"))
        let openTbls = ref (((tblId:string) * DesDocAux) list)
        let openWindows = ref (BrijWin list)
        do printfn "db: ટેબલ_પાન cTor"
        //...
	member d.refreshOpenTbls() =
           let openTblsFromWins = 
              openWindows.Value |> lim (fun win -> 
                 let (tblId, _) = win
                 tblId) |> List.unique
           newOpenTbls <- openTbls.filter(fun tbl -> List.contains tblID openTblsFromWins)
(*
    On the svrSide: 
        UI.ટેબલ_પાન<'a>.openHandler (frm icn) gets payload incl list<DesDocAux> 
          which's added 2 ટેબલ_પાન.openTbls
        @ToDo: For Design Updates, SaveHandler re-sends to cli
    Now dbl-click on dvRow will open local frm.

    BrijSvr has svrReqSvr
    let svrReqSvr = 
        fun cmd categ def ->
            match cmdTy cmd with
            | "chuno" -> chunoHandlerSvr cmd categ def (paanTy cmd)
            | "katho" -> kathoHandlerSvr cmd categ def (paanTy cmd)
            | "supari" -> supariHandlerSvr cmd categ def (paanTy cmd)
            | "lovely" -> lovelyHandlerSvr cmd categ def (paanTy cmd)
            | _ -> 
                tibbie "svr: unknown cmdTy recd"
                //can't really return this, tbfo
                chunoHandlerSvr cmd categ def (paanTy cmd)

     cmdTy = chuno|katho|supari|lovely
     paanTy = banarasi|calcutti|meethoo|saadoo

    (i)   chunoHandlerSvr (tbfo)
    (ii)  kathoHandlerSvr (fo)
    (iii) supariHandlerSvr(tbfo)
    (iv)  lovelyHandlerSvr(tbfo)

For All poss cmd paths below, EXCEPT the kathos(no updates):
       svr req cmpltd successfully ?
          -> cli gets the (single) updated DesDocAux w/same docID
          -> replaced in dsk.openTbls
       Note: might be more efficient to sendAll, consider if another user udpated def...
       chuno|banarasi
       chuno|calcutti
       chuno|meethoo
       chuno|saadoo
       katho|banarasi
       katho|calcutti
       katho|meethoo
       katho|saadoo
       supari|banarasi
       supari|calcutti
       supari|meethoo
       supari|saadoo
       lovely|banarasi 
       lovely|calcutti
       lovely|meethoo
       lovely|saadoo

    So for all DesDoc updates ->
    openConnections.map(fun conn -> match (List.contains updatedTbl conn.getOpenTbls) with
                                       | true -> pushUpdate2User) |> Async

    Current UIAux def ->
    type ફીલ્ડ_પેનલ_Aux (nm, fTy, slg, સુપારી, dzMode, ownr:Form)  as p =
    * The ty maps on dzMode (true will set Ctrl.enabled = false + add hndlrs)
    We nd to convert dzMode to a new DU |Regular|Dz|PriVer and add match cases 4 latter
    which ONLY disable + auto-add a new frm infobox @ top with helpInfo

    Related update to બનારસી_પાન<'t when 't :> ITblMarker> (ctor recieves dzMode)
    (insert in codeBase somewhere near ```let titTxt = new TextBox...```
    match | PriVer -> let priVerInfo = new TextBox...; (l8r) f.Ctrls.Add...
      """You are currently viewing a Prior Document Version.
       Data in this document may not be edited.
       If you wish to use this data, please click the "Make Current" button.
       This will promote this version to the Current Version.
       PLEASE NOTE that all subsequent changes to this document will be replaced.
       We suggest carefully comparing Prior Versions in separate windows before promoting any document."""

   Brij.fs updates ->

     type BrijTy ...
         override this.ContainsMod(Mod) = 
            let (BrijTy(mds, tpl, s, tblTy)) = this
            lico Mod mds
         member this.hasPriorVers:bool = this.ContiainsMod(Mod.DocVersionModule) && lastVerNum > 1
         UPDATE ctor (curr Bld w/o versioning) to take `1` as lastVerNum

     type DocVersionModule = | DocVersionModule of 
OptModID * lastVerNum:int * hasPriorVers:bool with
        REMOVE fld hasPriorVersions (only a coupla refs to this in codeBase; update to this.hasPriorVers)
        ADD fld priorVers:list<(docId * verNum * user * lastMod)>

    let સ_બાઇટ = 
    let સ_સપ્લીટ = 
    let સ_કન્ટેનર્સ = 
    let સ_રિપ્લ = 
    let સ_એક_અજ઼_વાર = 
    let સ_લેન = 
    let સ_ટ્રીમ = 
    let સ_ઇસ_લોઅર = 
    let સ_ઇસ_ડિજીટ = 
    let સ_ટીલ = 
    let સ_આના_થી_ચાલૂ  =
    let સ_આફટર = 
    let સ_આફટર_ટીલ = 
    let સ_એચ_ટી_એમ_એલ_વગર = 
    let સ_ક્વોટ_વગર = 

    let લ_રાન્ડ = 
    let લ_રાન્ડ_આઈ = 
    let લ_રાન્ડ_મનોજ = 
    let લ_રાન્ડ_મનોજ_સ = 
    let લ_રાન્ડ_મનોજ_લ = 
    let લ_આઈ_વગર = 
    let લ_આઈ_બદલો = 
    let લ_આ_છે = 
    let લ_આ_સિવાય = 
    let લ_સિવાય_ચાર_આંગળી_છે_પણ_આ_નઈ = 
    let લ_કોણી_બિલી = 
    let લ_આઈ_જોડે_ટપલ = 
    let લ_છાપો = 
    let લ_આઈ_જોડે_છાપો = 
    let લ_ટૂ_સ = 
    let લ_ટૂ_સ_બાર = 
    let લ_લેન = 
    let લ_ફ્રોમ_ઈ_નમ = 
    let લ_લેનકાસ્ટર_નું_પાઈ = 

    exception Trivedi_ex of string * obj

    let mapRaise = fun x ->
                match x > 5 with
                | true -> Error(Trivedi_ex("no way!", box x))
                | _ -> Ok(x)
    
    let hasErr = fun l -> (l |> List.filter (fun x -> 
                                    match x with
                                    | Error e -> true
                                    | _ -> false)) <> []

    let getErrs = fun l -> (l |> List.map (fun x ->  match x with
                                                        | Error e -> Some(x)
                                                        | _ -> None) |> flat

    let getOks = fun l -> (l |> List.map (fun x ->  match x with
                                                        | Error e -> None
                                                        | _ -> Some(x)) |> flat

    let chkRes = fun l ->
                    let errIO() = printfn "there were errors...(details)...continue?" |> ignore
                    if hasErr l then errIO() else getOks l

    List.map mapRaise <| [1;2;3;4;5;6]
    |> hasErr
    |> printfn "res:%A"

*)

module propBox = 
    open System
    open System.Drawing
    open System.Resources
    open System.Windows.Forms

    printfn "...in mod propBox..."

    let pg = new TabPage( TabIndex = 1, Dock = DockStyle.Fill, Text = pgTxt ,Font = defFont, Name = "TabPg")

(*

    @TBD: we nd a dropDn _above_ the tabs to allow switching scope...
          IF we're allowing it (or a userMsg asking to sel itm/props
          Oct19: Save for v2; currently insist on sel via dzEl

	Notes Thu Oct 19 '23
    * NO direct setting of props from propBox: as b4, do this:
        - chng state
        - call reBuild() on ctrl (tearDown/doLayout)
    * Fix: There are some missing propBox els incl validation
        + some missing FROM dzDef

    * for getDefaultValue() (sp?) we nd to chng wrkflow to:
        - getDfltTpl(): all flds w/default vals
        - map the tpl/replace flds w/vals
        - cleaner + no chance of missing + no if/else chking
        - Related: in frmCtor don't pass Option<rec> but Tpl(withVals|dfltTpl)
            @TBD: we nd a dropDn _above_ the tabs to allow switching scope...
                  IF we're allowing it (or a userMsg asking to sel itm/props
*)

    let cellMainTab = new TabPage( TabIndex = 1, Dock = DockStyle.Fill, ImageKey = "settingsTabIcn", ToolTipText = "Cell Settings", Name = "cellMainTab")

type BorderTab (dzEl:'d) as bt = 
    inherit TabPage( TabIndex = 2, Dock = DockStyle.Fill, ImageKey = "borderTabIcn", ToolTipText = "Cell Borders", Name = "cellBorderTab")
    let BrdrCB = ComboBox(Name = "Brdr", Source = ["None";"Single";"3D"])
    let setAllBtn = Button(Text = "Set All", ToolTipText = "Set all similar items to this value")
    let init =
        match (typeof<'d>) with
        | fldP -> 
            BrdrCB.SelectedIndex <- dzEl.BorderStyle
            BrdrCB.SelectedIndexChanged.Add(fun e -> printfn "tibbie: update immed.")
            bt.Controls.AddRange([|BrdrCB;setAllBtn|])
        | frm  -> 
            BrdrCB.SelectedIndex <- mainTblPnl.BorderStyle
            BrdrCB.SelectedIndexChanged.Add(fun e -> printfn "tibbie: update immed.")
            bt.Controls.Add(BrdrCB)
            let mainTblPnl = !!~ "mainPnl" dzEl
        | g ->
            let gBrdrCB = ComboBox(Source = ["None";"Single";"Raised";"Sunken";"SingleVertical";"RaisedVertical";"SunkenVertical";"SingleHorizontal";"RaisedHorizontal";"SunkenHorizontal"], Name="Brdr")
            gBrdrCB.SelectedItem <- dzEl.DataGridViewCellBorderStyle
            gBrdrCB.SelectedIndexChanged.Add(fun e -> printfn "tibbie: update immed.")
            bt.Controls.Add(gBrdrCB)
        | col -> ()


type FontTab (dzEl:'d) as ft = 
    inherit TabPage(Dock = DockStyle.Fill, ImageKey = "fontTabIcn", ToolTipText = "Fonts", Name = "FontTab")
    let LblFont = "LblFont"
    let ValFont = "ValFont"
    let setAllBtn = "setAllBtn"

type ColorTab (dzEl:'d) as ft = 
    inherit TabPage(Dock = DockStyle.Fill,  ImageKey = "colorTabIcn", ToolTipText = "Colors", Name = "ColorTab")
    let ForeClr = "ForeClr"
    let BackClr = "BackClr"
    let setAllBtn = "setAllBtn"

type SizeTab (dzEl:'d) as ft = 
    inherit TabPage(Dock = DockStyle.Fill, ImageKey = "resizeTabIcn", ToolTipText = "Size", Name = "SizeTab") 
    let Wid = "Wid" //dropDn
    let Ht = "Ht"  //dropDn
    //@TBD: hide-when?
 
    let getTabsForTy (dzEl:'d) =
        match (typeof<'d>) with
        | fldP -> [MainTab(dzEl);BorderTab(dzEl);FontTab(dzEl);ColorTab(dzEl);SizeTab(dzEl)]
        | frm -> [MainTab(dzEl);BorderTab(dzEl);FontTab(dzEl);ColorTab(dzEl);SizeTab(dzEl)]
        | g -> [MainTab(dzEl);BorderTab(dzEl);FontTab(dzEl);ColorTab(dzEl);SizeTab(dzEl)]
        | col -> [MainTab(dzEl);BorderTab(dzEl);FontTab(dzEl);ColorTab(dzEl);SizeTab(dzEl)]

type propBox<'d when 'd :> Control>(dzEl:'d) as p =
    inherit Form(Text = "DnD ops", Visible = true, TopMost = true, WindowState = FormWindowState.Maximized, BackColor = Color.SkyBlue)
    let tc = new TabControl(SelectedIndex = 0)
    let init =
        p.SuspendLayout()
        p.Controls.Add(tc)
        getTabsForTy dzEl
        |> limi (fun t i -> t.TabIndex <- i
                            tc.Controls.Add(t)) |> ignore


module DnD_ops = 
    open System
    open System.Drawing
    open System.Windows.Forms

(*
    What:           MVP for impl/testing DnD func + (l8r) Dojo wireframes...
    Last updated:   Mon Nov 06 2023  //ready 4 work on the UIMonad; runs on glob etc.
    Stat:           interimRes3:
                    [RoTgt -0.5; DzCell ("Cell 1", 1, 1, 0, 0); DzCell ("Cell 2", 1, 1, 1, 0); DzCell ("Cell 3", 1, 1, 2, 0); 
                    RoTgt 0.5; DzCell ("Cell 4", 1, 1, 0, 1); DzCell ("Cell 5", 3, 1, 1, 1); DzCell ("Cell 6", 2, 1, 4, 1); 
                    RoTgt 1.5; DzCell ("Cell 7", 2, 1, 0, 2); DzCell ("Cell 8", 2, 1, 2, 2); 
                    RoTgt 2.5; DzCell ("Cell 9", 2, 1, 0, 3); DzCell ("Cell 10", 3, 1, 2, 3); 
                    RoTgt 3.5; DzCell ("Cell 11", 2, 1, 0, 4); DzCell ("Cell 12", 2, 1, 2, 4); 
                    RoTgt 4.5; DzCell ("Cell 13", 2, 1, 0, 5); DzCell ("Cell 14", 2, 1, 2, 5); 
                    RoTgt 5.5]
    
    Notes:           - Prior vers of this mod exist, chk past commits (bld_v1 interleaves BlankRows)
                     - DDnDTgt spans Row (see PostPitch Notes)
                     - this curr impl autoCasts to DzCell_v2Struc
                     - as decided, changes via UI updates def & autoUpdates UI
                     - @Add: bld_v1 interleaves dropCells(see output); nd to manually do that

*)
    
    
    printfn "1.1"
    
    let printHR() = printfn " - - - - - - - - - - - - - - - - - "
    let tibbie = fun (s:string) -> MessageBox.Show(s, "System Msg") |> ignore
    let defPadding:Padding = new Padding(40)
    
    //Throws on glot
(*
    let defFont:Font = new Font("Tahoma", 26.0F)
    let getCtrlHt() = 
            let g = (new Button()).CreateGraphics()
            ((g.MeasureString("nm", defFont)).ToSize()).Height
*)
    let getCtrlHt() = 100


    let defColor:Color = Color.White
    let defForeColor:Color = Color.Black //dCobaltBlue
    let defBackColor:Color = Color.White

    let colN = 3
    let isEven num = (num % 2 = 0)
    let toCellSlug = fun n -> "Cell " + n.ToString()
    let show a = 
            let (n, r, l:list<'t>) = a
            let fixedOrd = List.map (fun innr -> List.rev innr) l
            printfn "fixedOrd: %A" l
            //l.GetType() |> printfn "res:%A ty:%A" (List.splitInto (l.Length / colN) fixedOrd)// |> List.rev)
    let thirdOf3T = fun (x,y,z) -> z
    let getRndLen =
        fun (v:string) -> 
            let r = int (v.Substring 5) //rm cell
            if r%5 = 0 then 3
                elif r < 5 then 1
                else 2

    printfn "2.1"
    
    type DzCell =    | DzCell of string * int * int * int * int
                     | BetwTgt of float * float
                     | RoTgt of float

    type DzRow = | DzRow of list<DzCell>

    type DzTbl = | DzTable of list<DzRow>

    printfn "3.1"

    ///No longer using Random values + State Support + consise 4 doL()
    let tbl = [DzCell ("Cell 1",1,1,0,0); DzCell ("Cell 2",1,1,1,0);
               DzCell ("Cell 3",1,1,2,0); DzCell ("Cell 4",1,1,0,1);
               DzCell ("Cell 5",3,1,1,1); DzCell ("Cell 6",2,1,4,1);
               DzCell ("Cell 7",2,1,0,2); DzCell ("Cell 8",2,1,2,2);
               DzCell ("Cell 9",2,1,0,3); DzCell ("Cell 10",3,1,2,3);
               DzCell ("Cell 11",2,1,0,4); DzCell ("Cell 12",2,1,2,4);
               DzCell ("Cell 13",2,1,0,5); DzCell ("Cell 14",2,1,2,5)]

    //*  Utils for processing cells: interleaving Tgts   */
    
    let addTgts = 
        fun cellStruc ->
            (*
                 - map over [0..totRows]
                 - split by rows (either earlier approach: conv cells 2 rows
                                  OR approach used in v3 below)
                 - add DzRowBlank be4 ro
                     - addHandler
                 - add BetwTgt (CONSIDER changing nm 2 b4Tgt) before tgt
                     - no nd 4 after; as is evident
                     - addHandler
                 - add DzRowBlank @ end
                     - addHandler
            *)
            printfn "tibbie"
    
    let onlyCells = fun cell -> match cell with
                                   | DzCell(_,_,_,_,_) -> true
                                   | _ -> false

    //*  Tbl processing utils  */
    
    let preProc =
        fun tbl ->
            //if tbl.HasTargets then onlyCells
            printfn "tibbie"
            
    let postProc =
        fun tbl ->
            //if not(tbl.HasTargets) then addTgts
            printfn "tibbie"
    
    let totRows = 
        fun tbl -> 
            tbl
            |> List.filter(onlyCells)
            |> List.collect(fun x -> 
                              let (DzCell(_, _, _, _, crI)) = x
                              [crI]) 
            |> List.max

    let getIdxOfFirstCellForRow =
        fun tbl ro -> 
            printfn "tibbie"

    let insertCellAt =
        fun tbl pos newCell ->
            printfn "tibbie"
            
    let moveCellTo =
        fun tbl oldPos newPos ->
            printfn "tibbie"

(*
    //*  Dnd handlers   */

    ///Add to src.MouseDown
    let dragInitHandler = 
        new DragEventHandler( fun (sender:obj) (e:System.Windows.Forms.DragEventArgs) -> 
            let button1 = (Button) obj
            button1.DoDragDrop(src.Text, DragDropEffects.Copy) |> ignore)

    let tgtDragEnterHandler =
        new DragEventHandler( fun (sender:obj) (e:System.Windows.Forms.DragEventArgs) -> 
            match e.Data.GetDataPresent(DataFormats.Text) with
            | true -> e.Effect <- DragDropEffects.Copy
            | _ -> e.Effect <- DragDropEffects.None)

    let rowTgtDragDropHandler =
        new DragEventHandler( fun (sender:obj) (e:System.Windows.Forms.DragEventArgs) -> 
            //update the struct directly
            textBox1.Text <- e.Data.GetData(DataFormats.Text).ToString())
            
    let b4TgtDragDropHandler =
        new DragEventHandler( fun (sender:obj) (e:System.Windows.Forms.DragEventArgs) -> 
            //update the struct directly
            textBox1.Text <- e.Data.GetData(DataFormats.Text).ToString())

*)

    let bld_v3 = 
        fun li -> 
            li  |> List.fold (fun s v -> 
                    let (c:int, r:int, inLi:list<'t>) = s
                    let (DzCell(slg, cc, cr, ccI, crI)) = v
                    match inLi with
                    | [] -> 0, 0, [RoTgt((float) 0 - 0.5)]
                    | _ ->
                        match (not(c+1 < colN)) with
                        | true -> 
                            //Oct 11: reverted to manual pop of RoTgt...
                            //let CellAndTgt = [RoTgt((float) r + 0.5);DzCell(slg,cc,cr,c,r)] @ inLi
                            //0, r+1, CellAndTgt
                            0, r+1, DzCell(slg,cc,cr,c,r) :: inLi
                        | _ -> 
                            c+cc, r, DzCell(slg,cc,cr,c,r) :: inLi)  (0,0,[]) 
                |> thirdOf3T |> List.rev
    
    printfn "interimRes3:%A" (bld_v3 tbl)

    let getTblRo =
      fun tbl idx ->
        List.filter (fun c ->
                  let (DzCell(_,_,_,_, cRo)) = c 
                  cRo = idx) tbl

    let getRTPnl() = new Panel(Dock = DockStyle.Fill, AutoSize = true, BorderStyle = BorderStyle.Fixed3D)
    let getCell = 
      fun slg -> new Button(Text = slg, Dock = DockStyle.Fill, AutoSize = true)
    let tblRef = ref tbl
    let frm = new Form(Text = "DnD ops", Visible = true, TopMost = true, WindowState = FormWindowState.Maximized, BackColor = Color.SkyBlue)
    frm.SuspendLayout()
    let lbl = new Label(Text = "Dnd Tester", AutoSize = true, Dock = DockStyle.Top)
    let cliP = new TableLayoutPanel(Dock = DockStyle.Fill, CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset, RowCount = 5, ColumnCount = colN, AutoScroll = true, BackColor = Color.Linen)
    lbl.DoubleClick.Add(fun e -> 
        printfn "tibbie tblRef:\n %A" (tblRef.Value)
        printfn "cliP ctrlCount: %A" (cliP.Controls).Count
        printfn "li:\n"
        List.mapi (fun i cl -> printfn "%A) %A" i cl) (cliP.Controls |> Seq.cast |> List.ofSeq ) |> ignore)
    cliP.SuspendLayout()
    cliP.Controls.Clear()
    cliP.ColumnStyles.Clear()
    cliP.RowStyles.Clear()
    List.map (fun c -> cliP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, ((float32) ((1/colN) * 100))))) [1.. colN] |> ignore
    frm.Controls.Add(cliP)
    frm.Controls.Add(lbl)
    cliP.Layout.AddHandler(new LayoutEventHandler( fun (sender:obj) (e:System.Windows.Forms.LayoutEventArgs) -> 
        //necc? let thisF = sender :?> Form
        let rec procTbl currRo remainder =
          let remLen =
            getTblRo remainder currRo 
            |> List.map (fun currcell -> 
                           let (DzCell(slg,cSp,rSp,cCo, cRo)) = currcell
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
        procTbl 0 tblRef.Value
        ))
    cliP.ResumeLayout(false)
    frm.ResumeLayout(false)
    printfn "eom..."

module DnDMonad =

(*
    What:           UIMonad 4 above mod; init tests OK
    Last updated:   Tue Nov 07 2023
    Stat:           @ end of this mod
*)

    type DnDState<'M, 'T> = 'M -> 'M * 'T

    let adder = fun l -> ("adder" + (List.length l).ToString()) :: l
        //(List.tail l @ l)

    let getS = fun s -> (s,s)
    let putS s = fun _ -> (s,())
    let eval m s = m s |> fst
    let exec m s = m s |> snd
    let empty = fun s -> (s,())
    let modif f s = let x = getS in (putS (f x))
    let bind k m = fun s -> 
        let (s', a) = m s
        printfn "Step (a) bind #1: %A" s'
        let s'' = adder s'
        printfn "adder res: %A" s''
        let tmp = (k a) s''
        printfn "Step (b) bind #2 for inSt: %A %A" tmp s'
        tmp

    type DnDStateBuilder() =
        member this.Return(a) : DnDState<'M,'T> = fun s -> (s,a)
        member this.ReturnFrom(m:DnDState<'M, 'T>) = m
        member this.Bind(m:DnDState<'M,'T>, k:'T -> DnDState<'M,'U>) : DnDState<'M,'U> =  bind k m
        member this.Zero() = this.Return()
        member this.Delay(f) = this.Bind(this.Return (), f)
    let ``⍒`` = new DnDStateBuilder()

    let sRun = 
        ["ob"] |>
        ``⍒`` {
               printfn "DnDStateful init()"
               let! a = getS
               printfn "Step (c) tplRun 1: %A" a
               do! putS ("ob2" :: a)
               let! b = getS
               printfn "Step (d) tplRun 2: %A" b
               return b
            }

(*  res: (note the lag in effects)
    Step (a) bind #1: [ob]
    adder res: [adder1; ob]
    DnDStateful init()
    Step (a) bind #1: [adder1; ob]
    adder res: [adder2; adder1; ob]
    Step (c) tplRun 1: [adder1; ob]
    Step (a) bind #1: [ob2; adder1; ob]
    adder res: [adder3; ob2; adder1; ob]
    Step (a) bind #1: [adder3; ob2; adder1; ob]
    adder res: [adder4; adder3; ob2; adder1; ob]
    Step (d) tplRun 2: [adder3; ob2; adder1; ob]
    Step (b) bind #2 for inSt: [adder4; adder3; ob2; adder1; ob],[adder3; ob2; adder1; ob] [adder3; ob2; adder1; ob]
    Step (b) bind #2 for inSt: [adder4; adder3; ob2; adder1; ob],[adder3; ob2; adder1; ob] [ob2; adder1; ob]
    Step (b) bind #2 for inSt: [adder4; adder3; ob2; adder1; ob],[adder3; ob2; adder1; ob] [adder1; ob]
    Step (b) bind #2 for inSt: [adder4; adder3; ob2; adder1; ob],[adder3; ob2; adder1; ob] [ob]
*)

module bldDat =
    open System.Web

    //just a placeholdr 4 eventual wobbly() generation...

    let quoted = fun input -> HttpUtility.JavaScriptStringEncode(input)

    let inStr = """
    This is a "line" with many 'quotes' and 'quotations'.
    This is the 2nd line...
"""

    
(*
    from https://github.com/oria/gridx/blob/master/tests/support/data/TreeNestedTestData.js
	var generateItem = function(parentId, index, level){
		return {
			id: parentId + "-" + (index + 1),
			number: level <= 1 ? randomNumber(10000) : null,
			string: level <= 2 ? randomString() : null,
			date: level <= 3 ? randomDate().toDateString() : null,
			time: randomDate().toTimeString().split(' ')[0],
			bool: randomNumber(10) < 5
		};
	};    //note that the layout below is pro'lly not the one actually used (there's another one in calling pg)
		layouts: [
			[
				{id: 'number', name: 'number', field: 'number', expandLevel: 1},
				{id: 'string', name: 'string', field: 'string', expandLevel: 2},
				{id: 'date', name: 'date', field: 'date', expandLevel: 3},
				{id: 'time', name: 'time', field: 'time'},
				{id: 'bool', name: 'bool', field: 'bool'},
				{id: 'id', name: 'id', field: 'id'}
			],...
*)
    
    printfn "res:%A" (quoted(inStr))

#if forRepl
module main =
  open System
  open System.Drawing
  open System.Windows.Forms
  open DnD_ops
  
  [<EntryPoint>]
  let main argv =
      Application.Run(frm)
      0
#endif //forRepl

    
module main = 
    open System
    open System.Diagnostics
    open System.Windows.Forms
    open Trivedi.Core
    open Trivedi.UI
//    open GridTester ModGridTester_RemmedForMonkeyBastas_Jul12_2023
//    open Ide  ModuleIde_RemmedForMonkeyBastas_Jul12_2023
//    open Ext ModuleExt_RemmedForMonkeyBastas_Jul12_2023
//    open deck //ModuleDeck_RemmedForMonkeyBastas_Jul12_2023
//    open perms
//    open FilePanelUpdates
//    open jimmy

    printfn "in mod winFrms..."

    type Bfty = | Bfty of id:string * fty:DocFldType * valu:obj with
        override this.ToString() = 
          let (Bfty(nm, t, v)) = this
          match t with
          | DFldString -> "Bfty(" + nm + ", " + "string)"
          | DFldCurrency -> "Bfty(" + nm + ", $" + v.ToString() + ")"
          | _ -> "Bfty(" + nm + ", " + "unknown)"
        member this.toPer() = 
            "test"
        member this.fromPer() = 
          "test"

#if Camba
    let x = Bfty("test1", DFldCurrency, box 2.22)
    printfn "toString: x:%A x.ToS:%A" x (x.ToString())
//toString: x:Bfty ("test1", DFldCurrency, 2.22) x.ToS:"Bfty(test1, $2.22)"
#endif //Camba

module dndState = 
    open System
    open System.Diagnostics
    open System.Drawing
    open System.Drawing.Imaging
    open System.IO
    open System.Text
    open System.Text.RegularExpressions
    open System.Globalization
    //open System.Windows.Forms
    open Trivedi
    open Trivedi.Core
    open Trivedi.Control
    open Trivedi.Brij
    open Trivedi.UI
    open Trivedi.UI.Form
    open Trivedi.UI.Dlg
    open Trivedi.UIAux
    open FSharp.Reflection

(*
Nov 15 '23:
****DnDMonad: (Notes to be ins into mod)

Here's the deal:
  - For scope within ob (after ctor in monad), we got working mechanics but no get/put.  POSSIBLY this is coz in the new scope (inside) it does NOT continue outside scope; in which case the 'let x = getS' will assign a fn to x.  Check/verify with simple console app if necc.  and then simply revert to getting local state for ea Monaic fn (this is NO PROB coz ea event is indiv & won't adversely affect state in any case)
  - For now, try the foll.:
    - Tinker w/ctor (do!)
    - Perhaps removing the inner {} scope will let getS work
  - If not, rever to approach identified above.
*)

    printfn "hey dnd"

    //impl console state test w/bind + doPrnLayout() 
    //on ty ext Control; impl basic flow to see if works; 
    //use timer.pause() etc. 4 updates
    //INCLUDE stubs 4 all handlers reqd 4 cpy/paste into dzOps mod
    (*
    Actions:
      = Move
      - Change Props (Sz)
      - Change Props (Details)
      - Ins BlankRow
      - New Tbl ColSz
    Consider: match inside bind with cmd.RequiresLayout -> doL()
    *)
    let adder = fun l -> ("adder" + (List.length l).ToString()) :: l
    //(List.tail l @ l)

    let dndBind_v1 k m = 
      fun s -> 
        let (s', a) = m s
        printfn "Step (a) bind #1: %A" s'
        let s'' = adder s'
        printfn "adder res: %A" s''
        let tmp = (k a) s''
        printfn "Step (b) bind #2 for inSt: %A %A" tmp s'
        tmp

    let dndBind k m = 
      fun s -> 
        let (s', a) = m s in (k a) s'

    type DnDStateBuilder() =
        member this.Return(a) : State<'M,'T> = fun s -> (s,a)
        member this.ReturnFrom(m:State<'M, 'T>) = m
        member this.Bind(m:State<'M,'T>, k:'T -> State<'M,'U>) : State<'M,'U> =  dndBind k m
        member this.Zero() = this.Return()
        member this.Delay(f) = this.Bind(this.Return (), f)
    let ``⍒`` = new DnDStateBuilder()


    type StatefulC(inS:string) as c =
      //inherit Control()
      do printfn "new with ctor:%A" inS
      let moveCmd =
        let eachSec = new System.Timers.Timer(Interval = 1000)
        eachSec.Elapsed.AddHandler(fun o e -> 
              printfn "eachSec timer triggered @ %A" e.SignalTime
              ``⍒`` {
                    let! currS = getS
                    do! putS ((e.SignalTime).ToString() :: currS)
                    } |> ignore)
        eachSec.Enabled <- true
      let propsCmd =
        let twoSec = new System.Timers.Timer(Interval = 2000)
        twoSec.Elapsed.AddHandler(fun o e -> 
              printfn "**TwoSec timer triggered @ %A" e.SignalTime
              ``⍒`` {
                  let! currS = getS
                  do printfn "**current State: %A" (liToString currS)
                  do! putS ((e.SignalTime).ToString() :: currS)
                  } |> ignore)
        twoSec.Enabled <- true
      do printfn "*** outside initControl..."
      member c.prnStat() = 
          ``⍒`` {
                do printfn "prnStat.."
                let! currS = getS
                do printfn "current State: %A" (liToString currS)
          } |> ignore

(*    mucho issues with this...
      override c.ToString() =
          ``⍒`` {
                let! currS = getS
                return "current State: " + (liToString currS)
                } |> eval
*)

    let statAsync(inst:StatefulC) =
        async {
            try
                do Console.WriteLine("in statAsync, press any key...")
                while (not( Console.ReadKey().Key = ConsoleKey.Enter)) do inst.prnStat()
            with
                | ex -> printfn "err in statAsync: %s" (ex.Message)
            }

//        ["init"] |>
    let sRunnr = 
      fun inpt ->
        ``⍒`` {
                let instance = StatefulC("testRun")
                do statAsync(instance)
                  |> Async.RunSynchronously
                  |> ignore
              }

    let sRun = 
      fun inS -> 
        ``⍒`` {
               printfn "DnDStateful init()"
               let! a = getS
               printfn "Step (c) tplRun 1: %A" a
               do! putS ("ob2" :: a)
               let! b = getS
               printfn "Step (d) tplRun 2: %A" b
               return b
            }


    [<EntryPoint>]
    [<STAThread>]
    let main ag =
        match ag.Length = 0 with 
        | true -> 
            //Application.EnableVisualStyles()
            //Application.SetCompatibleTextRenderingDefault(false)
            try
                hr()
                printfn "winFrms main -> try ->  running main.frm().."
                hr()
                //chkArticleTblDizFile()
                //frm()
                //Application.Run(oldRunner())
                printfn "remmed: Application.Run(frm())"
                //Application.Run(frm())
                //printfn "remmed cardExt..."
                //Application.Run(cardExt())
                //permTests()
            with
                | e -> 
                    printfn "Exc in main: %A" e.Message
                    //remmed 2023.06.28: probably @mbi: Can't find ref 
                    //let st = ((new StackTrace(e, true)).GetFrames()).[0]
                    //printfn "Immed.-> method: %A lineNo:%A col: %A" (st.GetMethod().Name) (st.GetFileLineNumber()) (st.GetFileColumnNumber())
                    //remmed 2023.06: probably @mbi: Can't find ref 
                    //printfn "StackTr -> \r\n%A" (getStTrace e)
            //exeRunnr ag
        | false ->
            System.Console.Write("back in main...pls press any key to continue...")
            let c = System.Console.ReadKey(true)
            match c.Key with
            | ConsoleKey.Escape -> printfn "Esc..."
            | _ -> printfn "..."
        0
