using System;
using System.Linq;
using Ecubytes.Data.EntityFramework;
using Ecubytes.Identity.Application.Data.Repositories;

namespace Ecubytes.Identity.Application.Infrastructure.Repositories
{
    public class RedirectUriRepository : Repository<Data.Models.Auth.RedirectUri>, IRedirectUriRepository
    {
        public RedirectUriRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
