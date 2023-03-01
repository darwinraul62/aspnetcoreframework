using System;

namespace Microsoft.AspNetCore.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string AboluteUrl(this IUrlHelper helper, string relativeUrl)
        {
            var request = helper.ActionContext.HttpContext.Request;
            var absUrl = string.Format("{0}://{1}{2}", request.Scheme, request.Host, relativeUrl);

            return absUrl;
        }
    }
}
