module mockPV =
    open System
    //@NotAvail: remmed coz not avail on glot/paiza/any other
    //@NotAvail open System.Text.Json
    //@NotAvail open System.Text.Json.Serialization
    

    type VerPLd = | VerPLd of id:string * dt:DateTime * usr:string * log:string list with
        member x.toWeb() = 
            let (VerPLd(i,d,u,l)) = x
            "{'id':" + i.ToString() + ", 'user:'" + u + ", 'timeSt:'" + d.ToString() + ", 'log:'" + l.ToString() + "}"

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
                        List.map(fun d -> (fNms()).[d]) [0..(deltas-1)]) [0..(n-1)]

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

    let getSample = 
        fun n ->
            List.zip3 (getEms [] n) (getFNms n) (getDts n)
            |> List.mapi (fun i e -> 
                            let (em, fn, dt) = e
                            VerPLd(i.ToString(), dt, em, fn))
            |> List.map(fun v -> v.toWeb())
            //@NotAvail |> JsonSerializer.Serialize

    printfn "%A\n%A" "out:" (getSample 3)
