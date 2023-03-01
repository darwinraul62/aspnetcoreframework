using System;
using System.Linq;
using Ecubytes.Identity.User.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecubytes.Identity.User.Infrastructure.EntityConfigurations
{
    public class UserAttributeEntityTypeConfiguration : IEntityTypeConfiguration<UserAttribute>
    {
        public void Configure(EntityTypeBuilder<UserAttribute> builder)
        {
            builder.ToTable("userattribute");

            builder.HasKey(p => new { p.UserId, p.Value });

            builder.HasOne(p => p.User).WithMany(p => p.Attributes);
        }
    }
}
