(*  Brij (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 
    TrivediBldHdr->
    |Trivedi SrcCtrlHdr File.  Copyright (c) 2015-2025 M. P. Trivedi.  All rights reserved.|637901455|test.fs|none|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|

    fsc src\pbt\pbt_AI_Dsk.fs src\pbt\pbt_Dsk.fs  --platform:x64 --standalone --out:src\pbt\Trivedi.PbtDsk.dll -r:lib\Trivedi.Core.dll -r:lib\Trivedi.CoreAux.dll -r:lib\Trivedi.UI.dll -r:lib\Trivedi.UIAux.dll -I:C:\Windows\Microsoft.NET\Framework\v4.0.30319
   
    Edit History:
        Created: Thu Jun 26th
    
    Contains modules:  gg_Actual
                       //tibbie gg_Test
                      
*)

namespace Trivedi

#nowarn "20" "25" "58" "66" "67" "64" "760" "1182" "1558"

    [<AutoOpen>]
    module Gg_Actual = 

(*  from BrijSvr.fs; updated for gg (look for asterisks)

    Note Jul01_25: @Chk kathoHandlerAux in z_loggedUI_webCliFrm mainPlnk


    tbfo(svrSide): chunoHandlerSvr|supariHandlerSvr|lovelyHandlerSvr
let kathoHandlerSvr =
    fun cmd categ def paanTyp ->
        let (CalcuttiMasaloSvr(nm, tblDef, docInf, colCellFont, colHdrs, visCols, fldLi, fixedSz, fltr, categBy, sortBy, openCategs, exSt, rowTips, Ttips, pOpts)) = def
        let (SaadoMasaloSvr(tblNm, flds:DocFld list, dskIcn, tblTy)) = tblDef
        match paanTyp with
        | "calcutti" ->
            match def.GetType().GenericTypeArguments.[2] with
            | MusicTbl -> 
                let newCats = 
                    match cmd with
                    | "kathoCalcuttiCategExpando" ->
                        match exSt with
                        | eAll ->
#if tbfo
                            (getDatSvr def fldLi)
                            |> peru (fun ~ ~ ->
                                        let ro:list<_> = r
                                        let isC:bool = unbox r.[(ro.Length) - 2]
                                        match isC with
                                        | true -> Some(unbox r.[(ro.Length) - 1])
                                        | _ -> None ) |> flat
#endif //tbfo
                            []
                        | eNone -> []
                        | eUserSel -> openCategs
                    | _ ->
                        if List.contains categ openCategs then (List.except [categ] openCategs) else  (categ :: openCategs)
                let newDef = CalcuttiMasaloSvr(nm, tblDef, docInf, colCellFont, colHdrs, visCols, fldLi, fixedSz, fltr, categBy, sortBy, newCats , exSt, rowTips, Ttips, pOpts)
                let newD = 
                    lim(fun r ->
                        let ro:list<_> = r
                        let isC:bool = unbox r.[(ro.Length) - 2]
                        let prnt:string = unbox r.[(ro.Length) - 1]
                        if isC then Some(r)
                            else 
                            match cmd with
                            | "kathoCalcutti" -> None
                            | "kathoCalcuttiCategExpando" ->
                                if (not (List.contains prnt newCats)) then None
                                    else Some(r)
//** remove below (Alexis)
                            | _ ->  tibbie ("unknown svr Cmd (non-kathoCalcutti)" + cmd)
                                    None    ) (getDatSvr newDef flds)
                    |> List.filter (fun (x:option<_>) -> x.IsSome) |> List.map (fun x -> flattenOb x)
                newDef, newD
            | TaskTbl -> 
                tibbie "taskTbl Req recd"
                def, (getDatSvr def flds)
//** remove below (Alexis)
            | _ -> 
                tibbie "Brij: kathoHandler: updDat: unknown tblType encountered"
                def, (getDatSvr def flds)
//** remove below (Alexis)
        | _ -> 
            tibbie ("Brij: kathoHandler: updDat: unknown paanType encountered" 
                + crlf + "cmd:" + cmd 
                + crlf + "categ:" + categ
                + crlf + "paanTyp:" + paanTyp)
            def, (getDatSvr def flds)
//**
    each of the matches above returns (newDef, newDat)
    AMEND that by preceding w/
    1. let finalDat = 
    2.    processCalcFlds newDef newDat
    3.    |> gg cmd paanTyp newDef 
    4. newDef, finalDef

    let svrReqSvr = 
        fun cmd categ def ->
            match cmdTy cmd with
            | "chuno" -> chunoHandlerSvr cmd categ def (paanTy cmd)
            | "katho" -> kathoHandlerSvr cmd categ def (paanTy cmd)
            | "supari" -> supariHandlerSvr cmd categ def (paanTy cmd)
            | "lovely" -> lovelyHandlerSvr cmd categ def (paanTy cmd)
//** remove below (Alexis)
            | _ -> 
                tibbie "svr: unknown cmdTy recd"
                //can't really return this, tbfo
                chunoHandlerSvr cmd categ def (paanTy cmd)
*)

    //tbfo; belongs 2 RegMod, complete l8r, needed below
    //Aug03_25: These types updated (type TblAclTpl/FldAclTpl; see dskCli_SecurityPg_Rediz.fs)
    type NabMember = | NabGrp
                     | NabRole
                     | NabUser
    type TopLvlGg = | EveryBody
                    | EveryRegisteredUser
                    | Nobody
    type GgSwitch = | AndSwitch
                    | ExceptSwitch
    type ggTbl = | ggTbl of TopLvlGg * GgSwitch * list<NabMember>

    let gg = 
        fun cmd paanTyp def dat ->
            match paanTyp with
            | "calcutti" ->
                let (CalcuttiMasaloSvr(_, tblDef, _, _, ggTbl, ... )) = def
                let (SaadoMasaloSvr(tblNm, flds:DocFld list, dskIcn, tblTy)) = tblDef
                let (ggTpl(topLvl, sw, nabLi)) = ggTbl
                let gpLi, rLi = getEffAcc uId tblTy
                let firstPass = 
                    match topLvl with
                    | EveryBody -> OK(dat)
                    | EveryRegisteredUser -> 
                        match (isRegUsr uId) with
                        | true -> OK(dat)
                        | _ -> Error("User Not in NAB")
                    | Nobody -> Error("Insufficient Access Rights")
                let secPass = 
                    match firstPass, sw with
                    | Ok x, ExceptSwitch -> 
                        match (chkUsrNabLi uId nabLi) with
                        | true -> firstPass
                        | _ -> Error("Insufficient Access Rights")
                    | _ -> firstPass
                applyGg secPass Dat

    //we nd a similar fn for the fldLvlGg proc; OR b8r, repurpose above to run in a rec (for 2 passes) and spit out the dat.  FldLvl tpl shd take the same form, yeah?

(*  Jul 31st:
    @TBFO: Upcoming: For RecycleVw 
        AutoCreate Immed. AFTER 1st DV
            dvDefSaveHandler.add
            match isThisFirstDV with
            | true -> autoCreateRecycleBinDV(d)
            | _ -> ()
   - @TBD: Allow dev 2 specify which DV to use here (ie override)
     OR they can just edit it like the others
*)
    let autoCreateRecycleBinDV = 
        fun def:dvDef -> 
            //basically copy all defItems and modify a canned RecycleDV, saveToDz
