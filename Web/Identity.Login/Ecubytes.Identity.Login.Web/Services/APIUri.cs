using System;

namespace Ecubytes.Identity.Login.Web.Services
{
    public static class APIUri
    {
        public static class Login
        {
            public static string CreateLogin() => $"/api/login";
        }

        public static class User 
        {
            public static string GetEffectiveRoles(Guid userId, Guid applicationId) => 
                $"/api/users/{userId}/roles/effective?applicationId={applicationId}";
        }
    }
}
