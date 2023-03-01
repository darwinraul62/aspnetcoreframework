using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

namespace Ecubytes.AspNetCore.Authentication
{
    public class AuthenticationOptions
    {
        private AuthenticationSchemasOptions schemas;

        public const string SectionName = "Ecubytes:Authentication";
        public string DefaultSchema { get; set; }
        public string DefaultChallengeScheme { get; set; }
        public string Name { get; set; }
        public AuthenticationSchemasOptions Schemas
        {
            get
            {
                return schemas ?? (schemas = new AuthenticationSchemasOptions());
            }
            set
            {
                this.schemas = value;
            }
        }
    }

    public class AuthenticationSchemasOptions
    {
        private List<CookieAuthenticationSchemaOptions> cookies;
        private List<OpenIdSchemaOptions> openIdConnect;
        private List<JwtBearerSchemaOptions> jwtBearer;

        public List<CookieAuthenticationSchemaOptions> Cookies
        {
            get
            {
                return cookies ?? (cookies = new List<CookieAuthenticationSchemaOptions>());
            }
            set
            {
                this.cookies = value;
            }
        }
        public List<OpenIdSchemaOptions> OpenIdConnect
        {
            get
            {
                return openIdConnect ?? (openIdConnect = new List<OpenIdSchemaOptions>());
            }
            set
            {
                this.openIdConnect = value;
            }
        }

        public List<JwtBearerSchemaOptions> JwtBearer
        {
            get
            {
                return jwtBearer ?? (jwtBearer = new List<JwtBearerSchemaOptions>());
            }
            set
            {
                this.jwtBearer = value;
            }
        }
    }

    public class AuthenticationSchemaOptions
    {
        public string Schema { get; set; }
    }

    public class CookieAuthenticationCookieOptions : AuthenticationSchemaOptions
    {
        public string Name { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public SameSiteMode? SameSite { get; set; }
        public bool HttpOnly { get; set; } = true;
        public int? MaxAgeMinutes { get; set; }
        public CookieSecurePolicy? SecurePolicy { get; set; }
    }

    public class CookieAuthenticationSchemaOptions : AuthenticationSchemaOptions
    {
        public CookieAuthenticationCookieOptions Cookie { get; set; }
        public string RemoteLoginPath { get; set; }
        public string LoginPath { get; set; }
        public int? ExpireTimeMinutes { get; set; }
        public string AccessDeniedPath { get; set; }
        public string RemoteAccessDeniedPath { get; set; }
    }

    public class OpenIdSchemaOptions : AuthenticationSchemaOptions
    {
        public string SignInScheme { get; set; }
        public string Authority { get; set; }
        public string SignedOutRedirectUri { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ResponseType { get; set; }
        public bool SaveTokens { get; set; }
        public string Resource { get; set; }
    }

    public class JwtBearerSchemaOptions : AuthenticationSchemaOptions
    {
        public string Authority { get; set; }
        public string Audience { get; set; }
    }
}
