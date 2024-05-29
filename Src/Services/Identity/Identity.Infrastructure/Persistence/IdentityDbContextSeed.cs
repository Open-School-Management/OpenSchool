using Core.Security.Constants;
using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Identity.Infrastructure.Persistence;

public class IdentityDbContextSeed(IdentityDbContext context)
{
    /// <summary>
    /// Initialise database
    /// </summary>
    public async Task InitialiseAsync()
    {
        try
        {
            if (context.Database.IsNpgsql())
                await context.Database.MigrateAsync();
        }
        catch (Exception e)
        {
            Log.Error("An error occurred while initialising the database.");
            throw;
        }
    }

    /// <summary>
    /// Seed data 
    /// </summary>
    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Log.Error("An error occurred while seeding the database;");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        Log.Information("BEGIN: Starting seeding process for permissions, roles, and users. ");
        var supperAdmin = Default.GetSupperAdmin();
        var supperAdminId = Default.GetSupperAdminId;
        var admin = Default.GetAdmin();
        var roles = Default.GetRoles();
        var supperAdminRole = roles.FirstOrDefault(r => r.Code == RoleConstant.SupperAdmin);
        var adminRole = roles.FirstOrDefault(r => r.Code == RoleConstant.Admin);
        var permissions = Default.GetPermissions();
        
        // Seed permission
        if (!context.Permissions.Any())
        {
            await context.Permissions.AddRangeAsync(permissions); 
        }
        
        // Seed role and assign permission for role
        if (!context.Roles.Any())
        {
            await context.Roles.AddRangeAsync(roles);
            
            foreach (var permission in permissions)
            {
                var rp = new RolePermission()
                {
                    Id = Guid.NewGuid(),
                    Role = supperAdminRole,
                    Permission = permission,
                    IsDeleted = false, 
                    CreatedDate = DateHelper.Now, 
                    CreatedBy = supperAdminId, 
                    LastModifiedDate = null, 
                    LastModifiedBy = null, 
                    DeletedDate = null, 
                    DeletedBy = null
                };
            
                await context.RolePermissions.AddAsync(rp);

                if (permission.Exponent <= (int)ActionExponent.Admin)
                {
                    var rp2 = new RolePermission()
                    {
                        Id = Guid.NewGuid(),
                        Role = adminRole,
                        Permission = permission,
                        IsDeleted = false, 
                        CreatedDate = DateHelper.Now, 
                        CreatedBy = supperAdminId, 
                        LastModifiedDate = null, 
                        LastModifiedBy = null, 
                        DeletedDate = null, 
                        DeletedBy = null
                    };
                    await context.RolePermissions.AddAsync(rp2);
                }
            }
        }
        
        // Seed user and assign role, permission
        if (!context.Users.Any())
        {
            await context.Users.AddAsync(supperAdmin);
            await context.Users.AddAsync(admin);

            var supperAdminUserRole = new UserRole()
            {
                Id = Guid.NewGuid(),
                Role = supperAdminRole,
                User = supperAdmin,
                IsDeleted = false,
                CreatedDate = DateHelper.Now,
                CreatedBy = supperAdminId,
                LastModifiedDate = null,
                LastModifiedBy = null,
                DeletedDate = null,
                DeletedBy = null
            };

            var adminUserRole = new UserRole()
            {
                Id = Guid.NewGuid(),
                Role = adminRole,
                User = admin,
                IsDeleted = false,
                CreatedDate = DateHelper.Now,
                CreatedBy = supperAdminId,
                LastModifiedDate = null,
                LastModifiedBy = null,
                DeletedDate = null,
                DeletedBy = null
            };
            await context.UserRoles.AddRangeAsync(supperAdminUserRole, adminUserRole);
        }
        Log.Information("END: End seeding process for permissions, roles, and users. ");
    }

    public async Task SyncPermissionsBasedOnChanges()
    {
        Log.Information("BEGIN-SyncPermissionsBasedOnChanges: Sync Permissions Based On Changes");
        
        var supperAdmin = await context.Users.SingleOrDefaultAsync(e => e.Email.Equals("devbe2002@gmail.com"));
        var supperAdminRole = await context.Roles.SingleOrDefaultAsync(r => r.Code == RoleConstant.SupperAdmin);
        var adminRole = await context.Roles.SingleOrDefaultAsync(r => r.Code == RoleConstant.Admin);
        var currentPermissions = await context.Permissions.ToListAsync();

        var currentExponents = System.Enum.GetValues(typeof(ActionExponent)).Cast<ActionExponent>().ToList();
        var existingExponents = currentPermissions.Select(p => (ActionExponent)p.Exponent).ToList();

        if (existingExponents.OrderBy(x => x).SequenceEqual(currentExponents.OrderBy(x => x)))
        {
            Log.Information("END-SyncPermissionsBasedOnChanges: Sync Permissions Based On Changes: No changes detected.");
            return;
        };

        var permissionsToAdd = currentExponents
            .Except(existingExponents)
            .Select(exponent => new Permission
            {
                Code = exponent.ToString().ToSnakeCaseUpper(),
                Name = exponent.ToString().PascalToStandard(),
                Exponent = (int)exponent,
                IsDeleted = false,
                CreatedDate = DateHelper.Now,
                CreatedBy = supperAdmin.Id,
                LastModifiedDate = null,
                LastModifiedBy = null,
                DeletedDate = null,
                DeletedBy = null
            })
            .ToList();

        var permissionsToDelete = existingExponents
            .Except(currentExponents)
            .Select(exponent => currentPermissions
                .FirstOrDefault(p => (ActionExponent)p.Exponent == exponent))
            .ToList();
        
        if (permissionsToDelete.Any())
        {
            var permissionsToDeleteIds = permissionsToDelete.Select(p => p.Id).ToList();
            var rolePermissionsToDelete = await context.RolePermissions
                .Where(rp => permissionsToDeleteIds.Contains(rp.PermissionId))
                .ToListAsync();
            
            context.RolePermissions.RemoveRange(rolePermissionsToDelete);
            context.Permissions.RemoveRange(permissionsToDelete);
        };
        
        if(permissionsToAdd.Any()) await context.Permissions.AddRangeAsync(permissionsToAdd);
        
        foreach (var permission in permissionsToAdd)
        {
            var rp = new RolePermission()
            {
                Id = Guid.NewGuid(),
                Role = supperAdminRole,
                Permission = permission,
                IsDeleted = false, 
                CreatedDate = DateHelper.Now, 
                CreatedBy = supperAdmin.Id, 
                LastModifiedDate = null, 
                LastModifiedBy = null, 
                DeletedDate = null, 
                DeletedBy = null
            };
        
            await context.RolePermissions.AddAsync(rp);

            if (permission.Exponent <= (int)ActionExponent.Admin)
            {
                var rp2 = new RolePermission()
                {
                    Id = Guid.NewGuid(),
                    Role = adminRole,
                    Permission = permission,
                    IsDeleted = false, 
                    CreatedDate = DateHelper.Now, 
                    CreatedBy = supperAdmin.Id, 
                    LastModifiedDate = null, 
                    LastModifiedBy = null, 
                    DeletedDate = null, 
                    DeletedBy = null
                };
                await context.RolePermissions.AddAsync(rp2);
            }
        }

        await context.SaveChangesAsync();
        Log.Information("END-SyncPermissionsBasedOnChanges: Sync Permissions Based On Changes completed successfully.");
        
    }
    
}