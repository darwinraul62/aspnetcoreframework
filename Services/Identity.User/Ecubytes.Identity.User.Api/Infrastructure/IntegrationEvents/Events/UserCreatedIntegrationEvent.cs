using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.User.IntegrationEvents
{
    public class UserCreatedIntegrationEvent
    {
        public Guid UserId { get; set; }
        public string LogonName { get; set; }
        public string Names { get; set; }
        public string LastNames { get; set; }
        public string Email { get; set; }
        public bool BlockLogin { get; set; }
        public Guid TenantId { get; set; }
        public Dictionary<string, string> Attributtes { get; set; }
    }
}
