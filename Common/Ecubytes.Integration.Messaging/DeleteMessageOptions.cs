using System;
using System.Collections.Generic;

namespace Ecubytes.Integration.Messaging
{
    public class DeleteMessageOptions
    {
        public string ServiceName { get; set; }
        public string QueueName { get; set; }
        public List<MessageEntry> Messages { get; set; } = new List<MessageEntry>();
    }
}
