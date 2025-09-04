(*  Brij (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 
    TrivediBldHdr->
    |Trivedi SrcCtrlHdr File.  Copyright (c) 2015-2025 M. P. Trivedi.  All rights reserved.|637901455|test.fs|none|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS 

    See mBox: "WebSockets / SignalR / SSE"

    Created:   Mon Jul 28 2025
    Last updated: Mon Aug 11 2025

Aug 11th:
    * Cleanest approach here wld be no dlgs; just bckgrnd-create a conflict; leave it up to the dev (add to dox) to resolve conflicts.
    * This is a hard-code vw with intnlDz so we can use an iconCol for 1st col
    * Use material-symbols icon "dangerous" (use in red)
    * Place conflicted doc (2nd save) BELOW the other one (w/icon); show both

Jul 28th:
N.B.: (1) We nd this for users too
      (2) This will work over HTTP

Only for Edits: Svr records (dzElID, EditBeginTime)

        8am                   9am                     10am

   DevA  Edit-------------------------------------------SaveAttempt[1]

                 DevB   Edit-------Save[2]

(i)  At [2], Svr sees A is in EditMode for dzElID >> sets Flag 2 DevA (SaveOccured)
(ii)  IF/When A saves [1], dskCli shows MergeConflictDlg: 
    Descr of what happened & why (lists DevB's name) + link 2 helpDox + 3 options:
    REFRESH (accept A's edits, lose B edits)
    OVERWRITE (Creates MergeConflict doc by saving B under newID)
    SAVEAS (offers B a chance to save edits under new DzElID/Title)

Jul 29th:
Option 2 for above (may be b8r coz preEmptive approach) ->
dzElEditCmd >> svrAdminTbl gets (dzElId, userId, DateTime editCommenced)
dev2 editCmd >> Same el exists ? dlg("To prevent poss mergeConflicts...")

*)