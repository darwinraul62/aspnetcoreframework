using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SwaggerGenExtensions
    {
        public static IServiceCollection AddDefaultSwaggerGen(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddSwaggerGen(options =>
            {                
                
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Ecubytes - Identity HTTP API",
                    Version = "v1",
                    Description = "The Identuty Application Service HTTP API"                
                });

                
                
                
                // Set the comments path for the Swagger JSON and UI.
                // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // options.IncludeXmlComments(xmlPath);

                // options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                // {
                //     Type = SecuritySchemeType.OAuth2,
                //     Flows = new OpenApiOAuthFlows()
                //     {
                //         Implicit = new OpenApiOAuthFlow()
                //         {
                //             AuthorizationUrl = new Uri($"{configuration.GetValue<string>("IdentityUrlExternal")}/connect/authorize"),
                //             TokenUrl = new Uri($"{configuration.GetValue<string>("IdentityUrlExternal")}/connect/token"),
                //             Scopes = new Dictionary<string, string>()
                //             {
                //                 { "orders", "Ordering API" }
                //             }
                //         }
                //     }
                // });

                //options.OperationFilter<AuthorizeCheckOperationFilter>();
            });

           
            
            return services;
        }
    }
}
