using System;
using System.Collections.Generic;
using System.Linq;

namespace Ecubytes.Identity.User.Data.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
        public string LogonName { get; set; }
        public string Password { get; set; }
        public string Name
        {
            get
            {
                return $"{this.Names} {this.LastNames}"?.Trim();
            }
        }
        public string Names { get; set; }
        public string LastNames { get; set; }
        public string Email { get; set; }
        public bool BlockLogin { get; set; }
        public UserStates StateId { get; set; }
        public bool IsValid { get; set; }
        public DateTime? LastAccess { get; set; }
        public DateTime? RegisterDate { get; set; }
        public Tenant Tenant { get; set; }
        public List<UserAttribute> Attributes { get; set; }
    }
}
