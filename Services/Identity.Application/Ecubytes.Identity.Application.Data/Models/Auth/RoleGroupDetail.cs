using System;
using System.Linq;

namespace Ecubytes.Identity.Application.Data.Models.Auth
{
    public class RoleGroupDetail
    {
        public Guid RoleGroupId { get; set; }
        public Guid RoleDetailId { get; set; }
        public Role RoleGroup { get; set; }
        public Role RoleDetail { get; set; }
    }
}
