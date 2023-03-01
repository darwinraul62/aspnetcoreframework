using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Ecubytes.Identity.User.Data.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ecubytes.Identity.User.Api.Models
{
    public class UserViewModelDTO
    {
        public Guid UserId { get; set; }
        public string LogonName { get; set; }
        public string Names { get; set; }
        public string LastNames { get; set; }
        public string Email { get; set; }
        public bool BlockLogin { get; set; }
        public UserStates StateId { get; set; }
        public bool IsValid { get; set; }
        public string UserGroupNames { get; set; }
        public string LogonNameFullName { get; set; }
        public string FullName { get; set; }
        public DateTime? LastAccess { get; set; }
        public bool Online { get; set; }
    }

    public class UserInsertRequestDTO
    {
        public Guid? UserId { get; set; }
        [Required]
        public string LogonName { get; set; }
        [Required]
        public string Names { get; set; }
        public string LastNames { get; set; }
        [Required]
        public string Password { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        public bool BlockLogin { get; set; }
        public DateTime? RegisterDate { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
    }
    public class UserInsertResponseDTO
    {
        public Guid UserId { get; set; }
    }

    public class UserUpdateRequestDTO
    {
        [Required]
        public string LogonName { get; set; }
        [BindRequired]
        public string Names { get; set; }
        public string LastNames { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        public bool BlockLogin { get; set; }
        public DateTime? LastAccess { get; set; }
    }

    public class ChangePasswordRequestDTO
    {
        [Required]
        public string Password { get; set; }
    }

    public static class UserModelConverter
    {
        public static List<UserViewModelDTO> ToDTO(this IEnumerable<Data.Models.UserInfo> model)
        {
            return model.Select(p => p.ToDTO()).ToList();
        }
        public static UserViewModelDTO ToDTO(this Data.Models.UserInfo model)
        {
            return new UserViewModelDTO()
            {
                BlockLogin = model.BlockLogin,
                Email = model.Email,
                LastNames = model.LastNames,
                LogonName = model.LogonName,
                Names = model.Names,
                UserId = model.UserId,
                IsValid = model.IsValid,
                StateId = model.StateId,
                UserGroupNames = model.UserGroupNames,
                FullName = model.FullName,
                LogonNameFullName = model.LogonNameFullName,
                Online = model.Online,
                LastAccess = model.LastAccess
            };
        }
    }
}
