using System;
using System.Linq;
using System.Threading.Tasks;
using Ecubytes.Data.EntityFramework;
using Ecubytes.Identity.Application.Data.Models.Auth;
using Ecubytes.Identity.Application.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ecubytes.Identity.Application.Infrastructure.Repositories
{
    public class ApplicationSecretRepository : Repository<Data.Models.Auth.ApplicationSecret>, IApplicationSecretRepository
    {
        public ApplicationSecretRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<int> GetActiveSecretCountAsync(Guid applicationId)
        {
            return this.Context.Set<ApplicationSecret>().Where(p=>p.ApplicationId == applicationId).CountAsync();
        }
    }
}
