using Microsoft.Extensions.DependencyInjection;

namespace MessageBroker.Abstractions.Abstractions;

public interface IEventBusBuilder
{
    public IServiceCollection Services { get; }
}