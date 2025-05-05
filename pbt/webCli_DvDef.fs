(*  Brij (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 
    TrivediBldHdr->
    |Trivedi SrcCtrlHdr File.  Copyright (c) 2015-2025 M. P. Trivedi.  All rights reserved.|637901455|test.fs|none|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|

    fsc src\pbt\pbt_AI_Dsk.fs src\pbt\pbt_Dsk.fs  --platform:x64 --standalone --out:src\pbt\Trivedi.PbtDsk.dll -r:lib\Trivedi.Core.dll -r:lib\Trivedi.CoreAux.dll -r:lib\Trivedi.UI.dll -r:lib\Trivedi.UIAux.dll -I:C:\Windows\Microsoft.NET\Framework\v4.0.30319

    Provenance: Modified from loggedUIRnrDec20_FrmDz
    to incorp modified  UI els 4 pbt
    
    This ver uses Trivedi.UI_Nov02_Color+Grids.dll + UIAux + ...

    Last updated: Fri Apr 7 2025

    Contains modules:  DvDef_Actual
                                DvDef_Ext
                                DvDef_Test
                                
    Notes;
        - The abstract model is v. few recs (~10) a subset via Qry
        - The Model will be a list<str> comme: "|Module|SubMod|...|"
        - The Ext will have a .toModel() which uses HTTPCli 2 fetch & convert 2 above 4 chks
                                
*)

namespace Trivedi

#nowarn "20" "25" "58" "66" "67" "64" "760" "1182" "1558"
