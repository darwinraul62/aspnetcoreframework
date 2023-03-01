using System;
using Ecubytes.Extensions.Logging.Infrastructure;
using Ecubytes.Identity.Application.Infrastructure;
using Ecubytes.Identity.Application.Infrastructure.Repositories;
using Ecubytes.Integration.Event;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationDbContextExtensions
    {
        public static IServiceCollection AddIdentityApplicationDbContext(this IServiceCollection services,
            IConfiguration configuration)
        {
            string applicationConnectionString = configuration.GetConnectionString("ApplicationDbContext");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(applicationConnectionString)
            );

            services.AddLoggingDbContext(options =>
               options.UseNpgsql(applicationConnectionString)
           );

            services.AddIntegrationEventLogRepository();

            services.AddTransient<Ecubytes.Integration.Event.Repository.EntityFramework.IIntegrationEventRepositoryConnectionProvider,
                 IntegrationEventRepositoryConnectionProvider>();

            return services;
        }

    }
}
