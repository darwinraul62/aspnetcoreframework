// using System;
// using Ecubytes.Extensions.Logging;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Logging;

// namespace Microsoft.Extensions.DependencyInjection
// {
//     public static class DataBaseLoggerPersistenceExtensions
//     {
//         public static void AddDataBaseLoggerRepository(
//                                       this IServiceCollection services,
//                                       Type loggerPersistenceType)
//         {
//             services.AddTransient(typeof(IDataBaseLoggerPersistence),loggerPersistenceType);
//         }

        

//         // public static void UseDataBaseLogger(
//         //                               this IApplicationBuilder builder,
//         //                               Type loggerPersistenceType)
//         // {
//         //     builder.AddTransient(typeof(IDataBaseLoggerPersistence),loggerPersistenceType);
//         // }



//         // public static ILoggerFactory AddDataBaseLogger(
//         //                               this ILoggerFactory loggerFactory,
//         //                               DataBaseLoggerConfiguration config)
//         // {
//         //     loggerFactory.AddProvider(new DataBaseLoggerProvider(config));
//         //     return loggerFactory;
//         // }
//         // public static ILoggerFactory AddDataBaseLogger(
//         //                                   this ILoggerFactory loggerFactory,
//         //                                   IDataBaseLoggerPersistence loggerPersistence)
//         // {
//         //     var config = new DataBaseLoggerConfiguration(loggerPersistence);
//         //     return loggerFactory.AddDataBaseLogger(config);
//         // }
//         // public static ILoggerFactory AddDataBaseLogger(
//         //                                 this ILoggerFactory loggerFactory,
//         //                                 Action<DataBaseLoggerConfiguration> configure)
//         // {
//         //     var config = new DataBaseLoggerConfiguration();
//         //     configure(config);            
//         //     return loggerFactory.AddDataBaseLogger(config);
//         // }
//     }
// }
