# Playing with MongoDB

- To try out MongoDB, this console application serves as a playground
- You can find a general repository for CRUD operations and another one for Time Series Collections

## Resources

#### 🍃 `MongoDB - Official`

- [Documentation - Manual](https://www.mongodb.com/docs/manual)
- [C# driver documentation](https://www.mongodb.com/docs/drivers/csharp/current)
- [C# driver API Documentation](https://mongodb.github.io/mongo-csharp-driver/2.29.0/api/index.html) *- [IMongoCollection](https://mongodb.github.io/mongo-csharp-driver/2.29.0/api/MongoDB.Driver/MongoDB.Driver.IMongoCollection-1.html), [IFindFluent](https://mongodb.github.io/mongo-csharp-driver/2.29.0/api/MongoDB.Driver/MongoDB.Driver.IFindFluent-2.html), [IMongoQueryable](https://mongodb.github.io/mongo-csharp-driver/2.29.0/api/MongoDB.Driver/MongoDB.Driver.Linq.IMongoQueryable-1.html)*
- Time Series
  - [Manual](https://www.mongodb.com/docs/manual/core/timeseries-collections)
  - [C# fundamentals](https://www.mongodb.com/docs/drivers/csharp/current/fundamentals/time-series)

#### ✨ `Miscellaneous`

- [Dealing with DateTime](https://danielwertheim.se/mongodb-csharp-and-datetimes) 📓*Daniel Wertheim*
- Playing with [Geospatial queries](https://github.com/19balazs86/PlayingWithGeospatial) 👤*My repository*

## In the example
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
