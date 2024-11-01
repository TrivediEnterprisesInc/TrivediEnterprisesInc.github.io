  require(['dojo/_base/lang', "dijit/form/Select",
  "dijit/ConfirmDialog", "dijit/registry", "dojo/on", "dojo/dom-construct",
  "dijit/form/Button", "dijit/form/ValidationTextBox", 'dojo/dom', 'dojo/parser', 'dojo/domReady!'],
    function(lang, Select, Dialog, registry, on, domConstr, Button, ValidationTBox, dom, parser){

	/*	qEdDlg Script 	*/

  window.showqEdDlg = (struc) => {
    //struc is the grid struc w/fldInf
    console.log("qEdDlg.init()...");
    var qEdDlg;

  staticStruct = [
      {id: 'number', name: 'number', field: 'number', width: '10%', reg: '\\d{5}', invMsg: 'Please enter a valid 5-digit number'},
      {id: 'string', name: 'string', field: 'string', width: '45%', reg: '\\d{5}', invMsg: 'Please enter a valid 5-digit number'},
      {id: 'date', name: 'date', field: 'date', width: '22%', reg: '\\d{5}', invMsg: 'Please enter a valid 5-digit number'},
      {id: 'time', name: 'time', field: 'time', width: '15%', reg: '\\d{5}', invMsg: 'Please enter a valid 5-digit number'},
      {id: 'bool', name: 'bool', field: 'bool', width: '8%', reg: '(true)|(false)', invMsg: 'Please enter a valid 5-digit number'},
    ];

    qEdDlg = new Dialog({
            title: "Quick Edit",
            id: "qEdDlg",
            content: "",
            actionBarTemplate: dlgActionBarTempl
    });
    on(qEdDlg, "hide", function(){
        qEdDlg.destroy();
    });

    //console.log("qEd recd struc:", struc);

    fNms = struc.map((x, i) => {
        slg = x["name"];
        ret = {};
        ret["label"] = slg;
        ret["value"] = "";
        if (i == 0){
          ret["selected"] = true;
        }
        return ret;
      });

    //console.log("qEd nms:", fNms);

    Sel = new Select({
          name: "qEdSelect",
          options: fNms
    });
    Sel.on("change", function(){
        console.log("onChange");
        console.log("my value: ", this.get("value"))
        /*
        userEntry.set("regExp", newVal);
        userEntry.set("invalidMessage", newVal);
        */
    });


    staticContent = `
    <label for='qEdSelect'>Please select the Field you wish to update:</label>
      <div id='fldSel'></div>
      <input type="text" name="userEntry" data-dojo-id="userEntry" value="" required="true"
          data-dojo-type="dijit/form/ValidationTextBox"
          data-dojo-props="regExp:'\\d{5}', invalidMessage:'Invalid Num!'" />
    `;


    qEdDlg.set('title',"qEd Dlg ");
    qEdDlg.set('content', staticContent);
    qEdDlg.startup();
    Sel.placeAt(dom.byId("fldSel"));
    Sel.startup();
    qEdDlg.show();
  }

})
