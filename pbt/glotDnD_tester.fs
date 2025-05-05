module tmp = 
    open System
    open System.Drawing

    printfn "hey"

    type ITblMarker = interface end
    type TaskTbl() = interface ITblMarker

    type FldType =  | FldString
                    | FldNumber
                    | FldRange
                    | FldCurrency
                    | FldHtml
                    | FldAttachment
                    | FldBoolean
                    | FldChoiceList
                    | FldDate 
                    | FldTime
                    | FldColor
                    | FldRating
                    | FldFont

    type DocUNID = | DocUNID of string
    type Thingy = | Thingy of string

    let defColor:Color = Color.White
    let defForeColor:Color = Color.Black //dCobaltBlue
    let defBackColor:Color = Color.White
    let mutable timeStmp = DateTime.Now.Ticks
    let getUNID (cls:string) : DocUNID =
        timeStmp <- timeStmp + ((int64) 1)
        DocUNID(((timeStmp - (DateTime(2001, 1, 1)).Ticks).ToString() + "^" + cls))
    let defFont:Font = new Font("Tahoma", 26.0F)
    let defIt = Font(defFont.FontFamily, defFont.Size, FontStyle.Italic, defFont.Unit)
    let colN = 3
    let isEven num = (num % 2 = 0)
    

    type DocFld = | DocFld of FldType*string*bool*string

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
        static member getShorty(f, cc, cr, col, ro) = 
                let (DocFld(ft, intNm, isInt, tit)) = f
                { unid = (getUNID "BmFldTest"); title = tit ; docF = f; 
                colSpan = 1 ; rowSpan = 1 ; Pos = (0, 0); lblFont = defIt ; dataFont = defFont; 
                foreCol = Some(Color.Black); backCol = Some(Color.Black); 
                soopari = box 1; vFn = None ; CanUhear = Thingy("") ; fldTtip = None; tblTy = TaskTbl() }
                |> (fun c -> { c with colSpan = cc; rowSpan = cr; Pos = (col,ro) } )

    type BMdzCell<'t when 't :> ITblMarker> =   | BMdzCellItm of BMfld<'t>
                                                | BlankCell of (int * int)
                                                | B4Tgt of (float * float)


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
        DocFld(FldHtml, "cont", false, "Content");
        DocFld(FldString, "tags", false, "Tags");
        DocFld(FldBoolean, "flag", false, "Flag")]
    
//The Orig loc8d on Apr 22; 2 be modif.d for initSt...
    let bld = 
        fun inLi -> 
            inLi 
            |> List.fold (fun s v -> 
                    let (c:int, r:int, li:list<'t>) = s
                    match (c+1 = colN) with
                    | true -> 
                        //printfn "nxtRo: For c:%A r:%A liLen:%A, returning:%A" c r (li.Length) (DzCell(v))
                        c+1, r+1, BMfld.getShorty(v, 1, 1, c, r) :: li
                    | _ -> 
                        //printfn "_: For c:%A r:%A liLen:%A, returning:%A" c r (li.Length) (DzCell(v))
                        c+1, r, BMfld.getShorty(v, 1, 1, c, r) :: li
                    ) (0,0,[]) 

    tkFldList() |> bld |> printfn "res:%A"

#if ToBeCompletedLater
    let bldState li = 
            //(BMdzTbl -> UIModel)
            let cTot, rTot, uiM = 
                li  |> List.fold (fun s v -> 
                        let (c:int, r:int, roLi:list<'t>) = s
                        let ([BMdzCellItm(slg, cc, cr, ccI, crI)]) = v
                        match roLi with
                        | [] -> 0, 0, []
                        | _ ->
                            let thisRoSt = 
                                roLi 
                                |> lim (fun thisCl -> [BMdzCellItm(slg,cc,cr,c,r)])
                                |> lico
                            match (not(c+1 < colN)) with
                            | true -> 0, r+1, (thisRoSt @ roLi)
                            | _ -> c+cc, r, (thisRoSt @ roLi)) (0,0,[]) 
                    |> List.rev
            (li, uiM)
#endif //ToBeCompletedLater

    printfn "over & out ... "

(*
Output:
hey
res:(14, 1,
 [{unid = DocUNID "7675623285716134^BmFldTest";
   title = "Flag";
   docF = DocFld (FldBoolean,"flag",false,"Flag");
   colSpan = 1;
   rowSpan = 1;
   Pos = (13, 1);
   lblFont =
    [Font: Name=DejaVu Sans, Size=26, Units=3, GdiCharSet=1, GdiVerticalFont=False];
   dataFont =
    [Font: Name=DejaVu Sans, Size=26, Units=3, GdiCharSet=1, GdiVerticalFont=False];
   foreCol = Some Color [Black];
   backCol = Some Color [Black];
   soopari = 1;
   vFn = null;
   CanUhear = Thingy "";
   fldTtip = null;
   tblTy = Main+tmp+TaskTbl;};
  {unid = DocUNID "7675623285716133^BmFldTest";
   title = "Tags";
   docF = DocFld (FldString,"tags",false,"Tags");
   colSpan = 1;
   rowSpan = 1;
   Pos = (12, 1);
   lblFont =
    [Font: Name=DejaVu Sans, Size=26, Units=3, GdiCharSet=1, GdiVerticalFont=False];
   dataFont =
    [Font: Name=DejaVu Sans, Size=26, Units=3, GdiCharSet=1, GdiVerticalFont=False];
   foreCol = Some Color [Black];
   backCol = Some Color [Black];
   soopari = 1;
   vFn = null;
   CanUhear = Thingy "";
   fldTtip = null;
   tblTy = Main+tmp+TaskTbl;};
  {unid = DocUNID "7675623285716132^BmFldTest";
   title = "Content";
   docF = DocFld (FldHtml,"cont",false,"Content");
   colSpan = 1;
   rowSpan = 1;
   Pos = (11, 1);
   lblFont =
    [Font: Name=DejaVu Sans, Size=26, Units=3, GdiCharSet=1, GdiVerticalFont=False];
   dataFont =
    [Font: Name=DejaVu Sans, Size=26, Units=3, GdiCharSet=1, GdiVerticalFont=False];
   foreCol = Some Color [Black];
   backCol = Some Color [Black];
   soopari = 1;
   vFn = null;
   CanUhear = Thingy "";
   fldTtip = null;
   tblTy = Main+tmp+TaskTbl;};
  {unid = DocUNID "7675623285716131^BmFldTest";
   title = "DocLinks";
   docF = DocFld (FldString,"docLinks",false,"DocLinks");
   colSpan = 1;
   rowSpan = 1;
   Pos = (10, 1);
   lblFont =
    [Font: Name=DejaVu Sans, Size=26, Units=3, GdiCharSet=1, GdiVerticalFont=False];
   dataFont =
    [Font: Name=DejaVu Sans, Size=26, Units=3, GdiCharSet=1, GdiVerticalFont=False];
   foreCol = Some Color [Black];
   backCol = Some Color [Black];
   soopari = 1;
   vFn = null;
   CanUhear = Thingy "";
   fldTtip = null;
   tblTy = Main+tmp+TaskTbl;};
  {unid = DocUNID "7675623285716130^BmFldTest";
   title = "Target Version";
   docF = DocFld (FldChoiceList,"tgtVer",false,"Target Version");
   colSpan = 1;
   rowSpan = 1;
   Pos = (9, 1);
   lblFont =
    [Font: Name=DejaVu Sans, Size=26, Units=3, GdiCharSet=1, GdiVerticalFont=False];
   dataFont =
    [Font: Name=DejaVu Sans, Size=26, Units=3, GdiCharSet=1, GdiVerticalFont=False];
   foreCol = Some Color [Black];
   backCol = Some Color [Black];
   soopari = 1;
   vFn = null;
   CanUhear = Thingy "";
   fldTtip = null;
   tblTy = Main+tmp+TaskTbl;};
  {unid = DocUNID "7675623285716129^BmFldTest";
   title = "Completed On";
   docF = DocFld (FldDate,"completedOn",false,"Completed On");
   colSpan = 1;
   rowSpan = 1;
   Pos = (8, 1);
   lblFont =
    [Font: Name=DejaVu Sans, Size=26, Units=3, GdiCharSet=1, GdiVerticalFont=False];
   dataFont =
    [Font: Name=DejaVu Sans, Size=26, Units=3, GdiCharSet=1, GdiVerticalFont=False];
   foreCol = Some Color [Black];
   backCol = Some Color [Black];
   soopari = 1;
   vFn = null;
   CanUhear = Thingy "";
   fldTtip = null;
   tblTy = Main+tmp+TaskTbl;};
  {unid = DocUNID "7675623285716128^BmFldTest";
   title = "Urgency";
   docF = DocFld (FldRange,"urgency",false,"Urgency");
   colSpan = 1;
   rowSpan = 1;
   Pos = (7, 1);
   lblFont =
    [Font: Name=DejaVu Sans, Size=26, Units=3, GdiCharSet=1, GdiVerticalFont=False];
   dataFont =
    [Font: Name=DejaVu Sans, Size=26, Units=3, GdiCharSet=1, GdiVerticalFont=False];
   foreCol = Some Color [Black];
   backCol = Some Color [Black];
   soopari = 1;
   vFn = null;
   CanUhear = Thingy "";
   fldTtip = null;
   tblTy = Main+tmp+TaskTbl;};
  {unid = DocUNID "7675623285716127^BmFldTest";
   title = "Importance";
   docF = DocFld (FldRange,"importance",false,"Importance");
   colSpan = 1;
   rowSpan = 1;
   Pos = (6, 1);
   lblFont =
    [Font: Name=DejaVu Sans, Size=26, Units=3, GdiCharSet=1, GdiVerticalFont=False];
   dataFont =
    [Font: Name=DejaVu Sans, Size=26, Units=3, GdiCharSet=1, GdiVerticalFont=False];
   foreCol = Some Color [Black];
   backCol = Some Color [Black];
   soopari = 1;
   vFn = null;
   CanUhear = Thingy "";
   fldTtip = null;
   tblTy = Main+tmp+TaskTbl;};
  {unid = DocUNID "7675623285716126^BmFldTest";
   title = "Objective";
   docF = DocFld (FldString,"objective",false,"Objective");
   colSpan = 1;
   rowSpan = 1;
   Pos = (5, 1);
   lblFont =
    [Font: Name=DejaVu Sans, Size=26, Units=3, GdiCharSet=1, GdiVerticalFont=False];
   dataFont =
    [Font: Name=DejaVu Sans, Size=26, Units=3, GdiCharSet=1, GdiVerticalFont=False];
   foreCol = Some Color [Black];
   backCol = Some Color [Black];
   soopari = 1;
   vFn = null;
   CanUhear = Thingy "";
   fldTtip = null;
   tblTy = Main+tmp+TaskTbl;};
  {unid = DocUNID "7675623285716125^BmFldTest";
   title = "SubModule";
   docF = DocFld (FldString,"submodule",false,"SubModule");
   colSpan = 1;
   rowSpan = 1;
   Pos = (4, 1);
   lblFont =
    [Font: Name=DejaVu Sans, Size=26, Units=3, GdiCharSet=1, GdiVerticalFont=False];
   dataFont =
    [Font: Name=DejaVu Sans, Size=26, Units=3, GdiCharSet=1, GdiVerticalFont=False];
   foreCol = Some Color [Black];
   backCol = Some Color [Black];
   soopari = 1;
   vFn = null;
   CanUhear = Thingy "";
   fldTtip = null;
   tblTy = Main+tmp+TaskTbl;};
  {unid = DocUNID "7675623285716124^BmFldTest";
   title = "Module";
   docF = DocFld (FldString,"moduleNm",false,"Module");
   colSpan = 1;
   rowSpan = 1;
   Pos = (3, 1);
   lblFont =
    [Font: Name=DejaVu Sans, Size=26, Units=3, GdiCharSet=1, GdiVerticalFont=False];
   dataFont =
    [Font: Name=DejaVu Sans, Size=26, Units=3, GdiCharSet=1, GdiVerticalFont=False];
   foreCol = Some Color [Black];
   backCol = Some Color [Black];
   soopari = 1;
   vFn = null;
   CanUhear = Thingy "";
   fldTtip = null;
   tblTy = Main+tmp+TaskTbl;};
  {unid = DocUNID "7675623285716123^BmFldTest";
   title = "Project";
   docF = DocFld (FldString,"project",false,"Project");
   colSpan = 1;
   rowSpan = 1;
   Pos = (2, 0);
   lblFont =
    [Font: Name=DejaVu Sans, Size=26, Units=3, GdiCharSet=1, GdiVerticalFont=False];
   dataFont =
    [Font: Name=DejaVu Sans, Size=26, Units=3, GdiCharSet=1, GdiVerticalFont=False];
   foreCol = Some Color [Black];
   backCol = Some Color [Black];
   soopari = 1;
   vFn = null;
   CanUhear = Thingy "";
   fldTtip = null;
   tblTy = Main+tmp+TaskTbl;};
  {unid = DocUNID "7675623285716122^BmFldTest";
   title = "Title";
   docF = DocFld (FldString,"title",false,"Title");
   colSpan = 1;
   rowSpan = 1;
   Pos = (1, 0);
   lblFont =
    [Font: Name=DejaVu Sans, Size=26, Units=3, GdiCharSet=1, GdiVerticalFont=False];
   dataFont =
    [Font: Name=DejaVu Sans, Size=26, Units=3, GdiCharSet=1, GdiVerticalFont=False];
   foreCol = Some Color [Black];
   backCol = Some Color [Black];
   soopari = 1;
   vFn = null;
   CanUhear = Thingy "";
   fldTtip = null;
   tblTy = Main+tmp+TaskTbl;};
  {unid = DocUNID "7675623285716121^BmFldTest";
   title = "Document UNID";
   docF = DocFld (FldString,"unid",true,"Document UNID");
   colSpan = 1;
   rowSpan = 1;
   Pos = (0, 0);
   lblFont =
    [Font: Name=DejaVu Sans, Size=26, Units=3, GdiCharSet=1, GdiVerticalFont=False];
   dataFont =
    [Font: Name=DejaVu Sans, Size=26, Units=3, GdiCharSet=1, GdiVerticalFont=False];
   foreCol = Some Color [Black];
   backCol = Some Color [Black];
   soopari = 1;
   vFn = null;
   CanUhear = Thingy "";
   fldTtip = null;
   tblTy = Main+tmp+TaskTbl;}])
over & out ... 
*)