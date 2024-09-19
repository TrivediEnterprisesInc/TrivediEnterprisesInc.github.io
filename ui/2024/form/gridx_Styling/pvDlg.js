  require(['dojo/_base/lang', 'dojo/store/Memory', 'gridx/core/model/cache/Sync', "gridx/Grid",
  "dijit/ConfirmDialog", "dijit/registry", "dojo/on", "dojo/dom-construct",
  "dijit/form/Button", 'dojo/dom', 'dojo/parser', 'dojo/domReady!'],
    function(lang, Memory, Cache, Grid,
             Dialog, registry, on, domConstr, Button, dom, parser){

	/*	pvDlg Script 	*/

  window.showPvDlg = () => {
    //Consider moving this to common for all the Dlgs
	console.log("pvDlg.init()...");
    var pvDlg;
    let dlgActionBarTempl = "<div class='dijitDialogPaneActionBar' data-dojo-attach-point='actionBarNode'>" +
	"<button data-dojo-type='dijit/form/Button' type='submit'" +
        "    data-dojo-attach-point='okButton'></button>" +
	"<button data-dojo-type='dijit/form/Button' type='button'" +
	"		data-dojo-attach-point='cancelButton' data-dojo-attach-event='click:onCancel'></button>" +
        "</div>";

        pvDlg = new Dialog({
                title: "PV...",
                id: "pvDlg",
                content: "",
                style: "height:80%;width:80%;",
                actionBarTemplate: dlgActionBarTempl
        });

	on(pvDlg, "hide", function(){
	     pvDlg.destroy();
	     pvGrid.destroy();
	});


    //output from ver.fs
    var verOutput = 
      [{"id":0, "user":"mcohen10@tinyurl.com", "timeSt":"06/10/1997 11:49:00", "log":"field_2 field_4 field_6 field_8"},
      {"id":1, "user":"eferagh1i@slate.com", "timeSt":"11/17/1997 12:53:00", "log":"field_1 field_2 field_7"},
      {"id":2, "user":"brobins3@cdbaby.com", "timeSt":"07/07/2004 19:31:00", "log":"field_2 field_3 field_4 field_5 field_8 field_9"},
      {"id":3, "user":"sellerayl@epa.gov", "timeSt":"08/01/2013 07:40:00", "log":"field_2 field_3 field_5 field_8 field_9"},
      {"id":4, "user":"jvanhalen2c@webnode.com", "timeSt":"07/13/2015 21:02:00", "log":"field_5 field_6 field_9"},
      {"id":5, "user":"jcristofalos@indiatimes.com", "timeSt":"07/22/2015 14:30:00", "log":"field_1 field_6"},
      {"id":6, "user":"wshovell1f@squarespace.com", "timeSt":"07/08/2020 00:21:00", "log":"field_1 field_3"},
      {"id":7, "user":"gmacconnulty2r@about.com", "timeSt":"08/21/2021 07:29:00", "log":"field_1 field_3 field_6 field_7 field_9"}];

    var data = {
      identifier: "id",
      label: 'id',
      items: verOutput
    };

    data.items = verOutput;
    var store = new Memory({data: data});
    var layout = [
      {id: '#', name: "#", field: 'id', width: '5%'},
      {id: 'EditedBy', name: "EditedBy", field: 'user', width: '50%'},
      {id: 'on', name: "on", field: 'timeSt', width: '35%'}
/*      {id: 'FieldsChanged', name: "FieldsChanged",  field: 'log', width: '0px'}*/
    ];

    var pvGrid = new Grid({
        id: 'pvGrid',
        store: store,
        cacheClass: Cache,
        structure: layout
        });

    dojo.connect(pvGrid, "onRowMouseOver", (e) => {
	let r = pvGrid.model.byId(e.rowId);
	let item = pvGrid.store.get(e.rowId);
	if (!item) return;
	console.log("fetchedItemById (store) : " + item['user']);
        /*
	        if (pvDlg.isValid()){
        	        var box0 = registry.byId("userId");
                	uId = box0.get('value');
	                alert("got uId: " + uId);
        	        pvDlg.hide();
	        } else {
        	        alert('event.stop(e)');
	        }
        */
    });

    dojo.connect(pvGrid, "onRowDblClick", (e) => {
	let r = pvGrid.model.byId(e.rowId);
	let item = pvGrid.store.get(e.rowId);
	if (!item) return;
	console.log("fetch/setFrmVals 4 : " + item['user']);
        //(i) fetch
        //(ii) frm/Mngr: setFormValues([{key,val}])
        //(iii) ensure frm is disabled via form/Mngr: dijit.byId('form').disable()
        //(iv) show newDlg w/frm
        //...handle makeCurr() if necc
    });
    pvDlg.set('title',"PV Dlg ");
    pvDlg.set('content', pvGrid);
    pvGrid.startup();
    pvDlg.show();
  }

})
