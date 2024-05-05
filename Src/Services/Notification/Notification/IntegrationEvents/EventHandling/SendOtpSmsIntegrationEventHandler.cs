using MessageBroker.Abstractions;
using Notification.IntegrationEvents.Events;
using Notification.Services;

namespace Notification.IntegrationEvents.EventHandling;

public class SendOtpSmsIntegrationEventHandler : IIntegrationEventHandler<SendOtpSmsIntegrationEvent>
{
    private readonly ISendSmsService _sendSmsService;
    private readonly ILogger<SendOtpSmsIntegrationEventHandler> _logger;
    public SendOtpSmsIntegrationEventHandler(ILogger<SendOtpSmsIntegrationEventHandler> logger, ISendSmsService sendSmsService)
    {
        _logger = logger;
        _sendSmsService = sendSmsService;
    }
    public async Task HandleAsync(SendOtpSmsIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

        await _sendSmsService.SendAsync(@event.Phone, @event.Otp, cancellationToken);
    }
}