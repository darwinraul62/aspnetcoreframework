using System;
using System.Linq;

namespace Ecubytes.Identity.User.Data.Models
{
    public class Tenant
    {
        public Guid TenantId { get; set; }
        public string Name { get; set; }
        public string DomainName { get; set; }
        public bool IsValid { get; set; } 
        public TenantStates StateId { get; set; }
    }
}
