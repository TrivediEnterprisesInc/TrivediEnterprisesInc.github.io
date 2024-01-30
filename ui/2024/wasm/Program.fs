module Main =

open System
open System.Runtime.InteropServices.JavaScript

printfn "Hello, Browser!"

type MyClass = 
    [JSExport]
    let Greeting():string = 
        let text = $"Hello, World! Greetings from {GetHRef()}";
        printfn(text)
        text
 
    [JSImport("window.location.href", "main.js")]
    let GetHRef():string = ()


    [<EntryPoint>]
    let main _ =
        let refOb = new MyClass()
        printfn "set the refOb..."
        0

