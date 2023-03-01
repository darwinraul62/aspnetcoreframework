using System;

namespace Ecubytes.Identity.Models
{
    internal class OtpRequestDTO
    {
        public Guid? UserId { get; set; }
        public string TargetId { get; set; }
        public string Challenge { get; set; }
    }
    internal class OtpResponseDTO
    {
        public string Password { get; set; }
    }

    internal class OtpActivateRequestDTO
    {
        public Guid? UserId { get; set; }
        public string Challenge { get; set; }
        public string TargetId { get; set; }
        public string Password { get; set; }
    }
}
