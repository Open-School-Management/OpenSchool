using SharedKernel.Contracts;
using SharedKernel.Domain;

namespace Identity.Application.IntegrationEvents.Events;

public class SignInAtNewLocationIntegrationEvent : IntegrationEvent
{
    public TokenUser TokenUser { get; set; }

    public string RequestId { get; set; }
    
    public SignInAtNewLocationIntegrationEvent(Guid eventId, DateTime timestamp, object body) : base(eventId, timestamp, body)
    {
    }
}