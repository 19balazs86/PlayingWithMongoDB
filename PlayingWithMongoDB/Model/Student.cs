using System;
using System.Collections.Generic;
using System.Linq;
using PlayingWithMongoDB.Types;

namespace PlayingWithMongoDB.Model
{
  public enum Gender
  {
    Man = 0, Woman
  }

  public class Student : IIdentifiable
  {
    private static readonly Random _random     = new Random();
    private static readonly string[] _subjects = { "English", "Mathematics", "Physics", "Chemistry", "Spanish" };

    //[BsonId] // According to the convention this will be the _id.
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    //[BsonRepresentation(BsonType.String)] // Set by globally.
    public Gender Gender { get; set; }
    public IEnumerable<string> Subjects { get; set; }

    public Student()
    {
      Subjects = Enumerable.Empty<string>();
    }

    public static Student GenerateStudent(int? i = null)
    {
      return new Student
      {
        Id       = Guid.NewGuid(),
        Name     = $"Name #{i ?? _random.Next(100, 1000)}",
        Age      = _random.Next(1, 100),
        Subjects = shuffleSubjects(),
        Gender = (Gender)_random.Next(0, 2)
      };
    }

    public static IEnumerable<Student> GenerateStudents(ushort count)
      => Enumerable.Range(1, count).Select(i => GenerateStudent(i));

    private static IEnumerable<string> shuffleSubjects()
    {
      List<string> list = new List<string>(_subjects);

      int n = list.Count;

      while (n > 1)
      {
        n--;

        int i = _random.Next(n + 1);

        string value = list[i];

        list[i] = list[n];
        list[n] = value;
      }

      return list.Take(_random.Next(_subjects.Length));
    }
  }
}
