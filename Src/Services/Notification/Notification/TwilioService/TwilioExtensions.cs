namespace Notification.TwilioHelper;

public static class TwilioExtensions
{
    public static IServiceCollection AddTwilio(this IServiceCollection services, IConfiguration configuration)
    {
        // Options support
        var configSection = configuration.GetSection(nameof(TwilioSettings));
        services.Configure<TwilioSettings>(configSection);

        services.AddSingleton<ITwilioService, TwilioService>();
        
        return services;
    }
}