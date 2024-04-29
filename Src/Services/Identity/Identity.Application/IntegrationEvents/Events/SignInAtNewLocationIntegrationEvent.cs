using MessageBroker.Abstractions.Events;
using SharedKernel.Contracts;

namespace Identity.Application.IntegrationEvents.Events;

public class SignInAtNewLocationIntegrationEvent : IntegrationEvent
{
    public TokenUser TokenUser { get; set; }

    public string RequestId { get; set; }
    
}