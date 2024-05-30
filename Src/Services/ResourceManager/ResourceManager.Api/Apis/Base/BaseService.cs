using MediatR;

namespace ResourceManager.Api.Apis.Base;

public abstract class BaseService<T>(IMediator mediator, ILogger<T> logger)
{
    public IMediator Mediator { get; set; } = mediator;
    public ILogger<T> Logger { get; } = logger;
}