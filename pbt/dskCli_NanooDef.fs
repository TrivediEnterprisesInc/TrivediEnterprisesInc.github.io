(*  Brij (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 
    TrivediBldHdr->
    |Trivedi SrcCtrlHdr File.  Copyright (c) 2015-2025 M. P. Trivedi.  All rights reserved.|637901455|test.fs|none|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS 

    //minus UIAux
    fsc src\pbt\Dnd_ops.fs  --platform:x64 --standalone --target:exe --out:src\pbt\dnd.exe -r:lib\Trivedi.Core.dll -r:lib\Trivedi.UI.dll -I:C:\Windows\Microsoft.NET\Framework\v4.0.30319

    Last updated: Tue Jul 02 2025

    Contains modules:      NanooDef_Actual | NanooDef_Test

*)

namespace Trivedi

#nowarn "20" "25" "58" "66" "67" "64" "760" "1182" "1558"


[<AutoOpen>]
module NanooDef_Actual =
    open System
    open System.Drawing
    open System.Windows.Forms
    open Trivedi
    open Trivedi.Core
    //open Trivedi.UI
    //open Trivedi.UIAux
    open System.Windows.Forms

    type NanooMasalo<'t when 't :> ITblMarker> = | NanooMasalo of unid:DocUNID * dispNm:string * isImgVw:bool * titleFld:DocFld * cntrFld:DocFld * botFld:DocFld * usrDefDataFont:Font * usrDefForeColor:Color * usrDefBackColor:Color * docInf:DesDocInf with
        static member toString() = 
            printfn "placeholder..."

    type NanooPaan<'t when 't :> ITblMarker>(d) = 
        inherit Form(WindowState = FormWindowState.Maximized, Visible = false, Text = "Dz Form: Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved.", TopMost=true)
        do printfn "NanooPaan ctor..."
        member val currDef = d with get, set
        interface IDef with
            member this.fromDef() = 
                this.currDef <- fnFromDef this.currDef
            member this.toDef() = 
                this.currDef <- fnToDef this.currDef

