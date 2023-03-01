using System;
using Ecubytes.Integration.Messaging;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DefaultQueueServiceExtensions
    {
        public static IServiceCollection AddDefaultQueueConfigureProvider(this IServiceCollection services,
            IConfiguration configuration)
        {
            if(!services.Contains(ServiceDescriptor.Transient<IMessageConfigureOptionsProvider,MessageConfigureOptionsProvider>()))
                services.AddTransient<IMessageConfigureOptionsProvider,MessageConfigureOptionsProvider>();
            
            return services;
        }
    }
}
