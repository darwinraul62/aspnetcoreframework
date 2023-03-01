using System;
using System.Linq;

namespace Ecubytes.Identity.User.Data.Models
{
    public class UserGroupRole
    {
        public Guid UserGroupId { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        public UserGroup UserGroup { get; set; }
    }
}
