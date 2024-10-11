namespace PlayingWithMongoDB.Model;

public sealed class TemperatureReport
{
    public const string TimeField = nameof(Timestamp);

    // [BsonId] // According to the convention this will be the _id
    public Guid Id { get; init; }
    public string DeviceId { get; init; }
    public DateTime Timestamp { get; init; }
    public double Temperature { get; init; }
}

public sealed record TemperatureStatistics(string DeviceId, int Year, int Month_Week, double Min, double Max, double Avg);