using System;
using Ecubytes.Integration.Event.Repository.EntityFramework;
using Ecubytes.Integration.Event.Repository.EntityFramework.Npgsql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IntegrationEventRepositoryProviderExtensions
    {
        public static void AddIntegrationEventDbContext<T>(this IServiceCollection serviceCollection) where T : DbContext
        {
            serviceCollection.AddIntegrationEventLogRepository();

            serviceCollection.AddTransient<IIntegrationEventRepositoryConnectionProvider,
                IntegrationEventRepositoryConnectionProvider<T>>();
        }   
    }
}
