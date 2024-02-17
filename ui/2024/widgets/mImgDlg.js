define(["dojo/_base/lang", "dijit/Dialog", "dojo/dom", "dijit/form/Select",
        "dojo/dom-class", "dojo/text!../support/mExpandoPane.html", 
        "dojo/domReady!"], function(lang, Dialog, dom, Select, domClass, template){

   var mDlg = 
      lang.extend(Dialog, {
         templateString: "<div class='dijitDialog' role='dialog' aria-labelledby='${id}_title'>" + 
"	<div data-dojo-attach-point='titleBar' class='dijitDialogTitleBar'>" + 
"		<span data-dojo-attach-point='titleNode' class='dijitDialogTitle' id='${id}_title'" + 
"				role='heading' level='1'></span>" + 
"		<span data-dojo-attach-point='closeButtonNode' class='dijitDialogCloseIcon' data-dojo-attach-event='ondijitclick: onCancel' title='${buttonCancel}' role='button' tabindex='-1'>" + 
"			<span data-dojo-attach-point='closeText' class='closeText' title='${buttonCancel}'>x</span>" + 
"		</span>" + 
"	</div>" + 
"	<div data-dojo-attach-point='containerNode' class='dijitDialogPaneContent'>" + 
"   <div data-dojo-attach-point='hdrLblDiv'></div>" + 
"   <select id='dlgSelect' data-dojo-type='dijit/form/Select' required=true>" + 
"     <option value='${selVal[0]}'></option>" + 
"     <option value='${selVal[1]}'></option>" + 
"     <option value='${selVal[2]}'></option>" + 
"     <option value='${selVal[3]}'></option>" + 
"     <option value='${selVal[4]}'></option>" + 
"  </select>" + 
"   <div data-dojo-attach-point='ftrLblDiv'></div>" + 
" </div>" + 
" <div class='dijitDialogPaneActionBar' data-dojo-attach-point='actionBarNode'>" +
"   <button data-dojo-type='dijit/form/Button' type='submit'" +
"     data-dojo-attach-point='okButton' onClick=\"return this.isValid();\"></button>" +
"   <button data-dojo-type='dijit/form/Button' type='button'" +
"		  data-dojo-attach-point='cancelButton' data-dojo-attach-event='click:onCancel'></button>" +
" </div>" +
"</div>",
        stepNum: 0,
        stepTot: 0,
        hdrLbl: "",
        hdrVal: "",
        ftrLbl: "",
        ftrVal: "",
        selLbl: [],
        selVal: [],
        postMixInProperties: function(evt){
                          this.titleNode.innerHTML = "Step " + stepNum + " of " stepTot;
                          alert("<<");
                          //1 chk if caller-site Var in scope & set
                          //2 don't hide, call this.destroy();
                        };
      });
   return mDlg;
})
