using System;

namespace Ecubytes.Identity.Models
{
    public class OtpGenerateResponse
    {
        public string Password { get; set; }
    }
    public class OtpGenerateRequest
    {
        public string Challenge { get; set; }
        public Guid? UserId { get; set; }
        public string TargetId { get; set; }
    }
}
