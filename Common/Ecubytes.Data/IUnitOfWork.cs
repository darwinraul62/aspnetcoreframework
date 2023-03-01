using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ecubytes.Data
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));    
        Task ResilientTransactionAsync(Func<Task> action);        
        //Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
