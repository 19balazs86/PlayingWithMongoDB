using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using PlayingWithMongoDB.Types;

namespace PlayingWithMongoDB.Mongo
{
  public interface IMongoRepository<TEntity> where TEntity : IIdentifiable
  {
    Task<TEntity> GetAsync(Guid id);

    Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter);

    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter);

    Task<PageResult<TEntity>> BrowseAsync(PageQuery<TEntity> pageQuery);

    Task<PageResult<TProjection>> BrowseAsync<TProjection>(PageQuery<TEntity, TProjection> pageQuery);

    Task InsertAsync(TEntity entity);

    Task InsertAsync(IEnumerable<TEntity> entities);

    Task<ReplaceOneResult> UpdateOrInsertAsync(TEntity entity, bool isUpsert = false);

    Task<DeleteResult> DeleteAsync(Guid id);

    Task<DeleteResult> DeleteAsync(Expression<Func<TEntity, bool>> filter);

    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter);

    Task<long> CountAsync(Expression<Func<TEntity, bool>> filter);
  }
}