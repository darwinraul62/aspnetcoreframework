using System;
using System.Collections.Generic;

namespace Ecubytes.AspNetCore.WebUtilities
{
    public class ApiProfileCollectionOptions : Dictionary<string,ApiProfileOptions>
    {
        public const string SectionName = "Ecubytes:ApiProfiles";        
    }

    public class ApiProfileOptions
    {
        public string BaseAddress { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
