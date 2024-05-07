using Core.Security.Models;
using MessageBroker.Abstractions.Events;

namespace Identity.Application.IntegrationEvents.Events;

public record SignInAtNewLocationIntegrationEvent(TokenUser TokenUser) : IntegrationEvent;

