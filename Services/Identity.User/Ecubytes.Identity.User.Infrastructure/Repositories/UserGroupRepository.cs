using System;
using System.Linq;
using Ecubytes.Data.EntityFramework;
using Ecubytes.Identity.User.Data.Models;
using Ecubytes.Identity.User.Data.Repositories;

namespace Ecubytes.Identity.User.Infrastructure.Repositories
{
    public class UserGroupRepository : Repository<UserGroup>, IUserGroupRepository
    {
        public UserGroupRepository(UserDbContext context) : base(context)
        {
        }
    }
}
