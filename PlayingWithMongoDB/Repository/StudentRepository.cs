using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using PlayingWithMongoDB.Model;
using PlayingWithMongoDB.Mongo;

namespace PlayingWithMongoDB.Repository
{
  public class StudentRepository : MongoRepository<Student>, IStudentRepository
  {
    public StudentRepository(IMongoDatabase database, string collectionName) : base(database, collectionName)
    {
    }

    public async Task<UpdateResult> RemoveSubjectAsync(Guid id, string subject)
    {
      // Use the Update.PullFilter, if the list of items are complex objects.

      UpdateDefinition<Student> updateDefinition = Builders<Student>.Update.Pull(s => s.Subjects, subject);

      return await _collection.UpdateOneAsync(s => s.Id == id, updateDefinition);
    }
  }
}
