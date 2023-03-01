using System;
using System.Linq;

namespace Ecubytes.Identity.User.Data.Models
{
    public class UserAttribute
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public User User { get; set; }
    }
}
