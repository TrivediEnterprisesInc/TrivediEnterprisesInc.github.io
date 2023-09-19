
# Table of Contents
- [Outstanding Tks](#outstanding-tks)
  - [Updates to this doc](#updates-to-this-doc)
  - [Qry](#qry)
  - [DbClipboard](#dbclipboard)
  - [Tasks+Notes: To Be Checked](#tasksnotes-to-be-checked)
    - [From Aug 2023](#from-aug-2023)
    - [From May 22 2023](#from-may-22-2023)
    - [From EOY 22](#from-eoy-22)
      - [FrmDz](#frmdz)
      - [TblDz](#tbldz)
      - [DvDz:](#dvdz)
      - [LookupDocs](#lookupdocs)
    - [Core](#core)
    - [Aux](#aux)
    - [Dat](#dat)
    - [Brij.Expr](#brijexpr)
    - [UI](#ui)
- [Architecture](#architecture)
  - [Topology Overview](#topology-overview)
  - [Versioning](#versioning)
    - [FldLvl Δs](#fldlvl-s)
    - [Nested/Embedded Dox](#nestedembedded-dox)
  - [Windowing](#windowing)
  - [Templating](#templating)
    - [Brij flow using ρ setup ->](#brij-flow-using--setup--)
  - [Process Flows `S`](#process-flows-s)
  - [Process Flows `A`](#process-flows-a)
  - [Process Flows `G`](#process-flows-g)
  - [Process Flows `H`](#process-flows-h)
  - [Process Flows `O`](#process-flows-o)
    - [Where's the info 4 orgAd / userAd?  (∃, yeah?) add:](#wheres-the-info-4-orgad--userad---yeah-add)
  - [ACL/Role Impl.](#aclrole-impl)
    - [Consider:](#consider)
      - [From Aug 2023](#from-aug-2023-1)
      - [From Sep18_23](#from-sep1823)
- [Ref](#ref)
  - [Reading Shelf](#reading-shelf)
  - [Languages](#languages)
    - [General](#general)
  - [Tools](#tools)
    - [CheerpX](#cheerpx)
    - [Theorem Provers](#theorem-provers)
    - [GitHub alternatives](#github-alternatives)
  - [FsLang](#fslang)
    - [Free Monad w/Interpreter](#free-monad-winterpreter)
    - [TomP's Update Monad](#tomps-update-monad)
    - [Optional Params](#optional-params)
    - [Eliminating Maybes](#eliminating-maybes)
    - [Symbolic Links](#symbolic-links)
    - [More FsLinks](#more-fslinks)
  - [Auth](#auth)
    - [From  Aug 5 2023](#from--aug-5-2023)
  - [LLM Notes](#llm-notes)
    - [ReACT pattern for LLMs](#react-pattern-for-llms)
    - [Embeddings](#embeddings)
    - [Llama2](#llama2)
    - [Prompt Injection](#prompt-injection)
  - [Due Diligence](#due-diligence)
    - ["Refine & React-Admin same tgt in the noCo space: they're going after ReTool"](#refine--react-admin-same-tgt-in-the-noco-space-theyre-going-after-retool)
    - [VisualDb.com](#visualdbcom)
    - [frappeframework.com](#frappeframeworkcom)
    - [Flask AppBuilder](#flask-appbuilder)
    - [jinjat.com](#jinjatcom)
    - [github.com/BudiBase](#githubcombudibase)
    - [github.com/appsmithorg](#githubcomappsmithorg)
    - [github.com/ToolJet](#githubcomtooljet)
    - [github.com/lowdefy](#githubcomlowdefy)
    - [github.com/windmill-labs/windmill](#githubcomwindmill-labswindmill)
    - [www.superblocks.com](#wwwsuperblockscom)
    - [Hansura / Supabase](#hansura--supabase)
- [Markdown Stuff](#markdown-stuff)
  - [Colors](#colors)
- [Mongo:](#mongo)
  - [Queries](#queries)
  - [Nested Docs](#nested-docs)
  - [Dynamic/ExpandoObject](#dynamicexpandoobject)
- [VC Podcasts](#vc-podcasts)
- [Rec](#rec)
  - [PO](#po)
  - [SSO](#sso)
  - [Prints](#prints)
- [Other](#other)
    - [SignalR](#signalr)
    - [Headers](#headers)
    - [allFlds (from baseTkDatAux)](#allflds-from-basetkdataux)
    - [TaskDVAux dat brkdn + raw](#taskdvaux-dat-brkdn--raw)
    - [Code linkx](#code-linkx)
    - [Rec linkx](#rec-linkx)
    - [Svr Hosting](#svr-hosting)
    - [Off-the-cuff](#off-the-cuff)

15. isCateg		16. Parent		17. catLvl`

### TaskDVAux dat brkdn + raw
`0: " |0 spxServer (235  items) |1 spxServer (235  items) |2 spxServer (235  items) |3 spxServer (235  items) |4 spxServer (235  items) |5 spxServer (235  items) |6 spxServer (235  items) |7 spxServer (235  items) |8 spxServer (235  items) |9 spxServer (235  items) |10 spxServer (235  items) |11 spxServer (235  items) |12 spxServer (235  items) |13 spxServer (235  items) |14 spxServer (235  items) |15 spxServer (235  items) |16 spxServer (235  items) |17 True |18 spxServer (235  items) |19 0"`

`1: " |0 Research (5  items) |1 Research (5  items) |2 Research (5  items) |3 Research (5  items) |4 Research (5  items) |5 Research (5  items) |6 Research (5  items) |7 Research (5  items) |8 Research (5  items) |9 Research (5  items) |10 Research (5  items) |11 Research (5  items) |12 Research (5  items) |13 Research (5  items) |14 Research (5  items) |15 Research (5  items) |16 Research (5  items) |17 True |18 Research (5  items) |19 1"
2: " |0  |1  |2 Data Import - json |3 spawn |4 9 |5 9 |6  |7 oldId:20187171202284654800^Task  |8 638056736839239230^Task |9 spxServer |10 Research |11  |12 1/1/0001 12:00:00 AM |13  |14  |15  |16 Q2hrIGpkayBqc29uIGltcG9ydC9wcm9jIGxpYnMuPC9kaXY+ |17 False |18 Research |19 "
3: " |0  |1  |2 Data Import - json |3 procEngine |4 5 |5 5 |6  |7 oldId:20187171202284654800^2^Task  |8 638056736839238497^Task |9 spxServer |10 Research |11  |12 1/1/0001 12:00:00 AM |13  |14  |15  |16 Q2hrIGpkayBqc29uIGltcG9ydC9wcm9jIGxpYnMuPC9kaXY+ |17 False |18 Research |19 "
4: " |0  |1  |2 Data Import - json |3 spawn |4 5 |5 5 |6  |7 oldId:20187171202284654800^3^Task  |8 638056736839238496^Task |9 spxServer |10 Research |11  |12 1/1/0001 12:00:00 AM |13  |14  |15  |16 Q2hrIGpkayBqc29uIGltcG9ydC9wcm9jIGxpYnMuPC9kaXY+ |17 False |18 Research |19 "
`

### Code linkx
   - Playing God w/[Static](https://lexi-lambda.github.io/blog/2020/08/13/types-as-axioms-or-playing-god-with-static-types/) types
   - Paul [Graham](http://paulgraham.com/)
   - Path [Sensitive](https://www.pathsensitive.com/p/archive.html)
   - Steve [Yegge](https://steve-yegge.blogspot.com/)
   - [Kalzumeus](https://www.kalzumeus.com/greatest-hits/)

### Svr Hosting
Free [dev](https://stackdiary.com/free-hosting-for-developers/) hosting
Free [docker](https://codeless.co/free-docker-hosting/) hosting
Hosting on [repl.it](https://blog.replit.com/powerful-servers)
	
### Off-the-cuff
et je vous presentez...
	Vacant - vacare (Latin) / shunya (Sanskrit)
	Void - voidus (Latin) / shunya (Sanskrit)
	Empty - vacuus (Latin) / shunya (Sanskrit)
	Unfilled - non impletus (Latin) / shunya (Sanskrit)
	Clear - clarus (Latin) / vishuddha (Sanskrit)
	Unoccupied - inhabitus (Latin) / avyapeta (Sanskrit)
	Spare - sparsus (Latin) / shunya (Sanskrit)

#### Tangibles
  - peru et al
  - hmr: dep inj - ngrm - bld/train - 

#### Intangibles
	- und mjr - frms sci basis for this - prob theory, outcomes, conf lvls

intm8 domain spec kn - e.g. for a spec. ind: diff betw ema/sma; sloSto/convDiv; Fed exprt infl
IIIly 4 mainSt

`ComplFlows >> Isol8|Identify Bottlen >> Sw2Mods >> Cons staticWsmRunViaAPI`
