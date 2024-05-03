using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Identity.Infrastructure.Persistence;

public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
{
    public IdentityDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>();
        optionsBuilder
            .UseNpgsql(@"Host=localhost:5433;Database=identity_db;Uid=admin;Pwd=admin", 
                opts =>
                {
                    opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds); 
                }
            );
        
        var context = new IdentityDbContext(optionsBuilder.Options);
        
        return context;
    }
}