using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.EntityFrameworkCore.DbContext;
using SharedKernel.UnitOfWork;

namespace SharedKernel.Contracts.Repositories;

public interface IWriteOnlyRepository<TEntity, in TKey ,TDbContext> : IReadOnlyRepository<TEntity, TKey>
    where TEntity : EntityBase<TKey>
    where TDbContext : IBaseDbContext
{
    IUnitOfWork UnitOfWork { get; }
    
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default);

    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default);

    Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false, CancellationToken cancellationToken = default);

    Task<ICollection<TEntity>> DeleteRangeAsync(
        ICollection<TEntity> entities,
        bool permanent = false,
        CancellationToken cancellationToken = default
    );
}