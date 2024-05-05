using System.IO.Compression;
using AspNetCoreRateLimit;
using Logging;
using MessageBroker.Abstractions.Extensions;
using MessageBroker.RabbitMQ;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Notification.IntegrationEvents.EventHandling;
using Notification.IntegrationEvents.Events;
using Notification.Services;
using Notification.TwilioHelper;
using SharedKernel.Configures;
using SharedKernel.Middlewares;
using ZymLabs.NSwag.FluentValidation;

namespace Notification.Extensions;

public static class Extensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(_ => configuration);
        
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        
        services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate();
        
        services.Configure<FormOptions>(x =>
        {
            x.ValueLengthLimit = int.MaxValue;
            x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            x.MultipartHeadersLengthLimit = int.MaxValue;
        });
        
        services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
        
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.MimeTypes = ResponseCompressionDefaults.MimeTypes;
            options.Providers.Add<GzipCompressionProvider>();
        });

        services.AddCors();

        services.AddCoreLocalization();

        services.AddCoreRateLimit();
        
        #region AddController + CamelCase + FluentValidation

        services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
            });

        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        
        services.AddScoped<FluentValidationSchemaProcessor>(provider =>
        {
            var validationRules = provider.GetService<IEnumerable<FluentValidationRule>>();
            var loggerFactory = provider.GetService<ILoggerFactory>();

            return new FluentValidationSchemaProcessor(provider, validationRules, loggerFactory);
        });

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);
        
        #endregion
        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        
        services.AddSwaggerGen(c =>
        {
            // Configure Swagger to use the JWT bearer authentication scheme
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };
            c.AddSecurityDefinition("Bearer", securityScheme);
    
            // Make Swagger require a JWT token to access the endpoints
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    securityScheme,
                    new string[] {}
                }
            });
        });
        
        return services;
    }
    
    public static void UseCoreWebApplication(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        if (!environment.IsDevelopment())
        {
            app.UseReject3P();
            app.UseDeveloperExceptionPage();
        }
        
        if (environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseCoreLocalization();
        app.UseCoreHandlerException();
        app.UseIpRateLimiting();
        app.UseForwardedHeaders();
        // app.UseHttpsRedirection();
        app.UseCoreUnauthorized();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
    
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRabbitMqEventBus(configuration)
            .AddSubscription<SendOtpSmsIntegrationEvent, SendOtpSmsIntegrationEventHandler>();

        services.AddTwilio(configuration);

        services.AddSingleton<ISendSmsService, SendSmsService>();
        
        return services;
    }
    
    
    public static WebApplicationBuilder AddCoreWebApplication(this WebApplicationBuilder builder)
    {
        // add app settings
        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName }.json", optional: true, reloadOnChange: true);
        
        // add environment variables
        builder.Configuration.AddEnvironmentVariables();

        builder.Host.UseCoreSerilog();
        
        return builder;
    }

}