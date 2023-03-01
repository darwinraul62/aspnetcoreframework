using System;
using System.Collections.Generic;

namespace Ecubytes.Integration.Queue
{
    public class ReceiveMessageResponse
    {
        public ReceiveMessageStatus Status { get; set; }
        public List<MessageEntry> Messages { get; set; } = new List<MessageEntry>();
    }

    public class MessageEntry
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public string ReceiptHandle { get; set; }
    }

    public enum ReceiveMessageStatus
    {
        Ok,
        Error
    }
}
