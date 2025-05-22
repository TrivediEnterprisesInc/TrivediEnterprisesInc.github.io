(*  Brij (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 

    Last updated: Wed May 17 2025
    The idea is to port the existing lexYacc stuff here & expand
    
    fsc src\pbt\lexYacc2Parsec.fs  --platform:x64 --standalone --target:exe --out:src\pbt\expParsec.exe -r:src\pbt\FParsecCS.dll -r:src\pbt\FParsec.dll -r:lib\Trivedi.Core.dll -I:C:\Windows\Microsoft.NET\Framework\v4.0.30319
    
    https://www.quanttec.com/fparsec/tutorial.html#parsing-alternatives
    
*)

namespace Trivedi

#nowarn "20" "25" "58" "66" "67" "64" "760" "1182" "1558"

[<AutoOpen>]
module IfThen_CQ =
    open System
    open FSharp.Quotations
    open Microsoft.FSharp.Linq.RuntimeHelpers
    open System.Linq.Expressions

    printfn "init mod IfThen_CQ..."

    let eval q = LeafExpressionConverter.EvaluateQuotation q

    let impFld = 5

    let gE = <@ (impFld > 6) @>
    let tE = <@ "High" @>
    let fE = <@ "Low" @>

    let q = <@ if (impFld > 6) then "High" else "Low" @>

    let q2 = <@ if (impFld = 5) then "Medium" else "Low" @>

    let exp = Expr.IfThenElse(gE, tE, fE) 

    let CompoundExp = Expr.IfThenElse(gE, tE, q2) 

    let runTests = 
        hr()
        printfn "now running tests in mod IfThen_CQ..."
        printfn "q:\n%A" q
        exp |> printfn "Expr:\n%A"        
        CompoundExp |> printfn "CompExpr:\n%A"        
        hr()
        printfn "results of evaluating the exp & CompoundExp ->"
        exp |> eval |> printfn "eval res:%A"
        CompoundExp |> eval |> printfn "CompoundExp eval res:%A"
        hr()

        
    #if PatternDiscriminatorNotFound

    match q with
    | IfThenElse (cond, trueE, falseE) -> printfn "Hello World!"
    | _ -> printfn "Hello!"

    #endif

[<AutoOpen>]
module Parsec_CQ =
    open FParsec

    type MathOp = | MathOp of string

    type Expression =
        | X
        | BrijFld of string
        | Constant of float
        | Add of Expression * Expression
        | Mul of Expression * Expression
        | MathExp of Expression * MathOp * Expression

    let rec interpret (ex:Expression) =
        match ex with
        | X -> fun (x:float) -> x
        | Constant(value) -> fun (x:float) -> value
        | BrijFld(value) -> fun (x) -> x
        | Add(leftExpression,rightExpression) ->
            let left = interpret leftExpression
            let right = interpret rightExpression
            fun (x:float) -> left x + right x
        | Mul(leftExpression,rightExpression) ->
            let left = interpret leftExpression
            let right = interpret rightExpression
            fun (x:float) -> left x * right x



    let test parser text =
        match (run parser text) with
        | Success(result,_,_) -> printfn "Success: %A" result
        | Failure(_,error,_) -> printfn "Error: %A" error

    let parseConstant = pfloat |>> Constant

    hr()
    printfn "testing parsing input..."

    printfn "test parseConstant 123.45"
    test parseConstant "123.45"
    printfn "test parseConstant nope"
    test parseConstant "nope"

    let parseVariable = stringReturn "x" X

    printfn """test parseVariable "x" """
    test parseVariable "x"
    
    printfn """test parseVariable "nope" """
    test parseVariable "nope"

    let parseBrijFld = 
        let normalCharSnippet = manySatisfy (fun c -> c <> '\\' && c <> ']' && c <> '"')
        pstring "[" >>. normalCharSnippet .>> pstring "]"
        |>> BrijFld

    printfn """test parseBrijFld "[Importance]" """
    test parseBrijFld "[Importance]"

    //printfn "add(1,[Importance])"
    //test parseBrijFld "add(1,[Importance])"

    let parseExpression = parseBrijFld <|> parseVariable <|> parseConstant

    hr()
    printfn "testing parseExpression..."

    test parseExpression "123.45"
    test parseExpression "x"
    test parseExpression "[Importance]"
    test parseExpression "nope"
    
    let parseExpressionsPair =
        between 
            (pstring "(")
            (pstring ")")
            (tuple2 
                (parseExpression .>> pstring ",") 
                parseExpression)

    let parseAddition =
        pstring "add" >>.
        parseExpressionsPair
        |>> Add

    let parseMultiplication =
        pstring "mul" >>.
        parseExpressionsPair
        |>> Mul

    let parseMathOp =
        (stringReturn "/"  (/)) <|> (stringReturn "-" (-))

    test parseMathOp "/"
    test parseMathOp "-"

(*   
    let parseMathExp =
        between 
            (pstring "(")
            (pstring ")")
            (tuple3
                parseExpression
                parseMathOp
                parseExpression)
                |>> MathExp
*)

    test parseAddition "add(1,x)"

    let fullParser = parseBrijFld <|> parseVariable <|> parseConstant <|> parseAddition <|> parseMultiplication //<|> parseMathExp

    type Program () =

        member this.Run (x:float,code:string) = 
            match (run fullParser code) with
            | Failure(message,_,_) -> 
                printfn "Malformed code: %s" message
            | Success(expression,_,_) ->
                let f = interpret expression
                let result = f x
                printfn "Result: %.2f" result

    let program = Program()

#if RunResults
    let code = "add(x,42)"
    program.Run(10.0,code)

    let code2 = "mul(x,x)"
    program.Run(10.0,code2)
#endif //RunResults    

    printfn "...eom..."