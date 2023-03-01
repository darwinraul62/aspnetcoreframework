using System;
using System.Collections.Generic;
using System.Linq;

namespace Ecubytes.Identity.Application.Data.Models.Auth
{
    public class Role
    {
        public Guid RoleId { get; set; }
        public Guid ApplicationId { get; set; }
        public string Name { get; set; }
        public string CodeName { get; set; }
        public string Description { get; set; }
        public bool IsRoleGroup { get; set; }
        public bool Active { get; set; }
        public App.Application Application { get; set; }
        public List<RoleGroupDetail> RoleDetails { get; set; }
        public List<RoleGroupDetail> RoleGroups { get; set; }
    }
}
