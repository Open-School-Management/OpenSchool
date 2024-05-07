using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace SharedKernel.Contracts.Repositories;

public interface IReadOnlyRepository<TEntity, in TKey, TDbContext> 
    where TEntity : EntityBase<TKey>
    where TDbContext : DbContext
{
    IQueryable<TEntity> FindAll(bool trackChanges = false);
    
    IQueryable<TEntity> FindAll(bool trackChanges = false, 
        params Expression<Func<TEntity, object>>[] includeProperties);
    
    IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression,
        bool trackChanges = false);
    
    IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, 
        bool trackChanges = false,
        params Expression<Func<TEntity, object>>[] includeProperties);
    
    Task<TEntity?> GetByIdAsync(TKey id, 
        CancellationToken cancellationToken = default);
    
    Task<TEntity?> GetByIdAsync(TKey id, 
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includeProperties);
    
    Task<TEntity?> GetByIdWithCachingAsync(TKey id, 
        CancellationToken cancellationToken = default);
    
    #region Cache
    Task<CacheResult<TEntity>> GetByIdCacheAsync(TKey id, 
        CancellationToken cancellationToken = default);
    
    #endregion
}