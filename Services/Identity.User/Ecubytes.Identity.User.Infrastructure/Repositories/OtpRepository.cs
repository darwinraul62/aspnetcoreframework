using System;
using System.Linq;
using System.Threading.Tasks;
using Ecubytes.Data.EntityFramework;
using Ecubytes.Identity.User.Data.Models;
using Ecubytes.Identity.User.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ecubytes.Identity.User.Infrastructure.Repositories
{
    public class OtpRepository : Repository<Otp>, IOtpRepository
    {
        public OtpRepository(UserDbContext context) : base(context)
        {
        }

        public Task<Otp> GetPendingByUserId(Guid tenantId, string targetId, Guid userId, 
            DateTime dateSearchFrom)
        {        
            return dbSet.Where(p =>
                p.TenantId == tenantId
                && p.UserId == userId
                && p.TargetId == targetId
                && p.StateId == "unused"
                && p.CreationDate > dateSearchFrom).
                OrderByDescending(p=>p.CreationDate).FirstOrDefaultAsync();
        }

        public Task<Otp> GetPendingByChallenge(Guid tenantId, string targetId, string challenge, 
            DateTime dateSearchFrom)
        {        
            return dbSet.Where(p =>
                p.TenantId == tenantId
                && p.Challenge == challenge
                && p.TargetId == targetId
                && p.StateId == "unused"
                && p.CreationDate > dateSearchFrom).
                OrderByDescending(p=>p.CreationDate).FirstOrDefaultAsync();
        }
    }
}
