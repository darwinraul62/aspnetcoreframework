using System;
using System.Linq;
using Autofac;
using Ecubytes.Identity.User.Data.Repositories;
using Ecubytes.Identity.User.Infrastructure.Repositories;
using Uti.Identity.User.Infrastructure;

namespace Ecubytes.Identity.User.Api.Infrastructure.AutofacModules
{
    public class UserDataModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<UserRepositoryContainer>()
                .As<IUserRepositoryContainer>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TenantRepository>()
                .As<ITenantRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserRepository>()
                .As<IUserRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserGroupRepository>()
                .As<IUserGroupRepository>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<UserRoleRepository>()
                .As<IUserRoleRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserGroupRoleRepository>()
                .As<IUserGroupRoleRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserGroupDetailRepository>()
                .As<IUserGroupDetailRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RoleRepository>()
                .As<IRoleRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserLoginRepository>()
                .As<IUserLoginRepository>()
                .InstancePerLifetimeScope();

             builder.RegisterType<OtpRepository>()
                .As<IOtpRepository>()
                .InstancePerLifetimeScope();
                        
        }
    }
}
