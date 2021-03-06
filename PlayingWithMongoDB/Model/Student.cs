﻿using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using PlayingWithMongoDB.Types;

namespace PlayingWithMongoDB.Model
{
  public enum Gender
  {
    Man = 0, Woman
  }

  public class Student : IIdentifiable
  {
    private static readonly DateTime _now      = DateTime.Now;
    private static readonly DateTime _toDate   = _now.AddYears(-10);
    private static readonly DateTime _fromDate = _toDate.AddYears(-90);
    private static readonly Random _random     = new Random();
    private static readonly string[] _subjects = { "English", "Mathematics", "Physics", "Chemistry", "Spanish" };

    //[BsonId] // According to the convention this will be the _id.
    public Guid Id { get; set; }
    public string Name { get; set; }

    [BsonDateTimeOptions(DateOnly = true)]
    public DateTime DateOfBirth { get; set; }
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
      var dateOfBirth = getRandomDate(_fromDate, _toDate);

      return new Student
      {
        Id          = Guid.NewGuid(),
        Name        = $"Name #{i ?? _random.Next(100, 1000)}",
        DateOfBirth = dateOfBirth.Date, // Has to be just .Date, otherwise error: "TimeOfDay component is not zero".
        Age         = calculateAge(dateOfBirth),
        Subjects    = shuffleSubjects(),
        Gender      = (Gender)_random.Next(0, 2)
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

    private static DateTime getRandomDate(DateTime from, DateTime to)
    {
      TimeSpan range = new TimeSpan(to.Ticks - from.Ticks);
      return from + new TimeSpan((long)(range.Ticks * _random.NextDouble()));
    }

    private static int calculateAge(DateTime dob)
    {
      int age = _now.Year - dob.Year;

      if (dob.DayOfYear < _now.DayOfYear) age -= 1;

      return age;
    }
  }
}
