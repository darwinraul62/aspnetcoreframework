using System;
using System.Threading.Tasks;
using Ecubytes.Data.EntityFramework;
using Ecubytes.Extensions.Logging;

namespace Ecubytes.Extensions.Logging.Repository.EntityFramework
{
    public class LogRepository : Repository<LoggingModel>, IDataBaseLoggerPersistence
    {
        public LogRepository(LoggingDbContext context) : base(context)
        {
        }
        public async Task SaveLogAsync(LoggingModel model)
        {
            this.Add(model);
            await this.UnitOfWork.SaveChangesAsync();            
        }
    }
}
