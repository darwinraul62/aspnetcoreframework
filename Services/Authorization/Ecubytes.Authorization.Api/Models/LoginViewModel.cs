using System;
using System.ComponentModel.DataAnnotations;

namespace Ecubytes.AspNetCore.Authorization.Server.Api.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
