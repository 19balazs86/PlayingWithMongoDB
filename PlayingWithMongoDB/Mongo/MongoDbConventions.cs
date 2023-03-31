using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace PlayingWithMongoDB.Mongo;

public sealed class MongoDbConventions : IConventionPack
{
    public MongoDbConventions()
    {
        // You can use these lines, where you initialize the MongoDB.
        //BsonSerializer.RegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));
        //BsonSerializer.RegisterSerializer(typeof(decimal?), new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));

        BsonSerializer.RegisterSerializer(typeof(Guid), new GuidSerializer(BsonType.String));
    }

    public IEnumerable<IConvention> Conventions => new List<IConvention>
    {
        new IgnoreExtraElementsConvention(true),
        new EnumRepresentationConvention(BsonType.String),
        //new CamelCaseElementNameConvention()
    };
}
