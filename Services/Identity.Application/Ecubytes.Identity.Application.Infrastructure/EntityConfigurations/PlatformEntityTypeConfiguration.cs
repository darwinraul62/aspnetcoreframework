using System;
using System.Linq;
using Ecubytes.Identity.Application.Data.Models.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecubytes.Identity.Application.Infrastructure.EntityConfigurations
{
    public class PlatformEntityTypeConfiguration : IEntityTypeConfiguration<Platform>
    {
        public void Configure(EntityTypeBuilder<Platform> builder)
        {
            builder.ToTable("platform", schema: "app");

            builder.HasKey(p => p.PlatformId);
        }
    }
}
