bookmarks redo

mShowTxtDiv 
            if (strId == "misc_www_dojo_jsonp") {
              	getDojoPulls();
            } else if (strId == "filmMain") {
		...
            } else if (strId == "changeLog") {
		... manual blds (cld feed or gen) 
            calls fetchHtmlAsText brijLog23...
          } else if (strId == "devFS") {
		let titles = ["Links", "Core", "Combinators", "Editors", "Compiler", "Snippets", "Freeware", "Reading", "Crypto", "UI"];
	        (titles.map(function (tVal, idx) {....
		...
            } else if (strId == "db") {
		let titles = ["mongo", "other"];
	        (titles.map(function (tVal, idx) {
		...
            } else if (strId == "devMisc") {
		let titles = ["dojo", "www_Links", "www_dojo_links", "www_dojo_jsonp", "www_other","www_console","domino","crypto", "java_funct", "java_hotSwap", "java_spxDesign", "java_nlp"];
	        (titles.map(function (tVal, idx) {
		...
            } else if (strId == "film") {
		...
            } else {
              	curDivId = dom.byId(strId);
	        var newTxt = (curDivId.innerHTML);
		mainDiv.innerHTML = newTxt;

see alt type/bld @ https://github.com/TrivediEnterprisesInc/TrivediEnterprisesInc.github.io/blob/main/ui/2024/menu/ModelBuilder.fs

//to avoid confusion for the tree ren tags to 'par', rem list
type CDiv = | CDiv of unid:int * divId:string * title:string * tags:string list * content:string * notes:string with
    member this.toString() = 
        let (CDiv(u,d,t,tg,c,n)) = this
        "Unid: " + u.ToString() + " divId: " + d + " Title: " + t + " Tags: " + tg.ToString() + " Content len: " + (c.Length).ToString() + " Notes: " + n

//UNID: 1, id: "root", name: "Root", type: "base", population: "6 billion"
let dat = 
 [ CDiv(5, "devFS", "F#/.net", ["dev"],"","layer3") ;
 CDiv(10, "dev", "dev", ["career"],"","layer2");
 CDiv(52, "gearpers", "personal", ["gear"],"","layer2") ;
 CDiv(11, "sartorial", "sartorial", ["gearpers"],"","layer2") ;
 CDiv(14, "planning", "acc.", ["gearpers"],"","layer2") ;
 CDiv(12, "audio", "audio/ht", ["gear"],"","layer2") ;
 CDiv(13, "furniture", "furniture", ["gear"],"","layer2") ;
 CDiv(15, "film", "film", ["mindAmuse"],"","layer3") ;
 CDiv(16, "food", "food", ["body"],"","layer2") ;
 CDiv(51, "exercise", "exercise", ["body"],"","layer2") ;
 CDiv(17, "NYCeat", "NYC Eating Out", ["food"],"","layer3") ;
 CDiv(53, "miscFood", "food misc", ["food"],"","layer3") ;
 CDiv(18, "re", "real estate", ["gear"],"","layer3") ;
 CDiv(19, "NYCStorage", "storage", ["gear"],"","layer2") ;
 CDiv(20, "NYCdesi", "Desi Stuff", ["social"],"","layer2") ;
 CDiv(33, "db", "db", ["dev"],"","layer3") ;
 CDiv(39, "corp", "corp", ["dev"],"","layer3") ;
 CDiv(350, "devAPI", "currentAPI", ["corp"],"","layer4") ;
 CDiv(351, "flowcharts", "flowcharts", ["corp"],"","layer4") ;
 CDiv(352, "changeLog", "log", ["corp"],"","layer4") ;
 CDiv(353, "eagleEye", "eagleEye", ["corp"],"","layer4") ;
 CDiv(37, "career", "work", ["root"],"","layer1");
 CDiv(34, "mind", "mind", ["root"],"","layer1");
 CDiv(35, "body", "body", ["root"],"","layer1");
 CDiv(36, "gear", "gear", ["root"],"","layer1");
 CDiv(46, "music", "music", ["mindAmuse"],"","layer3");
 CDiv(38, "social", "social", ["mindAmuse"],"","layer3");
 CDiv(339, "devNotes", "notes", ["career"],"","layer2") ;
 CDiv(50, "miscJob", "jobs", ["career"],"","layer2") ;
 CDiv(90, "fin", "finance", ["career"],"","layer2") ;
 CDiv(44, "mindAmuse", "amuse", ["mind"],"","layer2");
 CDiv(40, "mindInstr", "instruct", ["mind"],"","layer2");
 CDiv(45, "elevate", "elevate", ["mind"],"","layer2");
 CDiv(270, "elevateLinks", "elevateLinks", ["elevate"],"","layer3");
 CDiv(271, "buddhism", "buddhism", ["elevate"],"","layer3");
 CDiv(21, "lrnlang", "languages", ["mindInstr"],"","layer3");
 CDiv(41, "lrnFrench", "french", ["lrnlang"],"","layer4");
 CDiv(42, "lrnUrdu", "urdu", ["lrnlang"],"","layer4");
 CDiv(43, "lrnGujarati", "gujarati", ["lrnlang"],"","layer4");
 CDiv(48, "artLit", "art/lit", ["mindInstr"],"","layer3");
 CDiv(49, "edOther", "other", ["mindInstr"],"","layer3");
 CDiv(47, "miscReading", "reading", ["mindAmuse"],"","layer3");
 CDiv(50, "amusetravel", "travel", ["mindAmuse"],"","layer3");
 CDiv(51, "travelNYC", "nyc", ["amusetravel"],"","layer4");
 CDiv(96, "tv", "tv", ["mindAmuse"],"","layer3");
 CDiv(399, "devMisc", "misc", ["dev"],"","layer3") ;
 CDiv(118, "recM2S", "m2s", ["mindAmuse"],"","layer3") ;
 CDiv(119, "recOther", "other", ["mindAmuse"],"","layer3")]

let prnDat = dat |> List.mapi (fun i x -> printfn "%A) %A" i (x.toString())) |> ignore
