using System;
using System.Linq;
using Ecubytes.Identity.User.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecubytes.Identity.User.Infrastructure.EntityConfigurations
{
    public class UserGroupEntityTypeConfiguration : IEntityTypeConfiguration<UserGroup>
    {
        public void Configure(EntityTypeBuilder<UserGroup> builder)
        {
            builder.ToTable("UserGroup");

            builder.HasKey(p => p.UserGroupId);

            builder.HasOne(p => p.Tenant).WithMany().HasForeignKey(p => p.TenantId);
        }
    }
}
