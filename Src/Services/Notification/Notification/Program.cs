using Caching;
using Caching.Extensions;
using Core.Security;
using Core.Security.Filters;
using Microsoft.AspNetCore.HttpOverrides;
using Notification.Extensions;
using Serilog;
using SharedKernel.Configures;
using SharedKernel.Core;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();


var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;


try
{
    builder.AddCoreWebApplication();
    
    // Core setting project
    CoreSettings.SetJWTConfig(configuration);
    CoreSettings.SetConnectionStrings(configuration);
    
    // Services
    services.AddCoreServices(configuration);
    
    services.AddCoreAuthentication(builder.Configuration);
    
    services.AddCoreCaching(builder.Configuration);
    
    services.Configure<ForwardedHeadersOptions>(o => o.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);
    
    services.AddCurrentUser();
    
    services.AddControllersWithViews(options =>
    {
        options.Filters.Add(new AccessTokenValidatorAsyncFilter());
    });
    
    services.AddApplicationServices(configuration);
    
    // Configure the HTTP request pipeline.
    var app = builder.Build();
    
    // Pipelines
    app.UseCoreCors(configuration);

    app.UseCoreWebApplication(app.Environment);
    
    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;

    Log.Fatal(ex, $"Unhandled exception: {ex.Message}");
}
finally
{
    Log.Information($"Shutdown {builder.Environment.ApplicationName} complete");
    Log.CloseAndFlush();
}