using System;
using System.Linq;

namespace Ecubytes.Identity.User.Api.Models
{
    public class OtpRequestDTO
    {
        public Guid? UserId { get; set; }
        public string TargetId { get; set; }
        public string Challenge { get; set; }
    }
    public class OtpResponseDTO
    {
        public string Password { get; set; }
    }

    public class OtpActivateRequestDTO
    {
        public Guid? UserId { get; set; }
        public string Challenge { get; set; }
        public string TargetId { get; set; }
        public string Password { get; set; }
    }
}