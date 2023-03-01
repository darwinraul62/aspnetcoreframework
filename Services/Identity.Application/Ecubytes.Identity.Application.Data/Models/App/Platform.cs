using System;
using System.Linq;

namespace Ecubytes.Identity.Application.Data.Models.App
{
    public class Platform
    {
        public int PlatformId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}
