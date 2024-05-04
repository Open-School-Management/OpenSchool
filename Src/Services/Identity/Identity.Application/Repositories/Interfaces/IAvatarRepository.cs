using Identity.Application.DTOs.User;
using Identity.Application.Persistence;

namespace Identity.Application.Repositories.Interfaces;

public interface IAvatarRepository : IWriteOnlyRepository<Avatar, Guid, IIdentityDbContext>
{
    Task<AvatarDto> GetAvatarAsync(CancellationToken cancellationToken = default);
    
    Task SetAvatarAsync(string fileName, CancellationToken cancellationToken = default);

    Task RemoveAvatarAsync(Guid ownerId, CancellationToken cancellationToken = default);
}