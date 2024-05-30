using ResourceManager.Api.Application.DTOs;
using SharedKernel.Contracts;
using SharedKernel.Contracts.Repositories;
using SharedKernel.EntityFrameworkCore.Paging;

namespace ResourceManager.Api.Infrastructure.Repositories.Directory;

public interface IDirectoryReadOnlyRepository : IReadOnlyRepository<Domain.Entities.Directory, Guid>
{
    Task<IPagedList<DirectoryDto>> GetPagedListAsync(Guid? directoryId, PagingRequest request, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<Domain.Entities.Directory>> GetListDirectoryByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default);
    
    Task<int> GetMaxDirectoryDuplicateNoAsync(Domain.Entities.Directory directory, CancellationToken cancellationToken = default);

    Task<Domain.Entities.Directory> GetParentDirectoryAsync(Guid parentDirectoryId, CancellationToken cancellationToken = default);

    Task<List<DirectoryDto>> GetChildrenDirectoryAsync(Guid parentDirectoryId, CancellationToken cancellationToken = default);

    Task<PathDto> GetPathAsync(Guid directoryId, CancellationToken cancellationToken = default);

    Task<IEnumerable<string>> GetChildrenNodeIdAsync(Guid rootId, CancellationToken cancellationToken = default);

    Task<Domain.Entities.Directory> FindFirstNodeLockedDirectoryAsync(Guid sourceId, CancellationToken cancellationToken = default);

    Task<DirectoryRelationshipType> GetRelationshipBetweenTwoDirectoriesAsync(Guid leftId, Guid rightId, CancellationToken cancellationToken = default);

    Task<bool> IsLockedDirectoryAsync(Guid directoryId, CancellationToken cancellationToken = default);

    Task<bool> HasPasswordAsync(Guid directoryId, CancellationToken cancellationToken = default);

    Task<DirectoryPropertyDto> GetPropertiesAsync(Guid id, CancellationToken cancellationToken = default);
}