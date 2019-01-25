using System;

namespace PlayingWithMongoDB.Model
{
  public class StudentDao
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int SubjectCount { get; set; }
  }
}
