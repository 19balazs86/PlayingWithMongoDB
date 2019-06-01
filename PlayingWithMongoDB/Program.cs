using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using PlayingWithMongoDB.Model;
using PlayingWithMongoDB.Mongo;
using PlayingWithMongoDB.Repository;
using PlayingWithMongoDB.Types;

namespace PlayingWithMongoDB
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      try
      {
        const string collectionName = "TestCollection";
        string connectionString     = Environment.GetEnvironmentVariable("CUSTOMCONNSTR_MongoDB");

        ConventionRegistry.Register("Conventions", new MongoDbConventions(), _ => true);

        MongoClient mongoClient = new MongoClient(connectionString);
        IMongoDatabase database = mongoClient.GetDatabase("TestDB");

        // Put a break point here and F10.
        // --> Drop: Collection.
        database.DropCollection(collectionName);

        IStudentRepository repository = new StudentRepository(database, collectionName);

        // --> Seed.
        await repository.InsertAsync(Student.GenerateStudents(100));

        Student student = Student.GenerateStudent();

        ReplaceOneResult updateOrInsertResult = await repository.UpdateOrInsertAsync(student, true);
        // updateOrInsertResult.UpsertedId.AsString // It has a value, in case of insert.

        student = await repository.GetAsync(student.Id);

        DeleteResult deleteResult = await repository.DeleteAsync(student.Id);

        bool exists = await repository.ExistsAsync(s => s.Id == student.Id);

        IEnumerable<Student> physicists = await repository.FindAsync(s => s.Subjects.Contains("Physics"));

        // ModifiedCount has to be 0.
        UpdateResult updateResult = await repository.AddSubjectAsync(physicists.First().Id, "Physics");

        updateResult = await repository.RemoveSubjectAsync(physicists.First().Id, "Physics");

        long count = await repository.CountAsync(s => s.Subjects.Contains("Physics"));

        // --> Paged query.
        PageQuery<Student> pageQuery = PageQuery<Student>
          .Create(page: 1, pageSize: 20)
          .Filter(s => s.Age >= 18 && s.Age <= 65)
          .Sort(sb => sb.Ascending(s => s.Age).Ascending(s => s.Name));
        
        PageResult<Student> studentPageResult = null;

        do
        {
          studentPageResult = await repository.BrowseAsync(pageQuery);
          // if (studentPageResult.IsNotEmpty) studentPageResult.Items
          pageQuery.Page++;
        } while (studentPageResult.HasNextPage);

        // --> Paged query with projection.
        PageQuery<Student, StudentDto> pageQueryProjection = PageQuery<Student, StudentDto>
          .Create(page: 1, pageSize: 20)
          .Filter(s => s.Age >= 18 && s.Age <= 65)
          .Sort(sb => sb.Descending("$natural")) // https://docs.mongodb.com/manual/reference/glossary/#term-natural-order
          .Project(s => new StudentDto { Id = s.Id, Name = s.Name, SubjectCount = s.Subjects.Count() });

        PageResult<StudentDto> studentDtoPageResult = null;

        do
        {
          studentDtoPageResult = await repository.BrowseAsync(pageQueryProjection);
          pageQueryProjection.Page++;
        } while (studentDtoPageResult.HasNextPage);

        deleteResult = await repository.DeleteAsync(s => s.Age < 5);
      }
      catch (Exception ex)
      {
        // MongoException
        // Do something...
        Console.WriteLine(ex.Message);
      }
    }
  }
}
