(*  Brij.  HP. QP. (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 
    TrivediBldHdr->
    |Trivedi SrcCtrlHdr File.  Copyright (c) 2015-2025 M. P. Trivedi.  All rights reserved.|637901455|test.fs|none|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|

      PBT:   (last updated Jan 28th 2025)


  NOTES
- Model-based for Functionality
- Model-based for Invariance i.e.,
    brijOps like frmDzCellColorChg/FontChg/ValidationRuleChg in ANY order result in the same state
- No need to cover inputs, the types will do + take EXTRA care w/Gen in all above
- Create testMods based on 'Areas of Funct', not lib/ass
- The final test will consist of three comprehensive flows covering all areas of funct (@ToDo: see if we can compose earliers 2 bld this)
  (i)    DzChanges (All dzEls) -> DzCli
  (ii)   Db Funct (All areas) -> WebCli
  (iii)  Auth/UserMngt/SessMngt/Incl var subSites/MktgPlansOrCohorts
- The dzCli test will nd 2 be run on all Clis (@ToDo: bld all + @rsch emulators/virtual options 4 mac/linux)

  @RSCH
  * Model-based testing of reactive systems Manfred Broy, 2005
  "seminal paper on models":
  * Proof of correctness of data representations C. A. Hoare 1972
  * Using a Specification-based Model (The Z Spec. lang Mike Spivey)

  Tasks:

- To begin, setup/bld/run the example FsChk (note the ass ver#)
- Next, impl a desktop test
- For models, bld using fs structs.  Add fns like Saadu.toModel() (dvDz -> Model)

  Run is (Model -> Model)
  Check is (Model from Run === (ActualResults.toModel())) The return ty is Property

  Areas of Functionality:
    Desktop
    Table
    Form
    Grid  [both clis]
    Settings
    Other

uses wasmPhat_Jul18.exe frm iceDrive
uses nuget fschk: https://www.nuget.org/packages/FsCheck/3.0.1
bin\fsc.exe src\pbt\pbt.fs  --platform:x64 --target:exe --out:bin\pbt.exe -r:bin\FsCheck.dll -r:bin\Trivedi.Core.dll -r:lib\Trivedi.CoreAux.dll -r:lib\Trivedi.UI.dll -r:lib\Trivedi.UIAux.dll -I:C:\Windows\Microsoft.NET\Framework\v4.0.30319

*)

namespace Trivedi

#nowarn "20" "25" "58" "66" "67" "64" "760" "1182" "1558"


module ModuleRunner = 
    open System
    open FsCheck
    //v3 open FsCheck.FSharp

(*
see: https://fscheck.github.io/FsCheck//RunningTests.html#Running-tests-using-only-modules

FsCheck determines the intent of the function based on its return type:

Properties: public functions that return unit, bool, Property or function of any arguments 
to those types or Lazy value of any of those types. So myprop is the only property that is run; 
helper' also returns bool but is private.

Arbitrary instances: return Arbitrary<_>
All other functions are respectfully ignored.
*)

    printfn "init mod ModuleRunner..."

    let myprop (i:int) = i >= 0
    let mygen = ArbMap.defaults |> ArbMap.arbitrary<int> |> Arb.mapFilter (fun i -> Math.Abs i) (fun i -> i >= 0)
    let helper = "a string"
    let private helper' = true

    type Marker = class end
    let config = Config.QuickThrowOnFailure.WithArbitrary([typeof<Marker>.DeclaringType])
    Check.All(config, typeof<Marker>.DeclaringType)

    printfn "eom mod ModuleRunner..."

(*
module ModelExample =
    open System
    open FsCheck
    open FsCheck.FSharp
    open FsCheck.Experimental

    printfn "init mod ModuleRunner..."

    type Counter(?initial:int) =
      let mutable n = defaultArg initial 0
      member __.Inc() = 
        //silly bug
        if n <= 3  then n <- n + 1 else n <- n + 2
        n
      member __.Dec() = if n <= 0 then failwithf "Precondition fail" else n <- n - 1; n
      member __.Reset() = n <- 0
      override __.ToString() = sprintf "Counter = %i" n

    let spec =
      let inc = 
        { new Operation<Counter,int>() with
            member __.Run m = m + 1
            member __.Check (c,m) = 
                let res = c.Inc() 
                m = res 
                |> Prop.label (sprintf "Inc: model = %i, actual = %i" m res)
            override __.ToString() = "inc"}
      let dec = 
        { new Operation<Counter,int>() with
            member __.Run m = m - 1
            override __.Pre m = 
                m > 0
            member __.Check (c,m) = 
                let res = c.Dec()
                m = res 
                |> Prop.label (sprintf "Dec: model = %i, actual = %i" m res)
            override __.ToString() = "dec"}
      let create initialValue = 
        { new Setup<Counter,int>() with
            member __.Actual() = new Counter(initialValue)
            member __.Model() = initialValue }
      { new Machine<Counter,int>() with
        member __.Setup = Gen.choose (0,3) |> Gen.map create |> Arb.fromGen
        member __.Next _ = Gen.elements [ inc; dec ] }

      Check.Quick (StateMachine.toProperty spec)
*)
      printfn "eof..."