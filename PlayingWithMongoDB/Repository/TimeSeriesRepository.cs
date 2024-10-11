using MongoDB.Bson;
using MongoDB.Driver;
using PlayingWithMongoDB.Model;

namespace PlayingWithMongoDB.Repository;

public sealed class TimeSeriesRepository(IMongoDatabase _database)
{
    private const string _collectionName = "Temperatures";

    private IMongoCollection<TemperatureReport> _collection => _database.GetCollection<TemperatureReport>(_collectionName);

    public async Task<List<TemperatureStatistics>> GetStatisticsMonthly(DateTime fromDate)
    {
        var aggregateFluent = _collection
            .Aggregate()
            .Match(tr => tr.Timestamp >= fromDate)
            .Group(tr => new { tr.DeviceId, tr.Timestamp.Year, tr.Timestamp.Month }, g => new TemperatureStatistics(
                g.Key.DeviceId, g.Key.Year, g.Key.Month, g.Min(x => x.Temperature), g.Max(x => x.Temperature), g.Average(x => x.Temperature)));

        return await aggregateFluent.ToListAsync();
    }

    public async Task<List<TemperatureStatistics>> GetStatisticsWeekly(int year)
    {
        var fromDate = new DateTime(year, 1, 1);

        var pipeline = new[]
        {
            new BsonDocument("$match", new BsonDocument("Timestamp", new BsonDocument("$gte", fromDate))),
            new BsonDocument("$group", new BsonDocument
            {
                { "_id", new BsonDocument
                    {
                        { "DeviceId",   "$DeviceId" },
                        { "Year",       new BsonDocument("$year",    "$Timestamp")},
                        { "WeekOfYear", new BsonDocument("$isoWeek", "$Timestamp")}
                    }
                },
                { "Min", new BsonDocument("$min", "$Temperature") },
                { "Max", new BsonDocument("$max", "$Temperature") },
                { "Avg", new BsonDocument("$avg", "$Temperature") }
            }),
            new BsonDocument("$sort",    new BsonDocument("_id.WeekOfYear", -1)),
            new BsonDocument("$project", new BsonDocument
            {
                { "DeviceId",   "$_id.DeviceId" },
                { "Year",       "$_id.Year" },
                { "Month_Week", "$_id.WeekOfYear" },
                { "Min",        1 },
                { "Max",        1 },
                { "Avg",        1 }
            })
        };

        // There is a BsonSerializer.Deserialize version of this query, scroll down

        return await _collection.Aggregate<TemperatureStatistics>(pipeline).ToListAsync();
    }

    public async Task<List<TemperatureReport>> GetLatestTemperatures()
    {
        var sortDefinition = Builders<TemperatureReport>.Sort.Descending(tr => tr.Timestamp);

        var aggregateFluent = _collection
            .Aggregate()
            .Sort(sortDefinition)
            .Group(tr => tr.DeviceId, group => group.First());

        // There is a BSON document version of this query, scroll down

        return await aggregateFluent.ToListAsync();
    }

    public async Task InsertMany(IEnumerable<TemperatureReport> temperatures)
    {
        foreach (var deviceGroup in temperatures.GroupBy(x => x.DeviceId))
        {
            foreach(var timeGroup in deviceGroup.GroupBy(x => new { x.Timestamp.Year, x.Timestamp.Month }))

            await _collection.InsertManyAsync(timeGroup.ToList());
        }
    }

    public async Task CreateCollection()
    {
        const int seconds = 2_592_000; // Seconds in a 30-days month and align buckets to month start

        var tsOptions = new TimeSeriesOptions(TemperatureReport.TimeField, bucketMaxSpanSeconds: seconds, bucketRoundingSeconds: seconds);

        var options = new CreateCollectionOptions { TimeSeriesOptions = tsOptions };

        await _database.CreateCollectionAsync(_collectionName, options);

        // Create index
        IndexKeysDefinition<TemperatureReport>[] indexDefinitions =
        [
            Builders<TemperatureReport>.IndexKeys.Descending(x => x.Timestamp),
            Builders<TemperatureReport>.IndexKeys.Ascending(x  => x.DeviceId),
            Builders<TemperatureReport>.IndexKeys.Descending(x => x.Timestamp).Ascending(x => x.DeviceId)
        ];

        var indexes = indexDefinitions.Select(x => new CreateIndexModel<TemperatureReport>(x));

        await _collection.Indexes.CreateManyAsync(indexes);
    }

    public async Task DropCollection()
    {
        await _database.DropCollectionAsync(_collectionName);
    }

    public async Task<bool> IsCollectionExists()
    {
        IAsyncCursor<string> asyncCursor = await _database.ListCollectionNamesAsync();

        while (await asyncCursor.MoveNextAsync())
        {
            foreach (string collectionName in asyncCursor.Current)
            {
                if (_collectionName.Equals(collectionName))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public async Task<bool> Ping()
    {
        var command = new BsonDocument("ping", 1);

        BsonDocument result = await _database.RunCommandAsync<BsonDocument>(command);

        Console.WriteLine("Ping {0}", result);

        return result.Contains("ok") && result["ok"] == 1;
    }

    //public async Task<List<TemperatureReport>> GetLatestTemperaturesBson()
    //{
    //    var pipeline = new[]
    //    {
    //        new BsonDocument("$sort",  new BsonDocument("Timestamp", -1)),
    //        new BsonDocument("$group", new BsonDocument
    //        {
    //            { "_id", "$DeviceId" },
    //            { "latestRecord", new BsonDocument("$first", "$$ROOT") }
    //        }),
    //        new BsonDocument("$replaceRoot", new BsonDocument("newRoot", "$latestRecord"))
    //        //new BsonDocument("$project", new BsonDocument
    //        //{
    //        //    { "_id",         "$latestRecord._id" },
    //        //    { "DeviceId",    "$latestRecord.DeviceId" },
    //        //    { "Timestamp",   "$latestRecord.Timestamp" },
    //        //    { "Temperature", "$latestRecord.Temperature" }
    //        //})
    //    };

    //    return await _collection.Aggregate<TemperatureReport>(pipeline).ToListAsync();
    //}

    //public async Task<List<TemperatureStatistics>> GetStatisticsWeeklyBson(int year)
    //{
    //    var fromDate = new DateTime(year, 1, 1);

    //    string jsonPipeline =
    //        $$"""
    //        [
    //          {
    //            "$match": {
    //              "Timestamp": {
    //                "$gte": ISODate("{{fromDate.ToString("o")}}")
    //              }
    //            }
    //          },
    //          {
    //            "$group": {
    //              "_id": {
    //                "DeviceId": "$DeviceId",
    //                "Year": {
    //                  "$year": "$Timestamp"
    //                },
    //                "WeekOfYear": {
    //                  "$isoWeek": "$Timestamp"
    //                }
    //              },
    //              "Min": {
    //                "$min": "$Temperature"
    //              },
    //              "Max": {
    //                "$max": "$Temperature"
    //              },
    //              "Avg": {
    //                "$avg": "$Temperature"
    //              }
    //            }
    //          },
    //          {
    //            "$sort": {
    //              "_id.WeekOfYear": -1
    //            }
    //          },
    //          {
    //            "$project": {
    //              "DeviceId": "$_id.DeviceId",
    //              "Year": "$_id.Year",
    //              "Month_Week": "$_id.WeekOfYear",
    //              "Min": 1,
    //              "Max": 1,
    //              "Avg": 1
    //            }
    //          }
    //        ]
    //        """;

    //    var pipeline = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<List<BsonDocument>>(jsonPipeline);

    //    return await _collection.Aggregate<TemperatureStatistics>(pipeline).ToListAsync();
    //}
}
