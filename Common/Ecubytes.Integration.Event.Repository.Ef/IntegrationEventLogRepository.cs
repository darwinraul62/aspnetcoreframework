using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Ecubytes.Integration.Event;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ecubytes.Integration.Event.Repository.EntityFramework
{
    public class IntegrationEventLogRepository : IIntegrationEventLogRepository, IDisposable
    {
        private readonly IntegrationEventLogContext _integrationEventLogContext;
        protected readonly IIntegrationEventRepositoryConnectionProvider connectionProvider;
        private readonly List<Type> _eventTypes;
        private volatile bool disposedValue;

        public IntegrationEventLogRepository(IIntegrationEventRepositoryConnectionProvider connectionProvider)
        {
            // this.primaryDbContext = primaryDbContext;
            this.connectionProvider = connectionProvider ?? throw new ArgumentNullException(nameof(connectionProvider));

            // ConfigureConnection(primaryDbContext);

            _integrationEventLogContext = new IntegrationEventLogContext(connectionProvider.GetDbContextOptions());

            _eventTypes = Assembly.Load(Assembly.GetEntryAssembly().FullName)
                .GetTypes()
                .Where(t => t.Name.EndsWith(nameof(IntegrationEvent)))
                .ToList();
        }

        public async Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync(Guid? transactionId = null)
        {
            List<IntegrationEventLogEntry> list = new List<IntegrationEventLogEntry>();
            if (transactionId.HasValue)
            {
                var tid = transactionId.ToString();

                list = await _integrationEventLogContext.IntegrationEventLogs
                    .Where(e => e.TransactionId == tid && e.State == EventState.NotPublished).ToListAsync();

            }
            else
            {
                list = await _integrationEventLogContext.IntegrationEventLogs
                    .Where(e => e.State == EventState.NotPublished).ToListAsync();
            }

            if (list != null && list.Any())
            {
                return list.OrderBy(o => o.CreationTime)
                    .Select(e => e.DeserializeJsonContent(_eventTypes.Find(t => t.Name == e.EventTypeShortName)));
            }

            return new List<IntegrationEventLogEntry>();
        }

        public Task SaveEventAsync(IntegrationEvent @event)
        {
            Guid? transactionId = null;

            var dbTransaction = connectionProvider.GetCurrentSharedDbContextTransaction();
            if (dbTransaction != null)
            {
                _integrationEventLogContext.Database.UseTransaction(dbTransaction.GetDbTransaction());
                transactionId = dbTransaction.TransactionId;
            }

            if (!transactionId.HasValue)
                transactionId = Guid.NewGuid();

            var eventLogEntry = new IntegrationEventLogEntry(@event, transactionId.Value);
            _integrationEventLogContext.IntegrationEventLogs.Add(eventLogEntry);

            return _integrationEventLogContext.SaveChangesAsync();
        }

        public Task MarkEventAsPublishedAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventState.Published);
        }

        public Task MarkEventAsInProgressAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventState.InProgress);
        }

        public Task MarkEventAsFailedAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventState.PublishedFailed);
        }

        private Task UpdateEventStatus(Guid eventId, EventState status)
        {
            var eventLogEntry = _integrationEventLogContext.IntegrationEventLogs.Single(ie => ie.EventId == eventId);
            eventLogEntry.State = status;

            if (status == EventState.InProgress)
                eventLogEntry.TimesSent++;

            _integrationEventLogContext.IntegrationEventLogs.Update(eventLogEntry);

            return _integrationEventLogContext.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _integrationEventLogContext?.Dispose();
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
