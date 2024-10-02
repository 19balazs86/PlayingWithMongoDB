# Playing with MongoDB

This is a .NET console application that serves as a playground to try out MongoDB.

## Resources

- MongoDB - Official 📓
  - [Documentation - Manual](https://www.mongodb.com/docs/manual)
  - [C# driver documentation](https://www.mongodb.com/docs/drivers/csharp/current)
  - [.NET Driver API Documentation](https://mongodb.github.io/mongo-csharp-driver/2.29.0/api/index.html) *- [IMongoCollection](https://mongodb.github.io/mongo-csharp-driver/2.29.0/api/MongoDB.Driver/MongoDB.Driver.IMongoCollection-1.html), [IFindFluent](https://mongodb.github.io/mongo-csharp-driver/2.29.0/api/MongoDB.Driver/MongoDB.Driver.IFindFluent-2.html), [IMongoQueryable](https://mongodb.github.io/mongo-csharp-driver/2.29.0/api/MongoDB.Driver/MongoDB.Driver.Linq.IMongoQueryable-1.html)*

- [Dealing with DateTime](https://danielwertheim.se/mongodb-csharp-and-datetimes) 📓*Daniel Wertheim*

## In the example you can find
- Repository with CRUD operations and example of using it
- Pagination solution with `PageResult` and `PageQuery` object

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

#### MongoDB server

- [Community Server](https://www.mongodb.com/download-center/community) 📓*MongoDB*
- [Docker hub](https://hub.docker.com/_/mongo)
- [Create MongoDB in the Cloud with Atlas](https://www.youtube.com/watch?v=KKyag6t98g8) 📽️*15min - Traversy Media (Brad)*
