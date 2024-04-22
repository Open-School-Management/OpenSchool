using Identity.Application.Persistence;
using Identity.Application.Repositories.Interfaces;
using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;
using SharedKernel.Auth;
using SharedKernel.Contracts;
using SharedKernel.Contracts.Constants;
using SharedKernel.Contracts.Repositories;
using SharedKernel.Domain;
using SharedKernel.Infrastructures;
using SharedKernel.Libraries;
using SharedKernel.UnitOfWork;

namespace Identity.Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUser _currentUser;
    private readonly IServiceProvider _provider;

    public AuthRepository(
        IApplicationDbContext context, 
        ICurrentUser currentUser, 
        IServiceProvider provider)
    {
        _context = context;
        _currentUser = currentUser;
        _provider = provider;
    }
    
    public IUnitOfWork UnitOfWork => _context;
    
    public async Task<TokenUser?> GetTokenUserByIdentityAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        return await GetTokenUserByIdentityOrOwnerIdAsync(username, password, null, cancellationToken);
    }

    public async Task<TokenUser?> GetTokenUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await GetTokenUserByIdentityOrOwnerIdAsync(null, null, userId, cancellationToken);
    }
    public async Task SignOutAsync(CancellationToken cancellationToken = default)
    {
        await RemoveRefreshTokenAsync(cancellationToken);
    }

    public async Task<bool> CheckRefreshTokenAsync(string value, Guid userId, CancellationToken cancellationToken = default)
    {
        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => 
                    rt.RefreshTokenValue.Equals(value)  
                    && rt.OwnerId.Equals(userId) 
                    && rt.ExpiredDate >= DateHelper.Now, 
                cancellationToken);
        
        return refreshToken != null;
    }

    public async Task CreateOrUpdateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        var existingRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.OwnerId.Equals(refreshToken.OwnerId), cancellationToken);
        
        if (existingRefreshToken == null)
        {
            await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        }
        else
        {
            existingRefreshToken.RefreshTokenValue = refreshToken.RefreshTokenValue;
            existingRefreshToken.CurrentAccessToken = refreshToken.CurrentAccessToken;
            existingRefreshToken.ExpiredDate = refreshToken.ExpiredDate;
            
            _context.RefreshTokens.Update(existingRefreshToken);
        }
    }

    public async Task RemoveRefreshTokenAsync(CancellationToken cancellationToken = default)
    {
        var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.CurrentAccessToken.Equals(_currentUser.Context.AccessToken), cancellationToken);
        if (refreshToken == null) return;
        
        _context.RefreshTokens.Remove(refreshToken);
    }

    public async Task AddRoleForUserAsync(Guid userId, List<Guid> roleIds, CancellationToken cancellationToken = default)
    {
        await _context.UserRoles.AddRangeAsync(roleIds.Select(r => new UserRole { UserId = userId, RoleId = r }), cancellationToken);
    }
    
    public void RevokeRoleForUserAsync(Guid userId, List<UserRole> userRoles, CancellationToken cancellationToken = default)
    {
        _context.UserRoles.RemoveRange(userRoles);
    }

    public async Task AddPermissionForRoleAsync(Guid roleId, List<Guid> permissionIds, CancellationToken cancellationToken = default)
    {
        await _context.RolePermissions.AddRangeAsync(permissionIds.Select(p => new RolePermission { RoleId = roleId, PermissionId = p }), cancellationToken);
    }
    
    public void RevokePermissionForRoleAsync(Guid roleId, List<RolePermission> rolePermissions, CancellationToken cancellationToken = default)
    {
        _context.RolePermissions.RemoveRange(rolePermissions);
    }

    public async Task AddPermissionForUserAsync(Guid userId, List<Guid> permissionIds, CancellationToken cancellationToken = default)
    {
        await _context.UserPermissions.AddRangeAsync(permissionIds.Select(p => new UserPermission { UserId = userId, PermissionId = p }), cancellationToken);
    }
    
    public void RevokePermissionForUserAsync(Guid userId, List<UserPermission> userPermissions, CancellationToken cancellationToken = default)
    {
        _context.UserPermissions.RemoveRange(userPermissions);
    }

    public async Task<bool> VerifySecretKeyAsync(string secretKey, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    #region Privates

    private async Task<TokenUser?> GetTokenUserByIdentityOrOwnerIdAsync(string? username, string? password, Guid? userId, CancellationToken cancellationToken = default)
    {
        IQueryable<User> query = _context.Users.AsNoTracking()
            .Include(u => u.UserPermissions)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .ThenInclude(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission);
        
        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            var md5Password = password.ToMD5();
            query =  query.Where(u => u.Username == username && u.PasswordHash == md5Password);
        }
        else if (userId != null)
        {
            query = query.Where(u => u.Id.Equals(userId));
        }
        else
        {
            return null;
        }

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
            DateOfBirth = user.DateOfBirth,
            Gender = user.Gender,
        };
        
        var roles = user.UserRoles?.Select(u => u.Role).DistinctBy(r => r.Code).ToList();
        if (roles == null || !roles.Any()) return tokenUser;
        
        var supperAdmin = roles.FirstOrDefault(x => x.Code.Equals(RoleConstant.SupperAdmin));
        var admin = roles.FirstOrDefault(x => x.Code.Equals(RoleConstant.Admin));
        
        if (supperAdmin != null)
        {
            tokenUser.Permission = AuthUtility.CalculateToTalPermision(Enumerable.Range(0, (int)ActionExponent.SupperAdmin + 1));
        }
        else if (admin != null)
        {
            tokenUser.Permission = AuthUtility.CalculateToTalPermision(Enumerable.Range(0, (int)ActionExponent.Admin + 1));
        }
        else
        {
            var permission = roles.SelectMany(r => r.RolePermissions).Select(rp => rp.Permission)
                .Concat(user.UserPermissions.Select(e => e.Permission))
                .DistinctBy(p => p.Code)
                .ToList();
            
            tokenUser.Permission = AuthUtility.CalculateToTalPermision(permission.Select(x => x.Exponent));
        }
        
        tokenUser.RoleNames = roles.Select(x => x.Name).ToList();
        return tokenUser;
    }
    

    #endregion
}