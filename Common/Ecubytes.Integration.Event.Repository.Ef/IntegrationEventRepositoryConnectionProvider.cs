using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ecubytes.Integration.Event.Repository.EntityFramework
{
    public abstract class IntegrationEventRepositoryConnectionProviderBase<T> : 
        IIntegrationEventRepositoryConnectionProvider where T : DbContext
    {
        protected T primaryContext;
        public IntegrationEventRepositoryConnectionProviderBase(T primaryContext)
        {
            if (primaryContext == null) throw new ArgumentNullException(nameof(primaryContext));
            this.primaryContext = primaryContext;
        }
        
        public IDbContextTransaction GetCurrentSharedDbContextTransaction()
        {
            return primaryContext.Database.CurrentTransaction;
        }

        public abstract DbContextOptions<IntegrationEventLogContext> GetDbContextOptions();        
    }
}
