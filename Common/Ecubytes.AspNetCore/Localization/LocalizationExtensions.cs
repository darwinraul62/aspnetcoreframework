using System;
using System.Collections.Generic;
using Ecubytes.AspNetCore.Localization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LocalizationExtensions
    {
        //
        // Summary:
        // Localization configuration is added
        // Define Localization Settings in appsettings.json
        public static IServiceCollection AddDefaultLocalization(this IServiceCollection services,
            IConfiguration configuration)
        {
            LocalizationOptions options = configuration.GetSection(LocalizationOptions.SectionName).
                Get<LocalizationOptions>();

            services.AddDefaultLocalization(options);

            return services;
        }

        //
        // Summary:
        // Localization configuration is added
        public static IServiceCollection AddDefaultLocalization(this IServiceCollection services,
            Action<LocalizationOptions> options)
        {
            LocalizationOptions dataOptions = new LocalizationOptions();
            options(dataOptions);

            services.AddDefaultLocalization(dataOptions);

            return services;
        }

        private static IServiceCollection AddDefaultLocalization(this IServiceCollection services,
            LocalizationOptions options)
        {
            services.AddLocalization(opt => opt.ResourcesPath = options?.ResourcesPath);

            services.Configure<RequestLocalizationOptions>(
                opts =>
            {
                if (options != null)
                {
                    if (options.SupportedCultures != null)
                        opts.SupportedCultures = options.SupportedCultures.Select(p => new CultureInfo(p)).ToList();
                    if (options.SupportedUICultures != null)
                        opts.SupportedUICultures = options.SupportedUICultures.Select(p => new CultureInfo(p)).ToList();

                    opts.DefaultRequestCulture = new RequestCulture(options.DefaultCulture, options.DefaultUICulture);

                    opts.ApplyCurrentCultureToResponseHeaders = true;
                }
            });

            return services;
        }

        public static IApplicationBuilder UseDefaultLocalization(
                                              this IApplicationBuilder builder)
        {
            builder.UseRequestLocalization(
               builder.ApplicationServices.GetService<Options.IOptions<RequestLocalizationOptions>>().Value
            );
            return builder;
        }
    }
}
