using System;
using Microsoft.Extensions.DependencyInjection;

namespace Ecubytes.DependencyInjection
{
    /// <summary>
    /// Add static service resolver to use when dependencies injection is not available
    /// </summary>
    public class ServiceActivator
    {
        internal static IServiceProvider _serviceProvider = null;
 
        /// <summary>
        /// Configure ServiceActivator with full serviceProvider
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void Configure(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
 
        /// <summary>
        /// Create a scope where use this ServiceActivator
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static IServiceScope GetScope(IServiceProvider serviceProvider = null)
        {            
            var provider = serviceProvider ?? _serviceProvider;
            return provider?
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
        }

        /// <summary>
        /// Create a scope where use this ServiceActivator
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static bool CheckService(Type serviceToCheck, IServiceProvider serviceProvider = null)
        {            
           using(var service = GetScope(serviceProvider))
           {
               return service.ServiceProvider.GetService(serviceToCheck) != null;
           }
        }
    }
}
