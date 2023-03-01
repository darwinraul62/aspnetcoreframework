using System;
using System.Linq;

namespace Ecubytes.Identity.User.Data.Models
{
    public class UserGroup
    {
        public Guid UserGroupId { get; set; }
        public Guid TenantId { get; set; }
        public string Name { get; set; }
        public UserGroupStates StateId { get; set; }
        public bool IsValid { get; set; }
        public Tenant Tenant { get; set; }
    }
}
