using System;
using System.Threading.Tasks;
using Ecubytes.Data.EntityFramework;
using Ecubytes.Identity.Application.Data.Repositories;

namespace Ecubytes.Identity.Application.Infrastructure.Repositories
{
    public class ApplicationRepository : Repository<Data.Models.App.Application>, IApplicationRepository
    {
        public ApplicationRepository(ApplicationDbContext context) : base(context)
        {
        }        
    }
}
