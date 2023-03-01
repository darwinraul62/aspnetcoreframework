using System;
using Ecubytes.Identity.Login.Web.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HttpClientServiceExtensions
    {
        public static IServiceCollection AddHttpClientServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDefaultHttpHandlers(configuration);
            
            // services.AddHttpClient<ILoginService,LoginService>()
            //     .AddHttpClientDefaultTenantIdHandler()
            //     .SetHandlerLifetime(TimeSpan.FromMinutes(5));
            
            // services.AddHttpClient<IUserService,UserService>()
            //     .AddHttpClientAuthorizationHandler()
            //     .AddHttpClientDefaultTenantIdHandler()
            //     .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            return services;
        }
    }
}
