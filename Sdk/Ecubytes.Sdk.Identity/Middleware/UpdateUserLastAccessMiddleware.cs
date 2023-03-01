using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Ecubytes.AspNetCore.WebUtilities.Api;
using Ecubytes.Identity;
using Microsoft.AspNetCore.Http;

namespace Ecubytes.Sdk.Identity.Middleware
{
    public class UserLastAccessUpdateMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ApiProfileManager apiProfileManager;
        private UsetLastAccesUpdateOptions dataOptions = new UsetLastAccesUpdateOptions();

        public UserLastAccessUpdateMiddleware(RequestDelegate next,
            Action<UsetLastAccesUpdateOptions> options,
            ApiProfileManager apiProfileManager        
        )
        {
            _next = next;
            options(dataOptions);
            this.apiProfileManager = apiProfileManager;
        }

        public async Task InvokeAsync(HttpContext context)
        {            
            if (context.User.Identity.IsAuthenticated
                && context.User.FindFirstValue(dataOptions.UserIdClaimType) != null
                && context.User.FindFirstValue(dataOptions.TenantIdClaimType) != null)
            {
                //UsetLastAccesUpdateOptions dataOptions = new UsetLastAccesUpdateOptions();
                //options(dataOptions);

                Guid userId = Guid.Parse(context.User.FindFirstValue(dataOptions.UserIdClaimType));
                Guid tenantId = Guid.Parse(context.User.FindFirstValue(dataOptions.TenantIdClaimType));

                string baseAddres = null;
                string clientSecret = null;
                string clientId = null;

                if(!string.IsNullOrWhiteSpace(dataOptions.ApiProfileName))
                {
                    var profile = apiProfileManager.Get(dataOptions.ApiProfileName);
                    baseAddres = profile.BaseAddress;
                    clientId = profile.ClientId;
                    clientSecret = profile.ClientSecret;
                }
                else
                {
                    baseAddres = dataOptions.BaseAddress;
                    clientId = dataOptions.ClientId;
                    clientSecret = dataOptions.ClientSecret;
                }

                IdentityClient identityClient = new IdentityClient(
                    baseAddres,
                    clientId,
                    clientSecret,
                    tenantId);

                await identityClient.SetLastAccessAsync(userId);
            }
            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
    }

    public class UsetLastAccesUpdateOptions
    {
        public string ApiProfileName { get; set; }
        public string BaseAddress { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string UserIdClaimType { get; set; } = "sub";
        public string TenantIdClaimType { get; set; } = "tenantId";
    }
}