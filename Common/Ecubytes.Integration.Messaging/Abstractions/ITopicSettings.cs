using System;
using System.Collections.Generic;

namespace Ecubytes.Integration.Messaging
{
    public interface ITopicSettings
    {
        string ServiceName { get; }
        string TopicName { get; }
        string AccessKey { get; }
        string SecretAccessKey { get; }
        string ConnectionString { get; }
        Dictionary<string, string> TopicAttributes { get; }
        Dictionary<string, string> ServiceAttributes { get; }
    }
}
