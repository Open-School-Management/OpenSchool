using Asp.Versioning;
using Microsoft.OpenApi.Models;

namespace ResourceManager.Api.Extensions;

public static partial class Extensions
{
    public static IApplicationBuilder UseDefaultOpenApi(this WebApplication app)
    {
        
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
        }

        return app;
    }
    
    public static IServiceCollection AddDefaultOpenApi(this IServiceCollection services, IApiVersioningBuilder? apiVersioning = default)
    {
        services.AddEndpointsApiExplorer();
        
        if (apiVersioning is not null)
        {
            apiVersioning.AddApiExplorer(options => options.GroupNameFormat = "'v'VVV");
        
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ResourceManager.Api V1", Version = "v1" });
            
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
        }

        return services;
    }
}