using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ecubytes.Configuration;
using Microsoft.Extensions.Options;

namespace Ecubytes.AspNetCore.Http
{
    public class HttpClientDefaultTenantIdDelegatingHandler
       : DelegatingHandler
    {
        private readonly GlobalOptions globalOptions;

        public HttpClientDefaultTenantIdDelegatingHandler(IOptions<GlobalOptions> globalOptions)
        {
            this.globalOptions = globalOptions.Value;            
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (globalOptions.DefaultTenantId != Guid.Empty)
            {
                request.Headers.Add("TenantId", globalOptions.DefaultTenantId.ToString());
            }
                     
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
