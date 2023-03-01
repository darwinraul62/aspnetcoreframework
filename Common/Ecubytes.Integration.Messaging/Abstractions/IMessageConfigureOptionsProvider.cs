using System;
using System.Collections.Generic;

namespace Ecubytes.Integration.Messaging
{
    public interface IMessageConfigureOptionsProvider
    {
        IntegrationMessageBusOptions GetBaseOptions();
        IMessageServiceSettings GetServiceSettings(string serviceName);
        IQueueSettings GetQueueSettings(string serviceName, string queueName);
        ITopicSettings GetTopicSettings(string serviceName, string topicName);
        IEnumerable<IQueueSettings> GetAll();
    }
}
