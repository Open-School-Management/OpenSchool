using Identity.Application.Persistence;

namespace Identity.Application.Repositories.Interfaces;

public interface IUserConfigRepository : IWriteOnlyRepository<UserConfig, Guid, IIdentityDbContext>
{
    Task<UserConfig> CreateOrUpdateAsync(UserConfig userConfig, CancellationToken cancellationToken);
    
    Task<UserConfig> GetConfigAsync(CancellationToken cancellationToken);
}