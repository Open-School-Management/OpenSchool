using Identity.Application.IntegrationEvents.Services;
using Identity.Application.Persistence;
using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Services.IntegrationEvents;
using IntegrationEventLogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SharedKernel.Core;

namespace Identity.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add DbContext, DbContextSeed
        services.AddDbContext<IdentityDbContext>(options =>
            options.UseNpgsql(CoreSettings.ConnectionStrings["IdentityDb"])
                .EnableSensitiveDataLogging(true)
                .EnableDetailedErrors(true)
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
        );
        
        services.AddScoped<ApplicationDbContextSeed>();
        
        // Add the integration services that consume the DbContext
        services.AddTransient<IIntegrationEventLogService, IntegrationEventLogService<IdentityDbContext>>();

        services.AddTransient<IIdentityIntegrationEventService, IdentityIntegrationEventService>();
        
        // Add Service
        
        // Add DI Repositories
        

        return services;
    }
}