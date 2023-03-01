using System;
using Microsoft.AspNetCore.Authorization;

namespace Ecubytes.AspNetCore.Authorization
{
    public class ClaimAuthorizeAttribute : AuthorizeAttribute
    {
        const string POLICY_PREFIX = "ClaimAuthorize";

        private readonly string[] claimValues;
        private readonly string claimType;
        
        public ClaimAuthorizeAttribute(string ClaimType, params string[] Values)
        { 
            this.claimType = ClaimType;
            this.claimValues = Values;
            this.SetPolicyName();
        }

        public string[] ClaimValues
        {
            get
            {
                return claimValues;
            }
        }

        public string ClaimType
        {
            get
            {
               return claimType;
            }            
        }

        private void SetPolicyName()
        {
            this.Policy = $"{POLICY_PREFIX}~{ClaimType}~{String.Join("~",ClaimValues)}";
        }
    }
}
