using System;
using System.Linq;
using Ecubytes.Identity.Application.Data.Models.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecubytes.Identity.Application.Infrastructure.EntityConfigurations
{
    public class RedirectUriEntityTypeConfiguration
    : IEntityTypeConfiguration<RedirectUri>
    {
        public void Configure(EntityTypeBuilder<RedirectUri> builder)
        {
            builder.ToTable("redirecturi", schema: "auth");

            builder.HasKey(ci => ci.RedirectUriId);

            builder.HasOne(cb => cb.Application).WithMany().HasForeignKey(cb => cb.ApplicationId);
            builder.HasOne(cb => cb.Platform).WithMany().HasForeignKey(cb => cb.PlatformId);
        }
    }
}
