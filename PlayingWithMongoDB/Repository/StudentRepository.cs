using MongoDB.Driver;
using PlayingWithMongoDB.Model;
using PlayingWithMongoDB.Mongo;

namespace PlayingWithMongoDB.Repository;

public sealed class StudentRepository : MongoRepository<Student>, IStudentRepository
{
    public StudentRepository(IMongoCollection<Student> collection) : base(collection)
    {
    }

    public async Task<UpdateResult> RemoveSubjectAsync(Guid id, string subject)
    {
        // Array Update Operators: https://docs.mongodb.com/manual/reference/operator/update-array

        // Use the Update.PullFilter, if the list of items are complex objects.

        UpdateDefinition<Student> updateDefinition = Builders<Student>.Update.Pull(s => s.Subjects, subject);

        return await _collection.UpdateOneAsync(s => s.Id == id, updateDefinition);
    }

    public async Task<UpdateResult> AddSubjectAsync(Guid id, string subject)
    {
        // $addToSet only ensures that there are no duplicate items added to the set.

        UpdateDefinition<Student> updateDefinition = Builders<Student>.Update.AddToSet(s => s.Subjects, subject);

        return await _collection.UpdateOneAsync(s => s.Id == id, updateDefinition);
    }
}
