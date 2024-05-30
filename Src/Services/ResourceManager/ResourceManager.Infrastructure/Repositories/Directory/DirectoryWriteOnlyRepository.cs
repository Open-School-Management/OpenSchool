using Caching.Sequence;
using ResourceManager.Infrastructure.Contexts;
using SharedKernel.Auth;
using SharedKernel.Infrastructures;

namespace ResourceManager.Infrastructure.Repositories.Directory;

public class DirectoryWriteOnlyRepository : ReadOnlyRepository<Domain.Entities.Directory, Guid, ResourceManagerDbContext>
{
    public DirectoryWriteOnlyRepository(ResourceManagerDbContext context, 
        ICurrentUser currentUser, 
        ISequenceCaching sequenceCaching
        ) : base(context, currentUser, sequenceCaching)
    {
        
    }
}