(*  Brij (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 
   
    Created:      Thu Oct 16 2025
    Last updated: Fri 17th

    Notes:
    * DateOnly/TimeOnly is an issue with the glot ver so the 2 flds've been conv to str in the ty
    * Some further data (non-working img wikip urls/grammys/standards) @ EjxoFDoG3UtiELfw

*)
module NoCoTester = 
    open System
    open System.Text

    type NoCoTester = { ID:string;emailAdd:string;Name:string;genre:string;sales:int;salary:int;
    longText:string; percent:float; date:string; time:string; dateTime:DateTime; booln:Boolean;
    MultiSel:string list; phone:string; url:string; rating:int; duration:string}

    type Addl = {longText:string; percent:float; date:string; time:string; dateTime:DateTime; booln:Boolean;
    MultiSel:string list; phone:string; url:string; rating:int; duration:string}

    //Add'l col hdrs: 
    //longText percent date time dateTime bool MultiSel phone url rating duration
    
    //Music col headers: ID,emailAdd,Name,genre,sales,salary
    let musicTbl = 
      [|
        [box "1"; box "michael.jackson@popworld.com"; box "Michael Jackson"; box "Pop"; box 1490000; box 500000];
        [box "2"; box "madonna@popworld.com"; box "Madonna"; box "Pop"; box 1270000; box 480000];
        [box "3"; box "prince@funkbeat.com"; box "Prince"; box "Funk"; box 1460000; box 450000];
        [box "4"; box "stevie.wonder@funkbeat.com"; box "Stevie Wonder"; box "Funk"; box 1110000; box 430000];
        [box "5"; box "beyonce@popworld.com"; box "Beyonce"; box "Pop"; box 1060000; box 420000];
        [box "6"; box "rihanna@popworld.com"; box "Rihanna"; box "Pop"; box 1450000; box 410000];
        [box "7"; box "justin.timberlake@popworld.com"; box "Justin Timberlake"; box "Pop"; box 1100000; box 400000];
        [box "8"; box "usher@popworld.com"; box "Usher"; box "Pop"; box 1340000; box 390000];
        [box "9"; box "lady.gaga@popworld.com"; box "Lady Gaga"; box "Pop"; box 1160000; box 380000];
        [box "10"; box "bruno.mars@popworld.com"; box "Bruno Mars"; box "Pop"; box 1080000; box 370000];
        [box "11"; box "drake@hiphopzone.com"; box "Drake"; box "HipHop"; box 1470000; box 360000];
        [box "12"; box "kendrick.lamar@hiphopzone.com"; box "Kendrick Lamar"; box "HipHop"; box 1280000; box 350000];
        [box "13"; box "jay.z@hiphopzone.com"; box "Jay-Z"; box "HipHop"; box 1270000; box 340000];
        [box "14"; box "nicki.minaj@hiphopzone.com"; box "Nicki Minaj"; box "HipHop"; box 1250000; box 330000];
        [box "15"; box "cardi.b@hiphopzone.com"; box "Cardi B"; box "HipHop"; box 1030000; box 320000];
        [box "16"; box "travis.scott@hiphopzone.com"; box "Travis Scott"; box "HipHop"; box 1420000; box 310000];
        [box "17"; box "post.malone@hiphopzone.com"; box "Post Malone"; box "HipHop"; box 1300000; box 300000];
        [box "18"; box "meek.mill@hiphopzone.com"; box "Meek Mill"; box "HipHop"; box 1440000; box 290000];
        [box "19"; box "future@hiphopzone.com"; box "Future"; box "HipHop"; box 1430000; box 280000];
        [box "20"; box "lil.wayne@hiphopzone.com"; box "Lil Wayne"; box "HipHop"; box 1290000; box 270000];
        [box "21"; box "john.legendsoul@soulvibes.com"; box "John Legend"; box "Soul"; box 1140000; box 260000];
        [box "22"; box "mary.j.blige@soulvibes.com"; box "Mary J. Blige"; box "Soul"; box 1290000; box 250000];
        [box "23"; box "alicia.keys@soulvibes.com"; box "Alicia Keys"; box "Soul"; box 1050000; box 240000];
        [box "24"; box "marvin.gaye@soulvibes.com"; box "Marvin Gaye"; box "Soul"; box 1260000; box 230000];
        [box "25"; box "aretha.franklin@soulvibes.com"; box "Aretha Franklin"; box "Soul"; box 1320000; box 220000];
        [box "26"; box "sam.cooke@soulvibes.com"; box "Sam Cooke"; box "Soul"; box 1210000; box 210000];
        [box "27"; box "otis.redding@soulvibes.com"; box "Otis Redding"; box "Soul"; box 1470000; box 200000];
        [box "28"; box "stevie.wonder@soulvibes.com"; box "Stevie Wonder"; box "Soul"; box 1080000; box 190000];
        [box "29"; box "ray.charles@soulvibes.com"; box "Ray Charles"; box "Soul"; box 1400000; box 180000];
        [box "30"; box "james.brown@soulvibes.com"; box "James Brown"; box "Soul"; box 1460000; box 170000];
        [box "31"; box "coldplay@rocknation.com"; box "Coldplay"; box "Rock"; box 1100000; box 160000];
        [box "32"; box "queen@rocknation.com"; box "Queen"; box "Rock"; box 1270000; box 150000];
        [box "33"; box "led.zepplin@rocknation.com"; box "Led Zeppelin"; box "Rock"; box 1350000; box 140000];
        [box "34"; box "pink.floyd@rocknation.com"; box "Pink Floyd"; box "Rock"; box 1110000; box 130000];
        [box "35"; box "the.rolling.stones@rocknation.com"; box "The Rolling Stones"; box "Rock"; box 1370000; box 120000];
        [box "36"; box "nirvana@rocknation.com"; box "Nirvana"; box "Rock"; box 1320000; box 110000];
        [box "37"; box "foo.fighters@rocknation.com"; box "Foo Fighters"; box "Rock"; box 1130000; box 100000];
        [box "38"; box "red.hot.chili.peppers@rocknation.com"; box "Red Hot Chili Peppers"; box "Rock"; box 1280000; box 90000];
        [box "39"; box "u2@rocknation.com"; box "U2"; box "Rock"; box 1170000; box 80000];
        [box "40"; box "green.day@rocknation.com"; box "Green Day"; box "Rock"; box 1480000; box 70000];
        [box "41"; box "adele@popworld.com"; box "Adele"; box "Pop"; box 1340000; box 60000];
        [box "42"; box "sam.smith@popworld.com"; box "Sam Smith"; box "Pop"; box 1460000; box 59000];
        [box "43"; box "ed.sheeran@popworld.com"; box "Ed Sheeran"; box "Pop"; box 1400000; box 58000];
        [box "44"; box "taylor.swift@popworld.com"; box "Taylor Swift"; box "Pop"; box 1090000; box 57000];
        [box "45"; box "shawn.mendes@popworld.com"; box "Shawn Mendes"; box "Pop"; box 1340000; box 56000];
        [box "46"; box "justin.bieber@popworld.com"; box "Justin Bieber"; box "Pop"; box 1320000; box 55000];
        [box "47"; box "selena.gomez@popworld.com"; box "Selena Gomez"; box "Pop"; box 1060000; box 54000];
        [box "48"; box "ariana.grande@popworld.com"; box "Ariana Grande"; box "Pop"; box 1190000; box 53000];
        [box "49"; box "dua.lipa@popworld.com"; box "Dua Lipa"; box "Pop"; box 1180000; box 52000];
        [box "50"; box "billie.eilish@popworld.com"; box "Billie Eilish"; box "Pop"; box 1220000; box 51000];
        [box "51"; box "miles.davis@jazzvibes.com"; box "Miles Davis"; box "Jazz"; box 1280000; box 50000];
        [box "52"; box "john.coltrane@jazzvibes.com"; box "John Coltrane"; box "Jazz"; box 1250000; box 49000];
        [box "53"; box "ella.fitzgerald@jazzvibes.com"; box "Ella Fitzgerald"; box "Jazz"; box 1260000; box 48000];
        [box "54"; box "louis.armstrong@jazzvibes.com"; box "Louis Armstrong"; box "Jazz"; box 1300000; box 47000];
        [box "55"; box "billie.holiday@jazzvibes.com"; box "Billie Holiday"; box "Jazz"; box 1040000; box 46000];
        [box "56"; box "charlie.parker@jazzvibes.com"; box "Charlie Parker"; box "Jazz"; box 1060000; box 45000];
        [box "57"; box "thelonious.monk@jazzvibes.com"; box "Thelonious Monk"; box "Jazz"; box 1080000; box 44000];
        [box "58"; box "cannonball.adderley@jazzvibes.com"; box "Cannonball Adderley"; box "Jazz"; box 1060000; box 43000];
        [box "59"; box "herbie.hancock@jazzvibes.com"; box "Herbie Hancock"; box "Jazz"; box 1150000; box 42000];
        [box "60"; box "dizzy.gillespie@jazzvibes.com"; box "Dizzy Gillespie"; box "Jazz"; box 1210000; box 41000];
        [box "61"; box "bob.dylan@folk.com"; box "Bob Dylan"; box "Folk"; box 1340000; box 40000];
        [box "62"; box "joan.baez@folk.com"; box "Joan Baez"; box "Folk"; box 1380000; box 39000];
        [box "63"; box "joni.mitchell@folk.com"; box "Joni Mitchell"; box "Folk"; box 1290000; box 38000];
        [box "64"; box "neil.young@folk.com"; box "Neil Young"; box "Folk"; box 1070000; box 37000];
        [box "65"; box "cat.stevens@folk.com"; box "Cat Stevens"; box "Folk"; box 1330000; box 36000];
        [box "66"; box "simon.garfunkel@folk.com"; box "Simon & Garfunkel"; box "Folk"; box 1060000; box 35000];
        [box "67"; box "willie.nelson@folk.com"; box "Willie Nelson"; box "Folk"; box 1280000; box 34000];
        [box "68"; box "joel@folk.com"; box "Billy Joel"; box "Folk"; box 1020000; box 33000];
        [box "69"; box "bruce.springsteen@folk.com"; box "Bruce Springsteen"; box "Folk"; box 1230000; box 32000];
        [box "70"; box "neil.diamond@folk.com"; box "Neil Diamond"; box "Folk"; box 1060000; box 31000];
        [box "71"; box "elton.john@popworld.com"; box "Elton John"; box "Pop"; box 1150000; box 30000];
        [box "72"; box "george.michael@popworld.com"; box "George Michael"; box "Pop"; box 1380000; box 80000];
        [box "73"; box "whitney.houston@popworld.com"; box "Whitney Houston"; box "Pop"; box 1310000; box 180000];
        [box "74"; box "mariah.carey@popworld.com"; box "Mariah Carey"; box "Pop"; box 1040000; box 150000];
        [box "75"; box "celine.dion@popworld.com"; box "Celine Dion"; box "Pop"; box 1310000; box 160000];
        [box "76"; box "diana.ross@popworld.com"; box "Diana Ross"; box "Pop"; box 1360000; box 0];
        [box "77"; box "stevie.nicks@rocknation.com"; box "Stevie Nicks"; box "Rock"; box 1130000; box 160000];
        [box "78"; box "mick.jagger@rocknation.com"; box "Mick Jagger"; box "Rock"; box 1040000; box 20000];
        [box "79"; box "keith.richards@rocknation.com"; box "Keith Richards"; box "Rock"; box 1210000; box 210000];
        [box "80"; box "jimi.hendrix@rocknation.com"; box "Jimi Hendrix"; box "Rock"; box 1410000; box 250000];
        [box "81"; box "jim.morrison@rocknation.com"; box "Jim Morrison"; box "Rock"; box 1170000; box 60000];
        [box "82"; box "robert.plant@rocknation.com"; box "Robert Plant"; box "Rock"; box 1430000; box 290000];
        [box "83"; box "freddie.mercury@rocknation.com"; box "Freddie Mercury"; box "Rock"; box 1320000; box 0];
        [box "84"; box "kurt.cobain@rocknation.com"; box "Kurt Cobain"; box "Rock"; box 1330000; box 10000];
        [box "85"; box "eddie.vedder@rocknation.com"; box "Eddie Vedder"; box "Rock"; box 1190000; box 70000];
        [box "86"; box "chris.cornell@rocknation.com"; box "Chris Cornell"; box "Rock"; box 1130000; box 240000];
        [box "87"; box "jack.white@rocknation.com"; box "Jack White"; box "Rock"; box 1400000; box 190000];
        [box "88"; box "john.mayer@popworld.com"; box "John Mayer"; box "Pop"; box 1470000; box 250000];
        [box "89"; box "james.blunt@popworld.com"; box "James Blunt"; box "Pop"; box 1260000; box 290000];
        [box "90"; box "gary.moore@rocknation.com"; box "Gary Moore"; box "Rock"; box 1320000; box 210000];
        [box "91"; box "eric.clapton@rocknation.com"; box "Eric Clapton"; box "Rock"; box 1360000; box 100000];
        [box "92"; box "bb.king@bluesworld.com"; box "B.B. King"; box "Blues"; box 1130000; box 80000];
        [box "93"; box "muddy.waters@bluesworld.com"; box "Muddy Waters"; box "Blues"; box 1150000; box 30000];
        [box "94"; box "stevie.ray.vaughan@bluesworld.com"; box "Stevie Ray Vaughan"; box "Blues"; box 1060000; box 280000];
        [box "95"; box "john.lee.hooker@bluesworld.com"; box "John Lee Hooker"; box "Blues"; box 1110000; box 230000];
        [box "96"; box "robert.johnson@bluesworld.com"; box "Robert Johnson"; box "Blues"; box 1480000; box 30000];
        [box "97"; box "buddy.guy@bluesworld.com"; box "Buddy Guy"; box "Blues"; box 1230000; box 270000];
        [box "98"; box "bb.king@bluesworld.com"; box "B.B. King"; box "Blues"; box 1420000; box 190000];
        [box "99"; box "bb.king@bluesworld.com"; box "B.B. King"; box "Blues"; box 1080000; box 140000];
        [box "100"; box "bb.king@bluesworld.com"; box "B.B. King"; box "Blues"; box 1170000; box 90000];
        [box "101"; box "bob.marley@reggaevibes.net"; box "Bob Marley"; box "Reggae"; box 1340000; box 320000];
        [box "102"; box "ziggy.marley@reggaevibes.net"; box "Ziggy Marley"; box "Reggae"; box 1370000; box 300000];
        [box "103"; box "toots.hibbert@reggaevibes.net"; box "Toots Hibbert"; box "Reggae"; box 1420000; box 290000];
        [box "104"; box "peter.tosh@reggaevibes.net"; box "Peter Tosh"; box "Reggae"; box 1050000; box 295000];
        [box "105"; box "damian.marley@reggaevibes.net"; box "Damian Marley"; box "Reggae"; box 1000000; box 280000];
        [box "106"; box "maxi.priest@reggaevibes.net"; box "Maxi Priest"; box "Reggae"; box 1320000; box 270000];
        [box "107"; box "beres.hammond@reggaevibes.net"; box "Beres Hammond"; box "Reggae"; box 1460000; box 260000];
        [box "108"; box "junior.murvin@reggaevibes.net"; box "Junior Murvin"; box "Reggae"; box 1180000; box 250000];
        [box "109"; box "dennis.brown@reggaevibes.net"; box "Dennis Brown"; box "Reggae"; box 1150000; box 255000];
        [box "110"; box "gregory.issacs@reggaevibes.net"; box "Gregory Isaacs"; box "Reggae"; box 1470000; box 270000];
        [box "111"; box "john.holt@reggaevibes.net"; box "John Holt"; box "Reggae"; box 1130000; box 280000];
        [box "112"; box "sizzla@reggaevibes.net"; box "Sizzla"; box "Reggae"; box 1080000; box 290000];
        [box "113"; box "buju.banton@reggaevibes.net"; box "Buju Banton"; box "Reggae"; box 1400000; box 310000];
        [box "114"; box "capleton@reggaevibes.net"; box "Capleton"; box "Reggae"; box 1420000; box 300000];
        [box "115"; box "damian.marley@reggaevibes.net"; box "Damian Marley"; box "Reggae"; box 1420000; box 290000];
        [box "116"; box "chronixx@reggaevibes.net"; box "Chronixx"; box "Reggae"; box 1360000; box 270000];
        [box "117"; box "protoje@reggaevibes.net"; box "Protoje"; box "Reggae"; box 1330000; box 260000];
        [box "118"; box "julian.marley@reggaevibes.net"; box "Julian Marley"; box "Reggae"; box 1150000; box 250000];
        [box "119"; box "alborosie@reggaevibes.net"; box "Alborosie"; box "Reggae"; box 1200000; box 270000];
        [box "120"; box "damian.marley@reggaevibes.net"; box "Damian Marley"; box "Reggae"; box 1120000; box 280000];
        [box "121"; box "matisyahu@reggaevibes.net"; box "Matisyahu"; box "Reggae"; box 1300000; box 290000];
        [box "122"; box "konshens@reggaevibes.net"; box "Konshens"; box "Reggae"; box 1210000; box 270000];
        [box "123"; box "serani@reggaevibes.net"; box "Serani"; box "Reggae"; box 1270000; box 250000];
        [box "124"; box "wayne.wonder@reggaevibes.net"; box "Wayne Wonder"; box "Reggae"; box 1160000; box 240000];
        [box "125"; box "popcaan@reggaevibes.net"; box "Popcaan"; box "Reggae"; box 1000000; box 280000];
        [box "126"; box "vybz.kartel@reggaevibes.net"; box "Vybz Kartel"; box "Reggae"; box 1430000; box 290000];
        [box "127"; box "tarrus.riley@reggaevibes.net"; box "Tarrus Riley"; box "Reggae"; box 1030000; box 280000];
        [box "128"; box "busy.signal@reggaevibes.net"; box "Busy Signal"; box "Reggae"; box 1310000; box 270000];
        [box "129"; box "demarco@reggaevibes.net"; box "Demarco"; box "Reggae"; box 1250000; box 260000];
        [box "130"; box "alton.ellis@reggaevibes.net"; box "Alton Ellis"; box "Reggae"; box 1010000; box 250000];
        [box "131"; box "don.carlos@reggaevibes.net"; box "Don Carlos"; box "Reggae"; box 1210000; box 240000];
        [box "132"; box "freddie.mcgill@reggaevibes.net"; box "Freddie McGill"; box "Reggae"; box 1380000; box 230000];
        [box "133"; box "horace.andy@reggaevibes.net"; box "Horace Andy"; box "Reggae"; box 1170000; box 260000];
        [box "134"; box "king.tubby@reggaevibes.net"; box "King Tubby"; box "Reggae"; box 1020000; box 250000];
        [box "135"; box "lee.perry@reggaevibes.net"; box "Lee Perry"; box "Reggae"; box 1370000; box 270000];
        [box "136"; box "burning.spear@reggaevibes.net"; box "Burning Spear"; box "Reggae"; box 1350000; box 280000];
        [box "137"; box "jimmy.cliff@reggaevibes.net"; box "Jimmy Cliff"; box "Reggae"; box 1110000; box 290000];
        [box "138"; box "eric.clapton@bluesrock.net"; box "Eric Clapton"; box "Blues Rock"; box 1440000; box 420000];
        [box "139"; box "bb.king@bluesrock.net"; box "BB King"; box "Blues Rock"; box 1000000; box 400000];
        [box "140"; box "muddy.waters@bluesrock.net"; box "Muddy Waters"; box "Blues Rock"; box 1190000; box 410000];
        [box "141"; box "john.lee.hooker@bluesrock.net"; box "John Lee Hooker"; box "Blues Rock"; box 1460000; box 405000];
        [box "142"; box "robert.johnson@bluesrock.net"; box "Robert Johnson"; box "Blues Rock"; box 1240000; box 390000];
        [box "143"; box "albert.king@bluesrock.net"; box "Albert King"; box "Blues Rock"; box 1180000; box 380000];
        [box "144"; box "stevie.ray.vaughan@bluesrock.net"; box "Stevie Ray Vaughan"; box "Blues Rock"; box 1000000; box 415000];
        [box "145"; box "buddy.guy@bluesrock.net"; box "Buddy Guy"; box "Blues Rock"; box 1120000; box 385000];
        [box "146"; box "bb.queen@bluesrock.net"; box "BB Queen"; box "Blues Rock"; box 1250000; box 370000];
        [box "147"; box "gary.clark.jr@bluesrock.net"; box "Gary Clark Jr."; box "Blues Rock"; box 1020000; box 360000];
        [box "148"; box "john.mayer@bluesrock.net"; box "John Mayer"; box "Blues Rock"; box 1170000; box 365000];
        [box "149"; box "joe.bonamassa@bluesrock.net"; box "Joe Bonamassa"; box "Blues Rock"; box 1390000; box 355000];
        [box "150"; box "robert.cray@bluesrock.net"; box "Robert Cray"; box "Blues Rock"; box 1420000; box 350000];
        [box "151"; box "guitar.acer@bluesrock.net"; box "Guitar Acer"; box "Blues Rock"; box 1340000; box 345000];
        [box "152"; box "jonny.lang@bluesrock.net"; box "Jonny Lang"; box "Blues Rock"; box 1290000; box 340000];
        [box "153"; box "shemekia.copeland@bluesrock.net"; box "Shemekia Copeland"; box "Blues Rock"; box 1400000; box 335000];
        [box "154"; box "annie.lennox@pop.net"; box "Annie Lennox"; box "Pop"; box 1410000; box 320000];
        [box "155"; box "celine.dion@pop.net"; box "Celine Dion"; box "Pop"; box 1220000; box 330000];
        [box "156"; box "whitney.houston@pop.net"; box "Whitney Houston"; box "Pop"; box 1480000; box 340000];
        [box "157"; box "mariah.carey@pop.net"; box "Mariah Carey"; box "Pop"; box 1450000; box 350000];
        [box "158"; box "barbra.streisand@pop.net"; box "Barbra Streisand"; box "Pop"; box 1120000; box 360000];
        [box "159"; box "diana.ross@pop.net"; box "Diana Ross"; box "Pop"; box 1400000; box 340000];
        [box "160"; box "madonna@pop.net"; box "Madonna"; box "Pop"; box 1430000; box 370000];
        [box "161"; box "janet.jackson@pop.net"; box "Janet Jackson"; box "Pop"; box 1150000; box 320000];
        [box "162"; box "britney.spears@pop.net"; box "Britney Spears"; box "Pop"; box 1010000; box 330000];
        [box "163"; box "justin.timberlake@pop.net"; box "Justin Timberlake"; box "Pop"; box 1280000; box 340000];
        [box "164"; box "pharrell.williams@pop.net"; box "Pharrell Williams"; box "Pop"; box 1490000; box 320000];
        [box "165"; box "robin.thicke@pop.net"; box "Robin Thicke"; box "Pop"; box 1120000; box 310000];
        [box "166"; box "usher@pop.net"; box "Usher"; box "Pop"; box 1110000; box 300000];
        [box "167"; box "rihanna@pop.net"; box "Rihanna"; box "Pop"; box 1270000; box 370000];
        [box "168"; box "katy.perry@pop.net"; box "Katy Perry"; box "Pop"; box 1240000; box 340000];
        [box "169"; box "lady.gaga@pop.net"; box "Lady Gaga"; box "Pop"; box 1400000; box 350000];
        [box "170"; box "beyonce@pop.net"; box "Beyonce"; box "Pop"; box 1430000; box 360000];
        [box "171"; box "adele@pop.net"; box "Adele"; box "Pop"; box 1110000; box 370000];
        [box "172"; box "sam.smith@pop.net"; box "Sam Smith"; box "Pop"; box 1250000; box 330000];
        [box "173"; box "halsey@pop.net"; box "Halsey"; box "Pop"; box 1010000; box 320000];
        [box "174"; box "lana.del.rey@pop.net"; box "Lana Del Rey"; box "Pop"; box 1370000; box 310000];
        [box "175"; box "billie.eilish@pop.net"; box "Billie Eilish"; box "Pop"; box 1020000; box 300000];
        [box "176"; box "dua.lipa@pop.net"; box "Dua Lipa"; box "Pop"; box 1410000; box 290000];
        [box "177"; box "marshmello@pop.net"; box "Marshmello"; box "Electronic"; box 1390000; box 280000];
        [box "178"; box "calvin.harris@pop.net"; box "Calvin Harris"; box "Electronic"; box 1140000; box 270000];
        [box "179"; box "avicii@pop.net"; box "Avicii"; box "Electronic"; box 1390000; box 260000];
        [box "180"; box "zedd@pop.net"; box "Zedd"; box "Electronic"; box 1110000; box 250000];
        [box "181"; box "deadmau5@pop.net"; box "Deadmau5"; box "Electronic"; box 1270000; box 240000];
        [box "182"; box "david.guetta@pop.net"; box "David Guetta"; box "Electronic"; box 1220000; box 230000];
        [box "183"; box "martin.garrix@pop.net"; box "Martin Garrix"; box "Electronic"; box 1280000; box 220000];
        [box "184"; box "diplo@pop.net"; box "Diplo"; box "Electronic"; box 1190000; box 210000];
        [box "185"; box "major.lazer@pop.net"; box "Major Lazer"; box "Electronic"; box 1030000; box 200000];
        [box "186"; box "marshmello@pop.net"; box "Marshmello"; box "Electronic"; box 1270000; box 190000];
        [box "187"; box "flume@pop.net"; box "Flume"; box "Electronic"; box 1340000; box 180000];
        [box "188"; box "kygo@pop.net"; box "Kygo"; box "Electronic"; box 1120000; box 170000];
        [box "189"; box "alesso@pop.net"; box "Alesso"; box "Electronic"; box 1160000; box 160000];
        [box "190"; box "calvin.harris@pop.net"; box "Calvin Harris"; box "Electronic"; box 1250000; box 150000];
        [box "191"; box "avicii@pop.net"; box "Avicii"; box "Electronic"; box 1260000; box 140000];
        [box "192"; box "zedd@pop.net"; box "Zedd"; box "Electronic"; box 1350000; box 130000];
        [box "193"; box "deadmau5@pop.net"; box "Deadmau5"; box "Electronic"; box 1390000; box 120000];
        [box "194"; box "david.guetta@pop.net"; box "David Guetta"; box "Electronic"; box 1420000; box 110000];
        [box "195"; box "martin.garrix@pop.net"; box "Martin Garrix"; box "Electronic"; box 1400000; box 100000];
        [box "196"; box "diplo@pop.net"; box "Diplo"; box "Electronic"; box 1310000; box 90000];
        [box "197"; box "major.lazer@pop.net"; box "Major Lazer"; box "Electronic"; box 1320000; box 80000];
        [box "198"; box "marshmello@pop.net"; box "Marshmello"; box "Electronic"; box 1040000; box 70000];
        [box "199"; box "flume@pop.net"; box "Flume"; box "Electronic"; box 1230000; box 60000];
        [box "200"; box "kygo@pop.net"; box "Kygo"; box "Electronic"; box 1370000; box 50000];
        [box "201"; box "avicii@pop.net"; box "Avicii"; box "Electronic"; box 1340000; box 130000];
        [box "202"; box "zedd@pop.net"; box "Zedd"; box "Electronic"; box 1130000; box 120000];
        [box "203"; box "deadmau5@pop.net"; box "Deadmau5"; box "Electronic"; box 1120000; box 110000];
        [box "204"; box "david.guetta@pop.net"; box "David Guetta"; box "Electronic"; box 1320000; box 100000];
        [box "205"; box "martin.garrix@pop.net"; box "Martin Garrix"; box "Electronic"; box 1170000; box 90000];
        [box "206"; box "diplo@pop.net"; box "Diplo"; box "Electronic"; box 1450000; box 80000];
        [box "207"; box "major.lazer@pop.net"; box "Major Lazer"; box "Electronic"; box 1140000; box 70000];
        [box "208"; box "marshmello@pop.net"; box "Marshmello"; box "Electronic"; box 1390000; box 60000];
        [box "209"; box "flume@pop.net"; box "Flume"; box "Electronic"; box 1250000; box 50000];
        [box "210"; box "kygo@pop.net"; box "Kygo"; box "Electronic"; box 1160000; box 40000];
        [box "211"; box "alesso@pop.net"; box "Alesso"; box "Electronic"; box 1450000; box 30000];
        [box "212"; box "calvin.harris@pop.net"; box "Calvin Harris"; box "Electronic"; box 1170000; box 50000];
        [box "213"; box "avicii@pop.net"; box "Avicii"; box "Electronic"; box 1160000; box 10000];
        [box "214"; box "zedd@pop.net"; box "Zedd"; box "Electronic"; box 1290000; box 50000];
        [box "215"; box "deadmau5@pop.net"; box "Deadmau5"; box "Electronic"; box 1450000; box 190000];
        [box "216"; box "david.guetta@pop.net"; box "David Guetta"; box "Electronic"; box 1210000; box 80000];
        [box "217"; box "martin.garrix@pop.net"; box "Martin Garrix"; box "Electronic"; box 1010000; box 270000];
        [box "218"; box "diplo@pop.net"; box "Diplo"; box "Electronic"; box 1010000; box 20000];
        [box "219"; box "major.lazer@pop.net"; box "Major Lazer"; box "Electronic"; box 1110000; box 130000];
        [box "220"; box "marshmello@pop.net"; box "Marshmello"; box "Electronic"; box 1440000; box 190000];
        [box "221"; box "flume@pop.net"; box "Flume"; box "Electronic"; box 1070000; box 270000];
        [box "222"; box "kygo@pop.net"; box "Kygo"; box "Electronic"; box 1210000; box 160000];
        [box "223"; box "alesso@pop.net"; box "Alesso"; box "Electronic"; box 1380000; box 70000];
        [box "224"; box "calvin.harris@pop.net"; box "Calvin Harris"; box "Electronic"; box 1490000; box 270000];
        [box "225"; box "avicii@pop.net"; box "Avicii"; box "Electronic"; box 1320000; box 230000];
        [box "226"; box "zedd@pop.net"; box "Zedd"; box "Electronic"; box 1440000; box 290000];
        [box "227"; box "deadmau5@pop.net"; box "Deadmau5"; box "Electronic"; box 1310000; box 130000];
        [box "228"; box "david.guetta@pop.net"; box "David Guetta"; box "Electronic"; box 1350000; box 100000];
        [box "229"; box "martin.garrix@pop.net"; box "Martin Garrix"; box "Electronic"; box 1440000; box 70000];
        [box "230"; box "diplo@pop.net"; box "Diplo"; box "Electronic"; box 1090000; box 160000];
        [box "231"; box "major.lazer@pop.net"; box "Major Lazer"; box "Electronic"; box 1470000; box 40000];
        [box "232"; box "marshmello@pop.net"; box "Marshmello"; box "Electronic"; box 1230000; box 40000];
        [box "233"; box "flume@pop.net"; box "Flume"; box "Electronic"; box 1380000; box 60000];
        [box "234"; box "kygo@pop.net"; box "Kygo"; box "Electronic"; box 1190000; box 30000];
        [box "235"; box "alesso@pop.net"; box "Alesso"; box "Electronic"; box 1470000; box 90000];
        [box "236"; box "calvin.harris@pop.net"; box "Calvin Harris"; box "Electronic"; box 1420000; box 50000];
        [box "237"; box "avicii@pop.net"; box "Avicii"; box "Electronic"; box 1060000; box 250000];
        [box "238"; box "zedd@pop.net"; box "Zedd"; box "Electronic"; box 1040000; box 40000];
        [box "239"; box "deadmau5@pop.net"; box "Deadmau5"; box "Electronic"; box 1100000; box 160000];
        [box "240"; box "david.guetta@pop.net"; box "David Guetta"; box "Electronic"; box 1060000; box 230000];
        [box "241"; box "martin.garrix@pop.net"; box "Martin Garrix"; box "Electronic"; box 1380000; box 110000];
        [box "242"; box "diplo@pop.net"; box "Diplo"; box "Electronic"; box 1130000; box 250000];
        [box "243"; box "major.lazer@pop.net"; box "Major Lazer"; box "Electronic"; box 1250000; box 40000];
        [box "244"; box "marshmello@pop.net"; box "Marshmello"; box "Electronic"; box 1180000; box 80000];
        [box "245"; box "flume@pop.net"; box "Flume"; box "Electronic"; box 1450000; box 120000];
        [box "246"; box "kygo@pop.net"; box "Kygo"; box "Electronic"; box 1450000; box 200000];
        [box "247"; box "alesso@pop.net"; box "Alesso"; box "Electronic"; box 1480000; box 20000];
        [box "248"; box "calvin.harris@pop.net"; box "Calvin Harris"; box "Electronic"; box 1220000; box 210000];
        [box "249"; box "avicii@pop.net"; box "Avicii"; box "Electronic"; box 1080000; box 290000];
        [box "250"; box "zedd@pop.net"; box "Zedd"; box "Electronic"; box 1240000; box 80000];
        [box "251"; box "deadmau5@pop.net"; box "Deadmau5"; box "Electronic"; box 1290000; box 180000];
        [box "252"; box "david.guetta@pop.net"; box "David Guetta"; box "Electronic"; box 1340000; box 10000];
        [box "253"; box "martin.garrix@pop.net"; box "Martin Garrix"; box "Electronic"; box 1320000; box 120000];
        [box "254"; box "diplo@pop.net"; box "Diplo"; box "Electronic"; box 1290000; box 150000];
        [box "255"; box "major.lazer@pop.net"; box "Major Lazer"; box "Electronic"; box 1040000; box 80000];
        [box "256"; box "marshmello@pop.net"; box "Marshmello"; box "Electronic"; box 1420000; box 20000];
        [box "257"; box "flume@pop.net"; box "Flume"; box "Electronic"; box 1420000; box 40000];
        [box "258"; box "kygo@pop.net"; box "Kygo"; box "Electronic"; box 1240000; box 80000];
        [box "259"; box "alesso@pop.net"; box "Alesso"; box "Electronic"; box 1310000; box 100000];
        [box "260"; box "calvin.harris@pop.net"; box "Calvin Harris"; box "Electronic"; box 1190000; box 100000];
        [box "261"; box "avicii@pop.net"; box "Avicii"; box "Electronic"; box 1280000; box 240000];
        [box "262"; box "zedd@pop.net"; box "Zedd"; box "Electronic"; box 1450000; box 70000];
        [box "263"; box "deadmau5@pop.net"; box "Deadmau5"; box "Electronic"; box 1060000; box 260000];
        [box "264"; box "david.guetta@pop.net"; box "David Guetta"; box "Electronic"; box 1460000; box 230000];
        [box "265"; box "martin.garrix@pop.net"; box "Martin Garrix"; box "Electronic"; box 1060000; box 30000];
        [box "266"; box "diplo@pop.net"; box "Diplo"; box "Electronic"; box 1340000; box 280000];
        [box "267"; box "major.lazer@pop.net"; box "Major Lazer"; box "Electronic"; box 1390000; box 160000];
        [box "268"; box "marshmello@pop.net"; box "Marshmello"; box "Electronic"; box 1450000; box 260000];
        [box "269"; box "flume@pop.net"; box "Flume"; box "Electronic"; box 1210000; box 280000];
        [box "270"; box "kygo@pop.net"; box "Kygo"; box "Electronic"; box 1460000; box 240000];
        [box "271"; box "alesso@pop.net"; box "Alesso"; box "Electronic"; box 1140000; box 170000];
        [box "272"; box "calvin.harris@pop.net"; box "Calvin Harris"; box "Electronic"; box 1460000; box 100000];
        [box "273"; box "avicii@pop.net"; box "Avicii"; box "Electronic"; box 1140000; box 160000];
        [box "274"; box "zedd@pop.net"; box "Zedd"; box "Electronic"; box 1280000; box 170000];
        [box "275"; box "deadmau5@pop.net"; box "Deadmau5"; box "Electronic"; box 1150000; box 260000];
        [box "276"; box "david.guetta@pop.net"; box "David Guetta"; box "Electronic"; box 1400000; box 160000];
        [box "277"; box "martin.garrix@pop.net"; box "Martin Garrix"; box "Electronic"; box 1160000; box 260000];
        [box "278"; box "diplo@pop.net"; box "Diplo"; box "Electronic"; box 1220000; box 140000];
        [box "279"; box "major.lazer@pop.net"; box "Major Lazer"; box "Electronic"; box 1210000; box 40000];
        [box "280"; box "marshmello@pop.net"; box "Marshmello"; box "Electronic"; box 1320000; box 150000];
        [box "281"; box "flume@pop.net"; box "Flume"; box "Electronic"; box 1110000; box 160000];
        [box "282"; box "kygo@pop.net"; box "Kygo"; box "Electronic"; box 1050000; box 200000];
        [box "283"; box "alesso@pop.net"; box "Alesso"; box "Electronic"; box 1160000; box 190000];
        [box "284"; box "calvin.harris@pop.net"; box "Calvin Harris"; box "Electronic"; box 1000000; box 0];
        [box "285"; box "avicii@pop.net"; box "Avicii"; box "Electronic"; box 1470000; box 100000];
        [box "286"; box "zedd@pop.net"; box "Zedd"; box "Electronic"; box 1280000; box 120000];
        [box "287"; box "deadmau5@pop.net"; box "Deadmau5"; box "Electronic"; box 1060000; box 90000];
        [box "288"; box "david.guetta@pop.net"; box "David Guetta"; box "Electronic"; box 1380000; box 210000];
        [box "289"; box "martin.garrix@pop.net"; box "Martin Garrix"; box "Electronic"; box 1310000; box 80000];
        [box "290"; box "diplo@pop.net"; box "Diplo"; box "Electronic"; box 1350000; box 80000];
        [box "291"; box "major.lazer@pop.net"; box "Major Lazer"; box "Electronic"; box 1010000; box 20000];
        [box "292"; box "marshmello@pop.net"; box "Marshmello"; box "Electronic"; box 1090000; box 280000];
        [box "293"; box "flume@pop.net"; box "Flume"; box "Electronic"; box 1010000; box 250000];
        [box "294"; box "kygo@pop.net"; box "Kygo"; box "Electronic"; box 1170000; box 190000];
        [box "295"; box "alesso@pop.net"; box "Alesso"; box "Electronic"; box 1420000; box 40000];
        [box "296"; box "calvin.harris@pop.net"; box "Calvin Harris"; box "Electronic"; box 1080000; box 290000];
        [box "297"; box "avicii@pop.net"; box "Avicii"; box "Electronic"; box 1000000; box 40000];
        [box "298"; box "zedd@pop.net"; box "Zedd"; box "Electronic"; box 1150000; box 140000];
        [box "299"; box "deadmau5@pop.net"; box "Deadmau5"; box "Electronic"; box 1090000; box 280000];
        [box "300"; box "david.guetta@pop.net"; box "David Guetta"; box "Electronic"; box 1360000; box 110000];
        [box "301"; box "queen@rocklegend.com"; box "Queen"; box "Rock"; box 1130000; box 200000];
        [box "302"; box "led.zeppelin@rocklegend.com"; box "Led Zeppelin"; box "Rock"; box 1130000; box 198000];
        [box "303"; box "pink.floyd@rocklegend.com"; box "Pink Floyd"; box "Rock"; box 1170000; box 196000];
        [box "304"; box "the.rolling.stones@rocklegend.com"; box "The Rolling Stones"; box "Rock"; box 1350000; box 194000];
        [box "305"; box "the.beatles@rocklegend.com"; box "The Beatles"; box "Rock"; box 1010000; box 192000];
        [box "306"; box "metallica@rocklegend.com"; box "Metallica"; box "Metal"; box 1060000; box 190000];
        [box "307"; box "nirvana@rocklegend.com"; box "Nirvana"; box "Grunge"; box 1070000; box 188000];
        [box "308"; box "guns.n.roses@rocklegend.com"; box "Guns N' Roses"; box "Rock"; box 1370000; box 186000];
        [box "309"; box "aerosmith@rocklegend.com"; box "Aerosmith"; box "Rock"; box 1420000; box 184000];
        [box "310"; box "u2@rocklegend.com"; box "U2"; box "Rock"; box 1220000; box 182000];
        [box "311"; box "red.hot.chili.peppers@rocklegend.com"; box "Red Hot Chili Peppers"; box "Rock"; box 1290000; box 180000];
        [box "312"; box "the.who@rocklegend.com"; box "The Who"; box "Rock"; box 1430000; box 178000];
        [box "313"; box "radiohead@rocklegend.com"; box "Radiohead"; box "Alternative"; box 1080000; box 176000];
        [box "314"; box "coldplay@rocklegend.com"; box "Coldplay"; box "Alternative"; box 1040000; box 174000];
        [box "315"; box "foo.fighters@rocklegend.com"; box "Foo Fighters"; box "Rock"; box 1100000; box 172000];
        [box "316"; box "green.day@rocklegend.com"; box "Green Day"; box "Punk"; box 1430000; box 170000];
        [box "317"; box "blink.182@rocklegend.com"; box "Blink 182"; box "Punk"; box 1040000; box 168000];
        [box "318"; box "muse@rocklegend.com"; box "Muse"; box "Alternative"; box 1440000; box 166000];
        [box "319"; box "the.killers@rocklegend.com"; box "The Killers"; box "Alternative"; box 1190000; box 164000];
        [box "320"; box "arctic.monkeys@rocklegend.com"; box "Arctic Monkeys"; box "Alternative"; box 1090000; box 162000];
        [box "321"; box "linkin.park@rocklegend.com"; box "Linkin Park"; box "Nu Metal"; box 1000000; box 160000];
        [box "322"; box "paramore@rocklegend.com"; box "Paramore"; box "Alternative"; box 1050000; box 158000];
        [box "323"; box "fall.out.boy@rocklegend.com"; box "Fall Out Boy"; box "Alternative"; box 1440000; box 156000];
        [box "324"; box "my.chemical.romance@rocklegend.com"; box "My Chemical Romance"; box "Alternative"; box 1250000; box 154000];
        [box "325"; box "panic.at.the.disco@rocklegend.com"; box "Panic! At The Disco"; box "Alternative"; box 1440000; box 152000];
        [box "326"; box "the.white.stripes@rocklegend.com"; box "The White Stripes"; box "Alternative"; box 1150000; box 150000];
        [box "327"; box "the.black.keys@rocklegend.com"; box "The Black Keys"; box "Alternative"; box 1060000; box 148000];
        [box "328"; box "creedence.clearwater@rocklegend.com"; box "Creedence Clearwater Revival"; box "Rock"; box 1350000; box 146000];
        [box "329"; box "the.eagles@rocklegend.com"; box "The Eagles"; box "Rock"; box 1350000; box 144000];
        [box "330"; box "fleetwood.mac@rocklegend.com"; box "Fleetwood Mac"; box "Rock"; box 1000000; box 142000];
        [box "331"; box "led.zeppelin@rocklegend.com"; box "Led Zeppelin"; box "Rock"; box 1130000; box 140000];
        [box "332"; box "aerosmith@rocklegend.com"; box "Aerosmith"; box "Rock"; box 1200000; box 138000];
        [box "333"; box "queen@rocklegend.com"; box "Queen"; box "Rock"; box 1140000; box 136000];
        [box "334"; box "nirvana@rocklegend.com"; box "Nirvana"; box "Grunge"; box 1030000; box 134000];
        [box "335"; box "metallica@rocklegend.com"; box "Metallica"; box "Metal"; box 1370000; box 132000];
        [box "336"; box "the.beatles@rocklegend.com"; box "The Beatles"; box "Rock"; box 1230000; box 130000];
        [box "337"; box "guns.n.roses@rocklegend.com"; box "Guns N' Roses"; box "Rock"; box 1220000; box 128000];
        [box "338"; box "u2@rocklegend.com"; box "U2"; box "Rock"; box 1090000; box 126000];
        [box "339"; box "red.hot.chili.peppers@rocklegend.com"; box "Red Hot Chili Peppers"; box "Rock"; box 1320000; box 124000];
        [box "340"; box "pink.floyd@rocklegend.com"; box "Pink Floyd"; box "Rock"; box 1320000; box 122000];
        [box "341"; box "coldplay@rocklegend.com"; box "Coldplay"; box "Alternative"; box 1210000; box 120000];
        [box "342"; box "radiohead@rocklegend.com"; box "Radiohead"; box "Alternative"; box 1160000; box 118000];
        [box "343"; box "the.who@rocklegend.com"; box "The Who"; box "Rock"; box 1250000; box 116000];
        [box "344"; box "foo.fighters@rocklegend.com"; box "Foo Fighters"; box "Rock"; box 1060000; box 114000];
        [box "345"; box "green.day@rocklegend.com"; box "Green Day"; box "Punk"; box 1140000; box 112000];
        [box "346"; box "blink.182@rocklegend.com"; box "Blink 182"; box "Punk"; box 1010000; box 110000];
        [box "347"; box "muse@rocklegend.com"; box "Muse"; box "Alternative"; box 1270000; box 108000];
        [box "348"; box "the.killers@rocklegend.com"; box "The Killers"; box "Alternative"; box 1320000; box 106000];
        [box "349"; box "arctic.monkeys@rocklegend.com"; box "Arctic Monkeys"; box "Alternative"; box 1330000; box 104000];
        [box "350"; box "linkin.park@rocklegend.com"; box "Linkin Park"; box "Nu Metal"; box 1080000; box 102000];
        [box "351"; box "paramore@rocklegend.com"; box "Paramore"; box "Alternative"; box 1060000; box 100000];
        [box "352"; box "fall.out.boy@rocklegend.com"; box "Fall Out Boy"; box "Alternative"; box 1010000; box 98000];
        [box "353"; box "my.chemical.romance@rocklegend.com"; box "My Chemical Romance"; box "Alternative"; box 1140000; box 96000];
        [box "354"; box "panic.at.the.disco@rocklegend.com"; box "Panic! At The Disco"; box "Alternative"; box 1130000; box 94000];
        [box "355"; box "the.white.stripes@rocklegend.com"; box "The White Stripes"; box "Alternative"; box 1030000; box 92000];
        [box "356"; box "the.black.keys@rocklegend.com"; box "The Black Keys"; box "Alternative"; box 1450000; box 90000];
        [box "357"; box "creedence.clearwater@rocklegend.com"; box "Creedence Clearwater Revival"; box "Rock"; box 1050000; box 88000];
        [box "358"; box "the.eagles@rocklegend.com"; box "The Eagles"; box "Rock"; box 1280000; box 86000];
        [box "359"; box "fleetwood.mac@rocklegend.com"; box "Fleetwood Mac"; box "Rock"; box 1270000; box 84000];
        [box "360"; box "led.zeppelin@rocklegend.com"; box "Led Zeppelin"; box "Rock"; box 1220000; box 82000];
        [box "361"; box "aerosmith@rocklegend.com"; box "Aerosmith"; box "Rock"; box 1350000; box 80000];
        [box "362"; box "queen@rocklegend.com"; box "Queen"; box "Rock"; box 1080000; box 78000];
        [box "363"; box "nirvana@rocklegend.com"; box "Nirvana"; box "Grunge"; box 1370000; box 76000];
        [box "364"; box "metallica@rocklegend.com"; box "Metallica"; box "Metal"; box 1280000; box 74000];
        [box "365"; box "the.beatles@rocklegend.com"; box "The Beatles"; box "Rock"; box 1450000; box 72000];
        [box "366"; box "guns.n.roses@rocklegend.com"; box "Guns N' Roses"; box "Rock"; box 1450000; box 70000];
        [box "367"; box "u2@rocklegend.com"; box "U2"; box "Rock"; box 1150000; box 68000];
        [box "368"; box "red.hot.chili.peppers@rocklegend.com"; box "Red Hot Chili Peppers"; box "Rock"; box 1420000; box 66000];
        [box "369"; box "pink.floyd@rocklegend.com"; box "Pink Floyd"; box "Rock"; box 1030000; box 64000];
        [box "370"; box "coldplay@rocklegend.com"; box "Coldplay"; box "Alternative"; box 1220000; box 62000];
        [box "371"; box "radiohead@rocklegend.com"; box "Radiohead"; box "Alternative"; box 1060000; box 60000];
        [box "372"; box "the.who@rocklegend.com"; box "The Who"; box "Rock"; box 1300000; box 58000];
        [box "373"; box "foo.fighters@rocklegend.com"; box "Foo Fighters"; box "Rock"; box 1480000; box 56000];
        [box "374"; box "green.day@rocklegend.com"; box "Green Day"; box "Punk"; box 1230000; box 54000];
        [box "375"; box "blink.182@rocklegend.com"; box "Blink 182"; box "Punk"; box 1240000; box 52000];
        [box "376"; box "muse@rocklegend.com"; box "Muse"; box "Alternative"; box 1210000; box 50000];
        [box "377"; box "the.killers@rocklegend.com"; box "The Killers"; box "Alternative"; box 1150000; box 48000];
        [box "378"; box "arctic.monkeys@rocklegend.com"; box "Arctic Monkeys"; box "Alternative"; box 1250000; box 46000];
        [box "379"; box "linkin.park@rocklegend.com"; box "Linkin Park"; box "Nu Metal"; box 1470000; box 44000];
        [box "380"; box "paramore@rocklegend.com"; box "Paramore"; box "Alternative"; box 1280000; box 42000];
        [box "381"; box "fall.out.boy@rocklegend.com"; box "Fall Out Boy"; box "Alternative"; box 1240000; box 40000];
        [box "382"; box "my.chemical.romance@rocklegend.com"; box "My Chemical Romance"; box "Alternative"; box 1100000; box 38000];
        [box "383"; box "panic.at.the.disco@rocklegend.com"; box "Panic! At The Disco"; box "Alternative"; box 1380000; box 36000];
        [box "384"; box "the.white.stripes@rocklegend.com"; box "The White Stripes"; box "Alternative"; box 1130000; box 34000];
        [box "385"; box "the.black.keys@rocklegend.com"; box "The Black Keys"; box "Alternative"; box 1300000; box 32000];
        [box "386"; box "creedence.clearwater@rocklegend.com"; box "Creedence Clearwater Revival"; box "Rock"; box 1480000; box 30000];
        [box "387"; box "the.eagles@rocklegend.com"; box "The Eagles"; box "Rock"; box 1420000; box 220000];
        [box "388"; box "fleetwood.mac@rocklegend.com"; box "Fleetwood Mac"; box "Rock"; box 1350000; box 170000];
        [box "389"; box "led.zeppelin@rocklegend.com"; box "Led Zeppelin"; box "Rock"; box 1050000; box 180000];
        [box "390"; box "aerosmith@rocklegend.com"; box "Aerosmith"; box "Rock"; box 1180000; box 270000];
        [box "391"; box "queen@rocklegend.com"; box "Queen"; box "Rock"; box 1330000; box 290000];
        [box "392"; box "nirvana@rocklegend.com"; box "Nirvana"; box "Grunge"; box 1140000; box 200000];
        [box "393"; box "metallica@rocklegend.com"; box "Metallica"; box "Metal"; box 1460000; box 30000];
        [box "394"; box "the.beatles@rocklegend.com"; box "The Beatles"; box "Rock"; box 1230000; box 120000];
        [box "395"; box "guns.n.roses@rocklegend.com"; box "Guns N' Roses"; box "Rock"; box 1150000; box 280000];
        [box "396"; box "u2@rocklegend.com"; box "U2"; box "Rock"; box 1380000; box 60000];
        [box "397"; box "red.hot.chili.peppers@rocklegend.com"; box "Red Hot Chili Peppers"; box "Rock"; box 1390000; box 160000];
        [box "398"; box "pink.floyd@rocklegend.com"; box "Pink Floyd"; box "Rock"; box 1250000; box 110000];
        [box "399"; box "coldplay@rocklegend.com"; box "Coldplay"; box "Alternative"; box 1150000; box 90000];
        [box "400"; box "radiohead@rocklegend.com"; box "Radiohead"; box "Alternative"; box 1170000; box 200000];
        [box "401"; box "the.who@rocklegend.com"; box "The Who"; box "Rock"; box 1340000; box 60000];
        [box "402"; box "foo.fighters@rocklegend.com"; box "Foo Fighters"; box "Rock"; box 1060000; box 60000];
        [box "403"; box "green.day@rocklegend.com"; box "Green Day"; box "Punk"; box 1330000; box 200000];
        [box "404"; box "blink.182@rocklegend.com"; box "Blink 182"; box "Punk"; box 1380000; box 80000];
        [box "405"; box "muse@rocklegend.com"; box "Muse"; box "Alternative"; box 1420000; box 60000];
        [box "406"; box "the.killers@rocklegend.com"; box "The Killers"; box "Alternative"; box 1040000; box 70000];
        [box "407"; box "arctic.monkeys@rocklegend.com"; box "Arctic Monkeys"; box "Alternative"; box 1480000; box 230000];
        [box "408"; box "linkin.park@rocklegend.com"; box "Linkin Park"; box "Nu Metal"; box 1260000; box 70000];
        [box "409"; box "paramore@rocklegend.com"; box "Paramore"; box "Alternative"; box 1420000; box 100000];
        [box "410"; box "fall.out.boy@rocklegend.com"; box "Fall Out Boy"; box "Alternative"; box 1140000; box 160000];
        [box "411"; box "my.chemical.romance@rocklegend.com"; box "My Chemical Romance"; box "Alternative"; box 1420000; box 230000];
        [box "412"; box "panic.at.the.disco@rocklegend.com"; box "Panic! At The Disco"; box "Alternative"; box 1310000; box 160000];
        [box "413"; box "the.white.stripes@rocklegend.com"; box "The White Stripes"; box "Alternative"; box 1110000; box 30000];
        [box "414"; box "the.black.keys@rocklegend.com"; box "The Black Keys"; box "Alternative"; box 1450000; box 10000];
        [box "415"; box "creedence.clearwater@rocklegend.com"; box "Creedence Clearwater Revival"; box "Rock"; box 1480000; box 10000];
        [box "416"; box "the.eagles@rocklegend.com"; box "The Eagles"; box "Rock"; box 1490000; box 150000];
        [box "417"; box "fleetwood.mac@rocklegend.com"; box "Fleetwood Mac"; box "Rock"; box 1490000; box 120000];
        [box "418"; box "led.zeppelin@rocklegend.com"; box "Led Zeppelin"; box "Rock"; box 1470000; box 50000];
        [box "419"; box "aerosmith@rocklegend.com"; box "Aerosmith"; box "Rock"; box 1280000; box 240000];
        [box "420"; box "queen@rocklegend.com"; box "Queen"; box "Rock"; box 1360000; box 290000];
        [box "421"; box "nirvana@rocklegend.com"; box "Nirvana"; box "Grunge"; box 1030000; box 260000];
        [box "422"; box "metallica@rocklegend.com"; box "Metallica"; box "Metal"; box 1140000; box 240000];
        [box "423"; box "the.beatles@rocklegend.com"; box "The Beatles"; box "Rock"; box 1280000; box 70000];
        [box "424"; box "guns.n.roses@rocklegend.com"; box "Guns N' Roses"; box "Rock"; box 1490000; box 210000];
        [box "425"; box "u2@rocklegend.com"; box "U2"; box "Rock"; box 1360000; box 290000];
        [box "426"; box "red.hot.chili.peppers@rocklegend.com"; box "Red Hot Chili Peppers"; box "Rock"; box 1120000; box 200000];
        [box "427"; box "pink.floyd@rocklegend.com"; box "Pink Floyd"; box "Rock"; box 1370000; box 150000];
        [box "428"; box "coldplay@rocklegend.com"; box "Coldplay"; box "Alternative"; box 1280000; box 180000];
        [box "429"; box "radiohead@rocklegend.com"; box "Radiohead"; box "Alternative"; box 1150000; box 80000];
        [box "430"; box "the.who@rocklegend.com"; box "The Who"; box "Rock"; box 1350000; box 80000];
        [box "431"; box "foo.fighters@rocklegend.com"; box "Foo Fighters"; box "Rock"; box 1240000; box 130000];
        [box "432"; box "green.day@rocklegend.com"; box "Green Day"; box "Punk"; box 1020000; box 150000];
        [box "433"; box "blink.182@rocklegend.com"; box "Blink 182"; box "Punk"; box 1470000; box 90000];
        [box "434"; box "muse@rocklegend.com"; box "Muse"; box "Alternative"; box 1480000; box 80000];
        [box "435"; box "the.killers@rocklegend.com"; box "The Killers"; box "Alternative"; box 1270000; box 20000];
        [box "436"; box "arctic.monkeys@rocklegend.com"; box "Arctic Monkeys"; box "Alternative"; box 1190000; box 230000];
        [box "437"; box "linkin.park@rocklegend.com"; box "Linkin Park"; box "Nu Metal"; box 1170000; box 10000];
        [box "438"; box "paramore@rocklegend.com"; box "Paramore"; box "Alternative"; box 1110000; box 200000];
        [box "439"; box "fall.out.boy@rocklegend.com"; box "Fall Out Boy"; box "Alternative"; box 1110000; box 160000];
        [box "440"; box "my.chemical.romance@rocklegend.com"; box "My Chemical Romance"; box "Alternative"; box 1350000; box 210000];
        [box "441"; box "panic.at.the.disco@rocklegend.com"; box "Panic! At The Disco"; box "Alternative"; box 1420000; box 260000];
        [box "442"; box "the.white.stripes@rocklegend.com"; box "The White Stripes"; box "Alternative"; box 1010000; box 210000];
        [box "443"; box "the.black.keys@rocklegend.com"; box "The Black Keys"; box "Alternative"; box 1260000; box 0];
        [box "444"; box "creedence.clearwater@rocklegend.com"; box "Creedence Clearwater Revival"; box "Rock"; box 1230000; box 60000];
        [box "445"; box "the.eagles@rocklegend.com"; box "The Eagles"; box "Rock"; box 1270000; box 10000];
        [box "446"; box "fleetwood.mac@rocklegend.com"; box "Fleetwood Mac"; box "Rock"; box 1290000; box 0];
        [box "447"; box "led.zeppelin@rocklegend.com"; box "Led Zeppelin"; box "Rock"; box 1400000; box 40000];
        [box "448"; box "aerosmith@rocklegend.com"; box "Aerosmith"; box "Rock"; box 1100000; box 90000];
        [box "449"; box "queen@rocklegend.com"; box "Queen"; box "Rock"; box 1280000; box 140000];
        [box "450"; box "nirvana@rocklegend.com"; box "Nirvana"; box "Grunge"; box 1300000; box 290000];
        [box "451"; box "metallica@rocklegend.com"; box "Metallica"; box "Metal"; box 1490000; box 130000];
        [box "452"; box "the.beatles@rocklegend.com"; box "The Beatles"; box "Rock"; box 1450000; box 110000];
        [box "453"; box "guns.n.roses@rocklegend.com"; box "Guns N' Roses"; box "Rock"; box 1150000; box 50000];
        [box "454"; box "u2@rocklegend.com"; box "U2"; box "Rock"; box 1030000; box 130000];
        [box "455"; box "red.hot.chili.peppers@rocklegend.com"; box "Red Hot Chili Peppers"; box "Rock"; box 1000000; box 190000];
        [box "456"; box "pink.floyd@rocklegend.com"; box "Pink Floyd"; box "Rock"; box 1050000; box 150000];
        [box "457"; box "coldplay@rocklegend.com"; box "Coldplay"; box "Alternative"; box 1150000; box 210000];
        [box "458"; box "radiohead@rocklegend.com"; box "Radiohead"; box "Alternative"; box 1080000; box 90000];
        [box "459"; box "the.who@rocklegend.com"; box "The Who"; box "Rock"; box 1080000; box 10000];
        [box "460"; box "foo.fighters@rocklegend.com"; box "Foo Fighters"; box "Rock"; box 1360000; box 190000];
        [box "461"; box "green.day@rocklegend.com"; box "Green Day"; box "Punk"; box 1280000; box 140000];
        [box "462"; box "blink.182@rocklegend.com"; box "Blink 182"; box "Punk"; box 1220000; box 60000];
        [box "463"; box "muse@rocklegend.com"; box "Muse"; box "Alternative"; box 1110000; box 30000];
        [box "464"; box "the.killers@rocklegend.com"; box "The Killers"; box "Alternative"; box 1340000; box 270000];
        [box "465"; box "arctic.monkeys@rocklegend.com"; box "Arctic Monkeys"; box "Alternative"; box 1270000; box 180000];
        [box "466"; box "linkin.park@rocklegend.com"; box "Linkin Park"; box "Nu Metal"; box 1400000; box 130000];
        [box "467"; box "paramore@rocklegend.com"; box "Paramore"; box "Alternative"; box 1320000; box 130000];
        [box "468"; box "fall.out.boy@rocklegend.com"; box "Fall Out Boy"; box "Alternative"; box 1220000; box 30000];
        [box "469"; box "my.chemical.romance@rocklegend.com"; box "My Chemical Romance"; box "Alternative"; box 1350000; box 110000];
        [box "470"; box "panic.at.the.disco@rocklegend.com"; box "Panic! At The Disco"; box "Alternative"; box 1060000; box 10000];
        [box "471"; box "the.white.stripes@rocklegend.com"; box "The White Stripes"; box "Alternative"; box 1330000; box 220000];
        [box "472"; box "the.black.keys@rocklegend.com"; box "The Black Keys"; box "Alternative"; box 1470000; box 190000];
        [box "473"; box "creedence.clearwater@rocklegend.com"; box "Creedence Clearwater Revival"; box "Rock"; box 1330000; box 240000];
        [box "474"; box "the.eagles@rocklegend.com"; box "The Eagles"; box "Rock"; box 1210000; box 240000];
        [box "475"; box "fleetwood.mac@rocklegend.com"; box "Fleetwood Mac"; box "Rock"; box 1350000; box 180000];
        [box "476"; box "led.zeppelin@rocklegend.com"; box "Led Zeppelin"; box "Rock"; box 1440000; box 70000];
        [box "477"; box "aerosmith@rocklegend.com"; box "Aerosmith"; box "Rock"; box 1240000; box 200000];
        [box "478"; box "queen@rocklegend.com"; box "Queen"; box "Rock"; box 1070000; box 70000];
        [box "479"; box "nirvana@rocklegend.com"; box "Nirvana"; box "Grunge"; box 1350000; box 120000];
        [box "480"; box "metallica@rocklegend.com"; box "Metallica"; box "Metal"; box 1430000; box 270000];
        [box "481"; box "the.beatles@rocklegend.com"; box "The Beatles"; box "Rock"; box 1110000; box 260000];
        [box "482"; box "guns.n.roses@rocklegend.com"; box "Guns N' Roses"; box "Rock"; box 1300000; box 30000];
        [box "483"; box "u2@rocklegend.com"; box "U2"; box "Rock"; box 1270000; box 110000];
        [box "484"; box "red.hot.chili.peppers@rocklegend.com"; box "Red Hot Chili Peppers"; box "Rock"; box 1280000; box 170000];
        [box "485"; box "pink.floyd@rocklegend.com"; box "Pink Floyd"; box "Rock"; box 1490000; box 230000];
        [box "486"; box "coldplay@rocklegend.com"; box "Coldplay"; box "Alternative"; box 1160000; box 150000];
        [box "487"; box "radiohead@rocklegend.com"; box "Radiohead"; box "Alternative"; box 1330000; box 110000];
        [box "488"; box "the.who@rocklegend.com"; box "The Who"; box "Rock"; box 1350000; box 160000];
        [box "489"; box "foo.fighters@rocklegend.com"; box "Foo Fighters"; box "Rock"; box 1130000; box 160000];
        [box "490"; box "green.day@rocklegend.com"; box "Green Day"; box "Punk"; box 1130000; box 40000];
        [box "491"; box "blink.182@rocklegend.com"; box "Blink 182"; box "Punk"; box 1200000; box 170000];
        [box "492"; box "muse@rocklegend.com"; box "Muse"; box "Alternative"; box 1460000; box 200000];
        [box "493"; box "the.killers@rocklegend.com"; box "The Killers"; box "Alternative"; box 1230000; box 90000];
        [box "494"; box "arctic.monkeys@rocklegend.com"; box "Arctic Monkeys"; box "Alternative"; box 1340000; box 210000];
        [box "495"; box "linkin.park@rocklegend.com"; box "Linkin Park"; box "Nu Metal"; box 1000000; box 280000];
        [box "496"; box "paramore@rocklegend.com"; box "Paramore"; box "Alternative"; box 1070000; box 0];
        [box "497"; box "fall.out.boy@rocklegend.com"; box "Fall Out Boy"; box "Alternative"; box 1120000; box 270000];
        [box "498"; box "my.chemical.romance@rocklegend.com"; box "My Chemical Romance"; box "Alternative"; box 1270000; box 150000];
        [box "499"; box "panic.at.the.disco@rocklegend.com"; box "Panic! At The Disco"; box "Alternative"; box 1420000; box 160000];
        [box "500"; box "the.white.stripes@rocklegend.com"; box "The White Stripes"; box "Alternative"; box 1410000; box 0];
        [box "501"; box "the.black.keys@rocklegend.com"; box "The Black Keys"; box "Alternative"; box 1480000; box 170000];
        [box "502"; box "creedence.clearwater@rocklegend.com"; box "Creedence Clearwater Revival"; box "Rock"; box 1280000; box 190000];
        [box "503"; box "the.eagles@rocklegend.com"; box "The Eagles"; box "Rock"; box 1490000; box 30000];
        [box "504"; box "fleetwood.mac@rocklegend.com"; box "Fleetwood Mac"; box "Rock"; box 1140000; box 240000];
        [box "505"; box "led.zeppelin@rocklegend.com"; box "Led Zeppelin"; box "Rock"; box 1340000; box 100000];
        [box "506"; box "aerosmith@rocklegend.com"; box "Aerosmith"; box "Rock"; box 1340000; box 260000];
        [box "507"; box "queen@rocklegend.com"; box "Queen"; box "Rock"; box 1440000; box 130000];
        [box "508"; box "nirvana@rocklegend.com"; box "Nirvana"; box "Grunge"; box 1120000; box 110000];
        [box "509"; box "metallica@rocklegend.com"; box "Metallica"; box "Metal"; box 1340000; box 200000];
        [box "510"; box "the.beatles@rocklegend.com"; box "The Beatles"; box "Rock"; box 1180000; box 280000];
        [box "511"; box "guns.n.roses@rocklegend.com"; box "Guns N' Roses"; box "Rock"; box 1200000; box 170000];
        [box "512"; box "u2@rocklegend.com"; box "U2"; box "Rock"; box 1150000; box 220000];
        [box "513"; box "red.hot.chili.peppers@rocklegend.com"; box "Red Hot Chili Peppers"; box "Rock"; box 1170000; box 10000];
        [box "514"; box "pink.floyd@rocklegend.com"; box "Pink Floyd"; box "Rock"; box 1490000; box 140000];
        [box "515"; box "coldplay@rocklegend.com"; box "Coldplay"; box "Alternative"; box 1280000; box 270000];
        [box "516"; box "radiohead@rocklegend.com"; box "Radiohead"; box "Alternative"; box 1200000; box 120000];
        [box "517"; box "the.who@rocklegend.com"; box "The Who"; box "Rock"; box 1210000; box 280000];
        [box "518"; box "foo.fighters@rocklegend.com"; box "Foo Fighters"; box "Rock"; box 1450000; box 160000];
        [box "519"; box "green.day@rocklegend.com"; box "Green Day"; box "Punk"; box 1370000; box 290000];
        [box "520"; box "blink.182@rocklegend.com"; box "Blink 182"; box "Punk"; box 1200000; box 20000];
        [box "521"; box "muse@rocklegend.com"; box "Muse"; box "Alternative"; box 1360000; box 170000];
        [box "522"; box "the.killers@rocklegend.com"; box "The Killers"; box "Alternative"; box 1370000; box 110000];
        [box "523"; box "arctic.monkeys@rocklegend.com"; box "Arctic Monkeys"; box "Alternative"; box 1480000; box 290000];
        [box "524"; box "linkin.park@rocklegend.com"; box "Linkin Park"; box "Nu Metal"; box 1420000; box 150000];
        [box "525"; box "paramore@rocklegend.com"; box "Paramore"; box "Alternative"; box 1190000; box 260000];
        [box "526"; box "fall.out.boy@rocklegend.com"; box "Fall Out Boy"; box "Alternative"; box 1270000; box 270000];
        [box "527"; box "my.chemical.romance@rocklegend.com"; box "My Chemical Romance"; box "Alternative"; box 1450000; box 170000];
        [box "528"; box "panic.at.the.disco@rocklegend.com"; box "Panic! At The Disco"; box "Alternative"; box 1160000; box 30000];
        [box "529"; box "the.white.stripes@rocklegend.com"; box "The White Stripes"; box "Alternative"; box 1490000; box 150000];
        [box "530"; box "the.black.keys@rocklegend.com"; box "The Black Keys"; box "Alternative"; box 1020000; box 200000];
        [box "531"; box "creedence.clearwater@rocklegend.com"; box "Creedence Clearwater Revival"; box "Rock"; box 1410000; box 160000];
        [box "532"; box "the.eagles@rocklegend.com"; box "The Eagles"; box "Rock"; box 1480000; box 140000];
        [box "533"; box "fleetwood.mac@rocklegend.com"; box "Fleetwood Mac"; box "Rock"; box 1320000; box 290000];
        [box "534"; box "led.zeppelin@rocklegend.com"; box "Led Zeppelin"; box "Rock"; box 1090000; box 190000];
        [box "535"; box "aerosmith@rocklegend.com"; box "Aerosmith"; box "Rock"; box 1130000; box 120000];
        [box "536"; box "queen@rocklegend.com"; box "Queen"; box "Rock"; box 1180000; box 130000];
        [box "537"; box "nirvana@rocklegend.com"; box "Nirvana"; box "Grunge"; box 1490000; box 0];
        [box "538"; box "metallica@rocklegend.com"; box "Metallica"; box "Metal"; box 1470000; box 160000];
        [box "539"; box "the.beatles@rocklegend.com"; box "The Beatles"; box "Rock"; box 1270000; box 190000];
        [box "540"; box "guns.n.roses@rocklegend.com"; box "Guns N' Roses"; box "Rock"; box 1250000; box 20000];
        [box "541"; box "u2@rocklegend.com"; box "U2"; box "Rock"; box 1460000; box 60000];
        [box "542"; box "red.hot.chili.peppers@rocklegend.com"; box "Red Hot Chili Peppers"; box "Rock"; box 1400000; box 120000];
        [box "543"; box "pink.floyd@rocklegend.com"; box "Pink Floyd"; box "Rock"; box 1050000; box 60000];
        [box "544"; box "coldplay@rocklegend.com"; box "Coldplay"; box "Alternative"; box 1120000; box 170000];
        [box "545"; box "radiohead@rocklegend.com"; box "Radiohead"; box "Alternative"; box 1180000; box 150000];
        [box "546"; box "the.who@rocklegend.com"; box "The Who"; box "Rock"; box 1140000; box 280000];
        [box "547"; box "foo.fighters@rocklegend.com"; box "Foo Fighters"; box "Rock"; box 1330000; box 130000];
        [box "548"; box "green.day@rocklegend.com"; box "Green Day"; box "Punk"; box 1490000; box 270000];
        [box "549"; box "blink.182@rocklegend.com"; box "Blink 182"; box "Punk"; box 1170000; box 290000];
        [box "550"; box "muse@rocklegend.com"; box "Muse"; box "Alternative"; box 1230000; box 210000]
    |]
    
    //colHdrs: unid,emailAdd,Name,MngrUserId,isMngrBool,DirectReportsLi,MemOfGrpsLi
    let nabTbl = 
      [|
        [box "390"; box "amelia.sales10@acmecorp.com"; box "Amelia Sales10"; box "michael.manager20@acmecorp.com"; box false; box []; box ["Sales"]];
        [box "391"; box "michael.marketing1@acmecorp.com"; box "Michael Marketing1"; box "john.manager11@acmecorp.com"; box false; box []; box ["Marketing"]] 
    |]
    
    printfn "Music:%A nab:%A" (musicTbl.Length) (nabTbl.Length)
    
    let random = Random()
    let sb = StringBuilder()
    
    // MultiSelect options
    let options = [|"IT"; "Logistics"; "Legal"; "HR"; "Sales"; "Marketing"|]
    
    let mutable listBldr = []
    
    // Generate 550 rows
    for i in 0..549 do
        // Long Text: 2-3 random lines
        let lineCount = random.Next(2, 4)
        let textLines = 
            [| for _ in 1..lineCount -> 
                let words = random.Next(5, 15)
                [| for _ in 1..words -> 
                    let prefixes = [|"This"; "The"; "A"; "An"; "Our"; "Your"; "Many"; "Few"|]
                    let suffixes = [|"description"; "system"; "process"; "solution"; "method"; "approach"; "framework"; "model"|]
                    sprintf "%s %s" prefixes.[random.Next(prefixes.Length)] suffixes.[random.Next(suffixes.Length)]
                |] |> String.concat " "
            |]
        let longText = String.concat " " textLines
        
        // Percent: 00.00 to 99.99 formatted as 0.75
        let percent = random.NextDouble() * 1.0
        let percentStr = percent.ToString("F2")
        
        // Date: random date in 2025
        let date = DateTime(2025, random.Next(1, 13), random.Next(1, 29))
        let dateStr = date.ToString("yyyy-MM-dd")
        
        // Time: random time hh:mm
        let time = DateTime.Today.AddHours(float (random.Next(24))).AddMinutes(float (random.Next(60)))
        let timeStr = time.ToString("HH:mm")
        
        // DateTime: random datetime
        let dateTime = DateTime(2025, random.Next(1, 13), random.Next(1, 29), random.Next(24), random.Next(60), 0)
        let dateTimeStr = dateTime.ToString("yyyy-MM-ddTHH:mm:ss")
        
        // Boolean: random true/false
        let boolValue = random.Next(2) //= 0
        let boolStr = 
            match boolValue with
            | 0 -> false
            | _ -> true
//boolValue.ToString().ToLower()

        
        // MultiSelect: 1-3 random values
        let count = random.Next(1, 4)
        let selected = 
            [| for _ in 1..count -> options.[random.Next(options.Length)] |]
            |> Array.distinct
            |> Array.sort
            |> List.ofArray

        // Phone: random US phone number
        let areaCode = random.Next(200, 1000)
        let exchange = random.Next(100, 1000)
        let number = random.Next(1000, 10000)
        let phoneStr = sprintf "+1-%03d-%03d-%04d" areaCode exchange number
        
        // URL: random weather.gov subdomains
        let subdomains = [|"www"; "forecast"; "radar"; "climate"; "safety"|]
        let subdomain = subdomains.[random.Next(subdomains.Length)]
        let urlStr = sprintf "https://%s.weather.gov" subdomain
        
        // Rating: 1-5
        let rating = random.Next(1, 6)
        
        // Duration: random HH:MM:SS
        let hours = random.Next(0, 24)
        let minutes = random.Next(0, 60)
        let seconds = random.Next(0, 60)
        let durationStr = sprintf "%02d:%02d:%02d" hours minutes seconds
        
        let cc = musicTbl.[i] //currCorrespMusicRec
        let ccSales:int = unbox cc.[4]
        let ccSal:int = unbox cc.[5]
        let recrd = { ID = (string) cc.[0]; emailAdd=(string) cc.[1];
                        Name=(string) cc.[2];genre=(string) cc.[3];sales=ccSales;salary=ccSal;
                        longText = longText; percent = percent; date = dateStr ; time=timeStr; 
                        dateTime= DateTime.Parse(dateTimeStr); booln= boolStr;
                        MultiSel=selected; phone=phoneStr; url=urlStr; rating=rating; duration=durationStr}

        listBldr <- recrd :: listBldr
    
    //coz 1st is id:550
    listBldr <- List.rev listBldr

    printfn "listBldr len: %s" (listBldr.Length.ToString())

    //for ref in case of snafus (this is a rec used by the fn below)
    let record = 
        { ID = "550"
        emailAdd = "muse@rocklegend.com"
        Name = "Muse"
        genre = "Alternative"
        sales = 1230000
        salary = 210000
        longText = "An approach Many system A process A model An description Few system An model Our description The approach Few system Your approach Your method This description Your model Few framework This framework This method Your solution This process Your process"
        percent = 0.5151943856
        date = "2025-09-03"
        time = "09:32"
        dateTime = System.DateTime(2025, 1, 18, 0, 37, 0)
        booln = false
        MultiSel = ["IT"; "Marketing"]
        phone = "+1-407-815-1012"
        url = "https://forecast.weather.gov"
        rating = 1
        duration = "04:36:20" }

    let toCsv (r: 'a) =
        let fields = 
            [| r.ID; r.emailAdd; r.Name; r.genre; string r.sales; string r.salary; 
            $"\"{r.longText.Replace("\"", "\"\"")}\""; // Escape quotes in longText
            string r.percent; r.date; r.time; r.dateTime.ToString("yyyy-MM-dd HH:mm:ss"); 
            string r.booln; String.concat ";" r.MultiSel; r.phone; r.url; 
            string r.rating; r.duration |]
        String.Join(",", fields)

    let csv = toCsv record
    printfn "%s" csv
