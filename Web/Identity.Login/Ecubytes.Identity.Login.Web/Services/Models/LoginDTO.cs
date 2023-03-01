using System;
using System.ComponentModel.DataAnnotations;

namespace Ecubytes.Identity.Login.Web.Services.Models
{
    public class LoginRequestDTO
    {                
        public string LogonName { get; set; }
        public string Password { get; set; }
    }
    public class LoginResponseDTO
    {
        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
        public string LogonName { get; set; }
        public string Name { get; set; }
    }
}
