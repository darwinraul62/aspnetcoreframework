using System;
using Ecubytes.Integration.EventBus.HostedServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EventBusAgentExtensions
    {
        public static void AddDefaultEventBusAgentService(this IServiceCollection services,
            IConfiguration configuration)
        {            
            services.AddHostedService<EventBusReceiverAgent>();   
            services.AddHostedService<EventBusSenderAgent>();            
        }   
    }
}
