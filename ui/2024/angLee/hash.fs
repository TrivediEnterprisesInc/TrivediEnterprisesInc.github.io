open System.Text.Encoding
open System.Security.Cryptography.MD5

    let r = Encoding.UTF8.GetBytes("adsfdae@epic.com")

    let toHashInt =
    List.map(fun e -> 
            System.Text.UTF8Encoding.GetBytes(e) 
            |> MD5.HashData
            |> Convert.ToInt32(hexValue, 16)) [r]
        |> printfn "res:%A"
        
    //Convert.ToInt32 will truncate the MD5 but ok for our purps if sufficiently varied (dupes ok)

    type Proc = | TypeA of a:string * b:string
                | TypeB of a:string * b:string * c:string 
    type ProcRunner with
        member new(uID:string, proc:Proc) = 
            printfn "created new rnr..."
        member runFor(inpA, inpB) = 
             //nds uID
            (uID |> toHashInt |> fetch <| inpA).equals(inpB)
        member run() =
            match proc with
            | TypeA -> runFor(a,b)
            | _ -> runFor(a,b,c)
