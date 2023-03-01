using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Ecubytes.Integration.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using Ecubytes.Integration.Event;
using System.Collections.Generic;

namespace Ecubytes.Integration.EventBus.Services
{
    public class IntegrationEventService : IIntegrationEventService
    {
        private readonly IEventBus eventBus;
        private readonly IIntegrationEventLogRepository integrationEventLogRepository;
        private readonly ILogger<IntegrationEventService> logger;
        private volatile bool disposedValue;

        public IntegrationEventService(
            ILogger<IntegrationEventService> logger,
            IEventBus eventBus,
            IIntegrationEventLogRepository integrationEventLogRepository)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.integrationEventLogRepository = integrationEventLogRepository;
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async Task PublishThroughEventBusAsync(object evt)
        {
            try
            {
                logger.LogInformation("----- Publishing integration event: {IntegrationEventId_published} from - ({@IntegrationEvent})", evt.EventId, evt);

                await integrationEventLogRepository.MarkEventAsInProgressAsync(evt.EventId);
                await eventBus.PublishAsync(evt);
                await integrationEventLogRepository.MarkEventAsPublishedAsync(evt.EventId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from - ({@IntegrationEvent})", evt.EventId, evt);
                await integrationEventLogRepository.MarkEventAsFailedAsync(evt.EventId);
            }
        }

        public async Task SaveEventAsync(IntegrationEvent evt)
        {
            logger.LogInformation("----- IntegrationEventService - Saving integrationEvent: {IntegrationEventId}", evt.EventId);

            await integrationEventLogRepository.SaveEventAsync(evt);
        }

        public Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventPendingToPublishAsync()
        {
            return integrationEventLogRepository.RetrieveEventLogsPendingToPublishAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    (integrationEventLogRepository as IDisposable)?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

       
    }
}
