using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Ecubytes.Integration.Event
{
    public interface IIntegrationEventLogRepository
    {
        Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync(Guid? transactionId = null);
        Task SaveEventAsync(IntegrationEvent @event);
        Task MarkEventAsPublishedAsync(Guid eventId);
        Task MarkEventAsInProgressAsync(Guid eventId);
        Task MarkEventAsFailedAsync(Guid eventId);
    }
}
