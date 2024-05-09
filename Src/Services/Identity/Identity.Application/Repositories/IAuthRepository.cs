using Core.Security.Models;
using SharedKernel.Domain;

namespace Identity.Application.Repositories;

public interface IAuthRepository : IWriteOnlyRepository<EntityBase, Guid>
{
    IUnitOfWork UnitOfWork { get; }

    Task<TokenUser?> GetTokenUserByIdentityAsync(string username, CancellationToken cancellationToken = default);

    Task<TokenUser?> GetTokenUserByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
    
    Task<bool> CheckRefreshTokenAsync(string value, Guid userId, CancellationToken cancellationToken = default);

    Task CreateOrUpdateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);

    Task RemoveRefreshTokenAsync(CancellationToken cancellationToken = default);
    
    Task RemoveRefreshTokenAsync(List<string> accessTokens, CancellationToken cancellationToken = default);
    
}