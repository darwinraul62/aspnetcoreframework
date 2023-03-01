using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.Hosting
{
    public static class ServiceProviderExtensions
    {        
        public static IHostBuilder UseDefaultServiceProviderFactory(this IHostBuilder builder)             
        {
            return builder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        }

        public static IServiceProvider Get(
            IServiceCollection services)             
        {
            var container = new ContainerBuilder();
            container.Populate(services);

            return new AutofacServiceProvider(container.Build());            
        }
        
    }
}
