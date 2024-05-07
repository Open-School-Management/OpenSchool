using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Dynamic;
using SharedKernel.Domain;
using SharedKernel.EntityFrameworkCore.Paging;

namespace SharedKernel.Contracts.Repositories;

public interface IReadOnlyRepository<TEntity, in TKey> : IQuery<TEntity>
    where TEntity : EntityBase<TKey>
{
    IQueryable<TEntity> FindAll(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool trackChanges = true,
        CancellationToken cancellationToken = default
    );
    
    Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool trackChanges = true,
        CancellationToken cancellationToken = default
    );
    
    Task<IPagedList<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 0,
        int size = 10,
        bool trackChanges = true,
        CancellationToken cancellationToken = default
    );

    Task<IPagedList<TEntity>> GetListByDynamicAsync(
        DynamicQuery dynamic,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 0,
        int size = 10,
        bool trackChanges = true,
        CancellationToken cancellationToken = default
    );

    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        CancellationToken cancellationToken = default
    );
}