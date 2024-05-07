using System.Collections;
using System.Reflection;
using Caching.Sequence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SharedKernel.Auth;
using SharedKernel.Contracts;
using SharedKernel.Contracts.Repositories;
using SharedKernel.Domain;
using SharedKernel.EntityFrameworkCore.DbContext;
using SharedKernel.Libraries;
using SharedKernel.UnitOfWork;

namespace SharedKernel.Infrastructures;

public class WriteOnlyRepository<TEntity, TKey, TDbContext>
    : ReadOnlyRepository<TEntity, TKey, TDbContext>, 
        IWriteOnlyRepository<TEntity, TKey, TDbContext>
    where TEntity : EntityBase<TKey>
    where TDbContext : BaseDbContext
{
    public WriteOnlyRepository(
        TDbContext context,
        ICurrentUser currentUser, 
        ISequenceCaching sequenceCaching
        ) : base(context, currentUser, sequenceCaching)
    {
    }

    public IUnitOfWork UnitOfWork => Context;
    
    public Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<TEntity>> DeleteRangeAsync(ICollection<TEntity> entities, bool permanent = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    protected virtual void EditEntityPropertiesToAdd(TEntity entity)
    {
        if (entity is IAuditable auditable)
        {
            
            auditable.CreatedBy = CurrentUser.Context.OwnerId;
            auditable.CreatedDate = DateHelper.Now;
            auditable.LastModifiedBy = null;
            auditable.LastModifiedDate = null;
            auditable.DeletedBy = null;
            auditable.DeletedBy = null;
            auditable.IsDeleted = false;
        } 
    }
    
    
    protected virtual void EditEntityPropertiesToUpdate(TEntity entity)
    {
        if (entity is IAuditable auditable)
        {
            auditable.LastModifiedBy = CurrentUser.Context.OwnerId;
            auditable.LastModifiedDate = DateHelper.Now;
        } 
    }
    
    protected async Task SetEntityAsDeleted(
        TEntity entity,
        bool permanent,
        CancellationToken cancellationToken = default
    )
    {
        if (!permanent && entity is IAuditable auditable)
        {
            CheckHasEntityHaveOneToOneRelation(entity);
            await SetEntityAsSoftDeleted(auditable, cancellationToken);
        }
        else
            Context.Remove(entity);
    }
    
    protected virtual void EditEntityPropertiesToDelete(TEntity entity)
    {
        if (entity is IAuditable auditable)
        {
            auditable.DeletedBy = CurrentUser.Context.OwnerId;
            auditable.DeletedDate = DateHelper.Now;
            auditable.IsDeleted = true;
        } 
    }

    protected virtual void EditRelationEntityPropertiesToCascadeSoftDelete(IAuditable entity)
    {
        entity.DeletedDate = DateTime.UtcNow;
    }
    
    protected virtual void CheckHasEntityHaveOneToOneRelation(TEntity entity)
    {
        IEnumerable<IForeignKey> foreignKeys = Context.Entry(entity).Metadata.GetForeignKeys();
        IForeignKey? oneToOneForeignKey = foreignKeys.FirstOrDefault(fk =>
            fk.IsUnique && fk.PrincipalKey.Properties.All(pk => Context.Entry(entity).Property(pk.Name).Metadata.IsPrimaryKey())
        );

        if (oneToOneForeignKey != null)
        {
            string relatedEntity = oneToOneForeignKey.PrincipalEntityType.ClrType.Name;
            IReadOnlyList<IProperty> primaryKeyProperties = Context.Entry(entity).Metadata.FindPrimaryKey()!.Properties;
            string primaryKeyNames = string.Join(", ", primaryKeyProperties.Select(prop => prop.Name));
            throw new InvalidOperationException(
                $"Entity {entity.GetType().Name} has a one-to-one relationship with {relatedEntity} via the primary key ({primaryKeyNames}). Soft Delete causes problems if you try to create an entry again with the same foreign key."
            );
        }
    }
    
    protected virtual IQueryable<object>? GetRelationLoaderQuery(IQueryable query, Type navigationPropertyType)
    {
        Type queryProviderType = query.Provider.GetType();
        MethodInfo createQueryMethod =
            queryProviderType
                .GetMethods()
                .First(m => m is { Name: nameof(query.Provider.CreateQuery), IsGenericMethod: true })
                ?.MakeGenericMethod(navigationPropertyType)
            ?? throw new InvalidOperationException("CreateQuery<TElement> method is not found in IQueryProvider.");
        var queryProviderQuery = (IQueryable<object>)createQueryMethod.Invoke(query.Provider, parameters: [query.Expression])!;
        return queryProviderQuery.Where(x => !((IAuditable)x).DeletedDate.HasValue);
    }
    
    protected virtual bool IsSoftDeleted(IAuditable entity)
    {
        return entity.DeletedDate.HasValue;
    }
    
    private async Task SetEntityAsSoftDeleted(
        IAuditable entity,
        CancellationToken cancellationToken = default,
        bool isRoot = true
    )
    {
        if (IsSoftDeleted(entity))
            return;
        if (isRoot) EditEntityPropertiesToDelete((TEntity)entity);
        else EditRelationEntityPropertiesToCascadeSoftDelete(entity);

        var navigations = Context
            .Entry(entity)
            .Metadata.GetNavigations()
            .Where(x =>
                x is { IsOnDependent: false, ForeignKey.DeleteBehavior: DeleteBehavior.ClientCascade or DeleteBehavior.Cascade }
            )
            .ToList();
        foreach (INavigation? navigation in navigations)
        {
            if (navigation.TargetEntityType.IsOwned())
                continue;
            if (navigation.PropertyInfo == null)
                continue;

            object? navValue = navigation.PropertyInfo.GetValue(entity);
            if (navigation.IsCollection)
            {
                if (navValue == null)
                {
                    IQueryable query = Context.Entry(entity).Collection(navigation.PropertyInfo.Name).Query();
                    
                    IQueryable<object>? relationLoaderQuery = GetRelationLoaderQuery(
                        query,
                        navigationPropertyType: navigation.PropertyInfo.GetType()
                    );
                    if (relationLoaderQuery is not null)
                        navValue = await relationLoaderQuery.ToListAsync(cancellationToken);
                    
                    if (navValue == null) continue;
                }

                foreach (object navValueItem in (IEnumerable)navValue)
                    await SetEntityAsSoftDeleted((IAuditable)navValueItem, cancellationToken, isRoot: false);
            }
            else
            {
                if (navValue == null)
                {
                    IQueryable query = Context.Entry(entity).Reference(navigation.PropertyInfo.Name).Query();
                    
                    IQueryable<object>? relationLoaderQuery = GetRelationLoaderQuery(
                        query,
                        navigationPropertyType: navigation.PropertyInfo.GetType()
                    );
                    if (relationLoaderQuery is not null) 
                        navValue = await relationLoaderQuery.FirstOrDefaultAsync(cancellationToken);
                    
                    if (navValue == null) continue;
                }

                await SetEntityAsSoftDeleted((IAuditable)navValue, cancellationToken, isRoot: false);
            }
        }

        Context.Update(entity);
    }
    
    protected virtual async Task ClearCacheWhenChangesAsync(List<TKey>? ids, CancellationToken cancellationToken = default)
    {
        var tasks = new List<Task>();
        var fullRecordKey = IsSystemTable ? BaseCacheKeys.GetSystemFullRecordsKey(TableName) : BaseCacheKeys.GetFullRecordsKey(TableName, CurrentUser.Context.OwnerId);
        tasks.Add(SequenceCaching.DeleteAsync(fullRecordKey));

        if (ids != null && ids.Any())
        {
            foreach (var id in ids)
            {
                var recordByIdKey = IsSystemTable ? BaseCacheKeys.GetSystemRecordByIdKey(TableName, id) : BaseCacheKeys.GetRecordByIdKey(TableName, id, CurrentUser.Context.OwnerId);
                tasks.Add(SequenceCaching.DeleteAsync(recordByIdKey));
            }
        }
        
        await Task.WhenAll(tasks);
    }
    
}