(*  Brij (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 

    Last updated: Wed May 21 2025
    The idea is to port the existing lexYacc stuff here & expand
    
    fsc src\pbt\dskCli_ExprParser.fs --platform:x64 --standalone --target:exe --out:src\pbt\expParsec.exe -r:src\pbt\FParsecCS.dll -r:src\pbt\FParsec.dll -r:lib\Trivedi.Core.dll -r:lib\Trivedi.CoreAux.dll -I:C:\Windows\Microsoft.NET\Framework\v4.0.30319
    
    https://www.quanttec.com/fparsec/tutorial.html#parsing-alternatives

5/19:
- Consider adding opt4Web (2 return JsTxt for FrmDz) 2 ea path
- pbt: add hardCoded Map<Key, errMsg> 2 rtn in Parsing (/0 etc)
    
5/20:
- AgentBldr mod: 
   Trigger on FldVal Condition -> onSaveHandler -> 
   Abstract: chngVal + ret logTxt
   Actual: inject logTxt to ret (if env = PBT)

*)

namespace Trivedi

#nowarn "20" "25" "58" "66" "67" "64" "760" "1182" "1558"

[<AutoOpen>]
module IfThen_CQ =
    open System
    open System.Diagnostics
    open FSharp.Quotations
    open Microsoft.FSharp.Linq.RuntimeHelpers
    open System.Linq.Expressions
    open Trivedi.CoreAux

    printfn "init mod IfThen_CQ..."

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

    printfn "...eom mod IfThen_CQ..."

[<AutoOpen>]
module Parsec_CQ =
    open FParsec

    printfn "...init module Parsec_CQ"

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
        //we don't want to ret the fn coz we're gonna match l8r
        //(stringReturn "/"  (/)) <|> (stringReturn "-" (-))
        pstring "/" <|> pstring "-"
        |>> MathOp

    let parseMathExp =
        between 
            (pstring "(")
            (pstring ")")
            (tuple3
                parseExpression
                parseMathOp
                parseExpression)
            |>> MathExp

    hr()
    printfn "now testing  parseMathExp..."
    test parseMathExp "([Importance]/3)"

    test parseAddition "add(1,x)"

    let fullParser = parseBrijFld <|> parseVariable <|> parseConstant <|> parseAddition <|> parseMultiplication <|> parseMathExp

#if dslTypes
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

    let code = "add(x,42)"
    program.Run(10.0,code)

    let code2 = "mul(x,x)"
    program.Run(10.0,code2)
#endif //dslTypes

    printfn "...eom mod Parsec_CQ..."
    
module CondBldr_Test = 
    open System
    open System.Drawing
    open System.Windows.Forms
    open Trivedi
    open Trivedi.Core

(*
  - Begin w/CondBldr: Actual runs on (mTpl -> bool)
  - Use sample dataSet 4 both (MusicTbl? nds work)
  - 4 Abstract use CQs; create bldrs 2 gen 4 ea CondFlavor
CondBldr: consider adding 
    (1) strNear (x,y) for within fixed (200) char
    (2) chkBox for caseIgnore
    Note that in certain circs this'll lead to 5tpls...
- CondBldr mod:
  Abstract: Test DataSet getRndRec + getRndFld + getRndCrit (basedOnFld) + getRndOp + getRndParam
  Actual: getRndRec (same DataSet; pump recId/skipNum from Abstract2Act + pump param) -> run 
    on the raw vals (not fldVars)
*)

    printfn "...init module CondBldr_Test"

    let chooseFrmLi_Idx xs = 
        gen{ 
              printfn "...in chooseFrmLi  %A" (List.length xs - 1)
              let! i = Gen.choose(0, List.length xs - 1)
              return (List.item i xs, i) }
          
    let opsLi = 
        fun fld ->
            let DocFld(fty, _, _, _) = fld
            //predConds currKeys: document/string/date/number/boolean
            Map.find fTy predConds

    let getFloat = ArbMap.defaults |> ArbMap.generate<float>
            
    let runCond cRec cFld cFldIdx cOp cParam = 
        { new Operation<CondWrapper, CondAbstractM>() with
            member __.Run m =  runCond (fldVal opF param)
            member __.Check (f,m) = f.runCond(cRec, cFld, cFldIdx, cOp, cParam)).toModel()
                |> Prop.label (sprintf "runCond: model = %A, actual = %A" m res)
            override __.ToString() = "runCond"}

(*
    let create initTpl = 
        { new Setup<CondWrapper, CondAbstractM>() with
            member __.Actual() = ((CondWrapper(initTpl)))
            member __.Model() = initTpl }

    type TearDownDsk<'Actual>() =
        inherit TearDown<'Actual>()
        override __.Actual actual = 
            pbt.Dsk(( (FrmWrapper) actual).toModel())
*)
 
    //consider switching betw Music+tkTbl?
    let initModel = MusicFldList() |> bld |> tblAbstractMod
    
    let CondStateMachine =
      { new Machine<CondWrapper, CondAbstractM>() with
          member __.Setup = Gen.constant(initModel)  |> Arb.fromGen
          member __.Next thisM = 
            Gen.Constant <| gen{  
                                            let! recd = chooseFrmLi thisM
                                            let! fld, fIdx = chooseFrmLi_Idx MusicFldList()
                                            let! opF = chooseFrmLi (opsLi fld)
                                            let! param = getFloat
                                            return (runCond recd fld fIdx opF param) 
                                            }
          //override __.TearDown = TearDownDsk<'Actual>()
            }
            
    
    printfn "now runing check..."
    
    Check.Quick (StateMachine.toProperty CondStateMachine)

    printfn "...eom mod CondBldr_Test..."

