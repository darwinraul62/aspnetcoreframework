using System;
using Ecubytes.Data;

namespace Ecubytes.Identity.Application.Data.Repositories
{
    public interface IApplicationRepositoryContainer : IUnitOfWork
    {
        IApplicationRepository Application { get; }
        IRedirectUriRepository RedirectUri { get; }
        IApplicationSecretRepository ApplicationSecret { get; }
        IRoleRepository Role { get; }
    }
}
