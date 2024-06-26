using System.Reflection;
using Identity.Application.Persistence;
using Identity.Domain.Entities;
using Identity.Infrastructure.Extensions;
using IntegrationEventLogs;
using Microsoft.EntityFrameworkCore;
using SharedKernel.EntityFrameworkCore.DbContext;

namespace Identity.Infrastructure.Persistence;

public class IdentityDbContext : BaseDbContext, IIdentityDbContext
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<UserConfig> UserConfigs { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<UserPermission> UserPermissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<SignInHistory> SignInHistories { get; set; }
    public DbSet<SecretKey> SecretKeys { get; set; }
    public DbSet<Otp> Otps { get; set; }
    public DbSet<MFA> Mfas { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.UseIntegrationEventLogs();
        
        var entityTypes = modelBuilder.Model.GetEntityTypes();
        foreach (var entityType in entityTypes)
        {
            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                modelBuilder.SetSoftDeleteFilter(entityType.ClrType);
        }
        
        base.OnModelCreating(modelBuilder);
        
    }
    
}