using System;
using System.Linq;
using Ecubytes.Identity.User.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecubytes.Identity.User.Infrastructure.EntityConfigurations
{
    public class UserGroupRoleEntityTypeConfiguration : IEntityTypeConfiguration<UserGroupRole>
    {
        public void Configure(EntityTypeBuilder<UserGroupRole> builder)
        {
            builder.ToTable("UserGroupRole");

            builder.HasKey(p => new { p.UserGroupId, p.RoleId });

            builder.HasOne(p => p.Role).WithMany().HasForeignKey(p => p.RoleId);

            builder.HasOne(p => p.UserGroup).WithMany().HasForeignKey(p => p.UserGroupId);
        }
    }
}
