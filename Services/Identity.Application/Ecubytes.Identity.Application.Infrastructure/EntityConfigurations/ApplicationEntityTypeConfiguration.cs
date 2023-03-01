using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ecubytes.Identity.Application.Data.Models.App;

namespace Ecubytes.Identity.Application.Infrastructure.EntityConfigurations
{
    public class ApplicationEntityTypeConfiguration : 
        IEntityTypeConfiguration<Data.Models.App.Application>
    {
         public void Configure(EntityTypeBuilder<Data.Models.App.Application> builder)
        {
            builder.ToTable("application", schema: "app");
            
            builder.HasKey(ci=> ci.ApplicationId);

            builder.HasOne(cb=>cb.State).WithMany().HasForeignKey(cb=>cb.StateId);
        }
    }
}
