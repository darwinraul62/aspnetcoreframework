using System;
using System.Linq;
using System.Threading.Tasks;
using Ecubytes.Data;

namespace Ecubytes.Identity.Application.Data.Repositories
{
    public interface IApplicationSecretRepository : IRepository<Models.Auth.ApplicationSecret>
    {
        Task<int> GetActiveSecretCountAsync(Guid applicationId);
    }
}
