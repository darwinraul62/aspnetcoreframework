using System;
using Ecubytes.Extensions.Logging;
using Ecubytes.Extensions.Logging.Repository.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DataBaseLoggerInfrastructureEfExtensions
    {
        public static IServiceCollection AddDataBaseLoggerRepository(
                                      this IServiceCollection services)
        {
            return services.AddTransient(typeof(IDataBaseLoggerPersistence), typeof(LogRepository));
        }

        public static IServiceCollection AddLoggingDbContext(this IServiceCollection services,
           Action<DbContextOptionsBuilder> optionsAction = null,
           ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
           ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) 
        {
            services.AddDataBaseLoggerRepository();

            return services.AddDbContext<LoggingDbContext>(optionsAction,
            contextLifetime: contextLifetime,
            optionsLifetime: optionsLifetime);            
        }
    }

}
