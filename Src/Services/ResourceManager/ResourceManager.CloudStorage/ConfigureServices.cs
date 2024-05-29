using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResourceManager.CloudStorage.Providers;

namespace ResourceManager.CloudStorage;

public static class ConfigureServices
{
    public static IServiceCollection AddCloudflareStorageProviderService(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var configSection = configuration.GetSection(nameof(CloudflareSettings));
        var settings = new CloudflareSettings();
        configSection.Bind(settings);
        
        // Options support
        services.Configure<CloudflareSettings>(configSection);
        
        services.AddScoped<ICloudflareStorageProvider, CloudflareStorageProvider>();

        return services;
    }
}