using System;
using System.Threading.Tasks;
using Ecubytes.Extensions.MassTransit.AwsSqs;
using Ecubytes.Integration.Messaging;
using MassTransit;
using MassTransit.AmazonSqsTransport;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MassTransitAwsSqsEventBusExtensions
    {
        public static void AddHost(this IAmazonSqsBusFactoryConfigurator configurator,
            string serviceName,
            IConfiguration configuration)
        {
            IMessageConfigureOptionsProvider providerOptions = new MessageConfigureOptionsProvider(configuration);
            IMessageServiceSettings serviceSettings = providerOptions.GetServiceSettings(serviceName);

            configurator.Host(serviceSettings.Attributes[Constants.RegionAttr], h =>
            {
                h.AccessKey(serviceSettings.AccessKey);
                h.SecretKey(serviceSettings.SecretAccessKey);

                h.EnableScopedTopics();
            });
        }

         public static void ReceiveEndpoint(this IAmazonSqsBusFactoryConfigurator busConfigurator,
            string serviceName,
            string queueName, 
            IConfiguration configuration, 
            Action<IAmazonSqsReceiveEndpointConfigurator> configureEndpoint)
        {
            IMessageConfigureOptionsProvider providerOptions = new MessageConfigureOptionsProvider(configuration);
            IQueueSettings settings = providerOptions.GetQueueSettings(serviceName, queueName);
            
            busConfigurator.ReceiveEndpoint(settings.ConnectionString, configureEndpoint);
        }

        public static void Subscribe(
            this IAmazonSqsReceiveEndpointConfigurator configurator,            
            string serviceName,
            string topicName, 
            IConfiguration configuration,
            Action<ITopicSubscriptionConfigurator> callback = null)
        {
            IMessageConfigureOptionsProvider providerOptions = new MessageConfigureOptionsProvider(configuration);
            ITopicSettings settings = providerOptions.GetTopicSettings(serviceName, topicName);

            configurator.Subscribe(settings.ConnectionString, callback);
        }        
    }
}
