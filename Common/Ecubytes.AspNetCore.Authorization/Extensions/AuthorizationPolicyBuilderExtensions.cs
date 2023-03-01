using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace Microsoft.AspNetCore.Authorization
{
    public static class AuthorizationPolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder RequireClaimByAnyAllowedValues(this AuthorizationPolicyBuilder builder, string claimType, params string[] allowedValues)
        {
            return builder.RequireAssertion(context =>
                context.User
                    .Claims
                    //.Where(c => _scopeClaimTypes.Contains(c.Type))
                    .Where(c => claimType == c.Type)
                    .SelectMany(c => c.Value.Split(' '))
                    .Any(s => allowedValues.Contains(s, StringComparer.OrdinalIgnoreCase))
            );
        }

        public static AuthorizationPolicyBuilder RequireScope(this AuthorizationPolicyBuilder builder, params string[] allowedValues)
        {
            return builder.RequireClaimByAnyAllowedValues("scope",allowedValues);
        }
    }
}
