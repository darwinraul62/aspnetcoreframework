using System;

namespace Ecubytes.Identity.Models
{
    internal class UserViewModelDTO
    {
        public Guid UserId { get; set; }
        public string LogonName { get; set; }
        public string Names { get; set; }
        public string LastNames { get; set; }
        public string Email { get; set; }
        public bool BlockLogin { get; set; }
        //public UserStates StateId { get; set; }
        public bool IsValid { get; set; }
        public string UserGroupNames { get; set; }
        public string LogonNameFullName { get; set; }
        public string FullName { get; set; }
        public bool Online { get; set; }
        public DateTime? LastAccess { get; set; }
    }
}
