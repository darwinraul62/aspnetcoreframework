using System;
using System.Net.Http;
using Ecubytes.AspNetCore.Http;
using Ecubytes.AspNetCore.WebUtilities;
using Ecubytes.AspNetCore.WebUtilities.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HttpClientHandlerExtensions
    {
        public static IServiceCollection AddDefaultHttpHandlers(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //register delegating handlers
            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();
            services.AddTransient<HttpClientRequestIdDelegatingHandler>();
            services.AddTransient<HttpClientDefaultTenantIdDelegatingHandler>();

            services.AddHttpClient("extendedhandlerlifetime").SetHandlerLifetime(TimeSpan.FromMinutes(5));

            services.Configure<ApiProfileCollectionOptions>(configuration.GetSection(
                                        ApiProfileCollectionOptions.SectionName));
            
            services.AddScoped<ApiProfileManager>();

            return services;
        }

        public static IHttpClientBuilder AddHttpClientAuthorizationHandler(this IHttpClientBuilder httpClientBuilder)
        {
            
            httpClientBuilder.AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            //AddTransientHttpErrorPolicy(p=>p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
            return httpClientBuilder;                                                
        }

        public static IHttpClientBuilder AddHttpClientRequestIdHandler(this IHttpClientBuilder httpClientBuilder)
        {
            httpClientBuilder.AddHttpMessageHandler<HttpClientRequestIdDelegatingHandler>();
            return httpClientBuilder;
        }

        public static IHttpClientBuilder AddHttpClientDefaultTenantIdHandler(this IHttpClientBuilder httpClientBuilder)
        {
            httpClientBuilder.AddHttpMessageHandler<HttpClientDefaultTenantIdDelegatingHandler>();
            return httpClientBuilder;
        }
    }
}