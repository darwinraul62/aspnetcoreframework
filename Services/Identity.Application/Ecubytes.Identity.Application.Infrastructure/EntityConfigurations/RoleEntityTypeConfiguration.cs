using System;
using System.Linq;
using Ecubytes.Identity.Application.Data.Models.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecubytes.Identity.Application.Infrastructure.EntityConfigurations
{
    public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Data.Models.Auth.Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("role", schema: "auth");

            builder.HasKey(p => p.RoleId);

            builder.HasOne(p => p.Application).WithMany().HasForeignKey(p => p.ApplicationId);
            builder.HasMany(p => p.RoleDetails).WithOne(p => p.RoleGroup).HasForeignKey(p => p.RoleGroupId);
            builder.HasMany(p => p.RoleGroups).WithOne(p => p.RoleDetail).HasForeignKey(p => p.RoleDetailId);
        }
    }
}
