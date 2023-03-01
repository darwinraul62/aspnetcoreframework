using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ecubytes.Data;
using Ecubytes.Data.Common;
using Ecubytes.Identity.User.Data.Models;

namespace Ecubytes.Identity.User.Data.Repositories
{
    public interface IUserRepository: IRepository<Models.User>
    {
        Task<QueryResult<UserInfo>> GetUsersInfoAsync(QueryRequest request, Expression<Func<UserInfo,bool>> filter = null);
        Task<int> OnlineCountAsync(Guid tenantId);     
        Task<long> RegisterCountAsync(Guid tenantId, DateTime dateFrom, DateTime dateTo);
    } 
}