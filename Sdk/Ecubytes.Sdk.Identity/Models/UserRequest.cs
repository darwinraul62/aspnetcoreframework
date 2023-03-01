using System;
using System.Collections.Generic;

namespace Ecubytes.Identity.Models
{
    public class UserCreateRequest
    {
        public Guid? UserId { get; set; }
        public string LogonName { get; set; }
        public string Names { get; set; }
        public string LastNames { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool BlockLogin { get; set; }
        public DateTime? RegisterDate { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
    }
    public class UserCreateResponse
    {
        public Guid UserId { get; set; }
    }
    public class UserUpdateRequest
    {
        public Guid UserId { get; set; }
        public string LogonName { get; set; }
        public string Names { get; set; }
        public string LastNames { get; set; }
        public string Email { get; set; }
        public bool BlockLogin { get; set; }
    }
    public class UserDeleteRequest
    {
        public Guid UserId { get; set; }
    }
    public class UserRegisterCountRequest
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }        
    }
    internal class UserCreateRequestDTO
    {
        public Guid? UserId { get; set; }
        public string LogonName { get; set; }
        public string Names { get; set; }
        public string LastNames { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool BlockLogin { get; set; }
        public DateTime? RegisterDate { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
    }
    internal class UserCreateResponseDTO
    {
        public Guid UserId { get; set; }
    }
    internal class UserUpdateRequestDTO
    {
        public string LogonName { get; set; }
        public string Names { get; set; }
        public string LastNames { get; set; }
        public string Email { get; set; }
        public bool BlockLogin { get; set; }
    }
    internal class UserChangePasswordRequestDTO
    {
        public string Password { get; set; }
    }
}
