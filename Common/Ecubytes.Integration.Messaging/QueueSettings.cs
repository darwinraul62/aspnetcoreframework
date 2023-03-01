using System;
using System.Collections.Generic;

namespace Ecubytes.Integration.Messaging
{
    internal class QueueSettings : IQueueSettings
    {
        public string ServiceName { get; set; }
        public string QueueName { get; set; }
        public string AccessKey { get; set; }
        public string SecretAccessKey { get; set; }
        public string ConnectionString { get; set; }
        public int WaitTimeSeconds { get; set; }
        public int VisibilityTimeoutSeconds { get; set; }
        public int MaxNumberOfMessages { get; set; } = 10;
        public bool EnableReceive { get; set; }
        public bool EnableSend { get; set; }        
        public Dictionary<string, string> QueueAttributes { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> ServiceAttributes { get; set; } = new Dictionary<string, string>();
    }

}
