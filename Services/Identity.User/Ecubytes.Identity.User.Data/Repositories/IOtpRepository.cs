using System;
using System.Linq;
using System.Threading.Tasks;
using Ecubytes.Data;
using Ecubytes.Identity.User.Data.Models;

namespace Ecubytes.Identity.User.Data.Repositories
{
    public interface IOtpRepository : IRepository<Otp>
    {
        Task<Otp> GetPendingByUserId(Guid tenantId, string targetId, Guid userId, DateTime dateSearchFrom);
        
        Task<Otp> GetPendingByChallenge(Guid tenantId, string targetId, string challenge, DateTime dateSearchFrom);
    }
}
