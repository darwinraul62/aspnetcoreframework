using System;
using Ecubytes.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DataProtectionExtensions
    {
        //
        // Summary:
        // DataProtection configuration is added
        // Set the directory where the keyring will be stored
        // to be shared by other applications that have in common
        // the application name that is set below
        // Required for Cookies shared     
        public static IServiceCollection AddDataProtectionSharedCookies(this IServiceCollection services, 
            IConfiguration configuration)
        {
            DataProtectionSharedCookiesOptions options = configuration.GetSection(DataProtectionSharedCookiesOptions.SectionName).
                Get<DataProtectionSharedCookiesOptions>();
                
            services.AddDataProtectionSharedCookies(options);

            return services;
        }

        //
        // Summary:
        // DataProtection configuration is added
        // Set the directory where the keyring will be stored
        // to be shared by other applications that have in common
        // the application name that is set below
        // Required for Cookies shared     
        public static IServiceCollection AddDataProtectionSharedCookies(this IServiceCollection services,
            Action<DataProtectionSharedCookiesOptions> options)
        {
            DataProtectionSharedCookiesOptions dataOptions = new DataProtectionSharedCookiesOptions();
            options(dataOptions);

            services.AddDataProtectionSharedCookies(dataOptions);
            
            return services;
        }

        //
        // Summary:
        // DataProtection configuration is added
        // Set the directory where the keyring will be stored
        // to be shared by other applications that have in common
        // the application name that is set below
        // Required for Cookies shared     
        private static IServiceCollection AddDataProtectionSharedCookies(this IServiceCollection services, 
            DataProtectionSharedCookiesOptions options)
        {
            services.AddDataProtection()
                .PersistKeysToFileSystem(new System.IO.DirectoryInfo(options.PersistKeysPath))
                .SetApplicationName(options.SharedApplicationName);

            return services;
        }
    }
}
