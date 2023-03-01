using System;
using System.Data.Common;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ecubytes.Integration.Event.Repository.EntityFramework
{
    public interface IIntegrationEventRepositoryConnectionProvider
    {
        DbContextOptions<IntegrationEventLogContext> GetDbContextOptions();
        /// summary
        /// Implement Transaction DB used for Save Events
        IDbContextTransaction GetCurrentSharedDbContextTransaction();
    }
}
