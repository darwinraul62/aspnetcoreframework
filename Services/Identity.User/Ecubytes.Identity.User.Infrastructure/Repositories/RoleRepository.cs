using System;
using System.Linq;
using Ecubytes.Data.EntityFramework;
using Ecubytes.Identity.User.Data.Models;
using Ecubytes.Identity.User.Data.Repositories;

namespace Ecubytes.Identity.User.Infrastructure.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(UserDbContext context) : base(context)
        {
        }
    }
}
