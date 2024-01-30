module bracketingAux =
    open System

/*
Gen:
    * Rnd(Flex, Durability) 0-9 >> uniq >> sort
    * Resx 
    getLiByNm >> AddTpl(Flex/~) >> ChangeTpl
    getRanges >> Split ~ 8x4 >> assign()

    @chk: windows does rev? (pretty sure it does)
    @chk: what changed in the replIt windowing specifically? Can we incorp existing?

let Manual_State_Test =
    tpl 
    |> A{
            let! tr = fst 
            let! match(,,) = snd
            let flag = true
            while flag do {
                do putS ((tr + 1), match)
                let thisRun = getTry(match, fst)
                log$('run #: {tr + 1} res: {thisRun}')
                if thisRun = true then flag = false
            }
        }

*/


    //res: len:100 
    let allPairs = [2,7; 9,5; 5,6; 8,5; 5,9; 3,2; 8,4; 2,8; 0,6; 0,8; 1,5; 4,5; 2,9; 0,2; 1,6; 5,3; 6,8; 3,5; 4,8; 5,2; 6,7; 4,6; 2,5; 1,2; 3,0; 9,2; 5,5; 4,0; 1,7; 2,3; 9,0; 4,7; 1,0; 5,0; 0,7; 8,2; 9,8; 2,1; 3,9; 3,6; 0,1; 5,8; 6,0; 2,4; 7,8; 9,1; 4,1; 8,8; 7,2; 4,9; 0,3; 6,3; 7,5; 8,3; 0,0; 7,7; 0,5; 3,3; 1,8; 5,7; 3,8; 7,6; 8,9; 9,4; 5,4; 9,3; 7,0; 1,3; 3,1; 7,1; 2,2; 0,9; 8,0; 0,4; 4,4; 9,9; 7,4; 2,6; 7,3; 3,4; 9,7; 3,7; 2,0; 9,6; 1,9; 4,3; 1,4; 6,4; 7,9; 6,5; 6,9; 8,1; 6,2; 4,2; 5,1; 6,6; 8,7; 6,1; 8,6; 1,1]

    let pickN = 
        fun n -> 
            List.fold(fun s v -> s.[(new Random()).Next(0, (s.Length - 1))]) xc [0..(n-1)]

    let testState = (false, true, false)
    let runMatch =
        fun inp -> 
            printfn "one:"
            printfn "two:"
            printfn "three:"
            if inp = testState then true else false

    //9 vals
    let allPossVals = 
        [0..100]
        |> List.map (fun x -> 
                        let troothy() = 
                            let thisT = (new Random()).Next(0,2)
                            match thisT with
                            | 0 -> true
                            | _ -> false 
                        (troothy(), troothy(), troothy()))
        |> List.distinct 


    allPossVals
    |> List.except([(false, true, false)] |> Seq.ofList)
    |> List.map(fun x -> runMatch x)
    |> ignore

