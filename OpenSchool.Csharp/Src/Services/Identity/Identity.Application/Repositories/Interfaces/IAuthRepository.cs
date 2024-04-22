using Identity.Domain.Entities;
using SharedKernel.Contracts;
using SharedKernel.UnitOfWork;

namespace Identity.Application.Repositories.Interfaces;

public interface IAuthRepository
{
    IUnitOfWork UnitOfWork { get; }

    Task<TokenUser?> GetTokenUserByIdentityAsync(string username, string password, CancellationToken cancellationToken = default);

    Task<TokenUser?> GetTokenUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task SignOutAsync(CancellationToken cancellationToken = default);

    Task<bool> CheckRefreshTokenAsync(string value, Guid userId, CancellationToken cancellationToken = default);

    Task CreateOrUpdateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);

    Task RemoveRefreshTokenAsync(CancellationToken cancellationToken = default);

    Task AddRoleForUserAsync(List<UserRole> userRoles, CancellationToken cancellationToken = default);
    void RevokeRoleForUserAsync(List<UserRole> userRoles, CancellationToken cancellationToken = default);

    Task AddPermissionForRoleAsync(List<RolePermission> rolePermissions, CancellationToken cancellationToken = default);

    void RevokePermissionForRoleAsync(List<RolePermission> rolePermissions, CancellationToken cancellationToken = default);

    Task AddPermissionForUserAsync(List<UserPermission> userPermissions, CancellationToken cancellationToken = default);

    void RevokePermissionForUserAsync(List<UserPermission> userPermissions, CancellationToken cancellationToken = default);

    Task<bool> VerifySecretKeyAsync(string secretKey, CancellationToken cancellationToken = default);

}