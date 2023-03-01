using System;
using System.Linq;
using Ecubytes.Identity.User.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecubytes.Identity.User.Infrastructure.EntityConfigurations
{
    public class OtpEntityTypeConfiguration : IEntityTypeConfiguration<Otp>
    {
        public void Configure(EntityTypeBuilder<Otp> builder)
        {
            builder.ToTable("otp");

            builder.HasKey(p => p.Id);
        }
    }
}
