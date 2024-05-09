using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Auth;

namespace Core.Security;

public static class SecurityDependencyInjection
{
    public static IServiceCollection AddCurrentUser(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUser, CurrentUser>();
        return services;
    }
}