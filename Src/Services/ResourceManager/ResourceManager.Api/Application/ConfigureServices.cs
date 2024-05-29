using System.Reflection;
using FluentValidation;
using MediatR;
using MessageBroker.Abstractions;
using MessageBroker.RabbitMQ;
using ResourceManager.Api.Application.Behaviors;

namespace ResourceManager.Api.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Auto Mapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        // Fluent Validator
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        // MediaR
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        
        // Behaviors
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

        // Features
        
        // Add RabbitMQ
        services.AddRabbitMqEventBus(configuration)
            .AddEventBusSubscriptions();
        
        return services;
    }
    
    private static void AddEventBusSubscriptions(this IEventBusBuilder eventBus)
    {
       
    }
}
