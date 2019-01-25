using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using PlayingWithMongoDB.Model;
using PlayingWithMongoDB.Mongo;
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
        string connectionString     = Environment.GetEnvironmentVariable("MongoConnString");

        ConventionRegistry.Register("Conventions", new MongoDbConventions(), _ => true);

        MongoClient mongoClient = new MongoClient(connectionString);
        IMongoDatabase database = mongoClient.GetDatabase("TestDB");
        
        // Put a break point here and F10.
        // --> Drop: Collection.
        database.DropCollection(collectionName);

        IMongoRepository<Student> repository = new MongoRepository<Student>(database, collectionName);

        // --> Seed.
        await repository.InsertAsync(Student.GenerateStudents(100));

        Student student = Student.GenerateStudent();

        ReplaceOneResult updateOrInsertResult = await repository.UpdateOrInsertAsync(student, true);
        // updateOrInsertResult.UpsertedId.AsString // It has a value, in case of insert.

        student = await repository.GetAsync(student.Id);

        DeleteResult deleteResult = await repository.DeleteAsync(student.Id);

        bool exists = await repository.ExistsAsync(s => s.Id == student.Id);

        long count = await repository.CountAsync(s => s.Gender == Gender.Woman);

        IEnumerable<Student> physicists = await repository.FindAsync(s => s.Subjects.Contains("Physics"));

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
        PageQuery<Student, StudentDao> pageQueryProjection = PageQuery<Student, StudentDao>
          .Create(page: 1, pageSize: 20)
          .Filter(s => s.Age >= 18 && s.Age <= 65)
          .Sort(sb => sb.Descending("$natural")) // https://docs.mongodb.com/manual/reference/glossary/#term-natural-order
          .Project(s => new StudentDao { Id = s.Id, Name = s.Name, SubjectCount = s.Subjects.Count() });

        PageResult<StudentDao> studentDaoPageResult = null;

        do
        {
          studentDaoPageResult = await repository.BrowseAsync(pageQueryProjection);
          pageQueryProjection.Page++;
        } while (studentDaoPageResult.HasNextPage);

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
