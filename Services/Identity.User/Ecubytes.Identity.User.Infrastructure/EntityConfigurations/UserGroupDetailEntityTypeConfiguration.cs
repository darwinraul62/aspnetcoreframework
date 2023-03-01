using System;
using System.Linq;
using Ecubytes.Identity.User.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecubytes.Identity.User.Infrastructure.EntityConfigurations
{
    public class UserGroupDetailEntityTypeConfiguration : IEntityTypeConfiguration<UserGroupDetail>
    {
        public void Configure(EntityTypeBuilder<UserGroupDetail> builder)
        {
            builder.ToTable("usergroupdetail");

            builder.HasKey(p => new { p.UserId, p.UserGroupId });

            builder.HasOne(p => p.UserGroup).WithMany().HasForeignKey(p => p.UserGroupId);
            builder.HasOne(p => p.User).WithMany().HasForeignKey(p => p.UserId);
        }
    }
}
