(*  Brij (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 
    
    IDeployable

output:
  hey from FrmDz_v0
  res: Main+FrmDz_v0
  hey from FrmDz
  res: Main+FrmDz
  ...eom...

*)

type IA<'T> =
    abstract member Get : unit -> 'T

type MyClass() =
    interface IA<int> with
        member x.Get() = 1
    //doesn't work on glot's version but IS OK if generics nd.ed
    //interface IA<string> with
        //member x.Get() = "hello"

type IDeployable =
    abstract member DeplDict: Map<(string * string), bool>
    abstract member setupDeplDict: unit -> unit
    abstract member updDeplDict: slg:string * msg:string -> unit
    abstract member updStatusWids: unit -> unit
    abstract member genDeplRpt: unit -> unit

type Params = | Params of (string * string * int)

type FrmDz_v0(p:Params) as this =
    do printfn "hey from FrmDz_v0"
    let thisT = this
    do printfn "res: %A" this

type FrmDz(p:Params) as this =
    do printfn "hey from FrmDz"
    let thisT = this
    do printfn "res: %A" this
    interface IDeployable with
        member this.DeplDict = Map.empty<(string * string), bool>
        member this.setupDeplDict() = 
            printfn "in this.setupDeplDict..."
        member this.updDeplDict(slg, msg) = 
            printfn "in this.updDeplDict for %A and %A ..." slg msg
        member this.updStatusWids() = 
            printfn "in this.updStatusWids..."
        member this.genDeplRpt() = 
            printfn "in this.genDeplRpt..."


("one", "two", 3) |> Params |> FrmDz_v0 |> ignore

("one", "two", 3) |> Params |> FrmDz |> ignore

printfn "...eom..."
