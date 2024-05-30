using SharedKernel.Contracts.Repositories;

namespace ResourceManager.Api.Infrastructure.Repositories.Directory;

public interface IDirectoryWriteOnlyRepository : IWriteOnlyRepository<Domain.Entities.Directory, Guid>
{
    
}