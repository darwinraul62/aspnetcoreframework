using System;

namespace Ecubytes.Integration.Queue
{
    public class ReceiveMessageOptions
    {
        public string ServiceName { get; set; }
        public string QueueName { get; set; }
    }
}
