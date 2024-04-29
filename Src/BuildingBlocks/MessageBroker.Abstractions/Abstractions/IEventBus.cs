using MessageBroker.Abstractions.Events;

namespace MessageBroker.Abstractions.Abstractions;

public interface IEventBus
{
    Task PublishAsync(IntegrationEvent @event, CancellationToken cancellationToken = default);
}
