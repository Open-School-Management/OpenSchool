using MessageBroker.Abstractions;
using Notification.IntegrationEvents.Events;

namespace Notification.IntegrationEvents.EventHandling;

public class SendOtpSmsIntegrationEventHandler : IIntegrationEventHandler<SendOtpSmsIntegrationEvent>
{
    private readonly ILogger<SendOtpSmsIntegrationEventHandler> _logger;
    public SendOtpSmsIntegrationEventHandler(ILogger<SendOtpSmsIntegrationEventHandler> logger)
    {
        _logger = logger;
    }
    public async Task HandleAsync(SendOtpSmsIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);
    }
}