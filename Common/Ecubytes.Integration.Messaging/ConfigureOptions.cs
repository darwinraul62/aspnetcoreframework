using System;
using System.Collections.Generic;

namespace Ecubytes.Integration.Messaging
{
    public class IntegrationMessageBusOptions
    {
        public const string SectionName = "Ecubytes:Integration:Bus";
        public int ReceiveWaitTimeSeconds { get; set; } = 15;
        public int SendWaitTimeSeconds { get; set; } = 15;
        public Dictionary<string, MessageServiceItemOptions> Services { get; set; }        
    }

    public class MessageServiceItemOptions
    {
        public string Name { get; set; }
        public string AccessKey { get; set; }
        public string ConnectionString { get; set; }
        public string SecretAccessKey { get; set; }        
        public Dictionary<string, QueueOptions> Queues { get; set; }
        public Dictionary<string, TopicOptions> Topics { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
    }

    public class QueueOptions
    {
        public string ConnectionString { get; set; }
        public int WaitTimeSeconds { get; set; }
        public int VisibilityTimeoutSeconds { get; set; }
        public int MaxNumberOfMessages { get; set; } = 10;
        public bool EnableReceive { get; set; }
        public bool EnableSend { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
    }

    public class TopicOptions
    {
        public string ConnectionString { get; set; }       
        public Dictionary<string, string> Attributes { get; set; }
    }
}
