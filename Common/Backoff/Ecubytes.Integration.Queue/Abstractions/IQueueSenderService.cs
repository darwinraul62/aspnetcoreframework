using System;
using System.Threading.Tasks;

namespace Ecubytes.Integration.Queue
{
    public interface IQueueSenderService
    {
        Task<SendMessageResponse> SendMessageAsync(Action<SendMessageOptions> options);  
        Task<Queue.SendBatchMessagesResponse> SendBatchMessagesAsync(Action<SendBatchMessagesOptions> options);      
    }
}
