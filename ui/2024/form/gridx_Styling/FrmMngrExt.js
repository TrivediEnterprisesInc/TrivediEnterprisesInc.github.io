require(["dojo/_base/lang", "dijit/registry", "dojox/form/Manager"], function(lang, registry, Mngr){
  lang.extend(Mngr, {
    frmDeltas:{},
    addDelta: function(f, v){
	//console.log("lang.extend.Mngr call 4 addDelta f=" + f + " v=" + v);
	if (has(this.frmDeltas, f)) {
		//console.log("addDelta: fld exists");
		if (get(this.frmDeltas, f) === v){
			//console.log("addDelta: same val exists");
		} else {
			//console.log("addDelta: new edit");
			this.frmDeltas[f] = v;
		}
	} else {
			//console.log("addDelta: fld doesn't exist: " + f);
			this.frmDeltas[f] = v;
	}
    },
    getDeltas: function(){
	   console.log("frmDeltas: ", this.frmDeltas);
    },
    insp: function(r){
        console.log("insp.formWidgets", Object.keys(this.formWidgets));
        //extra els ignored by FM...
        //this.setFormValues({bool: true, number:1011, bogus: "not exist", string:"Set in frmMngf.insp"});
        this.setFormValues(r);
        // collect all current values of attached nodes
        //this.inspect(inspector);
        
    }

/*,
    postCreate: function() {
        //apparently not necc (not in widgt) this.inherited();
        console.log("lang.extend.Mngr postCreate call...");
    }
*/
  });
});
