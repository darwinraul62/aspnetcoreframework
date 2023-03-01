using System;
using Ecubytes.Identity.Application.Data.Models.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecubytes.Identity.Application.Infrastructure.EntityConfigurations
{
    public class ApplicationStateEntityTypeConfiguration :
        IEntityTypeConfiguration<ApplicationState>
    {
        public void Configure(EntityTypeBuilder<ApplicationState> builder)
        {
            builder.ToTable("applicationstate", schema: "app");   
            builder.HasKey(p=>p.StateId);         
        }
    }
}
