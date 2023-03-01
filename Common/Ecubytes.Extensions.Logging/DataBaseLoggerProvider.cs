// using System;
// using System.Collections;
// using System.Collections.Concurrent;
// using System.Collections.Generic;
// using System.Diagnostics.CodeAnalysis;
// using Microsoft.Extensions.Logging;
// using System.Linq;

// namespace Ecubytes.Extensions.Logging
// {
//     public class DataBaseLoggerProvider : ILoggerProvider
//     {
//         private readonly LoggingOptions _config;
//         private readonly ConcurrentDictionary<string, DataBaseLogger> _loggers = 
//             new ConcurrentDictionary<string, DataBaseLogger>();

//         public DataBaseLoggerProvider(LoggingOptions config)
//         {
//             _config = config;
//         }
        
//         public ILogger CreateLogger(string categoryName)
//         {
//             LogLevel? logLevel = null; 

//             foreach(var key in _config.LogLevel.Keys.OrderByDescending(p=>p))
//             {
//                 if(categoryName.StartsWith(key))
//                 {
//                     logLevel = _config.LogLevel[key];
//                     break;
//                 }                
//             }

//             if(!logLevel.HasValue)
//             {
//                 LogLevel logDefault;
//                 if(_config.LogLevel.TryGetValue("Default", out logDefault))
//                     logLevel = logDefault;                
//                 else
//                     logLevel = LogLevel.Error;
//             }
            
//             return _loggers.GetOrAdd(categoryName, name => new DataBaseLogger(name, new LogLevelOptions()
//                 {
//                     LogLevel = logLevel.Value
//                 }));
//         }

//         public void Dispose()
//         {
//             _loggers.Clear();
//         }
//     }    
// }
