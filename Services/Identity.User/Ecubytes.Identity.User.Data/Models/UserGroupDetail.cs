using System;
using System.Linq;

namespace Ecubytes.Identity.User.Data.Models
{
    public class UserGroupDetail
    {
        public Guid UserId { get; set; }
        public Guid UserGroupId { get; set; }

        public Data.Models.User User { get; set; }
        public Data.Models.UserGroup UserGroup { get; set; }
    }
}
