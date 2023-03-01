using System;

namespace Ecubytes.Integration.EventBus
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class IntegrationEventPublishAttribute : Attribute
    {
        public IntegrationEventPublishAttribute(string Service, string Queue)
        {
            this.Service = Service;
            this.Queue = Queue;
        }

        public string Service { get; private init; }
        public string Queue { get; private init; }
    }
}
