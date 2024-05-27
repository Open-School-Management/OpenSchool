using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResourceManager.CloudStorage.Providers;

namespace ResourceManager.CloudStorage;

public static class ConfigureServices
{
    public static IServiceCollection AddCloudflareStorageProvider(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICloudflareStorageProvider, CloudflareStorageProvider>();

        return services;
    }
}