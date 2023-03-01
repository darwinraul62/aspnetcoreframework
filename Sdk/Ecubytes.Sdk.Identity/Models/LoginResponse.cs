using System;

namespace Ecubytes.Identity.Models
{
    public class LoginResponse
    {
        public Guid UserId { get; set; }        
        public string LogonName { get; set; }
        public string Name { get; set; }
    }
}
