using System;
using System.Linq;
using Ecubytes.Data.EntityFramework;
using Ecubytes.Identity.User.Data.Repositories;
using Ecubytes.Identity.User.Infrastructure;

namespace Uti.Identity.User.Infrastructure
{
    public class UserRepositoryContainer : RepositoryContainer<UserDbContext>, IUserRepositoryContainer
    {
        private ITenantRepository tenantRepository;
        private IUserRepository userRepository;
        private IUserGroupRepository userGroupRepository;
        private IUserRoleRepository userRoleRepository;
        private IUserGroupRoleRepository userGroupRoleRepository;
        private IUserGroupDetailRepository userGroupDetailRepository;
        private IRoleRepository roleRepository;
        private IUserLoginRepository userLoginRepository;
        private IOtpRepository otpRepository;
        private IServiceProvider serviceProvider;

        public UserRepositoryContainer(UserDbContext context, IServiceProvider serviceProvider)
            : base(context)
        {
            this.serviceProvider = serviceProvider;
        }

        public ITenantRepository Tenant => tenantRepository ??
            (tenantRepository = (ITenantRepository)serviceProvider.GetService(typeof(ITenantRepository)));

        public IUserRepository User => userRepository ??
            (userRepository = (IUserRepository)serviceProvider.GetService(typeof(IUserRepository)));

        public IUserGroupRepository UserGroup => userGroupRepository ??
            (userGroupRepository = (IUserGroupRepository)serviceProvider.GetService(typeof(IUserGroupRepository)));

        public IUserRoleRepository UserRole => userRoleRepository ??
            (userRoleRepository = (IUserRoleRepository)serviceProvider.GetService(typeof(IUserRoleRepository)));

        public IUserGroupRoleRepository UserGroupRole => userGroupRoleRepository ??
            (userGroupRoleRepository = (IUserGroupRoleRepository)serviceProvider.GetService(typeof(IUserGroupRoleRepository)));
        
        public IUserGroupDetailRepository UserGroupDetail => userGroupDetailRepository ??
            (userGroupDetailRepository = (IUserGroupDetailRepository)serviceProvider.GetService(typeof(IUserGroupDetailRepository)));

        public IRoleRepository Role => roleRepository ??
            (roleRepository = (IRoleRepository)serviceProvider.GetService(typeof(IRoleRepository)));

        public IUserLoginRepository UserLogin => userLoginRepository ?? 
            (userLoginRepository = (IUserLoginRepository)serviceProvider.GetService(typeof(IUserLoginRepository)));
        public IOtpRepository Otp => otpRepository ?? 
            (otpRepository = (IOtpRepository)serviceProvider.GetService(typeof(IOtpRepository)));
    }
}
