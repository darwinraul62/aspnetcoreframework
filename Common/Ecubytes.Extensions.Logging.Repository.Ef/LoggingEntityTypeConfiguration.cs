using System;
using Ecubytes.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecubytes.Extensions.Logging.Repository.EntityFramework
{
    public class LoggingEntityTypeConfiguration : IEntityTypeConfiguration<LoggingModel>
    {
        public void Configure(EntityTypeBuilder<LoggingModel> builder)
        {
            builder.ToTable("log");
            builder.HasKey(p=>p.LogId);
            // builder.Property(p=>p.LogId).UseIdentityAlwaysColumn();
        }
    }
}
