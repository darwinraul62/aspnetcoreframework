using System;
using Ecubytes.Integration.Messaging;
using MassTransit;
using MassTransit.Topology;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MassTransitBusExtensions
    {
        public static void SetTopicSettings<TMessage>(this IMessageTopologyConfigurator<TMessage> messageTopoligy,
            IConfiguration configuration, string serviceName, string topicName) where TMessage : class
        {
            IMessageConfigureOptionsProvider providerOptions = new MessageConfigureOptionsProvider(configuration);
            ITopicSettings settings = providerOptions.GetTopicSettings(serviceName, topicName);

            messageTopoligy.SetEntityName(settings.ConnectionString);
        }       

        public static void UseDefaultKillSwitch(this IBusFactoryConfigurator configurator)
        {
            configurator.UseKillSwitch(options => options
                .SetActivationThreshold(10)
                .SetTripThreshold(0.15)
                .SetRestartTimeout(m: 1));
        }
    }
}
