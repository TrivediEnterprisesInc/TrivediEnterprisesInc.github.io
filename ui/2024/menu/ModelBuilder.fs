//          [1; "root"; "Root", type: "base", population: "6 billion"},

type NoteItm = | NoteItm of unid:int * slg:string * title:string * parentSlg:string

[         ["1"; "root"; "Root"; ""];
          ["5"; "devFS"; "F#/.net"; "dev" ];
          ["10"; "dev"; "dev"; "career"];
          ["52"; "gearpers"; "personal"; "gear" ];
          ["11"; "sartorial"; "sartorial"; "gearpers" ];
          ["14"; "planning"; "acc."; "gearpers" ];
          ["12"; "audio"; "audio/ht"; "gear" ];
          ["13"; "furniture"; "furniture"; "gear" ];
          ["15"; "film"; "film"; "mindAmuse" ];
          ["16"; "food"; "food"; "body" ];       
          ["51"; "exercise"; "exercise"; "body" ];
          ["17"; "NYCeat"; "NYC Eating Out"; "food" ];
          ["53"; "miscFood"; "food misc"; "food" ];       
          ["18"; "re"; "real estate"; "gear" ];
          ["19"; "NYCStorage"; "storage"; "gear" ];
          ["20"; "NYCdesi"; "Desi Stuff"; "social" ];
          ["33"; "db"; "db"; "dev" ];
          ["39"; "corp"; "corp"; "dev" ];
          ["350"; "devAPI"; "currentAPI"; "corp" ];
          ["351"; "flowcharts"; "flowcharts"; "corp" ];
          ["352"; "changeLog"; "log"; "corp" ];
          ["353"; "eagleEye"; "eagleEye"; "corp" ];
          ["37"; "career"; "work"; "root"];
          ["34"; "mind"; "mind"; "root"];
          ["35"; "body"; "body"; "root"];
          ["36"; "gear"; "gear"; "root"];
          ["46"; "music"; "music"; "mindAmuse"];
          ["38"; "social"; "social"; "mindAmuse"];
          ["339"; "devNotes"; "notes"; "career" ];
          ["50"; "miscJob"; "jobs"; "career" ];       
          ["90"; "fin"; "finance"; "career" ];
          ["44"; "mindAmuse"; "amuse"; "mind"];
          ["40"; "mindInstr"; "instruct"; "mind"];
          ["45"; "elevate"; "elevate"; "mind"];
          ["270"; "elevateLinks"; "elevateLinks"; "elevate"];
          ["271"; "buddhism"; "buddhism"; "elevate"];
          ["21"; "lrnlang"; "languages"; "mindInstr"];
          ["41"; "lrnFrench"; "french"; "lrnlang"];
          ["42"; "lrnUrdu"; "urdu"; "lrnlang"];
          ["43"; "lrnGujarati"; "gujarati"; "lrnlang"];
          ["48"; "artLit"; "art/lit"; "mindInstr"];
          ["49"; "edOther"; "other"; "mindInstr"];
          ["47"; "miscReading"; "reading"; "mindAmuse"];
          ["50"; "amusetravel"; "travel"; "mindAmuse"];
          ["51"; "travelNYC"; "nyc"; "amusetravel"];
          ["96"; "tv"; "tv"; "mindAmuse"];
          ["399"; "devMisc"; "misc"; "dev" ];
          ["118"; "recM2S"; "m2s"; "mindAmuse" ];
          ["119"; "recOther"; "other"; "mindAmuse" ]]
|> List.map(fun l -> 
        NoteItm(((int) l.[0]), l.[1], l.[2], l.[3]))
|> List.length
|> printfn "list of %A itms created..."

//list of 49 itms created...

//list of 49 itms created...
