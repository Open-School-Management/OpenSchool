using AspNetCoreRateLimit;
using Logging;
using SharedKernel.Configures;
using SharedKernel.Middlewares;

namespace ResourceManager.Api.Extensions;

public  static partial class Extensions
{
    public static WebApplicationBuilder ConfigureCoreServices(this WebApplicationBuilder builder)
    {
        // Add app settings
        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName }.json", optional: true, reloadOnChange: true);
        
        // add environment variables
        builder.Configuration.AddEnvironmentVariables();

        builder.Host.UseCoreSerilog();

        return builder;
    }

    public static void UseCoreWebApplication(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        if (!environment.IsDevelopment())
        {
            app.UseReject3P();
        }
        
        app.UseCoreLocalization();
        app.UseCoreHandlerException();
        app.UseIpRateLimiting();
        app.UseForwardedHeaders();
        // app.UseHttpsRedirection();
        app.UseCoreUnauthorized();
        app.UseAuthentication();
        app.UseAuthorization();
    }
}