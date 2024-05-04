using MessageBroker.Abstractions.Events;

namespace Notification.IntegrationEvents.Events;

public record SendOtpSmsIntegrationEvent(string Phone, string Otp) : IntegrationEvent;