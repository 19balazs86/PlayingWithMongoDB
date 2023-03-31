namespace PlayingWithMongoDB.Model;

public sealed class StudentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int SubjectCount { get; set; }
}
