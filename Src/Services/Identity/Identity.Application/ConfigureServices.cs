using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Identity.Application.IntegrationEvents.EventHandling;
using Identity.Application.IntegrationEvents.Events;
using Identity.Application.IntegrationEvents.Services;
using MessageBroker.Abstractions;
using MessageBroker.Abstractions.Extensions;
using MessageBroker.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedKernel.Core;

namespace Identity.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        // Add  FluentValidator
        services.AddFluentValidation(options =>
        {
            options.DisableDataAnnotationsValidation = true;

            options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;
                
            ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        });
        
        // Add RabbitMQ
        services.AddRabbitMqEventBus(configuration)
            .AddEventBusSubscriptions();
        
        // Add UseCase
        
        
        return services;
    }
    
    private static void AddEventBusSubscriptions(this IEventBusBuilder eventBus)
    {
        eventBus.AddSubscription<SignInAtNewLocationIntegrationEvent, SignInAtNewLocationIntegrationEventHandler>();
    }
    
    
}