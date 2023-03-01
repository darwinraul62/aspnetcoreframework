using System;
using System.Linq;

namespace Ecubytes.Identity.User.Data.Models
{
    public class Otp
    {
        public Guid TenantId { get; set; }
        public Guid Id { get; set; }
        public string Challenge { get; set; }
        public Guid? UserId { get; set; }
        public string Password { get; set; }
        public string TargetId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime? ActivationDate { get; set; }
        public string StateId { get; set; }
        public User User { get; set; }
    }
}
