using System;
using System.Threading.Tasks;

namespace Ecubytes.Extensions.Logging
{
    public interface IDataBaseLoggerPersistence
    {
        Task SaveLogAsync(LoggingModel model);
    }
}
