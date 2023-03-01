using System;
using System.Collections.Generic;

namespace Ecubytes.Integration.Queue
{
    public class SendMessageResponse
    {
        public SendMessageStatus Status { get; set; }
        public string Id { get; set; }
        public string StatusMessage { get; set; }
    }

    public class SendBatchMessagesResponse
    {
        public List<SendMessageResponse> Messages { get; set; } = new List<SendMessageResponse>();
    }

    public enum SendMessageStatus
    {
        Ok,
        Error
    }
}
