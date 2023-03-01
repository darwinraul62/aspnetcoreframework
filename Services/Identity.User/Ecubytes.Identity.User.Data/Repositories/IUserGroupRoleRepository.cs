using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ecubytes.Data;
using Ecubytes.Identity.User.Data.Models;

namespace Ecubytes.Identity.User.Data.Repositories
{
    public interface IUserGroupRoleRepository : IRepository<UserGroupRole>
    {
        Task<List<RoleDetailViewModel>> GetDetailAsync(Guid tenantId, Guid userGroupId);
    }
}
