using System;
using System.Linq;
using Ecubytes.Integration.Event.Repository.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ecubytes.Identity.Application.Infrastructure.Repositories
{
    public class IntegrationEventRepositoryConnectionProvider : Ecubytes.Integration.Event.Repository.EntityFramework.IIntegrationEventRepositoryConnectionProvider
    {
        //Use as Scope Lifetime Primary Context
        private ApplicationDbContext primaryContext;
        public IntegrationEventRepositoryConnectionProvider(ApplicationDbContext primaryContext)
        {
            if (primaryContext == null) throw new ArgumentNullException(nameof(primaryContext));
            this.primaryContext = primaryContext;
        }

        public IDbContextTransaction GetCurrentSharedDbContextTransaction()
        {
            return primaryContext.Database.CurrentTransaction;
        }

        public DbContextOptions<IntegrationEventLogContext> GetDbContextOptions()
        {
            return new DbContextOptionsBuilder<IntegrationEventLogContext>()
                .UseNpgsql(primaryContext.Database.GetDbConnection())
                .Options;
        }
    }
}
