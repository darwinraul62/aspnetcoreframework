using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Ecubytes.AspNetCore.Authentication
{
    public static class JwtTokenHelper
    {
        public static Task<IPrincipal> GetPrincipal(string token, TokenValidationParameters parameters)
        {
            ClaimsPrincipal principal = GetValidatedClaimsPrincipal(token, parameters);
            if (principal == null)
                return null;
                
            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
                IPrincipal Iprincipal = new ClaimsPrincipal(identity);
                return Task.FromResult(Iprincipal);
            }
            catch (NullReferenceException)
            {
                return Task.FromResult<IPrincipal>(null);
            }
        }

        public static IEnumerable<Claim> GetClaimsPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null;
                
                return jwtToken.Claims;                                   
            }
            catch
            {
                return null;
            }
        }

        public static ClaimsPrincipal GetValidatedClaimsPrincipal(string token, TokenValidationParameters parameters)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null;
                    
                SecurityToken securityToken;

                ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
                      parameters, out securityToken);

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
