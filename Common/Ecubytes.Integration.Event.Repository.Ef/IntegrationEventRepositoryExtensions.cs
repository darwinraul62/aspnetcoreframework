using System;
using System.Linq;
using Ecubytes.Integration.Event;
using Ecubytes.Integration.Event.Repository.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IntegrationEventRepositoryExtensions
    {
        public static void AddIntegrationEventLogRepository(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IIntegrationEventLogRepository,
                IntegrationEventLogRepository>();
        }        
    }
}
