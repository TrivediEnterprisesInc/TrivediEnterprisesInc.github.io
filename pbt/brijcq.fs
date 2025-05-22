
//two mods frm Brij.fs earlier work on cqs... 

    module CodeQuotations =
        open System
        open System.Text
        open System.Reflection
        open Microsoft.FSharp.Reflection
        open Microsoft.FSharp.Quotations
        open FSharp.Linq.RuntimeHelpers
        open Microsoft.FSharp.Quotations.Patterns
        open Microsoft.FSharp.Quotations.DerivedPatterns
        open Microsoft.FSharp.Quotations.ExprShape

        let eval q = LeafExpressionConverter.EvaluateQuotation q

        let add x y = x + y
        let mul x y = x * y

        let timeStmp = lazy (DateTime.Now)
        let strFromBarr (bArr:byte[]):string = (Encoding.UTF8).GetString(bArr, 0, bArr.Length)
        let strBytes (s: string) : byte[] = (Encoding.UTF8).GetBytes(s)
        type DocUNID = | DocUNID of string
        let getUNID (cls:string) : DocUNID =
            DocUNID(((timeStmp.Force().Ticks - (DateTime(2001, 1, 1)).Ticks).ToString() + "^" + cls))

        type Dat = | Dat of DocUNID * createdOn:DateTime * lastModifiedOn:DateTime * title:string * content:byte[] * tags:string * flag:string with
            override this.ToString() =
               let (Dat(DocUNID(unid), crDt, modDt, tit, cont, tags, flag)) = this
               "Dat obj created on: " + crDt.ToString() + " lastMod on: " + modDt.ToString() + "\n title:" + tit
                 + " content: " + (strFromBarr cont) + " tags: " + tags.ToString()

        type TgtVer = | TgtVer of string

        type TaskDoc = | TaskDoc of Dat * project:string * moduleNm:string * submodule:string * objective:string * importance:int * urgency:int * parent:string * completed:bool * completedOn:DateTime * tgtVer:TgtVer  * docLinks:string with
            override this.ToString() = 
             let (TaskDoc(d, pNm, modu, subM, obj, imp, urg, par, comp, dtComp, tgt, docL)) = this
             "TaskDoc obj for Proj: " + pNm + " Module: " + modu + "\n subModule: " + subM + " imp: " + imp.ToString() +
             " urg: " + urg.ToString() + " parent: " + par.ToString() + 
             "\n completeStatus: " + comp.ToString() + " completedOn: " + dtComp.ToString() + 
             "\n tgtVer: " + tgt.ToString() + " docRefs: " + docL + "\ndesc: " + d.ToString()
        //let taskToSStr (t:TaskDoc) = 
            //let (TaskDoc(d, pNm, modu, subM, obj, imp, urg, par, comp, dtComp, tgt, docL)) = t
            //(datToSStr d) @ [pNm; modu; subM; obj; imp.ToString(); urg.ToString(); par; comp.ToString(); dtComp.ToShortDateString(); tgt.ToString(); docL]
        let newD(id,cr,md,cont) = Dat(id,cr,md,"", cont, "", "")

        let dTest = newD(getUNID "tstClass",timeStmp.Force(),timeStmp.Force(),strBytes "http://fssnip.net/ld/title/Dynamic-opearator  Author:	Zhukoff Dima\nhttp://fssnip.net/2V/title/Dynamic-operator-using-Reflection	Author:	Tomas Petricek\nhttp://fssnip.net/gM/title/Walking-object-graphs	Author:	Eirik Tsarpalis")
        let t = (TaskDoc(dTest, "Test Project Name", "Test module", "Test SubMod", "Test obj", 5, 5, "Test par", false, timeStmp.Force(), TgtVer("Alpha 0.3"), "docLink1"))
        //printfn "%A" (t.ToString())

        let detail op expr =
            let rec print expr =
                match expr with
                | Application(expr1, expr2) ->
                    // Function application.
                    print expr1
                    printf " "
                    print expr2
                | NewUnionCase(unionCase, args) ->
                    printfn "\nexpr is: >>%A<<\n****newUnionCaseOp:: case: %A args: %A" expr unionCase args
                //| NewRecord(ty, args) ->
                    //printfn $"****newRecordOp:: ty: {ty} args: {args}" 
                | SpecificCall <@@ (+) @@> (_, _, exprList) ->
                    // Matches a call to (+). Must appear before Call pattern.
                    print exprList.Head
                    printf " + "
                    print exprList.Tail.Head
                | Call(exprOpt, methodInfo, exprList) ->
                    // Method or module function call.
                    match exprOpt with
                    | Some expr -> print expr
                    | None -> printf "CALL: %s" methodInfo.DeclaringType.Name
                    printf ".%s(" methodInfo.Name
                    if (exprList.IsEmpty) then printf ")" else
                    print exprList.Head
                    for expr in exprList.Tail do
                        printf ","
                        print expr
                    printf ")"
                | Int32(n) ->
                    printf "%d" n
                | Lambda(param, body) ->
                    // Lambda expression.
                    printf "fun (%s:%s) -> " param.Name (param.Type.ToString())
                    print body
                | Let(var, expr1, expr2) ->
                    // Let binding.
                    if (var.IsMutable) then
                        printf "let mutable %s (ty:%s) = " var.Name var.Type.Name
                    else
                        printf "\nLET expr is: >>%A<<\n\tlet (var1)%s (ty:%s) = " expr var.Name var.Type.Name
                    print expr1
                    printf " in (var2)"
                    print expr2
                | PropertyGet(_, propOrValInfo, _) ->
                    printf "~propG~%s" propOrValInfo.Name
                //| PropertySet(_, pi, args, v) ->
                    //printf "***propS***~ pi:%A args:%A v:%A" pi args v
                //| String(str) ->
                    //printf "~str~%s" str
                //| Value(value, typ) ->
                    //printf "~val~%s" (value.ToString())
                //| Var(var) ->
                    //printf "~var~%s" var.Name
                | _ -> printf "%s" (expr.ToString())
            printfn "detail -------->"
            print expr
            printfn "\n<---------"

    //	let (var1)fld3 (ty:String) = ~propG~fld3 in (var2)

        type Innr = | Innr of bool
        type Ty = | Ty of id:DocUNID * i:Innr * fld1:string * fld2:int * fld3:string
        let ob1 = Ty(getUNID "firstObject", (Innr(true)), "one", 1, "anotherOne")
        let ob2 = Ty(getUNID "secondObject", (Innr(false)), "two", 2, "anotherTwo")
        printfn "ob2 is: %A" (ob2)
        
        let quotEx = <@ let f x = x + 10 in f 20 @>
        quotEx |> eval |> printfn "res of quotEx:%A"
        
        let chuckliFl = 
            let (Ty(DocUNID(unidFld), Innr(iFld), fld1, fld2, fld3)) = ob2
            fld3

        let chuckliFF = 
            <@ let (Ty(DocUNID(unidFld), Innr(iFld), fld1, fld2, fld3)) = ob2
               fld1  @>
        //printfn "detail of chuckliFF (returns fld1)->"
        //detail "chuckliFF" chuckliFF

        let chuckliEx = <@ let chuckliFF = 
                                fun ob ->
                                     let (Ty(DocUNID(unidFld), Innr(iFld), fld1, fld2, fld3)) = ob
                                     fld1
                                     in chuckliFF ob2 @>
        
        let popatEx = <@ let popatF = 
                            fun ob ->
                             let (Ty(DocUNID(unidFld), Innr(iFld), fld1, fld2, fld3)) = ob
                             Ty(DocUNID(unidFld), Innr(iFld), fld1, fld2, fld3)
                             in popatF ob2 @>
        
        let propInfo =
             match <@ Console.BackgroundColor <- ConsoleColor.Red  @> with
             | PropertySet(None, pi, _, _) -> pi
             | _ -> failwith "property set expected"
        
        //Expr.PropertySet(propInfo, <@ ConsoleColor.Blue @>) |> eval |> printfn "res of propset:%A"

        let letT = <@ let updFld = 
                        fun tk ->
                             let (TaskDoc(Dat(DocUNID(unid), crDt, modDt, tit, cont, tags, flag), pNm, modu, subM, obj, imp, urg, par, comp, dtComp, TgtVer(tgt), docL)) = tk
                             TaskDoc(Dat(DocUNID(unid), crDt, modDt, tit, strBytes "new content", tags, flag), pNm, modu, subM, obj, imp, urg, par, comp, dtComp, TgtVer(tgt), docL)
                             in updFld t @>
        
        let res = letT |> eval
        //printfn "res of letT:%A" (res.ToString())
        //detail letT
        
        
        /// Traverse an entire quotation and use the provided function
        /// to transform some parts of the quotation. If the function 'f'
        /// returns 'Some' for some sub-quotation then we replace that
        /// part of the quotation. The function then recursively processes
        /// the quotation tree.
        let rec xFrmQuot f q = 
          let q = defaultArg (f q) q
          match q with
          | ExprShape.ShapeCombination(a, args) -> 
              let nargs = args |> List.map (xFrmQuot f)
              //printfn "ShapeComb: %A args:%A" a args
              ExprShape.RebuildShapeCombination(a, nargs)
          | ExprShape.ShapeLambda(v, body)  -> 
              //printfn "ShapeLambda: %A body:%A" v body
              Expr.Lambda(v, xFrmQuot f body)
          | ExprShape.ShapeVar(v) ->
              //printfn "ShapeVar: %A" v
              Expr.Var(v)

        let popat nm vl quot =
          quot |> xFrmQuot (fun q -> 
            match q with 
            | Patterns.Var(v) -> if v.Name = nm then
                                    Some(Expr.Value(vl))
                                  else 
                                    //printfn "Var: %A ty:%A" v (v.GetType())
                                    None
            | _ -> None) 

        let chuckli vNm ty quot =
          quot |> xFrmQuot (fun q -> 
            match q with 
            | Patterns.Let(var, expr1, expr2) ->
                                //printfn "let..."
                                if var.Name = "fld1" then
                                    //printfn "var inner............."
                                    let fl3 = Var("fld3", typeof<string>)
                                    //printfn "exp1: %A exp2:%A -----> " expr1 expr2
                                    //exp1: PropertyGet (Some (ob), fld1, []) exp2:fld1 ----->
                                    Some(Expr.Let(fl3, expr1, Expr.Var(fl3)))
                                  else 
                                    //printfn "Var: %A ty:%A" v (v.GetType())
                                    None
            | Patterns.Lambda(v, body)  -> 
                  //printfn "@@@@ShapeLambda: %A body:%A" v body
                  let fl3 = Var("fld3", typeof<string>)
                  if body = Expr.Var(Var("fld1", typeof<string>)) then
                    Some(Expr.Lambda(fl3, Expr.Var(fl3)))
                  else
                    None
            | Patterns.Var(v) -> if v.Name = "fld1" then
                                    //printfn "@@@Var:***"
                                    let fl3 = Var("fld3", typeof<string>)
                                    Some(Expr.Var(fl3))
                                  else 
                                    //printfn "@@@Var: %A ty:%A" v (v.GetType())
                                    None
            | _ -> None) 
            

        let run() =
            popatEx |> eval |> printfn "res of popat eval:%A"
            //detail "popat" popatEx
            popat "fld1" "popatFLD" popatEx |> eval |> printfn "res of popatX:%A"
            
            chuckliEx |> eval |> printfn "res of chuckli eval:%A"
            //detail "chuckli" chuckliEx
            chuckli "fld3" typeof<string> chuckliEx |> eval |> printfn "res of chuckliEx:%A"
            

#if flt
        //type Ty = | Ty of fld1:string * fld2:bool * fld3:string

        let flt1 (exp:Expr<Ty>): bool = 
            fun (x:Ty) -> 
                let (Ty(fld1, fld2, fld3)) = x
                %exp

        let flt1 = fun (x:Ty) -> 
                    let (Ty(fld1, fld2, fld3)) = x
                    fld2 = false

        let flt2 = fun (x:Ty) -> 
                    let (Ty(fld1, fld2, fld3)) = x
                    fld1.Contains("test")

        let ob = Ty("jsting", false, "not a fld")

        let exp1 = (flt1 ob)
        let exp2 =  (flt2 ob)

        let combAnd e1 e2 = e1 && e2
        let combOr e1 e2 = e1 || e2
        let combNot e1 = not e1

        printfn "res1:%A" (combAnd exp1 exp2)
        printfn "res2:%A" (combOr exp1 exp2)
        printfn "res3:%A" (combNot exp2)
#endif //flt

        run()