using System;
using Ecubytes.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DbContextServiceExtensions
    {
        public static IServiceCollection AddAuthorizationDbContext(this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddDbContext<AuthorizationDbContext>(options=>
                {
                    options.UseNpgsql(configuration.GetConnectionString("AuthorizationContext"));
                    // Register the entity sets needed by OpenIddict.
                    // Note: use the generic overload if you need
                    // to replace the default OpenIddict entities.
                    options.UseOpenIddict();          
                }
            );
            
            return services;
        }
    }
}