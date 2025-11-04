(* Grok / Nov 4th '25

Snippet which adds support for GrpAggregation fns beyond count.  AirTable uses the following:

(Histogram)
Sum Average Median Min Max Range StandardDeviation Empty Filled Unique PercentEmpty PercentFilled PercentUnique

I requested the LLM to suggest further fns supported by Mongo; there are quite a few (we may have to research Date/other fldTys tho...
*)


open MongoDB.Bson
open MongoDB.Driver
open System
open System.Linq

[<EntryPoint>]
let main argv =
    // Connect to MongoDB
    let client = MongoClient("mongodb://localhost:27017")
    let database = client.GetDatabase("yourDatabaseName")
    let collection: IMongoCollection<BsonDocument> = database.GetCollection<BsonDocument>("yourCollectionName")

    // LINQ query to group by "status" and count occurrences
    let result = 
        collection.AsQueryable()
        |> Seq.groupBy (fun doc -> doc.["status"])
        |> Seq.map (fun (key, group) -> 
            BsonDocument(
                { "_id", key },
                { "count", BsonInt32(Seq.length group) }
            ))
        |> Seq.toList

    // Print the results
    for doc in result do
        printfn "%s" (doc.ToJson())

    0 // Return an integer exit code

(*
Explanation:
- AsQueryable(): Converts the collection to a LINQ-compatible queryable object.
- groupBy: Groups the documents by the specified field (e.g., "status").
- map: Projects the grouped results into a new BsonDocument containing the group key (_id) and the count of documents in each group.
- toList: Executes the query and retrieves the results as a list.

This is a direct translation to F#. Note that The BsonDocument constructor uses a list of BsonElement (created via { name, value } tuples).
*)

(*
(2) Extended F# code to include aggregations on "salary" for each group.
Assumptions:
- "salary" is a numeric field (treated as double via AsDouble; adjust if it's int or another type).
- Computations for Sum, Average, Min, Max use built-in LINQ methods (these translate to server-side MongoDB aggregations where possible).
- Median, Range, StandardDeviation, Empty, Filled, Unique, PercentEmpty, PercentFilled, PercentUnique are computed client-side after grouping, as they aren't directly supported in MongoDB's LINQ provider. For large datasets, consider switching to the aggregation pipeline for server-side execution (see part 3 for ideas).
- "Empty" means the field is missing or null. "Filled" is total count minus empty. "Unique" is the count of distinct non-null values.
- Percentages are calculated as (relevant / total count) * 100, using doubles for precision.
- If groups are empty, some values (e.g., averages) will be defaults like 0 or NaN; add handling if needed.
*)

let extendedResult = 
    collection.AsQueryable()
    |> Seq.groupBy (fun doc -> doc.["status"])
    |> Seq.map (fun (key, group) -> 
        let count = Seq.length group
        let salaries = group |> Seq.map (fun d -> if d.Contains("salary") && not d.["salary"].IsBsonNull then Some d.["salary"].AsDouble else None)
        let filledSalaries = salaries |> Seq.choose id |> Seq.toArray // Non-null values as array for further ops
        let filledCount = filledSalaries.Length
        let emptyCount = count - filledCount
        let uniqueCount = if filledCount > 0 then (filledSalaries |> Array.distinct |> Array.length) else 0
        let sum = if filledCount > 0 then Array.sum filledSalaries else 0.0
        let avg = if filledCount > 0 then sum / float filledCount else 0.0
        let min = if filledCount > 0 then Array.min filledSalaries else 0.0
        let max = if filledCount > 0 then Array.max filledSalaries else 0.0
        let range = max - min
        // Median: Sort and find middle (client-side)
        let median = 
            if filledCount = 0 then 0.0
            elif filledCount % 2 = 1 then Array.sort filledSalaries |> fun arr -> arr.[filledCount / 2]
            else Array.sort filledSalaries |> fun arr -> (arr.[filledCount / 2 - 1] + arr.[filledCount / 2]) / 2.0
        // StandardDeviation: Manual variance calculation (client-side, population stddev)
        let stdDev = 
            if filledCount <= 1 then 0.0
            else 
                let variance = filledSalaries |> Array.map (fun x -> (x - avg) ** 2.0) |> Array.average
                sqrt variance
        let percentEmpty = (float emptyCount / float count) * 100.0
        let percentFilled = (float filledCount / float count) * 100.0
        let percentUnique = if filledCount > 0 then (float uniqueCount / float filledCount) * 100.0 else 0.0

        BsonDocument(
            { "_id", key },
            { "count", BsonInt32(count) },
            { "sum", BsonDouble(sum) },
            { "average", BsonDouble(avg) },
            { "median", BsonDouble(median) },
            { "min", BsonDouble(min) },
            { "max", BsonDouble(max) },
            { "range", BsonDouble(range) },
            { "standardDeviation", BsonDouble(stdDev) },
            { "empty", BsonInt32(emptyCount) },
            { "filled", BsonInt32(filledCount) },
            { "unique", BsonInt32(uniqueCount) },
            { "percentEmpty", BsonDouble(percentEmpty) },
            { "percentFilled", BsonDouble(percentFilled) },
            { "percentUnique", BsonDouble(percentUnique) }
        ))
    |> Seq.toList

// Print the extended results
for doc in extendedResult do
    printfn "%s" (doc.ToJson())

(*
This extends the Select projection to compute all requested metrics on the group. Basic aggregations (sum, avg, min, max, count) can run server-side via the LINQ provider. The rest (median, stddev, range, uniques, empties, percentages) are computed client-side after materializing the groups. For purely server-side execution, use MongoDB's aggregation pipeline (example in part 3).
*)

(3) Besides the aggregations you mentioned (which map to $sum, $avg, $median, $min, $max, and custom for range/stddev/empties/uniques), MongoDB's aggregation framework supports additional accumulator operators for use in $group stages on numeric fields. These are server-side and efficient for large data. Here's a list of key numeric-relevant accumulators (focusing on those beyond basics; see MongoDB docs for full details, as some require version 5.0+). I've excluded pure array operators like $push unless they aid numerics.

For each, I've provided a snippet in the context of an F# aggregation pipeline (using MongoDB.Driver builders), grouping by "status" and applying the operator to "salary". This is an alternative to LINQ for more advanced ops. The pipeline is an array of stages, executed via collection.Aggregate().ToList().

- **$stdDevPop**: Computes the population standard deviation.
  Snippet: 
  let pipeline = 
      [ 
          Builders<BsonDocument>.Aggregation.Group(
              (fun doc -> doc.["status"]), 
              (fun g -> 
                  g.Field("stdDevPopSalary", BsonDocument { "$stdDevPop", "$salary" } ))
          ) 
      ]
  let results = collection.Aggregate(pipeline).ToList()

- **$stdDevSamp**: Computes the sample standard deviation.
  Snippet: 
  let pipeline = 
      [ 
          Builders<BsonDocument>.Aggregation.Group(
              (fun doc -> doc.["status"]), 
              (fun g -> 
                  g.Field("stdDevSampSalary", BsonDocument { "$stdDevSamp", "$salary" } ))
          ) 
      ]
  let results = collection.Aggregate(pipeline).ToList()

- **$first**: Returns the first value in the group (useful for sorted groups).
  Snippet: 
  let pipeline = 
      [ 
          Builders<BsonDocument>.Aggregation.Group(
              (fun doc -> doc.["status"]), 
              (fun g -> 
                  g.Field("firstSalary", BsonDocument { "$first", "$salary" } ))
          ) 
      ]
  let results = collection.Aggregate(pipeline).ToList()

- **$last**: Returns the last value in the group (useful for sorted groups).
  Snippet: 
  let pipeline = 
      [ 
          Builders<BsonDocument>.Aggregation.Group(
              (fun doc -> doc.["status"]), 
              (fun g -> 
                  g.Field("lastSalary", BsonDocument { "$last", "$salary" } ))
          ) 
      ]
  let results = collection.Aggregate(pipeline).ToList()

- **$firstN**: Returns the first N values as an array (MongoDB 5.2+).
  Snippet: 
  let pipeline = 
      [ 
          Builders<BsonDocument>.Aggregation.Group(
              (fun doc -> doc.["status"]), 
              (fun g -> 
                  g.Field("first3Salaries", BsonDocument { "$firstN", BsonDocument { "input", "$salary"; "n", 3 } } ))
          ) 
      ]
  let results = collection.Aggregate(pipeline).ToList()

- **$lastN**: Returns the last N values as an array (MongoDB 5.2+).
  Snippet: 
  let pipeline = 
      [ 
          Builders<BsonDocument>.Aggregation.Group(
              (fun doc -> doc.["status"]), 
              (fun g -> 
                  g.Field("last3Salaries", BsonDocument { "$lastN", BsonDocument { "input", "$salary"; "n", 3 } } ))
          ) 
      ]
  let results = collection.Aggregate(pipeline).ToList()

- **$maxN**: Returns the top N maximum values as an array (MongoDB 5.0+).
  Snippet: 
  let pipeline = 
      [ 
          Builders<BsonDocument>.Aggregation.Group(
              (fun doc -> doc.["status"]), 
              (fun g -> 
                  g.Field("max3Salaries", BsonDocument { "$maxN", BsonDocument { "input", "$salary"; "n", 3 } } ))
          ) 
      ]
  let results = collection.Aggregate(pipeline).ToList()

- **$minN**: Returns the top N minimum values as an array (MongoDB 5.0+).
  Snippet: 
  let pipeline = 
      [ 
          Builders<BsonDocument>.Aggregation.Group(
              (fun doc -> doc.["status"]), 
              (fun g -> 
                  g.Field("min3Salaries", BsonDocument { "$minN", BsonDocument { "input", "$salary"; "n", 3 } } ))
          ) 
      ]
  let results = collection.Aggregate(pipeline).ToList()

- **$top**: Returns the top value based on a sort (MongoDB 5.0+).
  Snippet: 
  let pipeline = 
      [ 
          Builders<BsonDocument>.Aggregation.Group(
              (fun doc -> doc.["status"]), 
              (fun g -> 
                  g.Field("topSalary", BsonDocument { "$top", BsonDocument { "sortBy", BsonDocument { "salary", -1 }; "output", "$salary" } } ))
          ) 
      ]
  let results = collection.Aggregate(pipeline).ToList()

- **$topN**: Returns the top N values based on a sort (MongoDB 5.0+).
  Snippet: 
  let pipeline = 
      [ 
          Builders<BsonDocument>.Aggregation.Group(
              (fun doc -> doc.["status"]), 
              (fun g -> 
                  g.Field("top3Salaries", BsonDocument { "$topN", BsonDocument { "n", 3; "sortBy", BsonDocument { "salary", -1 }; "output", "$salary" } } ))
          ) 
      ]
  let results = collection.Aggregate(pipeline).ToList()

- **$bottom**: Returns the bottom value based on a sort (MongoDB 5.0+).
  Snippet: 
  let pipeline = 
      [ 
          Builders<BsonDocument>.Aggregation.Group(
              (fun doc -> doc.["status"]), 
              (fun g -> 
                  g.Field("bottomSalary", BsonDocument { "$bottom", BsonDocument { "sortBy", BsonDocument { "salary", 1 }; "output", "$salary" } } ))
          ) 
      ]
  let results = collection.Aggregate(pipeline).ToList()

- **$bottomN**: Returns the bottom N values based on a sort (MongoDB 5.0+).
  Snippet: 
  let pipeline = 
      [ 
          Builders<BsonDocument>.Aggregation.Group(
              (fun doc -> doc.["status"]), 
              (fun g -> 
                  g.Field("bottom3Salaries", BsonDocument { "$bottomN", BsonDocument { "n", 3; "sortBy", BsonDocument { "salary", 1 }; "output", "$salary" } } ))
          ) 
      ]
  let results = collection.Aggregate(pipeline).ToList()

- **$addToSet**: Collects unique values into an array (useful for unique count via $size).
  Snippet: 
  let pipeline = 
      [ 
          Builders<BsonDocument>.Aggregation.Group(
              (fun doc -> doc.["status"]), 
              (fun g -> 
                  g.Field("uniqueSalaries", BsonDocument { "$addToSet", "$salary" } ))
          );
          // Optional follow-up projection to get count: $project { uniqueCount: { $size: "$uniqueSalaries" } }
      ]
  let results = collection.Aggregate(pipeline).ToList()

- **$percentile**: Computes percentiles (MongoDB 7.0+; approximate or exact).
  Snippet: 
  let pipeline = 
      [ 
          Builders<BsonDocument>.Aggregation.Group(
              (fun doc -> doc.["status"]), 
              (fun g -> 
                  g.Field("p90Salary", BsonDocument { "$percentile", BsonDocument { "input", "$salary"; "p", [0.9]; "method", "approximate" } } ))
          ) 
      ]
  let results = collection.Aggregate(pipeline).ToList()

Note: For median specifically (as in your example), use $median (MongoDB 5.0+), which is a convenience for 50th percentile:
Snippet (Median("salary")): 
let pipeline = 
    [ 
        Builders<BsonDocument>.Aggregation.Group(
            (fun doc -> doc.["status"]), 
            (fun g -> 
                g.Field("medianSalary", BsonDocument { "$median", BsonDocument { "input", "$salary"; "method", "approximate" } } ))
        ) 
    ]
let results = collection.Aggregate(pipeline).ToList()

These can be combined in a single $group stage for multiple aggregations. For empties/filled, use a pre-$match or conditional $sum (e.g., { $sum: { $cond: [{ $eq: ["$salary", null] }, 1, 0] } }). Range can be { $subtract: [{ $max: "$salary" }, { $min: "$salary" }] }. The pipeline approach is more flexible and runs entirely server-side.
