using System;

namespace Ecubytes.AspNetCore.DataProtection
{
    public class DataProtectionSharedCookiesOptions
    {             
        public const string SectionName = "Ecubytes:SharedCookies";
           
        public string PersistKeysPath { get; set; }
        public string SharedApplicationName { get; set; }
    }
}
