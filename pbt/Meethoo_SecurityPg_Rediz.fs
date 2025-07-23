(*    Last updated: Jul 23 '25

           Redesign notes (Jul 21st '25)

* lV for IdentityEntries
  - to pick users/grps 4 TblDefSecurity + FldDefSecurity
  - to choose (single) Itm 4 FldType (new) [FldIdentity]

* lV w/subCats BUT modelled like Dvs (w/Counts)

* Categs:
  Anonymous Users             Create  View  Edit  Delete
  Registered Users
  Groups
  Individuals
  FldIdentity flds 4 tbl
  DocAuthor

* Choosing adds Rows 2 tblDlg a la exprPnl

* For FldDef; only VIEW / EDIT necc.

* Consider changing this to a TabPgTy w/members (and similarly for other Tabs; ctor takes bossCtrl)
*)

//app.Environment.IsDevelopment()

//@ToDo: add new FldType matchCase for IdEntities (see notes above)
type FldType =  | FldIdentity

type IdentType =
    | AnonymousUsers = 0
    | AllRegUsrs = 1
    | Groups = 2
    | Individuals = 3
    | IdentityFields = 4
    | DocAuthor = 5

//@ToDo: add 2 tblDef
type TblAclTpl =
  { mutable IdNm:FldIdentity; mutable CRole:bool; mutable RRole:bool; 
    mutable URole:bool; mutable DRole:bool, mutable IType: IdentType }
  static member getDefault() = 
    [{ "Anonymous Users", false, false, false, false, 0 };
    { "Registered Users", false, false, false, false, 1 };
    { "DocAuthor", false, false, false, false, 5 }]

//@ToDo: Dvs, Frms: don't nd the tpl (DevsOnly by default)
//BUT we nd a mechanism to set Visible/Read in their defs; if false
//the element doesn't appear in the list

//@ToDo: add 2 tblDef
type FldAclTpl = 
  { mutable IdNm:FldIdentity; mutable RRole:bool; mutable URole:bool }

//This Nds 2 dovetail into the curr flow using IdentitySelect_પીચાક + IdentitySelectlV
//To be Hosted in secPg (+ others) - Initialized with AnonUsers+RegUsers+DocAuthrs
//Flow: AddBtn brings up IdentitySelect_પીચાક >> UserSelects from IdentitySelectlV
//>> forEa rtnVal >> new IdentitySelectPnl added to SecPg.  
//Returns the TblAclTpl on demand.
//@ToDo: Note that the Nab nds 2 supply the enumVal to this ctor

type IdentitySelectPnl(fldCand:FldIdentity, IdentTy:IdentType, bossCtrl) as pnl =
    inherit TableLayoutPanel(Dock = doc "F", RowCount = 1, ColumnCount = 6)
    do this.SuspendLayout()
    let fldIdNm = new TextBox(AutoSize = true, Dock = doc "T", Enabled = false, Text = fldCand.ToString(), ReadOnly = true, Multiline = false, Width = f.Width - 50, TextAlign = HorizontalAlignment.Left, BorderStyle = BorderStyle.None, ForeColor = (currentScheme ((!!~ "wld" dsk).Value)).titFore(), BackColor = (currentScheme ((!!~ "wld" dsk).Value)).titBack())
    let CRoleBox = new CheckBox(AutoSize = true, Text = "Negate", Dock = doc "F", AutoCheck=true)
    let RRoleBox = new CheckBox(AutoSize = true, Text = "Negate", Dock = doc "F", AutoCheck=true)
    let URoleBox = new CheckBox(AutoSize = true, Text = "Negate", Dock = doc "F", AutoCheck=true)
    let DRoleBox = new CheckBox(AutoSize = true, Text = "Negate", Dock = doc "F", AutoCheck=true)
    let RemoveBtn = new Button(Image = "delete.png")
    RemboveBtn.Click.AddHandler(new EventHandler(fun o e ->
        bossCtrl.Controls.Remove(pnl)))
    let layoutThis =
        pnl.Controls.Add(fldIdNm, 0, 0)
        pnl.Controls.Add(CRoleBox, 0, 1)
        pnl.Controls.Add(RRoleBox, 0, 2)
        pnl.Controls.Add(URoleBox, 0, 3)
        pnl.Controls.Add(DRoleBox, 0, 4)
        match fldIdNm with
        pnl.Controls.Add(RemoveBtn, 0, 5)
        //add self
        bossCtrl.Controls.Add(pnl)
        bossCtrl.SetColumnSpan(pnl, 6)
        pnl.ResumeLayout(false)
        bossCtrl.ResumeLayout(true)
    do printfn "IdentitySelectPnl layout done..."
    member pnl.getTpl() = 
      (fldCand, CRoleBox.Checked, RRoleBox.Checked, URoleBox.Checked, DRoleBox.Checked, IdentTy)

let IdentitySelectLV_Groups = [("HR", 2);("Sales", 2);("Marketing", 2);("IT", 2);("Distribution", 2);("Corporate", 2)]
let IdentitySelectLV_Indivs = [("Aaron Neville", 3);("Adele", 3);("Aerosmith", 3);("Alanis Morissette", 3);("Alicia Keys", 3);("Amy Grant", 3);("Anita Baker", 3);("Anne Murray", 3);("Art Garfunkel", 3);("B. J. Thomas", 3);("Barbra Streisand", 3);("Barry Manilow", 3);("Bee Gees", 3);("Bette Midler", 3);("Beyoncé", 3);("Bill Withers", 3);("Billie Eilish", 3);("Billy Joel", 3);("Billy Ray Cyrus", 3);("Blood, Sweat & Tears", 3);("Bonnie Raitt", 3);("Boone", 3);("Brandi Carlile", 3);("Brenda Russell", 3);("Bruce Springsteen", 3);("Bruno Mars", 3);("Bryan Adams", 3);("Captain & Tennille", 3);("Carly Simon", 3);("Carole King", 3);("Celine Dion", 3);("Chappell Roan", 3);("Charlie Rich", 3);("Christopher Cross", 3);("Crystal Gayle", 3);("Cyndi Lauper", 3);("Diana Ross", 3);("Dionne Warwick", 3);("Dionne Warwick et al", 3);("Dire Straits", 3);("DJ Khaled", 3);("Doja Cat", 3);("Dolly Parton", 3);("Don Henley", 3);("Don McLean", 3);("Donald Fagen", 3);("Dua Lipa", 3);("Earth, Wind & Fire", 3);("Ed Sheeran", 3);("Elton John", 3);("Eric Clapton", 3);("Foreigner", 3);("Frank Sinatra", 3);("Gayle", 3);("George Benson", 3);("Gilbert O'Sullivan", 3);("Glen Campbell", 3);("Gloria Gaynor", 3);("Gordon Lightfoot", 3);("Grover Washington Jr.", 3);("H.E.R.", 3);("Harry Styles*", 3);("Helen Reddy", 3);("Henry Mancini", 3);("Irene Cara", 3);("James Taylor", 3);("Janis Ian", 3);("Janis Joplin", 3);("Joan Osborne", 3);("Joe South", 3);("John Michael Montgomery", 3);("Jon Batiste", 3);("JP Saxe", 3);("Judy Collins", 3);("Julia Michaels", 3);("Justin Bieber", 3);("k.d. lang", 3);("Kendrick Lamar", 3);("Kenny Rogers", 3);("Kim Carnes", 3);("Kirk Franklin", 3);("Kris Kristofferson", 3);("Lady Gaga", 3);("Lana Del Rey", 3);("Lana Del Rey*", 3);("LeAnn Rimes", 3);("Lewis Capaldi", 3);("Lil Nas X", 3);("Linda Ronstadt", 3);("Linda Ronstadt & James Ingram", 3);("Lionel Richie", 3);("Lizzo", 3);("Los Lobos", 3);("Lynn Anderson", 3);("Marc Cohn", 3);("Maria Muldaur", 3);("Mariah Carey", 3);("McFerrin", 3);("Meat Loaf", 3);("Michael Jackson", 3);("Michael Sembello", 3);("Michel Legrand", 3);("Miley Cyrus", 3);("Morris Albert", 3);("Natalie Cole", 3);("Neil Diamond", 3);("Neil Sedaka", 3);("Neil Young", 3);("No Doubt", 3);("Olivia Newton-John", 3);("Olivia Rodrigo", 3);("Paul McCartney & Stevie Wonder", 3);("Paul Simon", 3);("Paul Young", 3);("Paula Cole", 3);("Peabo Bryson", 3);("Peaches & Herb", 3);("Perry Como", 3);("Peter Gabriel", 3);("Phil Collins", 3);("Post Malone", 3);("R. Kelly", 3);("R.E.M.", 3);("Ray Stevens", 3);("Regina Belle", 3);("Rickie Lee Jones", 3);("Robert Palmer", 3);("Roberta Flack", 3);("Roddy Ricch", 3);("Sabrina Carpenter", 3);("Seal", 3);("Shaboozey", 3);("Shania Twain", 3);("Shawn Colvin", 3);("Sheryl Crow", 3);("Silk Sonic", 3);("Sinéad O'Connor", 3);("Starland Vocal Band", 3);("Steve Lacy", 3);("Steve Winwood", 3);("Stevie Wonder", 3);("Sting", 3);("Survivor", 3);("Suzanne Vega", 3);("SZA", 3);("Tanya Tucker", 3);("Taylor Swift", 3);("The Beatles", 3);("The Carpenters", 3);("the Commodores", 3);("The Doobie Brothers", 3);("the Eagles", 3);("The Goo Goo Dolls", 3);("The Police", 3);("Tina Turner", 3);("Tony Orlando", 3);("Toto", 3);("Tracy Chapman", 3);("Trisha Yearwood", 3);("U2", 3);("USA for Africa", 3);("Vanessa Williams", 3);("Whitney Houston", 3);("Willie Nelson", 3);("Wilson Phillips", 3)]
let IdentitySelectLV_IdFields = [("Manager",4)]
let getIdentitySelectLV_CategNms = 
  fun tblNm ->
  //@ToDo @Chk this probably useless now; repl by enum
    [("Anonymous Users", []);
    ("Registered Users", []);
    ("Groups", IdentitySelectLV_Groups);
    ("Individuals", IdentitySelectLV_Indivs);
    ("IdentityFields", IdentitySelectLV_IdFields);
    ("DocAuthor", [])]

type IdentitySelectLV<'t when 't :> ITblMarker>() as lV =
  inherit ListView(MultiSelect = true, Dock = doc "F", CheckBoxes = true, FullRowSelect = true, HeaderStyle = ColumnHeaderStyle.Nonclickable, LabelEdit = false, View = View.Details, ForeColor = (currentScheme ((!!~ "wld" dsk).Value)).accentFore(), BackColor = (currentScheme ((!!~ "wld" dsk).Value)).accentBack())
    lV.SuspendLayout()
    lV.Columns.Add("Resource:", -2, HorizontalAlignment.Left)
    getIdentitySelectLV_CategNms (getTblNm (ctorDef.getType())
    |> lim (fun itm -> 
              let (hdr, li) = itm
              let cntr = 
                match li with
                | [] -> ""
                | _ -> "(" + li.Length + " items)"
              let listItm = new ListViewItem(hdr,0)
              listItm.ForeColor <- (currentScheme ((!!~ "wld" dsk).Value)).accentFore()
              li |> lim (fun x -> listItm.SubItems.Add(x))
              lV.Items.Add(listItm)) |> ignore
    lV.ResumeLayout(false)

let IdentitySelect_પીચાક =
    fun (ક્વિમામ:option<_>) (dlg:Form) ->
    //The IdentitySelectDlg just shows the lV + returns all selected IDs/Grps/Entities 4 further handling...
        if ક્વિમામ.IsSome then 
            //tibbie "લીસ્ટ_પીચાક isSome"
            let l:string list = unbox (ક્વિમામ.Value)
            let midP:TableLayoutPanel = (!!~ "midP" dlg).Value 
            midP.Controls.Clear()
            let lV = IdentitySelectLV()
            midP.Controls.Add(LV)
            !!^ ["IdentitySelectLV", box lV] dlg
            dlg.ResumeLayout(true)
            let okBtn:Button = (!!~ "okBtn" dlg).Value 
            if (dlg.ShowDialog() = DialogResult.OK && lV.SelectedItems.Length > 0) then
                let ret = 
                  lV.SelectedItems |> Seq.cast |> List.ofSeq |> lim (fun x -> x.Text)
                dlg.Dispose()
                Some(box ret)
            else 
                dlg.Dispose()
                None
          else None

type MeethooSecPg(fldCand:FldIdentity, IdentTy:IdentType, bossCtrl) as pg =
    inherit TabPage (Text = "Security", Dock = doc "F")
    let mutable currAcl = TblAclTpl.getDefault()
    do pg.SuspendLayout()
    //@ToDo: Jul22: flesh this out; show default Anon/RegUsers/Auths + btn 2 add @ bottm?  Removal handled inside pnl.  When mult itms selected; a new IdPnl created/added to SecMidP 4 ea.  Final OK/dispose collects all tpls to a List<IdTpl> 4 return 2 TblDef (chk if we nd a handle here in ctor)
    let MeethooSecPgInfoLbl = new Label(Text = "Please select the ACLs...", Dock = doc "F")
    pg.Controls.Add(MeethooSecPgInfoLbl, 0, 0)
    pg.SetColumnSpan(MeethooSecPgInfoLbl, 6)
    let SecMidP:TableLayoutPanel = new TableLayoutPanel(Margin = new Padding(25), RowCount = 1, ColumnCount = 6, Dock = doc "F", Name="SecMidP", BackColor = Color.White)
    SecMidP.Controls.Clear()
    //add default entries
    let anon = IdentitySelectPnl("Anonymous Users", 0, pg)
    let reg = IdentitySelectPnl("Registered Users", 1, pg)
    let auth = IdentitySelectPnl("DocAuthor", 5, pg)
    SecMidP.Controls.Add(anon, 0, 0)
    SecMidP.Controls.Add(reg, 0, 1)
    SecMidP.Controls.Add(auth, 0, 2)
    pg.Controls.Add(SecMidP, 0,1)
    pg.ResumeLayout(false)
    member pg.SortAcls(acl) = acl |> List.sortBy (fun a -> a.IType)
    member pg.rebuildTpl() = 
      let res = 
        [], (SecMidP.Controls |> Seq.cast |> List.ofSeq)
        |> lifo (fun s c -> 
                  match (typof c) with
                  | IdentitySelectPnl as i -> s @ [i.getTpl()]
                  | _ -> s)
        |> pg.SortAcls
      currAcl <- res






    SecurityPg.Controls.Add(SecMidP)
