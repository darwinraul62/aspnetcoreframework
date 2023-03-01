using System;
using System.Linq;

namespace Ecubytes.Identity.User.Data.Models
{
    public class Role
    {
        public Guid RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CodeName { get; set; }
        public Guid ApplicationId { get; set; }

    }
}
