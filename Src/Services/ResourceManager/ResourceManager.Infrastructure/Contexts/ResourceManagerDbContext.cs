using System.Reflection;
using IntegrationEventLogs;
using Microsoft.EntityFrameworkCore;
using ResourceManager.Api.Infrastructure.Contexts;
using ResourceManager.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.EntityFrameworkCore.DbContext;
using Directory = ResourceManager.Domain.Entities.Directory;
using File = ResourceManager.Domain.Entities.File;

namespace ResourceManager.Infrastructure.Contexts;

public class ResourceManagerDbContext : BaseDbContext, IResourceManagerDbContext
{
   

    public DbSet<ActivityLog> ActivityLogs { get; set; }
    public DbSet<Config> Configs { get; set; }
    public DbSet<Directory> Directories { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<LockedDirectory> LockedDirectories { get; set; }
    public DbSet<RecycleBin> RecycleBins { get; set; }

    public ResourceManagerDbContext(DbContextOptions options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.UseIntegrationEventLogs();
        
        base.OnModelCreating(modelBuilder);
        
    }
}