using MediatR;
using ResourceManager.Api.Apis.Base;

namespace ResourceManager.Api.Apis.Directory;

public class DirectoryService(IMediator mediator, ILogger<DirectoryService> logger) : BaseService<DirectoryService>(mediator, logger)
{
    
}