using Identity.Application.DTOs.Role.Responses;
using Identity.Application.Persistence;


namespace Identity.Application.Repositories.Interfaces;

public interface IRoleRepository : IWriteOnlyRepository<Role, Guid, IIdentityDbContext>
{
    Task<RoleDto> CreateRoleAsync(Role role, CancellationToken cancellationToken = default);

    Task<RoleDto> UpdateRoleAsync(Role role, CancellationToken cancellationToken = default);

    Task<bool> DeleteRoleAsync(Guid roleId, CancellationToken cancellationToken = default);
}