using System;

namespace Ecubytes.Identity.Models
{
    internal class LoginRequestDTO
    {                
        public string LogonName { get; set; }
        public string Password { get; set; }        
    }
    internal class LoginResponseDTO
    {
        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
        public string LogonName { get; set; }
        public string Name { get; set; }
    }
}
