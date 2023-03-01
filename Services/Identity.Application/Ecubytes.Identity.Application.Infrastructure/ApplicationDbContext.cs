using System;
using Ecubytes.Data.EntityFramework;
using Ecubytes.Identity.Application.Data.Models.App;
using Ecubytes.Identity.Application.Data.Models.Auth;
using Ecubytes.Identity.Application.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Ecubytes.Identity.Application.Infrastructure
{
    public class ApplicationDbContext : Ecubytes.Data.EntityFramework.DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        public DbSet<Application.Data.Models.App.Application> Applications { get; init; }
        public DbSet<ApplicationState> ApplicationStates { get; set; }
        public DbSet<ApplicationSecret> ApplicationSecrets { get; set; }
        public DbSet<RedirectUri> RedirectUris { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleGroupDetail> RoleGroupDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ApplicationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationStateEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PlatformEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RedirectUriEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationSecretEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RoleGroupDetailEntityTypeConfiguration());

            modelBuilder.ToLowerCaseEntities();
        }
    }
}
