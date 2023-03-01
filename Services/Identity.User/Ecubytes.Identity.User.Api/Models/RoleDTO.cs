using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Ecubytes.Identity.User.Data.Models;

namespace Ecubytes.Identity.User.Api.Models
{
    public class RoleViewModelDTO
    {
        public Guid RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UserRoleInsertRequestDTO
    {
        [Required]
        public Guid? RoleId { get; set; }
    }

    public class UserGroupRoleInsertRequestDTO
    {
        [Required]
        public Guid? RoleId { get; set; }
    }

    public class UserRoleEffectiveViewModelDTO
    {
        public string CodeName { get; set; }
    }

    public class RoleDetailViewModelDTO
    {
        public Guid RoleId { get; set; }
        public string Name { get; set; }
    }

    public static class RoleModelConverter
    {
        public static List<RoleViewModelDTO> ToDTO(this IEnumerable<Data.Models.Role> model)
        {
            return model.Select(p => p.ToDTO()).ToList();
        }
        public static RoleViewModelDTO ToDTO(this Data.Models.Role model)
        {
            return new RoleViewModelDTO()
            {
                Name = model.Name,
                RoleId = model.RoleId,
                Description = model.Description
            };
        }
    }

    public static class UserRoleDetailModelConvert
    {
        public static IEnumerable<RoleDetailViewModelDTO> ToDTO(this IEnumerable<RoleDetailViewModel> model)
        {
            return model.Select(p => new RoleDetailViewModelDTO()
            {
                Name = p.Name,
                RoleId = p.RoleId
            });
        }
    }
}
