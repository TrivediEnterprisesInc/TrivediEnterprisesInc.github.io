(*  Brij (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 
    TrivediBldHdr->
    |Trivedi SrcCtrlHdr File.  Copyright (c) 2015-2025 M. P. Trivedi.  All rights reserved.|637901455|test.fs|none|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|

Last updated: Aug 19 '25

new stuff Jul 25 '25 -> 
                type IdentType
                type TblAclTpl
                type FldAclTpl
                type IdentitySelectPnl
                type IdentitySelectLV
                let IdentitySelect_પીચાક
                type MeethooSecPg
*)

//app.Environment.IsDevelopment()

//@ToDo: add new FldType matchCase for IdEntities (see notes above)
type FldType =  | FldIdentity

type IdentType =
    | AnonymousUsers = 0
    | RegisteredUsrs = 1
    | Groups = 2
    | Individuals = 3
    | IdentityFields = 4
    | DocAuthor = 5
    
//Pulled out coz enums can't have members  @TBD: conv2Ty?
let IdentTypeToString =
    fun identTy -> 
        match identTy with
        | AnonymousUsers -> "Anonymous Users"
        | RegisteredUsrs -> "Registered Users"
        | Groups -> "Groups"
        | Individuals -> "Individuals"
        | IdentityFields -> "Identity Fields"
        | DocAuthor -> "Document Author"

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
//Aug19: _as part of TblAclTpl_; maybe the tpl becomes(_, list<FldAclTpl>)??
//let one = { Fld=DocFld("fld"); IdNm=FldIdentity("abe@acme.com"); RRole=false; IType=IdentType.DocAuthor }
type FldAclTpl = 
  { mutable IdNm:FldIdentity; mutable RRole:bool; mutable URole:bool }

//This Nds 2 dovetail into the curr flow using IdentitySelect_પીચાક + IdentitySelectlV
//To be Hosted in secPg (+ others) - Initialized with AnonUsers+RegUsers+DocAuthrs
//Flow: AddBtn brings up IdentitySelect_પીચાક >> UserSelects from IdentitySelectlV
//>> forEa rtnVal >> new IdentitySelectPnl added to SecPg.  
//Returns the TblAclTpl on demand.
//@ToDo: Note that the Nab nds 2 supply the enumVal to this ctor

type IdentitySelectPnl(fldCand:FldIdentity, IdentTy:IdentType, bossCtrl, pg) as pnl =
    inherit TableLayoutPanel(Dock = doc "F", RowCount = 1, ColumnCount = 6)
    do this.SuspendLayout()
    let fldIdNm = new TextBox(AutoSize = true, Dock = doc "T", Enabled = false, Text = fldCand.ToString(), ReadOnly = true, Multiline = false, Width = f.Width - 50, TextAlign = HorizontalAlignment.Left, BorderStyle = BorderStyle.None, ForeColor = (currentScheme ((!!~ "wld" dsk).Value)).titFore(), BackColor = (currentScheme ((!!~ "wld" dsk).Value)).titBack())
    let CRoleBox = new CheckBox(AutoSize = true, Text = "Negate", Dock = doc "F", AutoCheck=true)
    let RRoleBox = new CheckBox(AutoSize = true, Text = "Negate", Dock = doc "F", AutoCheck=true)
    let URoleBox = new CheckBox(AutoSize = true, Text = "Negate", Dock = doc "F", AutoCheck=true)
    let DRoleBox = new CheckBox(AutoSize = true, Text = "Negate", Dock = doc "F", AutoCheck=true)
    let RemoveBtn = new Button(Image = "delete.png")
    RemboveBtn.Click.AddHandler(new EventHandler(fun o e ->
        bossCtrl.Controls.Remove(pnl)
        pg.rebuildTpl() //resets currAcl too NEEDS TESTING THOROUGHLY))
    let layoutThis =
        pnl.Controls.Add(fldIdNm, 0, 0)
        pnl.Controls.Add(CRoleBox, 0, 1)
        pnl.Controls.Add(RRoleBox, 0, 2)
        pnl.Controls.Add(URoleBox, 0, 3)
        pnl.Controls.Add(DRoleBox, 0, 4)
        match fldCand with
        | IdentType.AnonymousUsers 
        | IdentType.RegisteredUsrs 
        | IdentType.DocAuthor -> () //no xBtn
        | _ -> pnl.Controls.Add(RemoveBtn, 0, 5)
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

type IdentitySelectLV<'t when 't :> ITblMarker>(ક્વિમામ) as lV =
  inherit ListView(MultiSelect = true, Dock = doc "F", CheckBoxes = true, FullRowSelect = true, HeaderStyle = ColumnHeaderStyle.Nonclickable, LabelEdit = false, View = View.Details, ForeColor = (currentScheme ((!!~ "wld" dsk).Value)).accentFore(), BackColor = (currentScheme ((!!~ "wld" dsk).Value)).accentBack())
    lV.SuspendLayout()
    lV.Columns.Add("Resource:", -2, HorizontalAlignment.Left)
    //@ToDo @TBD: tbfo fn to remove existing itms from the li (Jul25: poss not necc.; we just remove the incoming dupes & leave the existing ones in, therefore retaining any checkMarks 4 roles)
    getIdentitySelectLV_CategNms (getTblNm (ctorDef.getType())
    |> removeExistingAclItms
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
            //tibbie "ક્વિમામ isSome"
            let l:string list = unbox (ક્વિમામ.Value)
            let midP:TableLayoutPanel = (!!~ "midP" dlg).Value 
            midP.Controls.Clear()
            let lV = IdentitySelectLV(ક્વિમામ)
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

type MeethooSecPg(fldCand:FldIdentity, IdentTy:IdentType, aclTpl:TblAclTpl, bossCtrl) as pg =
    inherit TabPage(Text = "Security", Dock = doc "F")
    let mutable currAcl = aclTpl //init shd be TblAclTpl.getDefault()
    do pg.SuspendLayout()
    let MeethooSecPgInfoLbl = new Label(Text = "Please select the ACLs...", Dock = doc "F")
    pg.Controls.Add(MeethooSecPgInfoLbl, 0, 0)
    pg.SetColumnSpan(MeethooSecPgInfoLbl, 6)
    let SecMidP:TableLayoutPanel = new TableLayoutPanel(Margin = new Padding(25), RowCount = 1, ColumnCount = 6, Dock = doc "F", Name="SecMidP", BackColor = Color.White)
    SecMidP.Controls.Clear()
    do SecMidP.SuspendLayout()
    SecMidP.Layout.AddHandler(new LayoutEventHandler(fun o e ->
      SecMidP.RowCount <- (currAcl.Len + 3) //+1 for AddBtn @ bot +1 for hdrs @ top + 1 for len
      SecMidP.Controls.Clear()

      let hdrRow = new TableLayoutPanel(Dock = doc "F", RowCount = 1, ColumnCount = 6)
      hdrRow.Add(new Label(Text = "Create", Dock = doc "F"), 0, 3)
      hdrRow.Add(new Label(Text = "View", Dock = doc "F"), 0, 4)
      hdrRow.Add(new Label(Text = "Edit", Dock = doc "F"), 0, 5)
      hdrRow.Add(new Label(Text = "Delete", Dock = doc "F"), 0, 6)
      SecMidP.Add(hdrRow, 0, 0)

      currAcl
      |> limi (fun i r -> 
                  let thisPnl = new IdentitySelectPnl(r.IdNm, r.IType, SecMidP, pg))
                  SecMidP.Add(thisPnl, 0, i+1 //for hdrRow)))
    let AddPnl = new FlowLayoutPanel(Dock = doc "F", FlowDirection = FlowDirection.LeftToRight, AutoSize = true)
    //@ToDo: add icn img?  Nd 2 standardize on this
    let AddDlgBtn = new Button(Text = "Add", Dock = doc "F", BackColor = defBackColor)
    AddDlgBtn.Click.AddHandler(new EventHandler(fun o e -> 
        match (gappa IdentitySelect currAcl pg) with
        | Some newAcl -> 
          reconcileAcl(newAcl)
          SecMidP.Layout()
        | _ -> ()))
    SecMidP.Add(AddPnl, 0, (currAcl.Len + 2))
    do SecMidP.ResumeLayout(false)
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
    member pg.reconcileAcl(newAcl) = 
      newAcl
      |> lim (fun a -> 
                match lico currAcl a with
                | false -> 
                    newAcl = currAcl @ [a]
                    currAcl <- pg.SortAcls(newAcl)
                | -> ())
    SecurityPg.Controls.Add(SecMidP)

(*    Updates below: Aug 4,5 '25     *)

//rename this coz it has to also be used by the dzEls
type TblAclTpl =
  { mutable IdNm:FldIdentity; mutable CRole:bool; mutable RRole:bool; 
    mutable URole:bool; mutable DRole:bool; mutable IType: IdentType }
  static member getDefault() = 
        (* For ref: The Remaining fromDlg are:
            "Groups", 2
            "Individuals", 3
            "IdentityFields", 4
        *)
    [{ "Anonymous Users", false, false, false, false, 0 };
    { "Registered Users", false, false, false, false, 1 };
    { "DocAuthor", false, false, false, false, 5 }]
  static member getRev(aclLi) = 
    aclLi.sortBy(fun a -> 
                  let (_, _, _, _, _, IType) = a 
                  IType)

//updated 8_19_25 in order 2 Move into the TblAclTpl
type FldAclTpl = | FldAclTpl of Map<Fld:DocFld, ()>
  { Fld:DocFld; mutable IdNm:FldIdentity; mutable RRole:bool; mutable IType: IdentType }
  static member getRev(aclLi) = 
    aclLi.sortBy(fun a -> 
                  let (_, _, _, _, _, IType) = a 
                  IType)

type FldAclTpl_v0 = 
  { Fld:DocFld; mutable IdNm:FldIdentity; mutable RRole:bool; mutable IType: IdentType }
  static member getRev(aclLi) = 
    aclLi.sortBy(fun a -> 
                  let (_, _, _, _, _, IType) = a 
                  IType)

(*  Note poss performance/optimization issue with proc. fldLvlAcls:
- For now, we can just map on the dat; BUT
- CalcAcls (Author, IdentFld, otherCalcs) can vary by Doc whereas (Grps, Uids...) do not
- It is easier to apply the former to the dat
- For the latter we'll have to apply it on doc basis coz it differs (DocAuthor can differ, for instance)
*)
let rec getEffectiveFldAcl uId AclLi =
    match AclLi with
    | [] -> 
        //NB: we nd this for EA fld, right?
        [], getFldNms |> lifo (fun s f -> s @ [(fldNm, false)])
    | _ -> 
        //Aug 5: to be completed
        [], getFldNms |> lifo (fun s f -> s @ [(fldNm, false)])
      (*
      let allButLast = AclLi[0..((lilen AclLi) - 1)]
      let last = List.last AclLi
      let (_, cA, rA, uA, dA, _) = last
      let lastAcl = (cA, rA, uA, dA)
      match (lastAcl = (false, false, false, false)) with
      | true -> ((false, false, false, false), chkFldLvlAcls uId)
      | _ -> getEffectiveAcl allButLast uId
      *)

let chkFldLvlAcls uId =
  tblDef 
  |> getFldAcls
  |> FldAclTpl.getRev
  |> getEffectiveFldAcl uId

let rec getEffectiveAcl AclLi uId =
    match AclLi with
    | [] -> [(false, false, false, false), (chkFldLvlAcls uId)]
    | _ -> 
      let allButLast = AclLi[0..((lilen AclLi) - 1)]
      let last = List.last AclLi
      let (_, cA, rA, uA, dA, _) = last
      let lastAcl = (cA, rA, uA, dA)
      match (lastAcl = (false, false, false, false)) with
      | true -> ((false, false, false, false), chkFldLvlAcls uId)
      | _ -> getEffectiveAcl allButLast uId

//test inputs
let dzElAclTpl = 
  [ { "Anonymous Users", false, false, false, false, 0 };
    { "Registered Users", false, false, false, false, 1 };
    { "HR", false, false, false, false, 2 };
    { "mike@trivedi.com", false, false, false, false, 3 };
    { "Manager", false, false, false, false, 4 };
    { "DocAuthor", false, false, false, false, 5 }]
let TblAclTpl = TblAclTpl.getDefault()

//this's the entry pt.
let getAcl = 
  fun dzEl tblDef uId ->
    //we nd 2 run fst for the dzEl (since it overwrites) & THEN 4 the tblDef
    let dzAc = getEffectiveAcl (TblAclTpl.getRev(dzElAclTpl)) uId
    match (dzAc.CRole, dzAc.RRole,dzAc.URole,dzAc.DRole) with
    | (false, false, false, false) ->
          getEffectiveAcl (TblAclTpl.getRev(TblAclTpl)) uId
    | _ as a -> a //dzEl matchCase

//after we rec. the dat; run this
let applyFldLvlAcls dat =
  //removes (ie repl w defVal) if not VizTo
  //First we nd 2 getEffectiveDocAcl aclLi (IF set) forEa doc; THEN repl w defVal

(* Documentation:
Show an HR form (Nab) w/EmplInfo | SalaryBand | Mngr | SnrMngr
* User has RO acl (no Edit) + Mngr/Sr/HR have (TTTT) (THIS IS FRM_LVL)
* Now what if Dev wants the user to add/edit contactDetails or unrelated flds to the doc?  Ans: Another form, this time the user has Edit _but_ frm doesn't contain the salaryFld.  Add bckgrnd & feed to chatGPT for writeups.
*)

