using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ResourceManager.Infrastructure.Contexts;


public class ResourceManagerDbContextFactory : IDesignTimeDbContextFactory<ResourceManagerDbContext>
{
    public ResourceManagerDbContext CreateDbContext(string[] args)
    {
        var connectionString = "Server=localhost; Port=3306; Database=resource_manager_db; Uid=root; Pwd=root; SslMode=Preferred;";
        var optionsBuilder = new DbContextOptionsBuilder<ResourceManagerDbContext>();
        optionsBuilder
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .LogTo(s => System.Diagnostics.Debug.WriteLine(s))
            .EnableDetailedErrors(true)
            .EnableSensitiveDataLogging(true);
        
        var context = new ResourceManagerDbContext(optionsBuilder.Options);
        
        return context;
    }
}