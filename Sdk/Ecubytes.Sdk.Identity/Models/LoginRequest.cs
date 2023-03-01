using System;

namespace Ecubytes.Identity.Models
{
    public class LoginRequest
    {
        public string LogonName { get; set; }
        public string Password { get; set; }
    }
    public class LoginCountRequest
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public Guid? UserId { get; set; }
    }
}
