using System;
using System.Threading.Tasks;

namespace Ecubytes.Integration.Queue
{
    public interface IQueueReceiverService
    {        
        Task<ReceiveMessageResponse> ReceiveMessagesAsync(IQueueSettings options);
        Task<ReceiveMessageResponse> ReceiveMessagesAsync(Action<ReceiveMessageOptions> options);
        Task DeleteMessageAsync(Action<DeleteMessageOptions> options);
    }
}
