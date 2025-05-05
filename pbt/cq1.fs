(*
see: SO: Composing F# code quotations programmatically
https://stackoverflow.com/questions/70537490/composing-f-code-quotations-programmatically
*)

open System
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Linq.RuntimeHelpers
open System.Linq.Expressions

type Type() = 
    member _.Prop1 with get() = 1
    member _.Prop2 with get() = 2

let toFunc<'t when 't :> Type>(filter: 't -> Expr<bool>) =
    let xp = <@ Func<'t, bool>(fun (t: 't) -> %(filter t) && t.Prop2 = 2) @>
    LeafExpressionConverter.QuotationToExpression xp |> unbox<Expression<Func<'t, bool>>>

let getFunc (i: int) = 
    let filter (t: Type) = <@ t.Prop1 = i @>
    toFunc filter

//throws @ let xp...; suggested delta:
//let xp = <@ Func<'t, bool>(fun (t: 't) -> (%filter) t && t.Prop2 = 2) @>