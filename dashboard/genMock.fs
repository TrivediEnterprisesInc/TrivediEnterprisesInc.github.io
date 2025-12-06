(*  Brij (TM)
    Copyright (c) M. P. Trivedi 2016-2025.  All rights reserved. 
    TrivediBldHdr->
    |Trivedi SrcCtrlHdr File.  Copyright (c) 2015-2025 M. P. Trivedi.  All rights reserved.|637901455|test.fs|none|- - - - - - ->* louisa * St.Francis * PIUTE * princeedward * SALTLAKE * Jefferson * craven * <* Obion * CERROGORDO * fayette * LEFLORE * Suffolk * elmore * SweetGrass * <* BLAND * pittsburg * VANDERBURGH * Pittsburg * california * Coffee * PETROLEUM * <* westcarrollparish * ANTRIM * Titus * fortbend * Audrain * STANLEY * losangeles * <* LINCOLNPARISH * PointeCoupeeParish * lee * Nuckolls * CONECUH * hotspring * EDWARDS 

    fsc e:\genMock.fs --platform:x64 --standalone --target:exe --out:dashbd.exe -r:lib\Trivedi.Core.dll -r:lib\Trivedi.UI.dll -I:C:\Windows\Microsoft.NET\Framework\v4.0.30319

    Last updated: Sat Dec 06 2025
    Contains modules:      Pipelines
*)
namespace Trivedi

#nowarn "20" "25" "58" "66" "67" "64" "760" "1182" "1558"

[<AutoOpen>]
module Dashboard =
    open MongoDB.Driver
    open MongoDB.Bson
    open System
    open System.Linq
    
    printfn "Dashboard init..."

    // Define types (simplified from your structs)
    type DailyOpCounts = Map<string, int> // e.g., "Creates" -> count, but here per-day string keys for simplicity
    type TblOps = Map<string, Map<string, DailyOpCounts>> // TblID -> OpType -> DayCounts
    type SessionSummary = { totalSessions: int; avgDuration: int; peakUsers: int }
    type ErrorSummary = { totalErrors: int; topError: string; errorRate: double }
    type ActivityLog = {
        orgId: string
        date: string // YYYY-MM
        tblOps: BsonDocument // Sparse ops as BSON for flexibility
        sessionSummary: SessionSummary
        errorSummary: ErrorSummary
        lastUpdated: DateTime
    }

    type SessionLog = {
        orgId: string
        sessionId: string
        userId: string
        startTime: DateTime
        endTime: DateTime
        sessionDuration: int
    }

    type ErrorLog = {
        orgId: string
        timestamp: DateTime
        userId: string
        errorType: string
        message: string
        stackTrace: string option
    }

    // MongoDB connection
    let client = new MongoClient("mongodb://localhost:27017")
    let db = client.GetDatabase("BrijDB")
    let activityColl = db.GetCollection<ActivityLog>("ActivityLog")
    let sessionColl = db.GetCollection<SessionLog>("SessionLog")
    let errorColl = db.GetCollection<ErrorLog>("ErrorLog")
    
    // Helper: Random with growth
    let random = new Random(42)
    let growValue (baseVal: int) monthGrowth : int = 
        let variance = 1.0 + (random.NextDouble() - 0.5) * 0.2 // ±10%
        baseVal + int (float baseVal * monthGrowth * variance)
    
    // Generate weekdays in month
    let getWeekdays (year: int) (month: int) : string list =
        let days = [1..DateTime(year, month, 1).AddMonths(1).AddDays(-1).Day]
        days |> List.filter (fun d -> 
            let dt = DateTime(year, month, d)
            dt.DayOfWeek <> DayOfWeek.Saturday && dt.DayOfWeek <> DayOfWeek.Sunday
        ) |> List.map (sprintf "%02i")
    
    // Generate one monthly ActivityLog doc
    let generateActivityLog (year: int) (month: int) (baseCRUDQU: int array) : ActivityLog =
        let dateStr = sprintf "%d-%02i" year month
        let growth = 0.04
        let currentCRUDQU = baseCRUDQU |> Array.map (growValue growth) // Cumulative growth from base
        let weekdays = getWeekdays year month
        let opsPerDay = currentCRUDQU |> Array.map (fun v -> v / weekdays.Length) // Distribute evenly
        
        let buildDaily (opType: string) (dayCounts: string -> int) : BsonDocument =
            weekdays 
            |> List.fold (fun doc day -> 
                let count = dayCounts day
                doc.Add(day, BsonInt32(count)) |> ignore
                doc
            ) (new BsonDocument()) :> BsonDocument
        
        let tblIds = ["Tbl001"; "Tbl002"; "Tbl003"] // Customers, Products, Orders
        let userIds = [|"bill@msft.com"; "larry@oracle.com"; "peter@palantir.com"; "paul@hn.com"; "elon@x.com"; "sama@openai.com"; "sergei@google.com"; "steve@apple.com"|]
        let sizes = [|1.5; 1.0; 0.5|] // Relative sizes: Customers largest
        let tblOps = 
            tblIds 
            |> List.mapi (fun i tblId -> 
                let scaled = currentCRUDQU |> Array.map (fun v -> int (float v * sizes.[i]))
                let opTypes = ["Creates"; "Reads"; "Updates"; "Deletes"; "QuickUpdates"]
                let dailyOps = 
                    opTypes 
                    |> List.map (fun ot -> 
                    ot, (fun day -> opsPerDay.[List.findIndex ((=) ot) opTypes] |> (*) (weekdays |> List.findIndex ((=) day) + 1 |> float)) // Slight day variance
                    ) |> Map.ofList
                let tblDoc = new BsonDocument()
                dailyOps |> Map.iter (fun ot dayFn -> 
                    let dailyDoc = buildDaily ot dayFn
                    tblDoc.Add(ot, dailyDoc) |> ignore
                )
                tblId, tblDoc
            ) |> Map.ofList
        
        let totalOps = (currentCRUDQU |> Array.sum) * weekdays.Length |> float
        let sessionSummary = { 
            totalSessions = int (totalOps * 0.1 * (0.8 + random.NextDouble() * 0.4)); // 10-20% of ops
            avgDuration = 900 + random.Next(-300, 300); // 10-20 min
            peakUsers = 5 + random.Next(10)
        }
        let errorSummary = { 
            totalErrors = int (totalOps * 0.01 * (0.5 + random.NextDouble())); // 0.5-1.5%
            topError = ["Bad Request"; "Unauthorized"; "Request Timeout";
                    "Too Many Requests";"Internal Server Error";"Not Implemented";"Gateway Timeout"] |> List.item (random.Next(0,6))
            errorRate = totalOps * 0.01
        }

        { orgId = "AcmeCorp"; date = dateStr; tblOps = BsonDocument(tblOps |> Map.mapValues (fun v -> v :> BsonDocument)); 
          sessionSummary = sessionSummary; errorSummary = errorSummary; lastUpdated = DateTime.UtcNow }
    
    // Generate and insert ActivityLog (35 months)
    [ for y in 2023..2025 do
        for m in 1..12 do
            if y = 2025 && m > 12 then () else yield generateActivityLog y m [|500;700;300;20;40|] ]
    |> List.iter (fun doc -> activityColl.InsertOne(doc) |> ignore)
    
    // Generate sample SessionLog (~10k docs)
    let generateSessions() =
        [ for i in 0..9999 do
            let start = DateTime(2023,1,1).AddDays(random.Next(1095)).AddHours(random.Next(8,18)) // 3 years, business hrs
            if start.DayOfWeek in [DayOfWeek.Saturday; DayOfWeek.Sunday] then start.AddDays(1) |> ignore // Skip weekends
            let duration = random.Next(1800, 7200) // 30min-2hrs
            let endT = start.AddSeconds(float duration)
            let numActions = random.Next(3,10)
            { orgId = "AcmeCorp"; sessionId = $"sess_{Guid.NewGuid()}"; userId = userIds.[random.Next(0,7)];
              startTime = start; endTime = endT; sessionDuration = duration } ]
    |> List.iter (fun s -> sessionColl.InsertOne(s) |> ignore)
    
    generateSessions()

    // Generate sample ErrorLog (~10k docs, 1% of sessions)
    let generateErrors () =
        let errorTypes = ["HTTP 400"; "HTTP 401"; "HTTP 408"; 
                            "HTTP 429"; "HTTP 500"; "HTTP 501"; "HTTP 504"]
        let messages = ["Bad Request"; "Unauthorized"; "Request Timeout";
                        "Too Many Requests";"Internal Server Error";"Not Implemented";"Gateway Timeout"]
        [ for i in 0..999 do // Fewer errors
            let ts = DateTime(2023,1,1).AddDays(random.Next(1095)).AddHours(random.Next(24))
            if ts.DayOfWeek in [DayOfWeek.Saturday; DayOfWeek.Sunday] then () else // No weekend errors
            { orgId = "AcmeCorp"; timestamp = ts; userId = userIds.[random.Next(0,7)];
              errorType = errorTypes.[random.Next(0,6)]; message = messages.[random.Next(0,6)];
              stackTrace = Some("Fs stack trace...") } ]
    |> List.iter (fun e -> errorColl.InsertOne(e) |> ignore)
    
    generateErrors()

    [<AutoOpen>]
    module Pipelines = 
    
        printfn "Pipelines init..."
        
        // KPI: Total Monthly CRUD/QU for all tables (sum over ops)
        let kpiTotalOpsPipeline = [
            { $match = {| orgId = "AcmeCorp"; date = { $gte = "2024-01" } |} :> BsonDocument } // Last 12 months
            { $project = {|
                date = 1
                totalCreates = { $sum = "$tblOps.Tbl001.Creates" } // Example; use $objectToArray for dynamic tbl sum
                // Actually, for all: use $unwind or $reduce - see full below
              |} :> BsonDocument
            ]
        // Better full pipeline for totals:
        let fullTotalOps = [
            { $match = {| orgId = "AcmeCorp" |} }
            { $project = {|
                date = "$date"
                flattenedOps = { $objectToArray = "$tblOps" } // Flatten tbls
              |} }
            { $unwind = "$flattenedOps" }
            { $project = {|
                date = 1
                opType = { $objectToArray = "$flattenedOps.v" } // Flatten ops
              |} }
            { $unwind = "$opType" }
            { $group = {|
                _id = { date = "$date"; op = "$opType.k" }
                total = { $sum = { $size = "$opType.v" } } // Approx; adjust for daily sums
              |} }
            { $group = {|
                _id = "$_id.date"
                opsByType = { $push = { type = "$_id.op"; count = "$total" } }
              |} }
        ]
        activityColl.Aggregate<BsonDocument>(fullTotalOps).ToList() // For line chart data
        
        // For Growth Chart: % Change in Total Ops MoM
        let growthPipeline = [
            // Similar match/project as above, then:
            { $sort = { date = 1 } }
            { $group = { _id = null; ops: { $push = { date = "$date"; totalOps = { $sum = "$allOps" } } } } }
            { $project = {|
                growth = { $map = { input = { $slice = ["$ops", 1, { $size = "$ops" }] },
                    as = "curr",
                    in = { $divide = [{ $subtract = [ "$$curr.totalOps", { $arrayElemAt = ["$ops.totalOps", { $subtract = [{ $indexOfArray = ["$ops.date", "$$curr.date"] }, 1] }] } ] },
                        { $arrayElemAt = ["$ops.totalOps", { $subtract = [{ $indexOfArray = ["$ops.date", "$$curr.date"] }, 1] }] } ] } * 100
                  |} }
                |} }
        ]
        
        // KPI: Avg Duration, Total Sessions, Peak Users (rollup if avail, else from raw)
        let sessionKpiPipeline = [
            { $match = {| orgId = "AcmeCorp"; startTime = { $gte = DateTime(2024,1,1) } |} }
            { $group = {|
                _id = null
                totalSessions = { $sum = 1 }
                avgDuration = { $avg = "$sessionDuration" }
                peakUsers = { $sum = 1 } // Or group by day for true peak
              |} }
            { $project = {|
                totalSessions = "$totalSessions"
                avgDuration = { $round = [{ $multiply = ["$avgDuration", 0.01667], 0 }] } // To minutes
                peakUsers = { $max = "$dailyUsers" } // If pre-aggregated
              |} }
        ]
        let kpis = sessionColl.Aggregate<BsonDocument>(sessionKpiPipeline).FirstOrDefault()
        
        // For Bar Chart: Sessions by User
        let sessionsByUser = [
            { $match = {| orgId = "AcmeCorp" |} }
            { $group = { _id = "$userId"; count = { $sum = 1 }; totalDuration = { $sum = "$sessionDuration" } } }
            { $sort = { count = -1 } }
        ]
        sessionColl.Aggregate<BsonDocument>(sessionsByUser).ToList()
        
        // KPI: Total Errors, Error Rate, Top Error
        let errorKpiPipeline = [
            { $match = {| orgId = "AcmeCorp"; timestamp = { $gte = DateTime(2024,1,1) } |} }
            { $group = {|
                _id = null
                totalErrors = { $sum = 1 }
                byType = { $push = { type = "$errorType"; count = 1 } }
              |} }
            { $project = {|
                totalErrors = "$totalErrors"
                topError = { $arrayElemAt = [{ $sortArray = { input = "$byType", sortBy = { count = -1 } } }, 0] }.type
                errorRate = { $divide = [ "$totalErrors", 10000 ] } // Rel to total ops approx
              |} }
        ]
        let errorKpis = errorColl.Aggregate<BsonDocument>(errorKpiPipeline).FirstOrDefault()
        
        // For Pie Chart: Errors by Type
        let errorsByType = [
            { $match = {| orgId = "AcmeCorp" |} }
            { $group = { _id = "$errorType"; count = { $sum = 1 } } }
            { $sort = { count = -1 } }
        ]
        errorColl.Aggregate<BsonDocument>(errorsByType).ToList()
        
    printfn "Pipelines eom..."

    [<EntryPoint>]
    [<STAThread>]
    let main ag =
        db "Dashboard main():1"
        0
