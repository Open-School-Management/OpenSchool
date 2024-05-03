using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly.Retry;

namespace MessageBroker.RabbitMQ;

public static class RabbitMqDependencyInjectionExtensions
{
    private const string SectionName = "RabbitMQClientSettings";
    public static IEventBusBuilder AddRabbitMqEventBus(this IServiceCollection services, 
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var configSection = configuration.GetSection("RabbitMQClientSettings");
        var settings = new RabbitMQClientSettings();
        configSection.Bind(settings);
        
        IConnectionFactory CreateConnectionFactory(IServiceProvider sp)
        {
            var factory = new ConnectionFactory();
            
            // the connection string from settings should win over the one from the ConnectionFactory section
            var connectionString = settings.ConnectionString;
            if (!string.IsNullOrEmpty(connectionString))
            {
                factory.Uri = new(connectionString);
            }

            factory.DispatchConsumersAsync = true;

            return factory;
        }
        
        services.AddSingleton<IConnectionFactory>(CreateConnectionFactory);
        services.AddSingleton<IConnection>(sp => CreateConnection(sp.GetRequiredService<IConnectionFactory>(), settings.MaxConnectRetryCount));
        
        // Options support
        services.Configure<RabbitMQClientSettings>(configSection);

        // Abstractions on top of the core client API
        services.AddSingleton<IEventBus, RabbitMQEventBus>();
        // Start consuming messages as soon as the application starts
        services.AddSingleton<IHostedService>(sp => (RabbitMQEventBus)sp.GetRequiredService<IEventBus>());

        return new EventBusBuilder(services);
    }

    private class EventBusBuilder(IServiceCollection services) : IEventBusBuilder
    {
        public IServiceCollection Services => services;
    }
    
    private static IConnection CreateConnection(IConnectionFactory factory, int retryCount)
    {
        var resiliencePipelineBuilder = new ResiliencePipelineBuilder();
        if (retryCount > 0)
        {
            resiliencePipelineBuilder.AddRetry(new RetryStrategyOptions
            {
                ShouldHandle = static args => args.Outcome is { Exception: SocketException or BrokerUnreachableException }
                    ? PredicateResult.True()
                    : PredicateResult.False(),
                BackoffType = DelayBackoffType.Exponential,
                MaxRetryAttempts = retryCount,
                Delay = TimeSpan.FromSeconds(1),
            });
        }
        var resiliencePipeline = resiliencePipelineBuilder.Build();
        

        return resiliencePipeline.Execute(static factory =>  factory.CreateConnection(), factory);
    }
}