using System;
using System.Linq;

namespace Ecubytes.Integration.Event
{
    public enum EventState
    {
        NotPublished = 0,
        InProgress = 1,
        Published = 2,
        PublishedFailed = 3
    }
}
