using System;
using System.Linq;

namespace Ecubytes.Identity.Application.Data.Models.Auth
{
    public class ApplicationSecret
    {
        public Guid ApplicationSecretId { get; set; }
        public Guid ApplicationId { get; set; }
        public string Secret { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public Application.Data.Models.App.Application Application { get; set; }
    }
}
