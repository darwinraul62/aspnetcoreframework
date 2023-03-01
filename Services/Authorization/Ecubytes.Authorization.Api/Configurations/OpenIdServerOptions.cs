using System;

namespace Ecubytes.AspNetCore.Authorization.Server.Api.Configurations
{
    public class OpenIdServerOptions
    {
        public const string SectionName = "Ecubytes:OpenIdServer";
        public string EncryptionCertificateThumprint { get; set; }
        public string SigningCertificateThumprint { get; set; }
        public bool DisableAccessTokenEncryption { get; set; }
        public string OpenIdConnectionString { get; set; }
    }
}
