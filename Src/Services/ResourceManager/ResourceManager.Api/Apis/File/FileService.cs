using MediatR;
using ResourceManager.Api.Apis.Base;

namespace ResourceManager.Api.Apis.File;

public class FileService(IMediator mediator, ILogger<FileService> logger) : BaseService<FileService>(mediator, logger)
{
    
}