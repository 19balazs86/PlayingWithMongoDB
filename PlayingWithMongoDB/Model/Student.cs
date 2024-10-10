using MongoDB.Bson.Serialization.Attributes;
using PlayingWithMongoDB.Types;

namespace PlayingWithMongoDB.Model;

public enum Gender
{
    Man = 0, Woman
}

public sealed class Student : IIdentifiable
{
    // [BsonId] // According to the convention this will be the _id
    public Guid Id { get; init; }

    public string Name { get; init; }

    [BsonDateTimeOptions(DateOnly = true)]
    public DateTime DateOfBirth { get; init; }

    public int Age { get; init; }

    // [BsonRepresentation(BsonType.String)] // Defined globally
    public Gender Gender { get; init; }

    public IEnumerable<string> Subjects { get; init; } = [];
}
