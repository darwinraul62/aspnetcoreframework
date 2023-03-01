using System;
using System.Collections.Generic;

namespace Ecubytes.Integration.Messaging
{
    public interface IQueueSettings
    {
        string ServiceName { get; }
        string QueueName { get; }
        string AccessKey { get; }
        string SecretAccessKey { get; }
        string ConnectionString { get; }
        int WaitTimeSeconds { get; }
        int VisibilityTimeoutSeconds { get; }
        int MaxNumberOfMessages { get; }
        bool EnableReceive { get; }
        bool EnableSend { get; }        
        Dictionary<string, string> QueueAttributes { get; }
        Dictionary<string, string> ServiceAttributes { get; }
    }
}
