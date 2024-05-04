using IntegrationEventLogs;
using MessageBroker.Abstractions.Events;

namespace Identity.Application.IntegrationEvents.Services;

public interface IIdentityIntegrationEventService
{
    Task SaveEventAndIdentityContextChangesAsync(List<IntegrationEvent> evt, CancellationToken cancellationToken = default);

    Task PublishThroughEventBusAsync(List<IntegrationEvent> events, CancellationToken cancellationToken = default);
    
    Task PublishThroughEventBusAsync(IntegrationEvent evt, CancellationToken cancellationToken = default);
}