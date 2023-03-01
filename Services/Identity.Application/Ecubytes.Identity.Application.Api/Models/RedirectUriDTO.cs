using System;
using System.Linq;

namespace Ecubytes.Identity.Application.Api.Models
{
    public class RedirectUriViewModelDTO
    {
        public Guid UriId { get; set; }
        public Guid ApplicationId { get; set; }
        public int PlatformId { get; set; }
        public string Uri { get; set; }
        public string PlatformName { get; set; }
    }

    public class RedirectUriInsertRequestDTO
    {        
        public int PlatformId { get; set; }
        public string Uri { get; set; }
    }

    public class RedirectUriInsertResponseDTO
    {
        public Guid ApplicationId { get; set; }
        public Guid UriId { get; set; }        
    }
}
