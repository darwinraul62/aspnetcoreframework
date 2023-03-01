using System;
using System.Collections.Generic;

namespace Ecubytes.Integration.Queue
{
    public abstract class SendBaseMessageOptions
    {
        public string ServiceName { get; set; }
        public string QueueName { get; set; }
        public string Name { get; set; }
    }

    public class SendMessageOptions : SendBaseMessageOptions
    {
        public object MessageBody { get; set; }
    }
    public class SendBatchMessagesOptions : SendBaseMessageOptions
    {
        public Dictionary<string, object> MessagesBody { get; set; } = new Dictionary<string, object>();
    }
}
