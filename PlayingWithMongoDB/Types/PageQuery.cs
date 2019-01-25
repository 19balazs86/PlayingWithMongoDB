using System.Diagnostics;
using MongoDB.Driver;

namespace PlayingWithMongoDB.Types
{
  public sealed class PageQueryDefaults
  {
    public const int PageSize = 20;
  }

  [DebuggerDisplay("Page = {Page}, PageSize = {PageSize}")]
  public class PageQuery<TEntity>
  {
    #region Fields: Page and PageSize
    private int _page;
    private int _pageSize;

    public int Page
    {
      get => _page;
      set => _page = value <= 0 ? 1 : value;
    }

    public int PageSize
    {
      get => _pageSize;
      set => _pageSize = value <= 0 ? PageQueryDefaults.PageSize : value;
    }
    #endregion

    public FilterDefinition<TEntity> FilterDefinition { get; set; }

    public SortDefinition<TEntity> SortDefinition { get; set; }

    public PageQuery(int page = 1, int pageSize = PageQueryDefaults.PageSize)
    {
      Page     = page;
      PageSize = pageSize;
    }

    public static PageQuery<TEntity> Create(int page = 1, int pageSize = PageQueryDefaults.PageSize)
      => new PageQuery<TEntity>(page, pageSize);
  }

  [DebuggerDisplay("Page = {Page}, PageSize = {PageSize}")]
  public class PageQuery<TEntity, TProjection> : PageQuery<TEntity>
  {
    public ProjectionDefinition<TEntity, TProjection> ProjectionDefinition { get; set; }

    public PageQuery(int page = 1, int pageSize = PageQueryDefaults.PageSize) : base(page, pageSize)
    {
    }

    public new static PageQuery<TEntity, TProjection> Create(int page = 1, int pageSize = PageQueryDefaults.PageSize)
      => new PageQuery<TEntity, TProjection>(page, pageSize);
  }
}