using Core.Mailing.Abstractions;
using Core.Mailing.MailKit;
using Core.Mailing.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Mailing;

public static class MailingDependencyInjection
{
    public static IServiceCollection AddRabbitMqEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        // Options support
        services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));

        services.AddSingleton<IMailService, MailKitMailService>();

        return services;
    }
}