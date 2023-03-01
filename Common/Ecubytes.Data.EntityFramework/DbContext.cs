using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Ecubytes.Data.EntityFramework
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext, Data.IUnitOfWork
    {
        public DbContext(DbContextOptions options)
            :base(options)
        {
        }

        public Task ResilientTransactionAsync(Func<Task> action)
        {
             return ResilientTransaction.New(this).ExecuteAsync(action);
        }
    }
}
