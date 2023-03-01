using System;
using System.Linq;
using Ecubytes.Data;

namespace Ecubytes.Identity.User.Data.Repositories
{
    public interface IUserRepositoryContainer : IUnitOfWork
    {
        ITenantRepository Tenant { get; }
        IUserRepository User { get; }
        IUserGroupRepository UserGroup { get; }
        IUserRoleRepository UserRole { get; }
        IUserGroupRoleRepository UserGroupRole { get; }
        IUserGroupDetailRepository UserGroupDetail { get; }
        IRoleRepository Role { get; }
        IUserLoginRepository UserLogin { get; }
        IOtpRepository Otp { get; }
    }
}
