using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Ecubytes.Identity.User.Api.Models
{
    public class SignInRequestDTO
    {
        [Required]
        public string LogonName { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class SignInResponseDTO
    {   
        public Guid UserId { get; set; }    
        public Guid TenantId { get; set; }
        public string LogonName { get; set; }
        public string Name { get; set; }
    }
}
