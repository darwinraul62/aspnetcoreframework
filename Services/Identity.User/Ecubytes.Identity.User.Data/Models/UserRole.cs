using System;
using System.Linq;

namespace Ecubytes.Identity.User.Data.Models
{
    public class UserRole
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        public Data.Models.User User { get; set; }
    }
}
