using System;
using Ecubytes.Configuration;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class GlobalConfigurationExtensions
    {
        public static IServiceCollection AddGlobalOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GlobalOptions>(configuration.GetSection(
                                        GlobalOptions.SectionName));
        
            return services;
        }
    }
}
