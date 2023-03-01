using System;
using System.Linq;
using Ecubytes.Identity.User.Infrastructure;
using Ecubytes.Identity.User.Infrastructure.Repositories;
using Ecubytes.Integration.Event.Repository.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class UserDbContextExtensions
    {
        public static IServiceCollection AddIdentityUserDbContext(this IServiceCollection services, 
            IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("UserDbContext");

            services.AddDbContext<UserDbContext>(options=>
                options.UseNpgsql(connectionString)
            );

            services.AddLoggingDbContext(options=>            
                options.UseNpgsql(connectionString)
            );            

            services.AddIntegrationEventLogRepository();

            services.AddTransient<IIntegrationEventRepositoryConnectionProvider,
                IntegrationEventRepositoryConnectionProvider>();

            return services;
        }
    }
}
