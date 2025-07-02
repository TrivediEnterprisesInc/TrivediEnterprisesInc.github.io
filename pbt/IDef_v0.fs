(*  Brij (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 
    TrivediBldHdr->
    |Trivedi SrcCtrlHdr File.  Copyright (c) 2015-2025 M. P. Trivedi.  All rights reserved.|637901455|test.fs|none|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS 

    <<glot>>
    Last updated: Tue Jul 01 2025
                  Wed Jul 02: ren 2 v0, began new version (see below)

  Note: F# doesn't allow interface default mems; 
        so we're going 2 use a fn call instd
        Jul02: 
        - Fld-lvl def upd8s are overkill; no utility whatsoever
        - We only nd 2 gather (onLoad) and save (onSave/onSaveAs/onPreview)
        - No push 2 svr on prev
        - Redo the mechanics; bld one impl (p'haps cDef: nds Def)
*)
namespace Trivedi

#nowarn "20" "25" "58" "66" "67" "64" "760" "1182" "1558"

[<AutoOpen>]
module IDef =

open System
open System.Windows.Forms

type IDef  =
    abstract member fromDef: unit -> unit
    abstract member toDef: unit -> unit

let fnFromDef = 
    fun d ->
        printfn "in fnFromDef, recd: %A" d
        d + ":"

let fnToDef = 
    fun d ->
        printfn "in fnToDef, recd: %A" d
        d + ":"

let updateDef =
  fun d slg t ->
    let existingVal = getVal d slg
    match exitingVal = t with
    | true -> d
    | _ -> d + ":t"

let addCtrl = 
    fun slg c f d ->
        !!^ [slg, (box c)] f
        //Note: all controls have this handler (Test, however) and this shd work as long as we've covered all poss matches
        c.Leave.AddHandler (new EventHandler( fun o e ->
                match o with
                | ListView as lv -> lv.SelectedItems |> Seq.cast |> List.ofSeq |> li2str |> updateDef d slg
                | RadioButton as rb -> 
                    match rb.Checked with
                    | true -> rb.Text |> updateDef d slg
                    | _ -> ()
                | _ -> printfn "ERR: Unknown control encountered; nds matchCase in addCtrl to register changes 2 def"
                ))

type ThingyOne(d:string) = 
    //inherit Form(WindowState = FormWindowState.Maximized, Visible = false, Text = "Dz Form: Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved.", TopMost=true)
    do printfn "ThingyOne ctor..."
    member val currDef = d with get, set
    interface IDef with
        member this.fromDef() = 
            this.currDef <- fnFromDef this.currDef
        member this.toDef() = 
            this.currDef <- fnToDef this.currDef

type ThingyTwo(d:string, i:int) = 
    //inherit Form(WindowState = FormWindowState.Maximized, Visible = false, Text = "Dz Form: Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved.", TopMost=true)
    do printfn "ThingyTwo ctor..."
    member val currDef = d with get, set
    interface IDef with
        member this.fromDef() = 
            this.currDef <- fnFromDef this.currDef
        member this.toDef() = 
            this.currDef <- fnToDef this.currDef

let b = new Button(Text="button")

let one = ThingyOne "defOne"
let two = ThingyTwo ("defTwo", 99)
(one :> IDef).fromDef()
(one :> IDef).toDef()
(two :> IDef).fromDef()
(two :> IDef).toDef()

printfn "eom..."
