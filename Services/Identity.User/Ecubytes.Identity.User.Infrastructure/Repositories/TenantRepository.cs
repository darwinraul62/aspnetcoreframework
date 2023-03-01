using System;
using System.Linq;
using Ecubytes.Data.EntityFramework;
using Ecubytes.Identity.User.Data.Models;
using Ecubytes.Identity.User.Data.Repositories;

namespace Ecubytes.Identity.User.Infrastructure.Repositories
{
    public class TenantRepository : Repository<Tenant>, ITenantRepository
    {
        public TenantRepository(UserDbContext context) : base(context)
        {
        }
    }
}
