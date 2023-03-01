using System;
using System.Linq;
using System.Threading.Tasks;
using Ecubytes.Data.EntityFramework;
using Ecubytes.Identity.User.Data.Models;
using Ecubytes.Identity.User.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ecubytes.Identity.User.Infrastructure.Repositories
{
    public class UserLoginRepository : Repository<UserLogin>, IUserLoginRepository
    {
        public UserLoginRepository(UserDbContext context) : base(context)
        {
        }

        public Task<long> CountAsync(Guid tenantId, DateTime dateFrom, DateTime dateTo, Guid? userId = null)
        {
            var query = dbSet.Where(p => p.User.TenantId == tenantId &&
                p.LoginDate >= dateFrom && p.LoginDate <= dateTo);

            if (userId.HasValue)
                query = query.Where(p => p.UserId == userId);

            return query.LongCountAsync();
        }
    }
}
