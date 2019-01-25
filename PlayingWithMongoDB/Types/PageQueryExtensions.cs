using System;
using System.Linq.Expressions;
using MongoDB.Driver;

namespace PlayingWithMongoDB.Types
{
  public static class PageQueryExtensions
  {
    // Filter for PageQuery<T>
    public static PageQuery<T> Filter<T>(this PageQuery<T> pageQuery, Expression<Func<T, bool>> filterExp)
    {
      pageQuery.FilterDefinition = Builders<T>.Filter.Where(filterExp);

      return pageQuery;
    }

    // Sort for PageQuery<T>
    public static PageQuery<T> Sort<T>(
      this PageQuery<T> pageQuery,
      Func<SortDefinitionBuilder<T>, SortDefinition<T>> sortFunc)
    {
      pageQuery.SortDefinition = sortFunc(Builders<T>.Sort);

      return pageQuery;
    }

    // Filter for PageQuery<T, P>
    public static PageQuery<T, P> Filter<T, P>(this PageQuery<T, P> pageQuery, Expression<Func<T, bool>> filterExp)
    {
      pageQuery.FilterDefinition = Builders<T>.Filter.Where(filterExp);

      return pageQuery;
    }

    // Sort for PageQuery<T, P>
    public static PageQuery<T, P> Sort<T, P>(
      this PageQuery<T, P> pageQuery,
      Func<SortDefinitionBuilder<T>, SortDefinition<T>> sortFunc)
    {
      pageQuery.SortDefinition = sortFunc(Builders<T>.Sort);

      return pageQuery;
    }

    // Project for PageQuery<T, P>
    public static PageQuery<T, P> Project<T, P>(this PageQuery<T, P> pageQuery, Expression<Func<T, P>> projectionExp)
    {
      pageQuery.ProjectionDefinition = Builders<T>.Projection.Expression(projectionExp);

      return pageQuery;
    }
  }
}
