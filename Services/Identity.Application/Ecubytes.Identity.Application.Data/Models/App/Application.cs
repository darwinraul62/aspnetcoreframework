using System;
using Ecubytes.Identity.Application.Data.Models.Constants;

namespace Ecubytes.Identity.Application.Data.Models.App
{
    public class Application
    {
        public Guid ApplicationId { get; set; }
        public string Name { get; set; }
        public byte StateId { get; set; }
        public string HomePageUrl { get; set; }
        public string PrivacyStatementUrl { get; set; }
        public string TermsOfServiceUrl { get; set; }
        public ApplicationState State { get; set; }
    }
}
