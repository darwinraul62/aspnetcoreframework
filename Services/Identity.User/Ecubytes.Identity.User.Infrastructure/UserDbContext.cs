using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Ecubytes.Identity.User.Data.Models;
using Ecubytes.Identity.User.Infrastructure.EntityConfigurations;

namespace Ecubytes.Identity.User.Infrastructure
{
    public class UserDbContext : Ecubytes.Data.EntityFramework.DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
           : base(options)
        {
        }
        public DbSet<Tenant> Tenants { get; set; }        
        public DbSet<Data.Models.User> Users { get; set; }
        public DbSet<Data.Models.UserAttribute> UserAttributes { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserGroupRole> UserGroupRoles { get; set; }
        public DbSet<UserGroupDetail> UserGroupDetails { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRoleEffective> UserRoleEffective { get; set; }
        public DbSet<UserInfo> UsersInfo { get; set; }
        public DbSet<UserLogin> UsersLogin { get; set; }
        public DbSet<Otp> Otps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new TenantEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserAttributeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserGroupEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserGroupRoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserGroupDetailEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleEffectiveEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserInfoEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserLoginEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OtpEntityTypeConfiguration());

            modelBuilder.ToLowerCaseEntities();
        }
    }
}
