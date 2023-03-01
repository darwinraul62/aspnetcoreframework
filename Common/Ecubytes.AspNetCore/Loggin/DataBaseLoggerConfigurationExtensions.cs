using System;
using Ecubytes.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
    // public static class DataBaseLoggerConfigurationExtensions
    // {
    //     public static IApplicationBuilder UseDataBaseLogger(
    //                                   this IApplicationBuilder builder, IConfiguration configuration
    //                                   )
    //     {
    //         Ecubytes.Extensions.Logging.LoggingOptions loggingOptions =
    //             configuration.GetSection(Ecubytes.Extensions.Logging.LoggingOptions.SectionName)
    //                 .Get<Ecubytes.Extensions.Logging.LoggingOptions>();
            

    //         //builder.ApplicationServices.GetRequiredService            
    //         ILoggerFactory loggerFactory = (ILoggerFactory)builder.ApplicationServices.GetService(typeof(ILoggerFactory));            
    //         loggerFactory.AddProvider(new DataBaseLoggerProvider(loggingOptions));
            
    //         return builder;
    //         // if (config.UseExceptionMiddleWare)
    //         //      builder.UseMiddleware<ExceptionMiddleware>();
    //     }             
    // }
}
