using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using PlayingWithMongoDB.Types;

namespace PlayingWithMongoDB.Mongo
{
  public static class Pagination
  {
    public static async Task<PageResult<T>> PaginateAsync<T>(
      this IMongoCollection<T> collection,
      PageQuery<T> query)
    {
      if (query.FilterDefinition is null)
        throw new NullReferenceException("PageQuery.FilterDefinition can not be null.");

      IFindFluent<T, T> findFluent = collection
        .Find(query.FilterDefinition)
        .Sort(query.SortDefinition);

      return await findFluent.paginateAsync(query.Page, query.PageSize);
    }

    public static async Task<PageResult<P>> PaginateAsync<T, P>(
      this IMongoCollection<T> collection,
      PageQuery<T, P> query)
    {
      if (query.FilterDefinition is null)
        throw new NullReferenceException("PageQuery.FilterDefinition can not be null.");

      if (query.ProjectionDefinition is null)
        throw new NullReferenceException("PageQuery.ProjectionDefinition can not be null.");

      IFindFluent<T, P> findFluent = collection
        .Find(query.FilterDefinition)
        .Sort(query.SortDefinition)
        .Project(query.ProjectionDefinition);

      return await findFluent.paginateAsync(query.Page, query.PageSize);
    }

    private static async Task<PageResult<P>> paginateAsync<T, P>(this IFindFluent<T, P> findFluent, int page, int pageSize)
    {
      long totalCount = await findFluent.CountDocumentsAsync();

      if (totalCount == 0)
        return PageResult<P>.Empty;

      int pageCount = (int) Math.Ceiling((decimal)totalCount / pageSize);

      if (page > pageCount) // query.Page still has the original value.
        page = pageCount;

      int skip = (page - 1) * pageSize;

      IEnumerable<P> items = await findFluent
        .Skip(skip)
        .Limit(pageSize)
        .ToListAsync();

      return new PageResult<P>(items, page, pageSize, pageCount, totalCount);
    }
  }
}