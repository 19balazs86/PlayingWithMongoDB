using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using PlayingWithMongoDB.Examples;
using PlayingWithMongoDB.Mongo;

namespace PlayingWithMongoDB;

public static class Program
{
    private static readonly string _connectionString = Environment.GetEnvironmentVariable("CUSTOMCONNSTR_MongoDB");
    // private const string _connectionString = "mongodb://admin:admin@localhost:27017";

    public static async Task Main(string[] args)
    {
        ConventionRegistry.Register("Conventions", new MongoDbConventions(), _ => true);

        var mongoClient = new MongoClient(_connectionString);

        // Database is created if NOT exist
        IMongoDatabase database = mongoClient.GetDatabase("TestDB");

        await StudentExample.Run(database);
    }
}
