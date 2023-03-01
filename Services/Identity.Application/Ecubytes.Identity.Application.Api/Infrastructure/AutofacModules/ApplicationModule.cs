using System;
using Autofac;
using Ecubytes.Identity.Application.Data.Repositories;
using Ecubytes.Identity.Application.Infrastructure;
using Ecubytes.Identity.Application.Infrastructure.Repositories;

namespace Ecubytes.Identity.Application.Api.Infrastructure.AutofacModules
{
    public class ApplicationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationRepositoryContainer>()
                .As<IApplicationRepositoryContainer>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationRepository>()
                .As<IApplicationRepository>()
                .InstancePerLifetimeScope();

             builder.RegisterType<ApplicationSecretRepository>()
                .As<IApplicationSecretRepository>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<RedirectUriRepository>()
                .As<IRedirectUriRepository>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<RoleRepository>()
                .As<IRoleRepository>()
                .InstancePerLifetimeScope();
        }
    }
}
