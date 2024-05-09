using Caching.Sequence;
using Core.Security.Constants;
using Core.Security.Models;
using Core.Security.Utilities;
using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Infrastructures;

namespace Identity.Infrastructure.Repositories;

public class AuthRepository : WriteOnlyRepository<EntityBase, Guid, IdentityDbContext>, IAuthRepository
{
    
    public AuthRepository(
        IdentityDbContext context, 
        ICurrentUser currentUser, 
        ISequenceCaching sequenceCaching) : base(context, currentUser, sequenceCaching)
    {
    }
    
    public async Task<TokenUser?> GetTokenUserByIdentityAsync(string username, CancellationToken cancellationToken = default)
    {
        return await GetTokenUserByIdentityOrOwnerIdAsync(username, null, cancellationToken);
    }

    public async Task<TokenUser?> GetTokenUserByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        return await GetTokenUserByIdentityOrOwnerIdAsync(null, ownerId, cancellationToken);
    }
    public async Task<bool> CheckRefreshTokenAsync(string value, Guid userId, CancellationToken cancellationToken = default)
    {
        var refreshToken = await Context.RefreshTokens
            .FirstOrDefaultAsync(rt => 
                    rt.RefreshTokenValue.Equals(value)  
                    && rt.OwnerId.Equals(userId) 
                    && rt.ExpirationDate >= DateHelper.Now, 
                cancellationToken);
        
        return refreshToken != null;
    }

    public async Task CreateOrUpdateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        var existingRefreshToken = await Context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.CurrentAccessToken.Equals(CurrentUser.Context.AccessToken), cancellationToken);
        
        if (existingRefreshToken == null)
        {
            await Context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        }
        else
        {
            existingRefreshToken.RefreshTokenValue = refreshToken.RefreshTokenValue;
            existingRefreshToken.CurrentAccessToken = refreshToken.CurrentAccessToken;
            existingRefreshToken.ExpirationDate = refreshToken.ExpirationDate;
            
            Context.RefreshTokens.Update(existingRefreshToken);
        }
    }

    public async Task RemoveRefreshTokenAsync(CancellationToken cancellationToken = default)
    {
        var refreshToken = await Context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.CurrentAccessToken.Equals(CurrentUser.Context.AccessToken), cancellationToken);
        if (refreshToken == null) return;
        
        Context.RefreshTokens.Remove(refreshToken);
    }
    
    public async Task RemoveRefreshTokenAsync(List<string> accessTokens, CancellationToken cancellationToken = default)
    {
        var refreshTokens = await Context.RefreshTokens.Where(e => accessTokens.Contains(e.CurrentAccessToken)).ToListAsync(cancellationToken);
        if (refreshTokens.Any()) return;
        
        Context.RefreshTokens.RemoveRange(refreshTokens);
    }

    public async Task AddRoleForUserAsync(List<UserRole> userRoles, CancellationToken cancellationToken = default)
    {
        await Context.UserRoles.AddRangeAsync(userRoles, cancellationToken);
    }
    
    public void RevokeRoleForUserAsync(List<UserRole> userRoles, CancellationToken cancellationToken = default)
    {
        Context.UserRoles.RemoveRange(userRoles);
    }

    public async Task AddPermissionForRoleAsync(List<RolePermission> rolePermissions, CancellationToken cancellationToken = default)
    {
        await Context.RolePermissions.AddRangeAsync(rolePermissions, cancellationToken);
    }
    
    public void RevokePermissionForRoleAsync(List<RolePermission> rolePermissions, CancellationToken cancellationToken = default)
    {
        Context.RolePermissions.RemoveRange(rolePermissions);
    }

    public async Task AddPermissionForUserAsync(List<UserPermission> userPermissions, CancellationToken cancellationToken = default)
    {
        await Context.UserPermissions.AddRangeAsync(userPermissions, cancellationToken);
    }
    
    public void RevokePermissionForUserAsync(List<UserPermission> userPermissions, CancellationToken cancellationToken = default)
    {
        Context.UserPermissions.RemoveRange(userPermissions);
    }
    
    #region Privates

    private async Task<TokenUser?> GetTokenUserByIdentityOrOwnerIdAsync(string? username, Guid? userId, CancellationToken cancellationToken = default)
    {
        IQueryable<User> query = Context.Users.AsNoTracking()
            .Include(u => u.UserPermissions)
                .ThenInclude(up => up.Permission)
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission);
        
        if (!string.IsNullOrEmpty(username))
        {
            query =  query.Where(u => u.Username == username);
        }
        else if (userId != null) query = query.Where(u => u.Id.Equals(userId));
        else return null;
        
        var user = await query.SingleOrDefaultAsync(cancellationToken);
        if (user == null) return default!;
        
        var tokenUser = new TokenUser()
        {
            Id = user.Id,
            Username = user.Username,
            PasswordHash = user.PasswordHash,
            Salt = user.Salt,
            PhoneNumber = user.PhoneNumber,
            ConfirmedPhone = user.ConfirmedPhone,
            Email = user.Email,
            ConfirmedEmail = user.ConfirmedEmail,
            FirstName = user.FirstName,
            LastName = user.LastName,
            DateOfBirth = user.DateOfBirth
        };
        
        var roles = user.UserRoles.Select(u => u.Role).DistinctBy(r => r.Code).ToList();
        if (!roles.Any()) return tokenUser;
        
        var supperAdmin = roles.FirstOrDefault(x => x.Code.Equals(RoleConstant.SupperAdmin));
        var admin = roles.FirstOrDefault(x => x.Code.Equals(RoleConstant.Admin));
        
        if (supperAdmin != null)
        {
            tokenUser.Permission = AuthUtility.CalculateToTalPermission(Enumerable.Range(0, (int)ActionExponent.SupperAdmin + 1));
        }
        else if (admin != null)
        {
            tokenUser.Permission = AuthUtility.CalculateToTalPermission(Enumerable.Range(0, (int)ActionExponent.Admin + 1));
        }
        else
        {
            var permission = roles.SelectMany(r => r.RolePermissions).Select(rp => rp.Permission)
                .Concat(user.UserPermissions.Select(e => e.Permission))
                .DistinctBy(p => p.Code)
                .ToList();
            
            tokenUser.Permission = AuthUtility.CalculateToTalPermission(permission.Select(x => x.Exponent));
        }
        
        tokenUser.RoleNames = roles.Select(x => x.Name).ToList();
        
        return tokenUser;
    }
    
    #endregion
    
}