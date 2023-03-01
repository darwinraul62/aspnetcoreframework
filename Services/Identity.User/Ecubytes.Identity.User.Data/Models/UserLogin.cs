using System;
using System.Linq;

namespace Ecubytes.Identity.User.Data.Models
{
    public class UserLogin
    {
        public Guid LogId { get; set; }
        public Guid UserId { get; set; }
        public DateTime LoginDate { get; set; }
        public User User { get; set; }
    }
}
