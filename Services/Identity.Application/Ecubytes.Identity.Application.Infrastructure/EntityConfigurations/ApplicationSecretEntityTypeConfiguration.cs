using System;
using System.Linq;
using Ecubytes.Identity.Application.Data.Models.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecubytes.Identity.Application.Infrastructure.EntityConfigurations
{
    public class ApplicationSecretEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationSecret>
    {
        public void Configure(EntityTypeBuilder<ApplicationSecret> builder)
        {
            builder.ToTable("applicationsecret", schema: "auth");

            builder.HasKey(p => p.ApplicationSecretId);
            builder.HasOne(p => p.Application).WithMany().HasForeignKey(p => p.ApplicationId);
        }
    }
}
