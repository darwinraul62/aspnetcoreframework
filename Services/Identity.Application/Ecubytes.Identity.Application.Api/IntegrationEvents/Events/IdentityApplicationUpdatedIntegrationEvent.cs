using System;
using Ecubytes.Identity.Application.Data.Models.Constants;
using Ecubytes.Integration.Event;
using Ecubytes.Integration.EventBus;

namespace Ecubytes.Identity.Application.Api.IntegrationEvents.Events
{
    [IntegrationEventPublish("AWS-DEFAULT","test.fifo")]
    public record IdentityApplicationUpdatedIntegrationEvent : IntegrationEvent
    {
        public Guid ApplicationId { get; set; }
        public string Name { get; set; }
        public byte StateId { get; set; }
    }
}
