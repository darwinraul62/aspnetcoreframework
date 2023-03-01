using System;

namespace Ecubytes.Identity.Models
{
    internal class UserGroupViewModelDTO
    {
        public Guid UserGroupId { get; set; }
        public string Name { get; set; }
    }

    internal class UserGroupCreateRequestDTO
    {
        public string Name { get; set; }
    }
    internal class UserGroupCreateResponseDTO
    {
        public Guid UserGroupId { get; set; }
    }
    internal class UserGroupUpdateRequestDTO
    {        
        public string Name { get; set; }
    }
    public class UserGroupDetailViewModelDTO
    {
        public Guid UserGroupId { get; set; }
        public string Name { get; set; }
    }
    public class UserAddUserGroupRequestDTO
    {        
        public Guid UserGroupId { get; set; }
    }
}
