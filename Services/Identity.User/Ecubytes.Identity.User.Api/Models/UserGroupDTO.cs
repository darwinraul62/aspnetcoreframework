using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Ecubytes.Identity.User.Api.Models
{
    public class UserGroupViewModelDTO
    {
        public Guid? UserGroupId { get; set; }
        public string Name { get; set; }
    }

    public class UserGroupInsertRequestDTO
    {
        [Required]
        public string Name { get; set; }
    }
    public class UserGroupInsertResponseDTO
    {
        public Guid UserGroupId { get; set; }
    }
    public class UserGroupUpdateRequestDTO
    {
        [Required]
        public Guid? UserGroupId { get; set; }
        [Required]
        public string Name { get; set; }
    }

    public class UserAddUserGroupRequestDTO
    {
        [Required]
        public Guid? UserGroupId { get; set; }
    }

    public class UserGroupDetailViewModelDTO
    {
        public Guid UserGroupId { get; set; }
        public string Name { get; set; }
    }

    public static class UserGroupModelConverter
    {
        public static List<UserGroupViewModelDTO> ToDTO(this IEnumerable<Data.Models.UserGroup> model)
        {
            return model.Select(p => p.ToDTO()).ToList();
        }
        public static UserGroupViewModelDTO ToDTO(this Data.Models.UserGroup model)
        {
            return new UserGroupViewModelDTO()
            {
                Name = model.Name,
                UserGroupId = model.UserGroupId
            };
        }
    }

    public static class UserGroupDetailViewModelConverter
    {
        public static List<UserGroupDetailViewModelDTO> ToDTO(this IEnumerable<Data.Models.UserGroupDetailViewModel> model)
        {
            return model.Select(p => new UserGroupDetailViewModelDTO()
            {
                Name = p.Name,
                UserGroupId = p.UserGroupId
            }).ToList();
        }
    }

}
