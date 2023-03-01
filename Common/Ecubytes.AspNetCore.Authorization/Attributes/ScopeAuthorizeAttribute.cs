using System;

namespace Ecubytes.AspNetCore.Authorization
{
    public class ScopeAuthorizeAttribute : ClaimAuthorizeAttribute
    {
        public ScopeAuthorizeAttribute(params string[] Values)
            :base("scope",Values) 
        {
        }
    }
}
