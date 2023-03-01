using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Ecubytes.Integration.Event.Models
{
    public class IntegrationEventLogEntry
    {
        private IntegrationEventLogEntry() { }
        public IntegrationEventLogEntry(IntegrationEvent @event, Guid transactionId)
        {
            EventId = @event.EventId;
            CreationTime = @event.EventCreationDate;
            EventTypeName = @event.GetType().FullName;
            Content = System.Text.Json.JsonSerializer.Serialize(@event);
            State = EventState.NotPublished;
            TimesSent = 0;
            TransactionId = transactionId.ToString();
        }
        public Guid EventId { get; private set; }
        public string EventTypeName { get; private set; }
        [NotMapped]
        public string EventTypeShortName => EventTypeName.Split('.')?.Last();
        [NotMapped]
        public IntegrationEvent IntegrationEvent { get; private set; }
        public EventState State { get; set; }
        public int TimesSent { get; set; }
        public DateTime CreationTime { get; private set; }
        public string Content { get; private set; }
        public string TransactionId { get; private set; }

        public IntegrationEventLogEntry DeserializeJsonContent(Type type)
        {
            IntegrationEvent = System.Text.Json.JsonSerializer.Deserialize(Content, type) as IntegrationEvent;
            return this;
        }
    }
}
