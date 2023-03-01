using System;
using System.Linq;

namespace Ecubytes.Identity.User.Data.Models
{
    public class UserRoleEffective
    {
        public Guid TenantId { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public Guid ApplicationId { get; set; }
        public string CodeName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
