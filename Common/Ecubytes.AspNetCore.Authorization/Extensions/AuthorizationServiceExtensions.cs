using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AuthorizationServiceExtensions
    {
        public static IServiceCollection AddDefaultAuthorizationPolicyProvider(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationPolicyProvider, Ecubytes.AspNetCore.Authorization.Providers.AuthorizationPolicyProvider>();
            return services;
        }        
    }
}
