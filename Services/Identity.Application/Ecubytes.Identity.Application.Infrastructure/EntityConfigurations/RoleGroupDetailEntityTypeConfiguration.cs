using System;
using System.Linq;
using Ecubytes.Identity.Application.Data.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace Ecubytes.Identity.Application.Infrastructure.EntityConfigurations
{
    public class RoleGroupDetailEntityTypeConfiguration : IEntityTypeConfiguration<RoleGroupDetail>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<RoleGroupDetail> builder)
        {
            builder.ToTable("rolegroupdetail", schema:"auth");

            builder.HasKey(p=> new { p.RoleGroupId, p.RoleDetailId });            
        }
    }
}
