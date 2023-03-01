using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Ecubytes.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AuthenticationServiceExtensions
    {
        /// summary
        /// 
        public static AuthenticationBuilder AddDefaultAuthenticationService(this IServiceCollection services,
            IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            Ecubytes.AspNetCore.Authentication.AuthenticationOptions appOptions =
                configuration.GetSection(Ecubytes.AspNetCore.Authentication.AuthenticationOptions.SectionName)
                    .Get<Ecubytes.AspNetCore.Authentication.AuthenticationOptions>();

            if (appOptions == null)
                return services.AddAuthentication();

            //CookieAuthenticationDefaults.AuthenticationScheme
            var builder = services.AddAuthentication(options =>
            {
                options.DefaultScheme = appOptions.DefaultSchema;
                options.DefaultChallengeScheme = appOptions.DefaultChallengeScheme;                
            });

            foreach (var cookieSchemaOptions in appOptions.Schemas.Cookies)
            {
                builder.AddCookie(cookieSchemaOptions.Schema, options =>
                {                    
                    if (!string.IsNullOrWhiteSpace(cookieSchemaOptions.Cookie.Name))
                        options.Cookie.Name = cookieSchemaOptions.Cookie.Name;

                    if (!string.IsNullOrWhiteSpace(cookieSchemaOptions.Cookie.Path))
                        options.Cookie.Path = cookieSchemaOptions.Cookie.Path;

                    options.Cookie.SameSite = cookieSchemaOptions.Cookie.SameSite ?? AspNetCore.Http.SameSiteMode.Lax;
                    options.Cookie.HttpOnly = cookieSchemaOptions.Cookie.HttpOnly;

                    if(cookieSchemaOptions.Cookie.SecurePolicy.HasValue)
                        options.Cookie.SecurePolicy = cookieSchemaOptions.Cookie.SecurePolicy.Value;                    

                    if(!string.IsNullOrWhiteSpace(cookieSchemaOptions.Cookie.Domain))
                        options.Cookie.Domain = cookieSchemaOptions.Cookie.Domain;                    

                    if(cookieSchemaOptions.Cookie.MaxAgeMinutes.HasValue)
                        options.Cookie.MaxAge = TimeSpan.FromMinutes(cookieSchemaOptions.Cookie.MaxAgeMinutes.Value);

                    if (cookieSchemaOptions.ExpireTimeMinutes.HasValue)
                        options.ExpireTimeSpan = TimeSpan.FromMinutes(cookieSchemaOptions.ExpireTimeMinutes.Value);

                    if (!string.IsNullOrWhiteSpace(cookieSchemaOptions.RemoteAccessDeniedPath))
                    {
                        options.Events.OnRedirectToAccessDenied = (context) =>
                        {
                            context.HttpContext.Response.Redirect(cookieSchemaOptions.RemoteAccessDeniedPath);
                            return Task.CompletedTask;
                        };
                    }
                    else if (!string.IsNullOrWhiteSpace(cookieSchemaOptions.AccessDeniedPath))
                    {
                        options.AccessDeniedPath = cookieSchemaOptions.AccessDeniedPath;
                    }                

                    //Apply for Authorization Server (Login Path)
                    if (!string.IsNullOrWhiteSpace(cookieSchemaOptions.RemoteLoginPath))
                    {
                        options.Events.OnRedirectToLogin = (context) =>
                        {
                            string thisBaseUrl = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}";

                            string redirectUri = System.Net.WebUtility.UrlEncode($"{thisBaseUrl}{context.Properties.RedirectUri}");

                            context.HttpContext.Response.Redirect($"{cookieSchemaOptions.RemoteLoginPath}?{context.Options.ReturnUrlParameter}={redirectUri}");
                            return Task.CompletedTask;
                        };
                    }
                    else if (!string.IsNullOrWhiteSpace(cookieSchemaOptions.LoginPath))
                    {
                        options.LoginPath = cookieSchemaOptions.LoginPath;
                    }
                });
            }

            foreach (var openIdSchemaOptions in appOptions.Schemas.OpenIdConnect)
            {
                builder.AddOpenIdConnect(openIdSchemaOptions.Schema, options =>
                {
                    //options.NonceCookie.SameSite = SameSiteMode.Lax;
                    //options.CorrelationCookie.SameSite = SameSiteMode.Lax;

                    options.Resource = openIdSchemaOptions.Resource;
                    //DefaultInboundClaimTypeMap
                    options.SignInScheme = openIdSchemaOptions.SignInScheme;
                    options.Authority = openIdSchemaOptions.Authority;
                    options.SignedOutRedirectUri = openIdSchemaOptions.SignedOutRedirectUri;
                    options.ClientId = openIdSchemaOptions.ClientId;
                    options.ClientSecret = openIdSchemaOptions.ClientSecret;
                    options.ResponseType = openIdSchemaOptions.ResponseType;
                    options.SaveTokens = openIdSchemaOptions.SaveTokens;

                    //options.ClaimActions.Clear();
                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.MapInboundClaims = true;
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("roles");

                    options.ClaimActions.MapUniqueJsonKey("role", "role", ClaimValueTypes.String);

                    //OpenIdConnect does not read the custom claims of the access token 
                    //(only those of the userenpoint), so they are added manually 
                    options.Events.OnTicketReceived = ctx =>
                    {
                        List<Claim> newClaims = new List<Claim>();

                        IEnumerable<Claim> claims = JwtTokenHelper.GetClaimsPrincipal(ctx.Properties.GetTokenValue("access_token"));

                        foreach (var c in claims)
                        {
                            if (!ctx.Principal.HasClaim(p => p.Type == c.Type))
                                newClaims.Add(c);
                        }

                        var appIdentity = new ClaimsIdentity(newClaims,
                            //openIdSchemaOptions.Schema,
                            ctx.Principal.Identity.AuthenticationType,
                            "name", "role");

                        ctx.Principal.AddIdentity(appIdentity);

                        return Task.CompletedTask;
                    };

                    //pass aditional parameters
                    // options.Events.OnRedirectToIdentityProvider = context =>
                    // {
                    //     //if (context.Properties.Items.ContainsKey("connection"))
                    //     context.ProtocolMessage.SetParameter("ecubytesapp", "identity");

                    //     return Task.CompletedTask;
                    // };

                    options.TokenValidationParameters.RoleClaimType = "role";
                    options.TokenValidationParameters.NameClaimType = "name";// <-- add this                                      
                });
            }

            foreach (var jwtBearer in appOptions.Schemas.JwtBearer)
            {
                builder.AddJwtBearer("Bearer", options => {
                    //options = jwtBearer;     
                    options.Authority = jwtBearer.Authority;
                    options.Audience = jwtBearer.Audience;                    
                    
                    options.TokenValidationParameters.NameClaimType = "role";
                    options.TokenValidationParameters.NameClaimType = "name";
                });
            }

            return builder;
        }
    }
}
