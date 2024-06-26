using SharedKernel.EntityFrameworkCore.DbContext;

namespace Identity.Application.Persistence;

public interface IIdentityDbContext : IBaseDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<UserConfig> UserConfigs { get; set; }
    DbSet<RefreshToken> RefreshTokens { get; set; }
    DbSet<Role> Roles { get; set; }
    DbSet<UserRole> UserRoles { get; set; }
    DbSet<Permission> Permissions { get; set; }
    DbSet<UserPermission> UserPermissions { get; set; }
    DbSet<RolePermission> RolePermissions { get; set; }
    DbSet<SignInHistory> SignInHistories { get; set; }
    DbSet<SecretKey> SecretKeys { get; set; }
    DbSet<Otp> Otps { get; set; }
    DbSet<MFA> Mfas { get; set; }
    
}