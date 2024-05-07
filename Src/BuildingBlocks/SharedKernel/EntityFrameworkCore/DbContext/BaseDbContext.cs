using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SharedKernel.Auth;
using SharedKernel.Domain;
using SharedKernel.Libraries;

namespace SharedKernel.EntityFrameworkCore.DbContext;

public class BaseDbContext : Microsoft.EntityFrameworkCore.DbContext, IBaseDbContext
{
    public BaseDbContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public new virtual Task<int> SaveChangesAsync(bool applyAuditFields, CancellationToken cancellationToken = new CancellationToken())
    {
        if(applyAuditFields) ApplyAuditFieldsToModifiedEntities();
        return base.SaveChangesAsync(cancellationToken);
    }
    
    public new virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task RollbackAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        if (this.Database.CurrentTransaction is null)
        {
            return;
        }
        
        await this.Database.RollbackTransactionAsync(cancellationToken);
    }
        

    public virtual void BeginTransaction()
    {
        if (this.Database.CurrentTransaction is null)
        {
            this.Database.BeginTransaction();
        }
    }
        
    public virtual async Task CommitAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        await this.SaveChangesAsync(cancellationToken);
            
        if (this.Database.CurrentTransaction is not null)
        {
            await this.Database.CommitTransactionAsync(cancellationToken);
        }
    }
    
        
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
     private void ApplyAuditFieldsToModifiedEntities()
    {
        var currentUser = this.GetService<ICurrentUser>();
        var modified = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added 
                        || e.State == EntityState.Modified
                        || e.State == EntityState.Deleted);

        foreach (var entry in modified)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                {
                    if (entry.Entity is IUserTracking userTracking)
                    {
                        userTracking.CreatedBy = currentUser.Context.OwnerId;
                    }

                    if (entry.Entity is IDateTracking dateTracking)
                    {
                        dateTracking.CreatedDate = DateHelper.Now;
                    }
                    break;
                }
                case EntityState.Modified: 
                {
                    Entry(entry.Entity).Property("Id").IsModified = false;
                    if (entry.Entity is IUserTracking userTracking)
                    {
                        userTracking.LastModifiedBy = currentUser.Context.OwnerId;
                    }

                    if (entry.Entity is IDateTracking dateTracking)
                    {
                        dateTracking.LastModifiedDate = DateHelper.Now;
                    }
                    break;
                }
                case EntityState.Deleted:
                {
                    if (entry.Entity is ISoftDelete softDelete)
                    {
                        Entry(entry.Entity).Property("Id").IsModified = false;
                        softDelete.DeletedBy = currentUser.Context.OwnerId;
                        softDelete.DeletedDate = DateHelper.Now;
                        softDelete.IsDeleted = true;
                        entry.State = EntityState.Modified;
                    }
                    break;
                }
            }
        }
        
    }
    
}