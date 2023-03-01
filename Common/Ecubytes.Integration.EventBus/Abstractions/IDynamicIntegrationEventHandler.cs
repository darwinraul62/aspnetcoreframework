using System;
using System.Threading.Tasks;

namespace Ecubytes.Integration.EventBus.Abstractions
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
