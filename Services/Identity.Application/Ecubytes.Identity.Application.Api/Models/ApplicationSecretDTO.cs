using System;
using System.Linq;

namespace Ecubytes.Identity.Application.Api.Models
{
    public class ApplicationSecretViewModelDTO
    {
        public Guid SecretId { get; set; }
        public Guid ApplicationId { get; set; }        
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }

    public class ApplicationSecretInsertRequestDTO
    {                
        public string Description { get; set; }
        public DateTime ExpirationDate { get; set; }
    }

    public class ApplicationSecretInsertResponseDTO
    {
        public Guid ApplicationId { get; set; }
        public Guid SecretId { get; set; }
        public string Secret { get; set; }
    }
}
