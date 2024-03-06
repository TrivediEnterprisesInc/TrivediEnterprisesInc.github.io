 define([], function(){

   var bPl = 
       {
       colHdrs: ['Title', 'Module', 'SubModule', 'Importance', 'Urgency', 'Tags', 'Objective', 'Document UNID', 'Project','DocLinks', 'Completed On', 'rowTips', 'Parent', 'Flag', 'Content', 'isCateg', 'Target Version'],
       openCats: [],
       visCols: 6,
       pOpts: [200, 1, 2000, 200, 400],
       Layout: 
       [[
         {id: 'Title',   name: 'Title',   field: 'string', expandLevel: 1}, 
         {id: 'Module',   name: 'Module',   field: 'string', expandLevel: 2},
         {id: 'SubModule',   name: 'SubModule',   field: 'string', expandLevel: 3},
         {id: 'Importance',   name: 'Importance',   field: 'number'}, 
         {id: 'Urgency',   name: 'Urgency',   field: 'number'}, 
         {id: 'Tags',   name: 'Tags',   field: 'string'}, 
         {id: 'Objective',   name: 'Objective',   field: 'string'}, 
         {id: 'Document UNID',   name: 'Document UNID',   field: 'id'}, 
         {id: 'Project',   name: 'Project',   field: 'string'}, 
         {id: 'DocLinks',   name: 'DocLinks',   field: 'string'}, 
         {id: 'Completed On',   name: 'Completed On',   field: 'date'}, 
         {id: 'rowTips',   name: 'rowTips',   field: 'string'}, 
         {id: 'Parent',   name: 'Parent',   field: 'string'}, 
         {id: 'Flag',   name: 'Flag',   field: 'string'}, 
         {id: 'Content',   name: 'Content',   field: 'string'}, 
         {id: 'isCateg',   name: 'isCateg',   field: 'bool'}, 
         {id: 'Target Version',   name: 'Target Version',   field: 'string'}
       ],
        [
         {id: 'Title',   name: 'Title',   field: 'string'}, 
         {id: 'Module',   name: 'Module',   field: 'string'}, 
         {id: 'SubModule',   name: 'SubModule',   field: 'string'}, 
         {id: 'Importance',   name: 'Importance',   field: 'number'}, 
         {id: 'Urgency',   name: 'Urgency',   field: 'number'}, 
         {id: 'Tags',   name: 'Tags',   field: 'string'}, 
         {id: 'Objective',   name: 'Objective',   field: 'string'}, 
         {id: 'Document UNID',   name: 'Document UNID',   field: 'id'}, 
         {id: 'Project',   name: 'Project',   field: 'string'}, 
         {id: 'DocLinks',   name: 'DocLinks',   field: 'string'}, 
         {id: 'Completed On',   name: 'Completed On',   field: 'date'}, 
         {id: 'rowTips',   name: 'rowTips',   field: 'string'}, 
         {id: 'Parent',   name: 'Parent',   field: 'string'}, 
         {id: 'Flag',   name: 'Flag',   field: 'string'}, 
         {id: 'Content',   name: 'Content',   field: 'string'}, 
         {id: 'isCateg',   name: 'isCateg',   field: 'bool'}, 
         {id: 'Target Version',   name: 'Target Version',   field: 'string'}]],
          Dat: [{"id":'spxServer (235  items)', "Title":'spxServer (235  items)', "Module":'spxServer (235  items)', "SubModule":'spxServer (235  items)', "Importance":0, "Urgency":0, "Tags":'spxServer (235  items)', "Objective":'spxServer (235  items)', "Project":'spxServer (235  items)', "DocLinks":'spxServer (235  items)', "Completed On":null, "rowTips":'err', "Parent":'spxServer (235  items)', "Flag":'spxServer (235  items)', "Content":'err', "isCateg":true, "Target Version":'spxServer (235  items)', "children":[{ "id":'spxServer (235  items) CHILD', "Title":'spxServer (235  items)', "Module":'spxServer (235  items)', "SubModule":'spxServer (235  items)', "Importance":0, "Urgency":0, "Tags":'spxServer (235  items)', "Objective":'spxServer (235  items)', "Project":'spxServer (235  items)', "DocLinks":'spxServer (235  items)', "Completed On":null, "rowTips":'err', "Parent":'spxServer (235  items)', "Flag":'spxServer (235  items)', "Content":'err', "isCateg":true, "Target Version":'spxServer (235  items)', "children":[]}]}, {"id":'Research (5  items)', "Title":'Research (5  items)', "Module":'Research (5  items)', "SubModule":'Research (5  items)', "Importance":0, "Urgency":0, "Tags":'Research (5  items)', "Objective":'Research (5  items)', "Project":'Research (5  items)', "DocLinks":'Research (5  items)', "Completed On":null, "rowTips":'err', "Parent":'Research (5  items)', "Flag":'Research (5  items)', "Content":'err', "isCateg":true, "Target Version":'Research (5  items)', "children":[]}, {"id":'638056736839239230^Task', "Title":'Data Import - json', "Module":'spawn', "SubModule":'Research', "Importance":9, "Urgency":9, "Tags":'oldId:20187171202284654800^Task ', "Objective":'', "Project":'spxServer', "DocLinks":'', "Completed On":01/01/0001, "rowTips":'Chk jdk json import/proc libs.</div>', "Parent":'', "Flag":'', "Content":'Chk jdk json import/proc libs.</div>', "isCateg":false, "Target Version":'Research', "children":[]}, {"id":'638056736839238497^Task', "Title":'Data Import - json', "Module":'procEngine', "SubModule":'Research', "Importance":5, "Urgency":5, "Tags":'oldId:20187171202284654800^2^Task ', "Objective":'', "Project":'spxServer', "DocLinks":'', "Completed On":01/01/0001, "rowTips":'Chk jdk json import/proc libs.</div>', "Parent":'', "Flag":'', "Content":'Chk jdk json import/proc libs.</div>', "isCateg":false, "Target Version":'Research', "children":[]}, {"id":'638056736839238496^Task', "Title":'Data Import - json', "Module":'spawn', "SubModule":'Research', "Importance":5, "Urgency":5, "Tags":'oldId:20187171202284654800^3^Task ', "Objective":'', "Project":'spxServer', "DocLinks":'', "Completed On":01/01/0001, "rowTips":'Chk jdk json import/proc libs.</div>', "Parent":'', "Flag":'', "Content":'Chk jdk json import/proc libs.</div>', "isCateg":false, "Target Version":'Research', "children":[]}, {"id":'638056736839238495^Task', "Title":'Data Import - json', "Module":'general', "SubModule":'Research', "Importance":5, "Urgency":5, "Tags":'oldId:20187171202284654800^1^Task ', "Objective":'', "Project":'spxServer', "DocLinks":'', "Completed On":01/01/0001, "rowTips":'Chk jdk json import/proc libs.</div>', "Parent":'', "Flag":'', "Content":'Chk jdk json import/proc libs.</div>', "isCateg":false, "Target Version":'Research', "children":[]}]
          }

   return {
     getData: function(args){
       var data = {
         identifier: 'id', 
         label: 'id', 
         items: bPl.Dat
       };

       console.log(data);
       return data;
     },
     layouts: [[
         {id: 'Title',   name: 'Title',   field: 'string', expandLevel: 1}, 
         {id: 'Module',   name: 'Module',   field: 'string', expandLevel: 2},
         {id: 'SubModule',   name: 'SubModule',   field: 'string', expandLevel: 3},
         {id: 'Importance',   name: 'Importance',   field: 'number'}, 
         {id: 'Urgency',   name: 'Urgency',   field: 'number'}, 
         {id: 'Tags',   name: 'Tags',   field: 'string'}, 
         {id: 'Objective',   name: 'Objective',   field: 'string'}, 
         {id: 'Document UNID',   name: 'Document UNID',   field: 'id'}, 
         {id: 'Project',   name: 'Project',   field: 'string'}, 
         {id: 'DocLinks',   name: 'DocLinks',   field: 'string'}, 
         {id: 'Completed On',   name: 'Completed On',   field: 'date'}, 
         {id: 'rowTips',   name: 'rowTips',   field: 'string'}, 
         {id: 'Parent',   name: 'Parent',   field: 'string'}, 
         {id: 'Flag',   name: 'Flag',   field: 'string'}, 
         {id: 'Content',   name: 'Content',   field: 'string'}, 
         {id: 'isCateg',   name: 'isCateg',   field: 'bool'}, 
         {id: 'Target Version',   name: 'Target Version',   field: 'string'}
       ],
        [
         {id: 'Title',   name: 'Title',   field: 'string'}, 
         {id: 'Module',   name: 'Module',   field: 'string'}, 
         {id: 'SubModule',   name: 'SubModule',   field: 'string'}, 
         {id: 'Importance',   name: 'Importance',   field: 'number'}, 
         {id: 'Urgency',   name: 'Urgency',   field: 'number'}, 
         {id: 'Tags',   name: 'Tags',   field: 'string'}, 
         {id: 'Objective',   name: 'Objective',   field: 'string'}, 
         {id: 'Document UNID',   name: 'Document UNID',   field: 'id'}, 
         {id: 'Project',   name: 'Project',   field: 'string'}, 
         {id: 'DocLinks',   name: 'DocLinks',   field: 'string'}, 
         {id: 'Completed On',   name: 'Completed On',   field: 'date'}, 
         {id: 'rowTips',   name: 'rowTips',   field: 'string'}, 
         {id: 'Parent',   name: 'Parent',   field: 'string'}, 
         {id: 'Flag',   name: 'Flag',   field: 'string'}, 
         {id: 'Content',   name: 'Content',   field: 'string'}, 
         {id: 'isCateg',   name: 'isCateg',   field: 'bool'}, 
         {id: 'Target Version',   name: 'Target Version',   field: 'string'}]]

   };
 });
