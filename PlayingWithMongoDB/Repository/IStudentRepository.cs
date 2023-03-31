using MongoDB.Driver;
using PlayingWithMongoDB.Model;
using PlayingWithMongoDB.Mongo;

namespace PlayingWithMongoDB.Repository;

public interface IStudentRepository : IMongoRepository<Student>
{
    Task<UpdateResult> RemoveSubjectAsync(Guid id, string subject);

    Task<UpdateResult> AddSubjectAsync(Guid id, string subject);
}
