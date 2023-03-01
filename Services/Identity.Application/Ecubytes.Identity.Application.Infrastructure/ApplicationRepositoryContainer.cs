using System;
using System.Threading;
using System.Threading.Tasks;
using Ecubytes.Identity.Application.Data.Repositories;
using Ecubytes.Identity.Application.Infrastructure.Repositories;

namespace Ecubytes.Identity.Application.Infrastructure
{
    public class ApplicationRepositoryContainer : Ecubytes.Data.EntityFramework.RepositoryContainer<ApplicationDbContext>,
        IApplicationRepositoryContainer
    {
        private IApplicationRepository applicationRepository;
        private IApplicationSecretRepository applicationSecretRepository;
        private IRedirectUriRepository redirectUriRepository;
        private IRoleRepository roleRepository;
        private IServiceProvider serviceProvider;

        public ApplicationRepositoryContainer(ApplicationDbContext Context, IServiceProvider serviceProvider)
            : base(Context)
        {
            this.serviceProvider = serviceProvider;
        }

        public IApplicationRepository Application => applicationRepository ??
            (applicationRepository = (IApplicationRepository)serviceProvider.GetService(typeof(IApplicationRepository)));

        public IRedirectUriRepository RedirectUri => redirectUriRepository ??
            (redirectUriRepository = (IRedirectUriRepository)serviceProvider.GetService(typeof(IRedirectUriRepository)));

        public IApplicationSecretRepository ApplicationSecret => applicationSecretRepository ??
            (applicationSecretRepository = (IApplicationSecretRepository)serviceProvider.GetService(typeof(IApplicationSecretRepository)));
        
        public IRoleRepository Role => roleRepository ??
            (roleRepository = (IRoleRepository)serviceProvider.GetService(typeof(IRoleRepository)));
    }
}
