using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;

namespace Ecubytes.AspNetCore.Authorization.Providers
{
    public class AuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        const string POLICY_CLAIM_PREFIX = "ClaimAuthorize";
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }
        private ILogger logger;
        public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options, 
            ILogger<AuthorizationPolicyProvider> logger)
        {
            // ASP.NET Core only uses one authorization policy provider, so if the custom implementation
            // doesn't handle all policies (including default policies, etc.) it should fall back to an
            // alternate provider.
            //
            // In this sample, a default authorization policy provider (constructed with options from the 
            // dependency injection container) is used if this custom provider isn't able to handle a given
            // policy name.
            //
            // If a custom policy provider is able to handle all expected policy names then, of course, this
            // fallback pattern is unnecessary.
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
            this.logger = logger;
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();
        
        public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetFallbackPolicyAsync();

        // Policies are looked up by string name, so expect 'parameters' (like age)
        // to be embedded in the policy names. This is abstracted away from developers
        // by the more strongly-typed attributes derived from AuthorizeAttribute
        // (like [MinimumAgeAuthorize] in this sample)
        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            logger.LogDebug("Custom Policy Provider GetPolicyAsync");

            if (policyName.StartsWith(POLICY_CLAIM_PREFIX, StringComparison.OrdinalIgnoreCase))
            {
                logger.LogDebug($"Construct Policy Begin {policyName}");
                
                string[] values = policyName.Split('~');
                
                string claimType = values[1];       
                logger.LogDebug($"ClaimType {claimType}");

                string[] allowedValues = values.Skip(2).ToArray();
                
                logger.LogDebug($"AllowedValues {allowedValues}");

                var policy = new AuthorizationPolicyBuilder();
                policy.RequireClaimByAnyAllowedValues(claimType,allowedValues);
                
                logger.LogDebug($"Construct Policy End {policyName}");                
                return Task.FromResult(policy.Build());
            }

            // If the policy name doesn't match the format expected by this policy provider,
            // try the fallback provider. If no fallback provider is used, this would return 
            // Task.FromResult<AuthorizationPolicy>(null) instead.
            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
