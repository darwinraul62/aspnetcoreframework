using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Ecubytes.Identity.User.Infrastructure.EntityConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<Data.Models.User>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Data.Models.User> builder)
        {
            builder.ToTable("IdentityUser");
            
            builder.HasKey(p => p.UserId);

            builder.HasOne(p => p.Tenant).WithMany().HasForeignKey(p => p.TenantId);
        }
    }
}
