using System;
using Microsoft.EntityFrameworkCore;

namespace Ecubytes.Integration.Event.Repository.EntityFramework.Npgsql
{
    public class IntegrationEventRepositoryConnectionProvider<T> :
        IntegrationEventRepositoryConnectionProviderBase<T> where T : DbContext
    {
        public IntegrationEventRepositoryConnectionProvider(T primaryContext) 
            : base(primaryContext)
        {
        }

        public override DbContextOptions<IntegrationEventLogContext> GetDbContextOptions()
        {            
            return new DbContextOptionsBuilder<IntegrationEventLogContext>()
                .UseNpgsql(primaryContext.Database.GetDbConnection())
                .Options;
        }
    }
}
