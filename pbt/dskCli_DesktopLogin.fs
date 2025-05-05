(*  Brij (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 
    TrivediBldHdr->
    |Trivedi SrcCtrlHdr File.  Copyright (c) 2015-2025 M. P. Trivedi.  All rights reserved.|637901455|test.fs|none|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|

    fsc src\pbt\dskCli_Desktop_AI.fs src\pbt\dskCli_DesktopLogin.fs  --platform:x64 --standalone --target:exe --out:login.exe -r:lib\Trivedi.Core.dll -r:lib\Trivedi.CoreAux.dll -r:lib\Trivedi.UI.dll -r:lib\Trivedi.UIAux.dll -I:C:\Windows\Microsoft.NET\Framework\v4.0.30319
   
    This ver uses Trivedi.UI_Nov02_Color+Grids.dll + UIAux + ...

    Last updated: Apr 7 2025

    Contains modules:  Desktop_Login


*)

namespace Trivedi

#nowarn "20" "25" "58" "66" "67" "64" "760" "1182" "1558"

    [<AutoOpen>]
    module Desktop_Login = 
        open System
        open System.Diagnostics
        open System.Drawing
        open System.Drawing.Imaging
        open System.IO
        open System.Text
        open System.Text.RegularExpressions
        open System.Windows.Forms
        open Trivedi
        open Trivedi.Core
        open Trivedi.Control
        open FSharp.Reflection

        printfn "init login..."
        
        let msgs = ["Downloading Desktop settings..."; "Downloading Desktop icons..."; "Downloading Table settings..."; "Setting up Developer Environment..."; "Configuring Developer access to backend..."; "Downloading Table marked records..."; "Restoring Desktop state..."; "Restoring Desktop windows..."]
        
        [<EntryPoint>]
        [<STAThread>]
        let main ag =
            printfn "main:1"
            match ag.Length = 0 with 
            | true -> 
                printfn "main:2"
                Application.EnableVisualStyles()
                let f = Form(Visible = false, TopMost = true, WindowState = FormWindowState.Maximized, AutoScroll = true)
                let pBar = new ProgressBar(Dock = DockStyle.None, Visible = true, Minimum = 1, Maximum = msgs.Length, Value = 1, Step = 1)
                f.Controls.Add(pBar)
                let launcher = new System.Timers.Timer(Interval = (float) 2000, Enabled = true, AutoReset = false)
                launcher.Elapsed.AddHandler(new System.Timers.ElapsedEventHandler(fun o e -> 
                    msgs |> lim (fun m -> 
                        pBar.Text = m
                        printfn "%A" m
                        pBar.PerformStep()) |> ignore))
                Application.Run(f)
            | _ -> 
                printfn "exit no params"
            0