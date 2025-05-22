(*  Brij (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 

    Last updated: Thu May 22 2025
    
    Taking the CSV Provider for a spin...
    
    fsc src\pbt\csvProc.fs --platform:x64 --standalone --target:exe --out:src\pbt\csvProc.exe -r:Trivedi.Core.dll -I:C:\Windows\Microsoft.NET\Framework\v4.0.30319
    
    bin: https://www.nuget.org/packages/FSharp.Data#versions-body-tab
    dox: https://yukitos.github.io/FSharp.Data/library/CsvProvider.html
    test: https://github.com/fsprojects/FSharp.Data/blob/main/tests/FSharp.Data.Tests/CsvProvider.fs
*)

namespace Trivedi

#nowarn "20" "25" "58" "66" "67" "64" "760" "1182" "1558"

[<AutoOpen>]
module CsvParse =
    open System
    //open FSharp.Data
    open System.IO
    open System.Text
    open Trivedi.Core
    
    printfn "...init mod csvParse..."

#if fsharpData
    let csv = CsvProvider<"..\music1.txt", HasHeaders = true>.GetSample()

    for row in csv.Rows do
      printfn "crDt = %A title = %A cont = %A" row.crDt row.title row.cont

#endif //fsharpData
    //unid,crDt (date),modDt (date),title,cont,tags,flag (bool),track (int),sales (float),price (float),subgenre,rating,datePurch (date),length (date)
    let inp = File.ReadAllLines(@"C:\Users\inets\desktop\mike\src\pbt\music1.txt", Encoding.UTF8)
    printfn "true: inp len: %A" (inp.Length)
    inp |> List.ofArray |> limi (fun i r -> 
            match i with
            | 1 -> 
                let spl = સપ્લીટ r ","
                printfn "spl len: %A" (spl.Length)
(*
                spl len: 15
                0) "30579771^SongTbl"
                1) "07/26/2008"
                2) "03/22/2024"
                3) "Bohemian Rhapsody"
                4) "Imagine there's no heaven"
                5) "john paul metal classic rock"
                6) "False"
                7) "4"
                8) "54.7873893138727"
                9) "4.0392028730741"
                10) ""
                11) "Folk"
                12) "2"
                13) "12/24/1981 02:15:00 AM"
                14) "13:09:47"

//unid,crDt (date),modDt (date),title,cont,tags,flag (bool),track (int),sales (float),price (float),subgenre,rating,datePurch (date),length (date)
*)

                let unid = spl.[0]
                let crDt = (DateTime.TryParse(spl.[1])) |> snd
                let modDt = (DateTime.TryParse(spl.[2])) |> snd
                let title = spl.[3]
                let cont = spl.[4]
                let tags = spl.[5]
                let flag = (Boolean.TryParse(spl.[6]))
                let track = (float) spl.[7]
                let sales = (float) spl.[8]
                let price = (float) spl.[9]
                let subgenre = spl.[11]
                let rating = (float) spl.[12]
                let datePurch = (DateTime.TryParse(spl.[13]))  |> snd
                let length = ((DateTime.TryParse(spl.[13])) |> snd).ToShortTimeString()
                printfn "parsed"
                //spl |> limi (fun i f -> printfn "%A) %A" i f) |> ignore
            | _ -> ()) |> ignore

    printfn "...eom mod csvParse..."    