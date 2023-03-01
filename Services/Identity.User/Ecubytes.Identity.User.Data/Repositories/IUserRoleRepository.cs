using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecubytes.Data;
using Ecubytes.Identity.User.Data.Models;

namespace Ecubytes.Identity.User.Data.Repositories
{
    public interface IUserRoleRepository : IRepository<UserRole>
    {
        Task<List<string>> GetEffectiveRoleCodeNameAsync(Guid tenantId, Guid userId, Guid? applicationId = null);

        Task<List<RoleDetailViewModel>> GetDetailAsync(Guid tenanId, Guid userId);
    }
}
