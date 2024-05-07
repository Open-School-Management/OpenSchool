using System.Linq.Expressions;
using Caching.Sequence;
using MassTransit.Internals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Dynamic;
using SharedKernel.Auth;
using SharedKernel.Contracts.Repositories;
using SharedKernel.Domain;
using SharedKernel.EntityFrameworkCore.Paging;

namespace SharedKernel.Infrastructures;

public class ReadOnlyRepository<TEntity, TKey, TDbContext> : IReadOnlyRepository<TEntity, TKey>
    where TEntity :  EntityBase<TKey>
    where TDbContext : DbContext
{
    protected readonly TDbContext Context;
    protected readonly ICurrentUser CurrentUser;
    protected readonly ISequenceCaching SequenceCaching;
    protected readonly string TableName;
    protected readonly bool IsSystemTable;
    
    public ReadOnlyRepository(
        TDbContext context, 
        ICurrentUser currentUser,
        ISequenceCaching sequenceCaching)
    {
        Context = context;
        CurrentUser = currentUser;
        SequenceCaching = sequenceCaching;
        TableName = nameof(TEntity);
        IsSystemTable =  typeof(TEntity).HasInterface<IPersonalizeEntity>();
    }

    public IQueryable<TEntity> Query()
    {
        return Context.Set<TEntity>();
    }

    public IQueryable<TEntity> FindAll(
        Expression<Func<TEntity, bool>>? predicate = null, 
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, 
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, 
        bool trackChanges = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query();
        if (!trackChanges)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (predicate != null)
            queryable = queryable.Where(predicate);
        if (orderBy != null)
            queryable = orderBy(queryable);
        return queryable;
    }

    public async Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> predicate, 
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, 
        bool trackChanges = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query();
        if (!trackChanges)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        return await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<IPagedList<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>>? predicate = null, 
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, 
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 0,
        int size = 10, 
        bool trackChanges = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query();
        if (!trackChanges)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (predicate != null)
            queryable = queryable.Where(predicate);
        if (orderBy != null)
            return await orderBy(queryable).ToPagedListAsync(index, size, from: 0, cancellationToken);
        return await queryable.ToPagedListAsync(index, size, from: 0, cancellationToken);
    }
    
    public async Task<IPagedList<TEntity>> GetListByDynamicAsync(
        DynamicQuery dynamic, 
        Expression<Func<TEntity, bool>>? predicate = null, 
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 0,
        int size = 10, 
        bool trackChanges = true, 
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query().ToDynamic(dynamic);
        if (!trackChanges)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (predicate != null)
            queryable = queryable.Where(predicate);
        return await queryable.ToPagedListAsync(index, size, from: 0, cancellationToken);
    }

    public async Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>>? predicate = null, 
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, 
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        return await queryable.AnyAsync(cancellationToken);
    }
}