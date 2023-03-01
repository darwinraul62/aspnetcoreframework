using System;
using System.Linq;
using Ecubytes.Identity.User.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecubytes.Identity.User.Infrastructure.EntityConfigurations
{
    public class UserRoleEffectiveEntityTypeConfiguration : IEntityTypeConfiguration<UserRoleEffective>
    {
        public void Configure(EntityTypeBuilder<UserRoleEffective> builder)
        {
            builder.ToView("userroleeffective");

            builder.HasKey(p=>p.RoleId);
        }
    }
}
