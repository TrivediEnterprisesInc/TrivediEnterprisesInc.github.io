(*  Brij (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 
    TrivediBldHdr->
    |Trivedi SrcCtrlHdr File.  Copyright (c) 2015-2025 M. P. Trivedi.  All rights reserved.|637901455|test.fs|none|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS 

    //minus UIAux
    fsc src\pbt\expr.fs  --platform:x64 --standalone --target:exe --out:src\pbt\expr.exe -r:lib\Trivedi.Core.dll -r:bin\FSharp.Compiler.Private.dll -I:C:\Windows\Microsoft.NET\Framework\v4.0.30319
    
    bin\runShell.exe "fsc.exe" "src\pbt\expr.fs --platform:x64 --standalone --target:exe --out:src\pbt\expr.exe -r:lib\Trivedi.Core.dll -r:FSharp.Compiler.Service.dll -I:C:\Users\inets\Desktop\mike\sdk\8.0.300-preview.24211.8\FSharp"

    FSharpChecker dox: https://fsharp.github.io/fsharp-compiler-docs/reference/fsharp-compiler-codeanalysis-fsharpchecker.html
    
    Last updated: Fri May 02 2025

    Contains modules:      Expr_Actual

    * Compile a <a href='https://fssnip.net/rP/title/WebSharper-compile-a-quotation-to-JavaScript'>quotation2Js</a>
    * Eval Quotations: https://fssnip.net/h1/title/Eval-Quotations

//dotnet fsi src\pbt\expr.fsx
https://queil.net/2021/05/embedding-fsharp-compiler/
*)

//namespace Trivedi

//#nowarn "20" "25" "58" "66" "67" "64" "760" "1182" "1558"

#r "FSharp.Compiler.Service.dll"

[<AutoOpen>]
module Expr_Actual =
    open System
    open FSharp.Compiler.CodeAnalysis
    open FSharp.Compiler.Diagnostics
    open FSharp.Compiler.Text
    //open Trivedi
    //open Trivedi.Core
    //open Trivedi.UI
    //open Trivedi.UIAux

    printfn "init mod Expr_Actual ..."

    let checker = FSharpChecker.Create()
    let code = """
    let add x y = x + y
    add 2 3
    """

    let fileName = "DynamicCode.fsx"
    let sourceText = SourceText.ofString code

    let parseResults = checker.ParseFileInProject(fileName, code, None)
    let checkResults = checker.CheckFileInProject(parseResults, fileName, 0, sourceText, [])

    match checkResults with
    | FSharpCheckFileAnswer.Succeeded res ->
        let expr = res.GetExpression("add 2 3")
        printfn "Result: %A" expr.ReflectionValue
    | _ -> printfn "Type checking failed."

    printfn "eom Expr_Actual ..."