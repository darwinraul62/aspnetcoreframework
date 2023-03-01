using System;
using System.Linq;
using Identity.User.IntegrationEvents;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MassTransitExtensions
    {
        public static IServiceCollection AddMassTransitEventBus(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {                                                         
                x.UsingAmazonSqs((context, cfg) =>
                {
                    //Set Default OPTIONS From Configuration
                    cfg.AddHost("AWS",configuration); 

                    cfg.Message<UserCreatedIntegrationEvent>(p=> {
                        p.SetTopicSettings(configuration,"AWS","UserCreated");                                   
                    });                                       
                });                

            });

            services.AddMassTransitHostedService(true);

            return services;
        }
    }
}
