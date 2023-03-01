using System;
using System.Collections.Generic;

namespace Ecubytes.Integration.Messaging
{
    internal class MessageServiceSettings : IMessageServiceSettings
    {
        public MessageServiceSettings()
        {

        }
        public string ServiceName { get; set; }

        public string AccessKey { get; set; }

        public string SecretAccessKey { get; set; }

        public string ConnectionString { get; set; }

        public IDictionary<string, string> Attributes { get; set; }

        public IEnumerable<IQueueSettings> Queues { get; set; }

        public IEnumerable<ITopicSettings> Topics { get; set; }
    }
}
