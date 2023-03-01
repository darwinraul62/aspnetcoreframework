using System;

namespace Ecubytes.Json
{
    public class JsonOptions
    {
        public JsonOptions()
        {
            
        }
        public bool UseStringEnumConverter { get; set; } = true;
        public bool UseCamelCaseNamingPolicy { get; set; } = true;
    }
}
