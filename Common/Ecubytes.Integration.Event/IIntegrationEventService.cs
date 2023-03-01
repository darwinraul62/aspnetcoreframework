using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecubytes.Integration.Event
{
    public interface IIntegrationEventService
    {
        Task SaveEventAsync(IntegrationEvent evt);
        Task PublishThroughEventBusAsync(IntegrationEvent evt);
    }
}
