(*  Brij (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 
    TrivediBldHdr->
    |Trivedi SrcCtrlHdr File.  Copyright (c) 2015-2025 M. P. Trivedi.  All rights reserved.|637901455|test.fs|none|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS * <|

    fsc src\pbt\pbt_AI_Dsk.fs src\pbt\pbt_Dsk.fs  --platform:x64 --standalone --out:src\pbt\Trivedi.PbtDsk.dll -r:lib\Trivedi.Core.dll -r:lib\Trivedi.CoreAux.dll -r:lib\Trivedi.UI.dll -r:lib\Trivedi.UIAux.dll -I:C:\Windows\Microsoft.NET\Framework\v4.0.30319
   
    Created: Wed Jul 15 2025
    Last updated: 
    
    Contains modules:  UserSettings_Actual
                                
*)

namespace Trivedi

#nowarn "20" "25" "58" "66" "67" "64" "760" "1182" "1558"

[<AutoOpen>]
module UserSettings_Actual = 
  open pbtCore  //4 wrld; curr in dskTc.fs (mainRepo);@todo:move2pbtCore.fs

  let wrld = World()
  //UsrSettings get the same; can be modified in TblSettings which inherit these as init.
  let defFonts = ["Open Sans";"Roboto";"Monserrat";"Lora";"Oswald"]
  let defFontsWithParams = 
    //defWeight: 400; defWidth: 100
    [], defFonts |> lifo (fun s f -> s @ (f, 400, 100))
  wrld.Add("UsrSettingsDefFonts", box defFontsWithParams)

  type UserSettings<'t when 't :> ITblMarker> (dsk, સ્તિતિ) as f =
    inherit TabPage (Text = "UserSettings")
    let mutable currSettings = સ્તિતિ
    let setupPg = 
        let UsTc = new TabControl(Dock = doc "F", Width = f.Width - defPadding.Horizontal, Height = f.Height - (defPadding.Horizontal * 8))
        let IntPg = new TabPage("Main") //i18n + toggles
        let UXModePg = new TabPage (Text = "UX Mode")
        let DefaultsPg = new TabPage (Text = "Defaults")
        let HlpPg = new TabPage (Text = "Help Settings") //TipDialogs
        UsTc.Controls.Add([UXModePg; IntPg; DefaultsPg; HlpPg])
        UsTc.SelectTab(0)
        f.Controls.Add(UsTc)
(*    For UXModePg: offer the foll options
DEFAULT:  Ideal for business/enterprise/casual users.
          Clean, friendly UI.
          Optimized by hiding features you don't need.
          Switch-on-the-fly to ExpertMode by hitting Cntrl-X
UI:       Optimized for front-end developers.
          Allows the setting of detailed design specs.
          (CSS, Font Weights, Color Hues, Saturation...)
PROGRAMMER: Optimized for users who know programming.
          (RegExp, IfThenElse Statements, Manual code entry)
EXPERT:   The full-featured Brij User Experience with all feature sets exposed.
Setting in UserSettings will override all others; but local settings are "sticky"
*)

(*
  Need helpTxt incl a msg saying if fontName is mistyped it won't work
  i.e., this nds 2 be UI/XpertMode
https://delightfuldesignstudio.com/best-google-fonts-for-apps/
https://fonts.google.com/

CSS snippet nded to incl font:
<style>
@import url('https://fonts.googleapis.com/css2?family=Roboto:wdth,wght@100,400&display=swap');
</style>

CSS snippet nded to apply font:
.roboto-<uniquifier> {
  font-family: "Roboto", sans-serif;
  font-optical-sizing: auto;
  font-weight: 400;
  font-style: normal;
  font-variation-settings: "wdth" 100;
}
*)
        wld.get("UsrSettingsDefFonts")
        |> limi (fun i f -> 
            let (nm, wt, wd) = f 
            let flP = new FlowLayoutPanel(Dock = doc "F", FlowDirection = FlowDirection.LeftToRight, AutoSize = true)
            let fNm = new TextBox(Text = nm, Dock = doc "F")
            let fWt = new TextBox(Text = wt, Dock = doc "F")
            let fWd = new TextBox(Text = wd, Dock = doc "F")
            let commonHandler = new EventHandler(fun o e -> 
                let mutable currFonts = wld.get("UsrSettingsDefFonts")
                let newTpl = (fNm, fWt, fWd)
                currFonts <- List.updateAt i currFonts
                wld.set("UsrSettingsDefFonts" currFonts))
            fNm.OnTextChanged.AddHandler(commonHandler)
            fWt.OnTextChanged.AddHandler(commonHandler)
            fWd.OnTextChanged.AddHandler(commonHandler)
            flP.Controls.AddItems([fNm;fWt;fWd])
            !!^ [("fP" + i + "fNm"), box (fNm); ("fP" + i + "fWt"), box (fWt);("fP" + i + "fWd"), box (fWd)]
            DefaultsPg.Controls.Add(flP)) |> ignore



