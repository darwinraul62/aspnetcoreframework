using System;
using Ecubytes.Integration.Event;
using Ecubytes.Integration.EventBus.Abstractions;
using Ecubytes.Integration.EventBus.Services;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EventBusExtensions
    {
        public static IServiceCollection AddDefaultEventBusService(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IEventBusSubscriptionsManager,InMemoryEventBusSubscriptionsManager>();
            services.AddSingleton<IEventBus,EventBusService>();
            services.AddDefaultEventBusAgentService(configuration);
            
            services.AddTransient<IIntegrationEventService,IntegrationEventService>();            

            return services;
        }
    }
}
