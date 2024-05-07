using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.UnitOfWork;

namespace SharedKernel.Contracts.Repositories;

public interface IWriteOnlyRepository<TEntity, in TKey ,TDbContext> : IReadOnlyRepository<TEntity, TKey ,TDbContext>
    where TEntity : EntityBase<TKey>
    where TDbContext : DbContext
{
    IUnitOfWork UnitOfWork { get; }
    
    Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    Task<IList<TEntity>> InsertAsync(IList<TEntity> entities, CancellationToken cancellationToken = default);
    
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    Task DeleteAsync(IList<TEntity> entities, CancellationToken cancellationToken = default);
}