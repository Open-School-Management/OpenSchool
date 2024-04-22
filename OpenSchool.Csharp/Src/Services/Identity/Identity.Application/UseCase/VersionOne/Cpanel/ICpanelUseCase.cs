using Identity.Application.DTOs.Auth;
using Identity.Application.DTOs.Cpanel;
using SharedKernel.Contracts;

namespace Identity.Application.UseCase.VersionOne;

public interface ICpanelUseCase
{
    Task<CpanelUserDto> CreateUserAsync(CpanelUserCreateDto cpanelUserCreateDto, CancellationToken cancellationToken = default);
    
    Task<CpanelUserDto> UpdateUserAsync(Guid userId, CpanelUserUpdateDto cpanelUserUpdateDto, CancellationToken cancellationToken = default);
    
    Task<bool> DeleteAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<CpanelRoleDto> CreateRoleAsync(CpanelRoleCreateDto cpanelRoleCreateDto, CancellationToken cancellationToken = default);

    Task<CpanelRoleDto> UpdateRoleAsync(CpanelRoleUpdateDto cpanelRoleUpdateDto, CancellationToken cancellationToken = default);
    
    Task<bool> DeleteRoleAsync(CpanelRoleUpdateDto cpanelRoleUpdateDto, CancellationToken cancellationToken = default);
    
    Task<UserRoleDto> AssignOrUpdateRoleForUserAsync(Guid userId, List<Guid> roleIds, CancellationToken cancellationToken = default);

    Task<UserPermissionDto> AssignOrUpdatePermissionForUserAsync(Guid userId, List<Guid> permissionIds, CancellationToken cancellationToken = default);

    Task<RolePermissionDto> AssignOrUpdatePermissionForRoleAsync(Guid roleId, List<Guid> permissionIds, CancellationToken cancellationToken = default);
    
    Task<IPagedList<CpanelUserDto>> GetUserPagingAsync(PagingRequest request, CancellationToken cancellationToken = default);
    
    Task<IPagedList<CpanelRoleDto>> GetRolePagingAsync(PagingRequest request, CancellationToken cancellationToken = default);

    Task<IPagedList<CpanelRoleDto>> GetPermissionPagingAsync(PagingRequest request, CancellationToken cancellationToken = default);
    
    Task<CpanelUserDto> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<CpanelRoleDto> GetRoleByIdAsync(Guid roleId, CancellationToken cancellationToken = default);
}