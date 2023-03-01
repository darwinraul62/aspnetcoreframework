using System;
using System.Linq;
using Ecubytes.Identity.User.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecubytes.Identity.User.Infrastructure.EntityConfigurations
{
    public class UserInfoEntityTypeConfiguration : IEntityTypeConfiguration<UserInfo>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<UserInfo> builder)
        {
            builder.ToView("userinfo");
            
            builder.HasKey(p=>p.UserId);        
        }
    }
}
