using System;
using System.Linq;

namespace Ecubytes.Identity.Application.Data.Models.Auth
{
    public class RedirectUri
    {
        public Guid RedirectUriId { get; set; }
        public Guid ApplicationId { get; set; }
        public int PlatformId { get; set; }
        public string Uri { get; set; }
        public Application.Data.Models.App.Application Application { get; set; }
        public Application.Data.Models.App.Platform Platform { get; set; }
    }
}
