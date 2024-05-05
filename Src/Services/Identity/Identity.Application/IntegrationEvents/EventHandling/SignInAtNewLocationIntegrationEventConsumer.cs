using Identity.Application.IntegrationEvents.Events;
using MessageBroker.Abstractions;
using Microsoft.Extensions.Logging;

namespace Identity.Application.IntegrationEvents.EventHandling;

public class SignInAtNewLocationIntegrationEventHandler : IIntegrationEventHandler<SignInAtNewLocationIntegrationEvent>
{
    private readonly ILogger<SignInAtNewLocationIntegrationEventHandler> _logger;
    public SignInAtNewLocationIntegrationEventHandler(ILogger<SignInAtNewLocationIntegrationEventHandler> logger)
    {
        _logger = logger;
    }
    
    public async Task HandleAsync(SignInAtNewLocationIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);
    }
}