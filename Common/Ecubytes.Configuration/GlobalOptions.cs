using System;

namespace Ecubytes.Configuration
{
    public class GlobalOptions
    {
        public const string SectionName = "Ecubytes:Global";

        public Guid DefaultTenantId { get; set; }
        public Guid DefaultApplicationId { get; set; }
    }

}
