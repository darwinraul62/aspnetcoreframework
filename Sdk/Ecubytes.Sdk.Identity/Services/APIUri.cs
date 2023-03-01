using System;
using Ecubytes.Data.Common;

namespace Ecubytes.Identity.Services
{
    internal static class APIUri
    {
        public static class Login
        {
            public static string CreateLogin(string baseAddress) => $"{baseAddress}/api/login";
            public static string LoginCount(string baseAddress, DateTime dateFrom, DateTime dateTo, Guid? userId) 
                => $"{baseAddress}/api/login/count?dateFrom={dateFrom.ToString("s")}&dateTo={dateTo.ToString("s")}&userId={userId}";
            public static string UserOnlineCount(string baseAddress) 
                => $"{baseAddress}/api/login/online/count";
            public static string SetLastAccess(string baseAddress, Guid userId) 
                => $"{baseAddress}/api/login/{userId}/lastaccess";
        }

        public static class User 
        {
            public static string Get(string baseAddress, QueryRequest queryRequest) => queryRequest.ToQueryString($"{baseAddress}/api/users");
            
            public static string Get(string baseAddress, Guid userId) => 
                $"{baseAddress}/api/users/{userId}";
            
            public static string RegisterCount(string baseAddress, DateTime dateFrom, DateTime dateTo) 
                => $"{baseAddress}/api/users/registers/count?dateFrom={dateFrom.ToString("s")}&dateTo={dateTo.ToString("s")}";

            public static string CheckEmailUsed(string baseAddress, string email) => $"{baseAddress}/api/users/emails/used?email={email}";
            public static string CheckLogonNameUsed(string baseAddress, string logonName) => $"{baseAddress}/api/users/logonnames/used?logoname={logonName}";

            public static string GetUserGroups(string baseAddress, Guid userId) => 
                $"{baseAddress}/api/users/{userId}/groups";

            public static string GetEffectiveRoles(string baseAddress,Guid userId, Guid applicationId) => 
                $"{baseAddress}/api/users/{userId}/roles/effective?applicationId={applicationId}";

            public static string Create(string baseAddress) => 
                $"{baseAddress}/api/users";

            public static string Update(string baseAddress, Guid userId) => 
                $"{baseAddress}/api/users/{userId}";

            public static string ChangePassword(string baseAddress, Guid userId) => 
                $"{baseAddress}/api/users/{userId}/password";
            
            public static string Delete(string baseAddress, Guid userId) => 
                $"{baseAddress}/api/users/{userId}";

            public static string AddUserGroup(string baseAddress, Guid userId) => 
                $"{baseAddress}/api/users/{userId}/groups";

            public static string RemoveUserGroup(string baseAddress, Guid userId, Guid userGroupId) => 
                $"{baseAddress}/api/users/{userId}/groups/{userGroupId}";
            
            public static string RemoveAllUserGroups(string baseAddress, Guid userId) => 
                $"{baseAddress}/api/users/{userId}/groups";
        }

        public static class UserGroup
        {
            public static string Get(string baseAddress, QueryRequest queryRequest) => queryRequest.ToQueryString($"{baseAddress}/api/usergroups");
            
            public static string Get(string baseAddress, Guid userGroupId) => 
                $"{baseAddress}/api/usergroups/{userGroupId}";

            public static string Create(string baseAddress) => 
                $"{baseAddress}/api/usergroups";

            public static string Update(string baseAddress, Guid userGroupId) => 
                $"{baseAddress}/api/usergroups/{userGroupId}";
            
            public static string Delete(string baseAddress, Guid userGroupId) => 
                $"{baseAddress}/api/usergroups/{userGroupId}";
        }
        public static class Otp
        {
            public static string Generate(string baseAddress) => 
                $"{baseAddress}/api/otp/generate";
            
            public static string Activate(string baseAddress) => 
                $"{baseAddress}/api/otp/activate";
        }
        
    }
}
