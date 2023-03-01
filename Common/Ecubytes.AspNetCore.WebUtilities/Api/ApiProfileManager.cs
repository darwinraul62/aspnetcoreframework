using System;
using Microsoft.Extensions.Options;

namespace Ecubytes.AspNetCore.WebUtilities.Api
{
    public class ApiProfileManager
    {        
        private readonly ApiProfileCollectionOptions profiles;
        public ApiProfileManager(IOptions<ApiProfileCollectionOptions> profileOptions)
        {
            this.profiles = profileOptions.Value;            
        }

        public string GetBaseAddress(string profileName)
        {
            return this.profiles[profileName].BaseAddress;
        }

        public ApiProfileOptions Get(string profileName)
        {
            return this.profiles[profileName];
        }
    }
}
