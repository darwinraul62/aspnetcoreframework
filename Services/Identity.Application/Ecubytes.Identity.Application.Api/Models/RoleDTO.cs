using System;
using System.Collections.Generic;
using System.Linq;

namespace Ecubytes.Identity.Application.Api.Models
{
    public class RoleViewModelDTO
    {
        public Guid RoleId { get; set; }
        public Guid ApplicationId { get; set; }
        public string Name { get; set; }
        public string CodeName { get; set; }
        public string Description { get; set; }
        public bool IsRoleGroup { get; set; }
        public bool Active { get; set; }
    }

    public class RoleInsertRequestDTO
    {
        public string Name { get; set; }
        public string CodeName { get; set; }
        public string Description { get; set; }
        public bool IsRoleGroup { get; set; }
    }

    public class RoleInsertResponseDTO
    {
        public Guid ApplicationId { get; set; }
        public Guid RoleId { get; set; }
    }

    public class RoleUpdateRequestDTO
    {
        public Guid RoleId { get; set; }
        public string Name { get; set; }
        public string CodeName { get; set; }
        public string Description { get; set; }
        public bool IsRoleGroup { get; set; }
        public bool Active { get; set; }
    }   


    public static class RoleModelConverter
    {
        public static List<RoleViewModelDTO> ToDTO(this IEnumerable<Application.Data.Models.Auth.Role> model)
        {
            return model.Select(p => p.ToDTO()).ToList();
        }
        public static RoleViewModelDTO ToDTO(this Application.Data.Models.Auth.Role model)
        {
            return new RoleViewModelDTO()
            {
                Active = model.Active,
                ApplicationId = model.ApplicationId,
                Description = model.Description,
                IsRoleGroup = model.IsRoleGroup,
                Name = model.Name,
                RoleId = model.RoleId,
                CodeName = model.CodeName
            };
        }
    }
}
