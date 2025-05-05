(*  Brij (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 
    TrivediBldHdr->
    |Trivedi SrcCtrlHdr File.  Copyright (c) 2015-2022 M. P. Trivedi.  All rights reserved.|637901455|test.fs|none|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|

    Provenance: Modified from async example 2 tinker with csvParsing async
    
    fsc src\pbt\csvTest.fs --platform:x64 --target:exe --out:csvTest.exe -r:lib\Trivedi.Core.dll -r:lib\Trivedi.CoreAux.dll -r:lib\Trivedi.UI.dll -r:lib\Trivedi.UIAux.dll -r:src\pbt\System.Net.Http.dll -I:C:\Windows\Microsoft.NET\Framework\v4.0.30319
*)


namespace Trivedi

#nowarn "20" "25" "58" "66" "67" "64" "760" "1182" "1558"

module csvTester = 

    open System
    open System.Net
    open Microsoft.FSharp.Control.WebExtensions
    open System.Net.Http
    open System.Text
    open System.IO

    let urlList = [ "Microsoft.com", "http://www.microsoft.com/"
                    "MSDN", "http://msdn.microsoft.com/"
                    "Bing", "http://www.bing.com"
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


    printfn "b4 csvParse call..."
    
    csvParse None None mini_fDef
    
    printfn "res: %A" res