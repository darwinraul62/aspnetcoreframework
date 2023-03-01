using System;
using System.Linq;

namespace Ecubytes.Identity.User.Data.Models
{
    public enum UserStates : short
    {
        Active = 1,
        Deleted = 999
    }

    public enum UserGroupStates : short
    {
        Active = 1,
        Deleted = 999
    }
    public enum TenantStates : short
    {
        Active = 1,
        Deleted = 999
    }
}
