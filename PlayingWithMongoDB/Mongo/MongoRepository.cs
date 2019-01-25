using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using PlayingWithMongoDB.Types;

namespace PlayingWithMongoDB.Mongo
{
  public class MongoRepository<TEntity> : IMongoRepository<TEntity> where TEntity : IIdentifiable
  {
    protected readonly IMongoCollection<TEntity> _collection;

    public MongoRepository(IMongoDatabase database, string collectionName)
    {
      _collection = database.GetCollection<TEntity>(collectionName);
    }

    public virtual async Task<TEntity> GetAsync(Guid id)
      => await FirstOrDefaultAsync(e => e.Id == id);

    public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
      => await _collection.Find(filter).FirstOrDefaultAsync();

    public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter)
      => await _collection.Find(filter).ToListAsync();

    public virtual async Task<PageResult<TEntity>> BrowseAsync(PageQuery<TEntity> pageQuery)
      => await _collection.PaginateAsync(pageQuery);
     // => await _collection.AsQueryable().Where(filter).PaginateAsync(query);

    public virtual async Task<PageResult<TProjection>> BrowseAsync<TProjection>(PageQuery<TEntity, TProjection> pageQuery)
      => await _collection.PaginateAsync(pageQuery);

    public virtual async Task InsertAsync(TEntity entity)
      => await _collection.InsertOneAsync(entity);

    public virtual async Task InsertAsync(IEnumerable<TEntity> entities)
      => await _collection.InsertManyAsync(entities);

    public virtual async Task<ReplaceOneResult> UpdateOrInsertAsync(TEntity entity, bool isUpsert = false)
      => await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity, new UpdateOptions { IsUpsert = isUpsert });

    public virtual async Task<DeleteResult> DeleteAsync(Guid id)
      => await _collection.DeleteOneAsync(e => e.Id == id);

    public virtual async Task<DeleteResult> DeleteAsync(Expression<Func<TEntity, bool>> filter)
      => await _collection.DeleteManyAsync(filter);

    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter)
      => await _collection.Find(filter).AnyAsync();

    public virtual async Task<long> CountAsync(Expression<Func<TEntity, bool>> filter)
      => await _collection.CountDocumentsAsync(filter);
  }
}