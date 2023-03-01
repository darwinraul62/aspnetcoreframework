using System;
using System.Text.Json.Serialization;

namespace Ecubytes.Integration.Event
{
    public record IntegrationEvent
    {
        public Guid EventId { get; set; }        
        public DateTime EventCreationDate { get; set; }
        public IntegrationEvent()
        {
            this.EventId = Guid.NewGuid();
            this.EventCreationDate = DateTime.UtcNow;
        }

        [Newtonsoft.Json.JsonConstructor]
        public IntegrationEvent(Guid eventId, DateTime eventCreationDate)
        {
            this.EventId = eventId;           
            this.EventCreationDate = eventCreationDate;
        }
        // private IntegrationEventInfo info;
        // public IntegrationEventInfo Info
        // {
        //     get
        //     {
        //         return info ?? (info = new IntegrationEventInfo());
        //     }
        //     set
        //     {
        //         info = value;
        //     }
        // }
    }

    // public record IntegrationEventInfo
    // {                
    //     public IntegrationEventInfo()
    //     {
    //         this.Id = Guid.NewGuid();
    //         this.CreationDate = DateTime.UtcNow;
    //     }

    //     [Newtonsoft.Json.JsonConstructor]
    //     public IntegrationEventInfo(Guid Id, DateTime CreationDate)
    //     {
    //         this.Id = Id;           
    //         this.CreationDate = CreationDate;
    //     }
    //     // public string IntegrationName { get; private init; }
    //     public Guid Id { get; private init; }        
    //     public DateTime CreationDate { get; private init; }        
    // }
}
