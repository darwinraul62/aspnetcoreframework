using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Ecubytes.Extensions.Logging.Nlog
{
    [Target("DataBaseLogger")]
    public sealed class DataBaseLoggerTarget : AsyncTaskTarget
    {
        public DataBaseLoggerTarget()
        {
            this.IncludeEventProperties = true; // Include LogEvent Properties by default                                                
        }

        protected async override Task WriteAsyncTask(LogEventInfo logEvent, CancellationToken cancellationToken)
        {
            LoggingModel model = new LoggingModel()
            {
                Category = logEvent.LoggerName,
                Date = logEvent.TimeStamp,
                LogLevel = logEvent.Level.Name,
                Message = logEvent.Message,
            };

            if (logEvent.Exception != null)
            {
                model.Exception = logEvent.Exception.Message;
                model.Stack = logEvent.Exception.StackTrace;

                if (logEvent.Exception.InnerException != null)
                    model.InnerException = logEvent.Exception.InnerException.Message;
            }

            using (var services = Ecubytes.DependencyInjection.ServiceActivator.GetScope())
            {
                IDataBaseLoggerPersistence repository = (IDataBaseLoggerPersistence)services.ServiceProvider.GetService(typeof(IDataBaseLoggerPersistence));
                await repository.SaveLogAsync(model);
            }

        }
    }
}
