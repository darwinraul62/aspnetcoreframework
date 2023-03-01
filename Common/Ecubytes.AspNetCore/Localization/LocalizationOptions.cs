using System;
using System.Collections.Generic;

namespace Ecubytes.AspNetCore.Localization
{
    public class LocalizationOptions
    {
        public const string SectionName = "Ecubytes:Localization";
        public string DefaultCulture { get; set; }
        public string DefaultUICulture { get; set; }
        public string ResourcesPath { get; set; } = "Resources";

        public List<string> SupportedCultures { get; set; } = new List<string>();
        public List<string> SupportedUICultures { get; set; } = new List<string>();
    }

    // public class CultureInfoOptions
    // {
    //     public string Name { get; set; }
    // }
}
