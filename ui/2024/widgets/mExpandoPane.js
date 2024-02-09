define(["dojo/_base/lang", "dojox.layout.ExpandoPane", "dojo/dom", 
        "dojo/dom-class", "dojo/text!../support/mExpandoPane.html", 
        "dojo/domReady!"], function(lang, ExpandoPane, dom, domClass, template){

   var mExpandoPane = 
      lang.extend(ExpandoPane, {
         templateString: template,
         postMixInProperties: function(evt){
                        //alert('sel clicked 4: "' + this.title + '"');
                        //nd to copy id for action >> state >> selItems >> clear
                        if (this.expButton.innerHTML === "<<"){
                           //this.expButton.innerHTML = "check_box_outline_blank"
                           alert("<<");
                        } else {
                           //this.expButton.innerHTML = "check_box"
                           alert(">>");
                        };
                        //evt.stopPropagation();
                  }
      });
   return mExpandoPane;
})
