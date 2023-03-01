using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecubytes.Data.EntityFramework;
using Ecubytes.Identity.User.Data.Models;
using Ecubytes.Identity.User.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ecubytes.Identity.User.Infrastructure.Repositories
{
    public class UserGroupDetailRepository : Repository<UserGroupDetail>, IUserGroupDetailRepository
    {
        public UserGroupDetailRepository(UserDbContext context) : base(context)
        {
        }

        public Task<List<UserGroupDetailViewModel>> GetDetailAsync(Guid tenantId, Guid userId)
        {
            return this.GetQueryable(p =>
                p.UserId == userId &&
                p.User.TenantId == tenantId).Select(p => new UserGroupDetailViewModel()
                {
                    Name = p.UserGroup.Name,
                    UserGroupId = p.UserGroupId
                }).ToListAsync();
        }
    }
}
