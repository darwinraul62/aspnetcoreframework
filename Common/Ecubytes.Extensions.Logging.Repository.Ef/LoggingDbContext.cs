using System;
using Microsoft.EntityFrameworkCore;
using Ecubytes.Data.EntityFramework;

namespace Ecubytes.Extensions.Logging.Repository.EntityFramework
{
    public class LoggingDbContext : Ecubytes.Data.EntityFramework.DbContext
    {
        public LoggingDbContext(DbContextOptions<LoggingDbContext> options)
           : base(options)
        {
        }

        public DbSet<LoggingModel> Logs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {            
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new LoggingEntityTypeConfiguration());           

            builder.ToLowerCaseEntities();
        }
    }    
}
