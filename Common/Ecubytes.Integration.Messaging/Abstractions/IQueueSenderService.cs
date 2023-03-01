using System;
using System.Threading.Tasks;

namespace Ecubytes.Integration.Messaging
{
    public interface IQueueSenderService
    {
        Task<SendMessageResponse> SendMessageAsync(Action<SendMessageOptions> options);  
        Task<SendBatchMessagesResponse> SendBatchMessagesAsync(Action<SendBatchMessagesOptions> options);      
    }
}
