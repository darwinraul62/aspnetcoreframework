using System;
using Ecubytes.Sdk.Identity.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class UserLastAccessUpdateMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserLastAccessUpdate(
            this IApplicationBuilder builder, Action<UsetLastAccesUpdateOptions> options)
        {
            return builder.UseMiddleware<UserLastAccessUpdateMiddleware>(options);
        }
    }    
}
