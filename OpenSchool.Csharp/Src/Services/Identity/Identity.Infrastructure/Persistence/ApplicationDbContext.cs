using System.Reflection;
using Identity.Application.Persistence;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.EFCore;

namespace Identity.Infrastructure.Persistence;

public class ApplicationDbContext : CoreDbContext, IApplicationDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Avatar> Avatars { get; set; }
    public DbSet<UserConfig> UserConfigs { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Permission> Actions { get; set; }
    public DbSet<UserPermission> UserPermissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<SignInHistory> SignInHistories { get; set; }
    public DbSet<SecretKey> SecretKeys { get; set; }
    public DbSet<OTP> OTPs { get; set; }
    public DbSet<MFA> MFAs { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
    }
}