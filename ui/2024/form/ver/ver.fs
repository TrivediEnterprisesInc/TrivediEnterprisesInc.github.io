module mockPV =
    open System
    //@NotAvail: remmed coz not avail on glot/paiza/any other
    //@NotAvail open System.Text.Json
    //@NotAvail open System.Text.Json.Serialization

    //Note: only issue w/this mock generator: timeStamps are not sorted Desc
    //however they'll be auto-created in correct sortOrd in impl. so no probs.

    type VerPLd = | VerPLd of id:string * dt:DateTime * usr:string * log:string list with
        member x.toWeb() = 
            let qt = "'"
            let (VerPLd(i,d,u,l)) = x
            let logWeb() = (List.fold (fun s v -> s + " " + v) "" l).Trim()
            "{'id':" + i.ToString() + ", 'user':" + qt + u + qt + ", 'timeSt':" + qt + d.ToString() + qt + ", 'log':" + qt + logWeb() + qt +  "}"

    let r:Random = new Random()
    
    //100 random emails gen from mockaroo:
    let mockEmails() = ["ecasoni0@furl.net";"fberthouloume1@ucoz.com";"mbasek2@ox.ac.uk";"brobins3@cdbaby.com";"estife4@chicagotribune.com";"reltun5@harvard.edu";"cfullerlove6@techcrunch.com";"ddixcee7@tuttocitta.it";"bcurnok8@usatoday.com";"rarchanbault9@google.co.jp";"gfawthropa@time.com";"tcromleholmeb@walmart.com";"btukec@rediff.com";"nwickershamd@usda.gov";"ldavydzenkoe@sitemeter.com";"kbrayfordf@miitbeian.gov.cn";"mpuckhamg@usatoday.com";"gferreah@sakura.ne.jp";"mcastelaini@ucoz.com";"akenelinj@purevolume.com";"faburrowk@diigo.com";"sellerayl@epa.gov";"lcominim@webeden.co.uk";"lstychen@samsung.com";"ccardwello@odnoklassniki.ru";"cdeblasiisp@sbwire.com";"hpriddleq@walmart.com";"asissonr@instagram.com";"jcristofalos@indiatimes.com";"ipaddockt@ifeng.com";"fmorgueu@spiegel.de";"ypaddiev@de.vu";"fagronskiw@squidoo.com";"amccuex@squidoo.com";"lnattrassy@devhub.com";"jhallwardz@dion.ne.jp";"mcohen10@tinyurl.com";"wsturror11@loc.gov";"fsparke12@ebay.com";"nhugin13@vinaora.com";"rassel14@seattletimes.com";"mvarden15@biglobe.ne.jp";"bbarg16@yahoo.co.jp";"aclench17@nydailynews.com";"mgarretson18@tinypic.com";"dtenant19@biglobe.ne.jp";"sbarnfather1a@nymag.com";"rvedekhov1b@blogs.com";"cderobert1c@cpanel.net";"gallom1d@moonfruit.com";"kdurnall1e@sogou.com";"wshovell1f@squarespace.com";"ageorgeson1g@weather.com";"wrenfree1h@redcross.org";"eferagh1i@slate.com";"ehelkin1j@patch.com";"jhollyland1k@soundcloud.com";"funderdown1l@istockphoto.com";"hloker1m@miitbeian.gov.cn";"sbarneville1n@globo.com";"ewonham1o@amazon.co.uk";"bgourlie1p@smugmug.com";"jmackaig1q@answers.com";"vsmardon1r@gov.uk";"eattwood1s@discuz.net";"iminchell1t@scribd.com";"hstickens1u@livejournal.com";"rtarte1v@drupal.org";"dbute1w@answers.com";"bboller1x@ucsd.edu";"jnund1y@yelp.com";"kalbertson1z@typepad.com";"mbattell20@cafepress.com";"lwildash21@livejournal.com";"akenningley22@angelfire.com";"djellard23@desdev.cn";"ideverell24@ucsd.edu";"lrodnight25@feedburner.com";"agooday26@myspace.com";"slevick27@washington.edu";"nridsdole28@ehow.com";"aheadington29@miitbeian.gov.cn";"pcasperri2a@instagram.com";"mdemeyer2b@issuu.com";"jvanhalen2c@webnode.com";"hsudy2d@creativecommons.org";"aarmit2e@odnoklassniki.ru";"jmeco2f@ebay.com";"battreed2g@ca.gov";"ryakunkin2h@census.gov";"mgebb2i@accuweather.com";"tmeldrum2j@loc.gov";"sbatistelli2k@squidoo.com";"kpalumbo2l@utexas.edu";"chark2m@surveymonkey.com";"gmarioneau2n@linkedin.com";"cmordey2o@geocities.com";"nmincini2p@opera.com";"siowarch2q@gizmodo.com";"gmacconnulty2r@about.com"]
    let rec getEms = 
        fun st n ->
            let res:List<_> = List.fold(fun s v -> ((mockEmails()).[r.Next(0,100)]) :: s) st [0..(n-1)] |> List.distinct
            if res.Length = n then res else getEms st n

    //@FixMe: Placeholder
    let fNms() = [0..10] |> List.map(fun x -> "field_" + x.ToString())
    let getFNms = 
        fun n ->
            List.map(fun x -> 
                        let deltas = r.Next(1,10)
                        List.map(fun d -> (fNms()).[(r.Next(1,10))]) [0..(deltas-1)]
                        |> List.sort
                        |> List.distinct) [0..(n-1)]

    let getNxtDt =
        fun (f:System.DateTime) (s:System.DateTime) ->
            let range = s - f
            let interval = s - f
            let nextInt = r.Next(0,(int (interval.TotalMinutes)))
            f.AddMinutes((float) nextInt)
    
    let fstD = (new System.DateTime(1996, 6, 3, 22, 15, 0))
    let secD = (DateTime.Now)
    
    let getDts =
        fun n -> 
            [0..(n-1)]
            |> List.map (fun x -> (getNxtDt fstD secD))
            |> List.sort

    //hardCoded vals in lieu of Core fn.
    //note that mocked result lkups'll have to confirm to this spec
    let genDocID =
        fun d u -> 
            "DocID_" + (d.Ticks).ToString() + "^" + u + "^Table01" + "^BrijCorp"

    let getSample = 
        fun n ->
            List.zip3 (getEms [] n) (getFNms n) (getDts n)
            |> List.map (fun e -> 
                            let (em, fn, dt) = e
                            VerPLd((genDocID dt em), dt, em, fn))
            |> List.map(fun v -> v.toWeb())
            //@NotAvail |> JsonSerializer.Serialize

    let genPayloads() = 
            List.mapi (fun i x -> 
                        let pv_hist_len = r.Next(1,10)
                        printfn "%A)\n %A" i (getSample pv_hist_len)
                        ) [0..9]

/*
produces outpt in this fmt:
 [{"id":"DocID_632294224800000000^usr2@brij.com^Table01^BrijCorp", "user":"tmeldrum2j@loc.gov", "timeSt":"08/30/2004 00:28:00", "log":"field_1 field_6 field_7 field_8"},
 {"id":"DocID_633018827400000000^usr2@brij.com^Table01^BrijCorp", "user":"hloker1m@miitbeian.gov.cn", "timeSt":"12/16/2006 16:19:00", "log":"field_1 field_2 field_3 field_4 field_6 field_7 field_8 field_9"},
 ...
we need (to confirm to JSONspcs):
[{"id":"1","col1":"normal","col2":false,"col3":"But are not followed by two hexadecimal","col4":29.91},
{"id":"2","col1":"important","col2":false,"col3":"Because a % sign always indicates","col4":9.33},...
coding not worth it (we can use the serializer l8r)
so manually repl/massage the outpt.
*/

    //toBeDebugged
    let toWebOb =
        fun tplLi ->
            let writerOptions = new JsonWriterOptions() { Indented = true}
            use stream = new MemoryStream()
            use writer = new Utf8JsonWriter(stream, writerOptions)
            let rec procSingleTpl = 
                fun v ->
                    let (MTpl(slg, v, t)) = v
                        match t with
                        | String -> 
                            writer.WriteStartObject()
                            writer.WritePropertyName(slg)
                            writer.WriteStringValue(v)
                            writer.WriteEndObject()
                            writer.Flush()
                        | Boolean -> 
                            writer.WriteStartObject()
                            writer.WritePropertyName(slg)
                            writer.WriteBooleanValue(v)
                            writer.WriteEndObject()
                        | Number -> 
                            writer.WriteStartObject()
                            writer.WritePropertyName(slg)
                            writer.WriteNumberValue(v)
                            writer.WriteEndObject()
                        | List<_> ->
                            writer.WriteStartArray(slg)
                            lim (fun x -> procSingleTpl x) v
                            writer.WriteEndArray()
                        | _ ->
                            //we won't allow nulls
                            printfn "Lvl SEVERE: null encountered in tpl! for key: %A" slg
            lim (fun x -> procSingleTpl x) tplLi
            Encoding.UTF8.GetString(stream.ToArray())
