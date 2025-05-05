(***  
        run with: dotnet fsi src\pbt\pbt.fsx
        Output: 
Ok, passed 100 tests.
95% long sequences (>6 commands).
5% short sequences (between 1-6 commands).
***)

#r @"FsCheck.dll"
#r @"Trivedi.Core.dll"
#r @"Trivedi.PbtDsk.dll"
#r @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Windows.Forms.dll"
open FsCheck
open FsCheck.FSharp
open FsCheck.Experimental
open System
open System.IO
open Trivedi.Core
open Trivedi

printfn "...now loading customizations..."

type ITblMarker = interface end
type ArticleTbl() = interface ITblMarker
type AdminTbl() = interface ITblMarker
type TaskTbl() = interface ITblMarker


let baseIconsLi = [("articles.png", box (ArticleTbl()) ,"Article Tbl")]

let _baseIconsLi = [("articles.png", box (ArticleTbl()) ,"Article Tbl");
              ("ide.png", box (ArticleTbl()) ,"Ide");
              ("settings.png", box (AdminTbl()) ,"Settings");
              ("tasklist.png", box (TaskTbl()) ,"TaskList Tbl");
              ("new.png", box (ArticleTbl()) ,"Add New Icon...");
              ("music.png", box (ArticleTbl()) ,"MusicTbl")]

let additionalIconsLi = 
  [("database.png", box(ArticleTbl()), "Amazon DynamoDB");("database.png", box(ArticleTbl()), "Microsoft SQL Server");("database.png", box(ArticleTbl()), "MySQL");("database.png", box(ArticleTbl()), "PostgreSQL");("database.png", box(ArticleTbl()), "MongoDB");("database.png", box(ArticleTbl()), "Oracle Database");("database.png", box(ArticleTbl()), "SQLite");("database.png", box(ArticleTbl()), "Cassandra");("database.png", box(ArticleTbl()), "Firebase Realtime Database");("database.png", box(ArticleTbl()), "MariaDB");("database.png", box(ArticleTbl()), "Amazon Redshift");("database.png", box(ArticleTbl()), "Couchbase")]

let philos = 
  [ "Aristotle"; "Socrates"; "Epictetus"; "Seneca"; "Hegel"; "Russell"; "Wittgenstein"; "Rufus"; "Aurelius" ]

let chooseFrmLi xs = 
  gen{ 
      printfn "...in chooseFrmLi  %A" (List.length xs - 1)
      let! i = Gen.choose(0, List.length xs - 1)
      return List.item i xs }

let addl = additionalIconsLi

type DskWrapper(initial:list<string*obj*string>) as d =
    let mutable currState = initial
    member __.AddIcn(itm) = 
        Console.ForegroundColor <- ConsoleColor.Yellow
        //printfn "AddIcn(%A) actualState b4: %A" r currState
        currState <- currState @ ([itm])
        //printfn "AddIcn(%A) actualState after: %A" r currState
        Console.ResetColor()
        //hr()
        d
    member __.RemIcn(itm) = 
        Console.ForegroundColor <- ConsoleColor.Green
        //printfn "RemIcn(%A) actualState b4:\n %A" r currState
        let ll = List.length currState
        match ll <= 0 with
        | true -> failwithf "Precondition fail"
        | _ ->
        currState <- List.except [itm] currState
        //printfn "RemIcn(%A) actualState after:\n%A" r currState
        Console.ResetColor()
        //hr()
        d
    member __.ChangeLbl(n, p) = 
      let itm = currState.[n]
      let (str, o, str2) = itm
      let u = (str, o, p)
      currState <- List.updateAt n u currState
    member __.Reset() = 
      currState <- baseIconsLi
      d
    member __.toModel() = 
      currState
    override __.ToString() = "DskWrapper = ...%i n"
    
let addIcn itm = 
    { new Operation<DskWrapper,list<string*obj*string>>() with
        member __.Run m = 
            Console.ForegroundColor <- ConsoleColor.Cyan
            //printfn "addIcn() modelState b4:\n %A" m
            let res = m @ ([itm])
            //printfn "addIcn() modelState after:\n %A" res
            Console.ResetColor()
            //hr()
            res
        member __.Check (d,m) = 
            let res = (d.AddIcn(itm)).toModel()
            //(????_???_Nov<'a>()).dskSnap()
            m = res
            |> Prop.label (sprintf "AddIcn: model = %A, actual = %A" m res)
        override __.ToString() = "addIcn"}

let remIcn (itm:string*obj*string) = 
    { new Operation<DskWrapper,list<string*obj*string>>() with
        member __.Run m = 
            Console.ForegroundColor <- ConsoleColor.Red
            //printfn "remIcn() modelState b4:\n %A"  m
            let res = m |> List.except (Seq.singleton itm) 
            //printfn "remIcn() modelState after:\n %A"  res
            Console.ResetColor()
            //hr()
            res
        override __.Pre m = 
            List.length m > 1
        member __.Check (d,m) = 
            let res = (d.RemIcn(itm)).toModel()
            //(????_???_Nov<'a>()).dskSnap()
            m = res 
            |> Prop.label (sprintf "RemIcn: model = %A, actual = %A" m res)
        override __.ToString() = "remIcn"}


let create initialValue = 
    { new Setup<DskWrapper,list<string*obj*string>>() with
        member __.Actual() = ((DskWrapper(initialValue)))
        member __.Model() = initialValue }

//added Mar 1st
type TearDownDsk<'Actual>() =
    inherit TearDown<'Actual>()
    override __.Actual actual = 
        printfn "in TearDownDsk..."

//added Mar 3rd
let DskStateMachine =
  { new Machine<DskWrapper,list<string*obj*string>>() with
      member __.Setup = Gen.constant(_baseIconsLi) |> Gen.map create |> Arb.fromGen
      member __.Next thisM = 
        Gen.frequency [ (2, gen{  
                                            let! a = chooseFrmLi addl
                                            return (addIcn a) } ); 
                                (1, gen{    
                                            let! r = chooseFrmLi thisM
                                            return (remIcn r) } );
(*                                (1, gen{    
                                            let! n = Gen.choose(0, lilen thisM)
                                            let! p = chooseFrmLi philos
                                            return (ChangeLbl n p) } )                                
*)
                                ] 
      override __.TearDown = TearDownDsk<'Actual>()
        }

printfn "now runing check..."

Check.Quick (StateMachine.toProperty DskStateMachine)



