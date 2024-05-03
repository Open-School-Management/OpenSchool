using Identity.Application.IntegrationEvents.Events;
using MessageBroker.Abstractions;

namespace Identity.Application.IntegrationEvents.EventHandling;

public class SignInAtNewLocationIntegrationEventHandler : IIntegrationEventHandler<SignInAtNewLocationIntegrationEvent>
{
    public Task HandleAsync(SignInAtNewLocationIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}