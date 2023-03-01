using System;
using Ecubytes.AspNetCore.Mvc.ModelBinders;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DefaultBindersExtensions
    {
        public static IServiceCollection AddDefaultMvcSettings(this IServiceCollection services)
        {
            services.Configure<Microsoft.AspNetCore.Mvc.MvcOptions>(_options =>
            {
				// Should be inserted into first position because there is a generic binder which could end up resolving/binding model incorrectly.
                _options.ModelBinderProviders.Insert(0, new ModelBinderProvider(new QueryRequestModelBinder()));
            });

            return services;
        }
    }
}
