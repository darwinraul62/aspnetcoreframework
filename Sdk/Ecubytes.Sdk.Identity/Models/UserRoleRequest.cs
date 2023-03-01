using System;

namespace Ecubytes.Identity.Models
{
    public class UserRoleRequest
    {
        public Guid UserId {get; set;}
        public Guid ApplicationId { get; set; }        
    }
}
