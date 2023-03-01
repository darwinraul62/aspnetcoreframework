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
    public class UserGroupRoleRepository : Repository<UserGroupRole>, IUserGroupRoleRepository
    {
        public UserGroupRoleRepository(UserDbContext context) : base(context)
        {
        }
        public Task<List<RoleDetailViewModel>> GetDetailAsync(Guid tenantId, Guid userGroupId)
        {
            return this.GetQueryable(p =>
                p.UserGroupId == userGroupId &&
                p.UserGroup.TenantId == tenantId, includeProperties: "Role").Select(p => new RoleDetailViewModel()
                {
                    Name = p.Role.Name,
                    RoleId = p.RoleId
                }).ToListAsync();

        }
    }
}
