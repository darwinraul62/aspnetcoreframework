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
    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(UserDbContext context) : base(context)
        {
        }

        public Task<List<RoleDetailViewModel>> GetDetailAsync(Guid tenanId, Guid userId)
        {
            return GetQueryable(p =>
                p.UserId == userId &&
                p.User.TenantId == tenanId, includeProperties: "Role").Select(p => new RoleDetailViewModel()
                {
                    Name = p.Role.Name,
                    RoleId = p.RoleId
                }).ToListAsync();            
        }

        public Task<List<string>> GetEffectiveRoleCodeNameAsync(Guid tenantId, Guid userId, Guid? applicationId = null)
        {
            UserDbContext db = (UserDbContext)Context;
            
            var query = db.UserRoleEffective.Where(p => p.TenantId == tenantId &&
                p.UserId == userId);
            
            if(applicationId.HasValue)
                query = query.Where(p=>p.ApplicationId == applicationId.Value);

            return query.Select(p=>p.CodeName).ToListAsync();
        }
    }
}
