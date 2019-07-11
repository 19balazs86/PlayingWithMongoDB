# Playing with MongoDB

This is a small .NET Core application, a playground to try out MongoDB. The example just gives you a subtle insight into MongoDB.

#### Resources

- [MongoDB - Manual](https://docs.mongodb.com/manual).
- [MongoDB .NET Driver and Documentation](http://mongodb.github.io/mongo-csharp-driver). Installing the MongoDB.Driver package, you will get the MongoDB.Driver.Core and the MongoDB.Bson also.
- [.NET Driver API Documentation](http://mongodb.github.io/mongo-csharp-driver/2.7/apidocs/html/R_Project_CSharpDriverDocs.htm) ([IMongoCollection](http://mongodb.github.io/mongo-csharp-driver/2.7/apidocs/html/T_MongoDB_Driver_IMongoCollection_1.htm), [IFindFluent](http://mongodb.github.io/mongo-csharp-driver/2.7/apidocs/html/T_MongoDB_Driver_IFindFluent_2.htm), [IMongoQueryable](http://mongodb.github.io/mongo-csharp-driver/2.7/apidocs/html/T_MongoDB_Driver_Linq_IMongoQueryable_1.htm)).
- [Dealing with DateTime](http://www.binaryintellect.net/articles/6c715186-97b1-427a-9ccc-deb3ece7b839.aspx).

#### In the example you can find
- Connect to MongoDB.
- Repository with some basic CRUD operations.
- Pagination solution with `PageResult` and `PageQuery` object.

```csharp
PageQuery<Student> pageQuery = PageQuery<Student>
    .Create(page: 1, pageSize: 20)
    .Filter(s => s.Age >= 18 && s.Age <= 65)
    .Sort(sb => sb.Ascending(s => s.Age).Ascending(s => s.Name));

PageResult<Student> students = await repository.BrowseAsync(pageQuery);
```
```csharp
PageQuery<Student, StudentDto> pageQuery = PageQuery<Student, StudentDto>
    .Create(...).Filter(...).Sort(...)
    .Project(s => new StudentDto { Id = s.Id, Name = s.Name });

PageResult<StudentDto> students = await repository.BrowseAsync(pageQuery);
```

#### Get MongoDB server

- [MongoDB Community Server](https://www.mongodb.com/download-center/community).
- [From docker hub](https://hub.docker.com/_/mongo).
- Create MongoDB in the Cloud with Atlas: [Traversy Media (Brad) - video](https://www.youtube.com/watch?v=KKyag6t98g8).
