using System;
using System.Linq;
using Ecubytes.Data;

namespace Ecubytes.Identity.Application.Data.Repositories
{
    public interface IRedirectUriRepository : IRepository<Models.Auth.RedirectUri>
    {
    }
}
