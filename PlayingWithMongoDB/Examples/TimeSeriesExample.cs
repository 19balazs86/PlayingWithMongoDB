using MongoDB.Driver;
using PlayingWithMongoDB.Model;
using PlayingWithMongoDB.Repository;

namespace PlayingWithMongoDB.Examples;

public static class TimeSeriesExample
{
    public static async Task Run(IMongoDatabase database)
    {
        var repository = new TimeSeriesRepository(database);

        // --> Ping
        await repository.Ping();

        // --> Ensure collection is created and seed the DB
        await repository.ensureCollectionAndSeedDB();

        // --> Latest temperatures
        List<TemperatureReport> latestTemperatures = await repository.GetLatestTemperatures();

        // --> Statistics: Monthly
        List<TemperatureStatistics> statistics = await repository.GetStatisticsMonthly(latestTemperatures[0].Timestamp.Date.AddYears(-3));

        // --> Statistics: Weekly
        statistics = await repository.GetStatisticsWeekly(latestTemperatures[0].Timestamp.Year);
    }

    private static async Task ensureCollectionAndSeedDB(this TimeSeriesRepository repository)
    {
        // await repository.DropCollection();

        bool isExists = await repository.IsCollectionExists();

        if (!isExists)
        {
            await repository.CreateCollection();

            List<TemperatureReport> temperatures = DataGenerator.ForTemperature.Generate();

            await repository.InsertMany(temperatures);
        }
    }
}
