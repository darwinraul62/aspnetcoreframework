using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecubytes.Data;
using Ecubytes.Identity.User.Data.Models;

namespace Ecubytes.Identity.User.Data.Repositories
{
    public interface IUserGroupDetailRepository : IRepository<UserGroupDetail>
    {
        Task<List<UserGroupDetailViewModel>> GetDetailAsync(Guid tenantId, Guid userId);
    }    
}
