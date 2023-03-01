using System;

namespace Ecubytes.Identity.Models
{
    public class UserGroupCreateRequest
    {
        public Guid? UserGroupId { get; set; }
        public string Name { get; set; }
    }
    public class UserGroupCreateResponse
    {
        public Guid UserGroupId { get; set; }
    }
    public class UserGroupUpdateRequest
    {
        public Guid UserGroupId { get; set; }
        public string Name { get; set; }
    }
    public class UserGroupDeleteRequest
    {
        public Guid UserId { get; set; }
    }
    public class UserGroupAddToUserRequest
    {
        public Guid UserId { get; set; }
        public Guid UserGroupId { get; set; }
    }
    public class UserGroupRemoveToUserRequest
    {
        public Guid UserId { get; set; }
        public Guid UserGroupId { get; set; }
    }
}
