using System;
using System.Linq;

namespace Ecubytes.Identity.Application.Data.Models.Auth
{
    public class Scope
    {
        public Guid ScopeId { get; set; }
        public Guid ApplicationId { get; set; }
        public string Name { get; set; }
        public string CodeName { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}
