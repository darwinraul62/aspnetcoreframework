using System;
using System.ComponentModel.DataAnnotations;

namespace Ecubytes.Identity.Web.Models
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
