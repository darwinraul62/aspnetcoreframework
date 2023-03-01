// using System;
// using System.Threading.Tasks;
// using Microsoft.Extensions.Logging;

// namespace Ecubytes.Extensions.Logging
// {
//     public class DataBaseLogger : ILogger
//     {
//         private readonly string _name;
//         private readonly LogLevelOptions _config;
                
//         public DataBaseLogger(string name, LogLevelOptions config)
//         {
//             _name = name;
//             _config = config;
//         }

//         public IDisposable BeginScope<TState>(TState state)
//         {
//             return null;
//         }

//         public bool IsEnabled(LogLevel logLevel)
//         {
//             return logLevel >= _config.LogLevel;
//         }

//         public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
//                             Exception exception, Func<TState, Exception, string> formatter)
//         {
//             if (!IsEnabled(logLevel))
//             {
//                 return;
//             }

//             // if (_name == eventId.Name )
//             // {                
//                 LoggingModel model = new LoggingModel()
//                 {
//                     Category = _name,
//                     Date = DateTime.Now,
//                     LogLevel = (int)logLevel                   
//                 };

//                 if(exception!=null)
//                 {
//                     model.Message = exception.Message;
//                     model.Stack = exception.StackTrace;

//                     if(exception.InnerException!=null)
//                         model.Message = exception.Message + $"\nInner Exception: {exception.InnerException.Message}";
//                 }
//                 else
//                 {
//                     model.Message = state.ToString();
//                 }

//                 //var services = Ecubytes.Extensions.DependencyInjection.ServiceActivator.GetScope();
                
//                 using(var services = Ecubytes.DependencyInjection.ServiceActivator.GetScope())
//                 {
//                     try
//                     {
//                        IDataBaseLoggerPersistence repository = (IDataBaseLoggerPersistence)services.ServiceProvider.GetService(typeof(IDataBaseLoggerPersistence));
//                        await repository.SaveLogAsync(model);                       
//                     }
//                     catch(Exception)
//                     {                       
//                         //Verify Table Create
//                     }
//                 }
//                 //}

//                 //var color = Console.ForegroundColor;                
//                 // Console.ForegroundColor = _config.Color;
//                 // Console.WriteLine($"{logLevel} - {eventId.Id} " +
//                 //                   $"- {_name} - {formatter(state, exception)}");
//                 // Console.ForegroundColor = color;
                
//             //}
//         }
//     }
// }
