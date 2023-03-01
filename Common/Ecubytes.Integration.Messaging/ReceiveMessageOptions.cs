using System;

namespace Ecubytes.Integration.Messaging
{
    public class ReceiveMessageOptions
    {
        public string ServiceName { get; set; }
        public string QueueName { get; set; }
    }
}
