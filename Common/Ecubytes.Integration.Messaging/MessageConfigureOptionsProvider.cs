using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Ecubytes.Integration.Messaging
{
    public class MessageConfigureOptionsProvider : IMessageConfigureOptionsProvider
    {
        private readonly IConfiguration configuration;
        public MessageConfigureOptionsProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IntegrationMessageBusOptions GetBaseOptions()
        {
            IntegrationMessageBusOptions options = configuration.GetSection(IntegrationMessageBusOptions.SectionName).
                Get<IntegrationMessageBusOptions>();

            return options;
        }

        public IMessageServiceSettings GetServiceSettings(string serviceName)
        {
            IntegrationMessageBusOptions options = GetBaseOptions();
            var serviceOptions = options.Services.FirstOrDefault(p => p.Key == serviceName);

            IMessageServiceSettings serviceSettings = null;

            if (serviceOptions.Value != null)
            {
                serviceSettings = new MessageServiceSettings()
                {
                    AccessKey = serviceOptions.Value.AccessKey,
                    ConnectionString = serviceOptions.Value.ConnectionString,
                    SecretAccessKey = serviceOptions.Value.SecretAccessKey,
                    ServiceName = serviceName,
                    Attributes = serviceOptions.Value.Attributes
                };
            }

            return serviceSettings;
        }

        public IQueueSettings GetQueueSettings(string serviceName, string queueName)
        {
            IntegrationMessageBusOptions options = configuration.GetSection(IntegrationMessageBusOptions.SectionName).
                Get<IntegrationMessageBusOptions>();

            MessageServiceItemOptions serviceOptions = options.Services[serviceName];
            QueueOptions queueOptions = serviceOptions.Queues[queueName];

            return GetDataModel(serviceOptions, queueOptions, queueName);
        }

        public ITopicSettings GetTopicSettings(string serviceName, string topicName)
        {
            IntegrationMessageBusOptions options = configuration.GetSection(IntegrationMessageBusOptions.SectionName).
                Get<IntegrationMessageBusOptions>();

            MessageServiceItemOptions serviceOptions = options.Services[serviceName];
            TopicOptions topicOptions = serviceOptions.Topics[topicName];

            return GetDataModel(serviceOptions, topicOptions, topicName);
        }        

        public IEnumerable<IQueueSettings> GetAll()
        {
            IntegrationMessageBusOptions options = configuration.GetSection(IntegrationMessageBusOptions.SectionName).
                Get<IntegrationMessageBusOptions>();

            List<IQueueSettings> result = new List<IQueueSettings>();

            foreach (var serviceOptions in options.Services)
            {
                foreach (var queueOptions in serviceOptions.Value.Queues)
                {
                    IQueueSettings queueConfigureOption = GetDataModel(serviceOptions.Value,
                        queueOptions.Value,
                        queueOptions.Key);

                    result.Add(queueConfigureOption);
                }
            }

            return result;
        }

        private IQueueSettings GetDataModel(MessageServiceItemOptions serviceOptions, QueueOptions queueOptions, string queueName)
        {
            QueueSettings optionsResult = new QueueSettings();

            optionsResult.ServiceName = serviceOptions.Name;
            optionsResult.AccessKey = serviceOptions.AccessKey;
            optionsResult.SecretAccessKey = serviceOptions.SecretAccessKey;
            optionsResult.ServiceAttributes = serviceOptions.Attributes;

            optionsResult.QueueName = queueName;
            optionsResult.ConnectionString = queueOptions.ConnectionString;
            optionsResult.QueueAttributes = queueOptions.Attributes;

            optionsResult.WaitTimeSeconds = queueOptions.WaitTimeSeconds;
            optionsResult.VisibilityTimeoutSeconds = queueOptions.VisibilityTimeoutSeconds;
            optionsResult.MaxNumberOfMessages = queueOptions.MaxNumberOfMessages;
            optionsResult.EnableReceive = queueOptions.EnableReceive;
            optionsResult.EnableSend = queueOptions.EnableSend;

            return optionsResult;
        }

        private ITopicSettings GetDataModel(MessageServiceItemOptions serviceOptions, 
            TopicOptions topicOptions, string topicName)
        {
            TopicSettings optionsResult = new TopicSettings();

            optionsResult.ServiceName = serviceOptions.Name;
            optionsResult.AccessKey = serviceOptions.AccessKey;
            optionsResult.SecretAccessKey = serviceOptions.SecretAccessKey;
            optionsResult.ServiceAttributes = serviceOptions.Attributes;

            optionsResult.TopicName = topicName;
            optionsResult.ConnectionString = topicOptions.ConnectionString;
            optionsResult.TopicAttributes = topicOptions.Attributes;
          
            return optionsResult;
        }
    }
}
