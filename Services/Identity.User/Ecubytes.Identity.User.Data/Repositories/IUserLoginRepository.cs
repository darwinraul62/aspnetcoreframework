using System;
using System.Threading.Tasks;
using Ecubytes.Data;
using Ecubytes.Identity.User.Data.Models;

namespace Ecubytes.Identity.User.Data.Repositories
{
    public interface IUserLoginRepository : IRepository<UserLogin>
    {
        Task<long> CountAsync(Guid tenantId, DateTime dateTo, DateTime dateFrom, Guid? userId = null);        
    }
}
