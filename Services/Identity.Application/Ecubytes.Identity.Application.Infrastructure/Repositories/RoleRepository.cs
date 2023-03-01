using System;
using System.Linq;
using Ecubytes.Data.EntityFramework;
using Ecubytes.Identity.Application.Data.Repositories;

namespace Ecubytes.Identity.Application.Infrastructure.Repositories
{
    public class RoleRepository : Repository<Data.Models.Auth.Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
