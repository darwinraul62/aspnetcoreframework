using System;
using System.Collections.Generic;

namespace Ecubytes.Integration.Messaging
{
    public interface IMessageServiceSettings
    {
        string ServiceName { get; }
        string AccessKey { get; }
        string SecretAccessKey { get; }
        string ConnectionString { get; }
        IEnumerable<IQueueSettings> Queues { get; }
        IEnumerable<ITopicSettings> Topics { get; }
        IDictionary<string, string> Attributes { get; }
    }
}