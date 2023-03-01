using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Ecubytes.Data.EntityFramework
{
    public class RepositoryContainer<T> : IUnitOfWork  where T : Ecubytes.Data.EntityFramework.DbContext
    {
        protected readonly T Context;

        public RepositoryContainer(T Context)
        {
            this.Context = Context;
        }

        public void Dispose()
        {
            if(Context!=null)
                Context.Dispose();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return Context.SaveChangesAsync(cancellationToken);
        }

        //Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
        //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency     
        public Task ResilientTransactionAsync(Func<Task> action)
        {
            return ResilientTransaction.New(this.Context).ExecuteAsync(action);
        }

        public async Task<List<T>> ToListFromQueryableAsync(IQueryable<T> source)
        {
            return await source.ToListAsync();
        } 
    }
}
