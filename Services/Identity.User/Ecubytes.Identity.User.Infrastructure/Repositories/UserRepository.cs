using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ecubytes.Data.Common;
using Ecubytes.Data.EntityFramework;
using Ecubytes.Identity.User.Data.Models;
using Ecubytes.Identity.User.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ecubytes.Identity.User.Infrastructure.Repositories
{
    public class UserRepository : Repository<Data.Models.User>, IUserRepository
    {
        public UserRepository(UserDbContext context) : base(context)
        {
        }

        public async Task<QueryResult<UserInfo>> GetUsersInfoAsync(QueryRequest request, Expression<Func<UserInfo,bool>> filter = null)
        {
            UserDbContext db = (UserDbContext)Context;
            
            var query = db.UsersInfo.Where(filter);
            
            QueryResult<UserInfo> result = await query.GetQueryResultAsync<UserInfo>(request);

            return result;
        }

        public Task<int> OnlineCountAsync(Guid tenantId)
        {
            UserDbContext db = (UserDbContext)Context;
            
            return db.UsersInfo.Where(p=>p.TenantId == tenantId && p.Online).CountAsync();
        }
        public Task<long> RegisterCountAsync(Guid tenantId, DateTime dateFrom, DateTime dateTo)
        {
            var query = dbSet.Where(p => p.TenantId == tenantId &&
                p.RegisterDate >= dateFrom && p.RegisterDate <= dateTo);

            return query.LongCountAsync();
        }
    }
}
