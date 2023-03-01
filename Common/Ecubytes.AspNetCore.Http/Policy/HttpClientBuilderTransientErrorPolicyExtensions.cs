using System;
using System.Net.Http;
using Ecubytes.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HttpClientBuilderTransientErrorPolicyExtensions
    {
        public static IHttpClientBuilder AddRetryPolicy(this IHttpClientBuilder httpClientBuilder,
            int retryCount)
        {            
             return httpClientBuilder.AddTransientHttpErrorPolicy(p => 
                p.RetryAsync(retryCount));
        }

        public static IHttpClientBuilder AddWaitAndRetryPolicy(this IHttpClientBuilder httpClientBuilder,
            int retryCount, Func<int, TimeSpan> sleepDurationProvider)
        {
            return httpClientBuilder.AddTransientHttpErrorPolicy(p => 
                p.WaitAndRetryAsync(retryCount, sleepDurationProvider));
        }

        public static IHttpClientBuilder AddWaitAndRetryPolicy(this IHttpClientBuilder httpClientBuilder,
            int retryCount)
        {
            return httpClientBuilder.AddPolicyHandler(
                HttpTransientErrorPolicyExtensions.GetWaitAndRetryPolicy(retryCount));
        }

        public static IHttpClientBuilder AddWaitAndRetryPolicy(this IHttpClientBuilder httpClientBuilder)
        {
            return httpClientBuilder.AddPolicyHandler(
                HttpTransientErrorPolicyExtensions.GetWaitAndRetryPolicy());
        }

        public static IHttpClientBuilder AddCircuitBreakerPolicy(this IHttpClientBuilder httpClientBuilder,
            int handledEventsAllowedBeforeBreaking, TimeSpan durationOfBreak)
        {            
            return httpClientBuilder.AddPolicyHandler(
                HttpTransientErrorPolicyExtensions.GetCircuitBreakerPolicy(handledEventsAllowedBeforeBreaking, durationOfBreak));
        }

        public static IHttpClientBuilder AddCircuitBreakerPolicy(this IHttpClientBuilder httpClientBuilder)
        {
            return httpClientBuilder.AddPolicyHandler(
                HttpTransientErrorPolicyExtensions.GetCircuitBreakerPolicy());          
        }   
        
        public static IHttpClientBuilder AddCircuitBreakerPolicy(this IHttpClientBuilder httpClientBuilder,
            int handledEventsAllowedBeforeBreaking)
        {
            return httpClientBuilder.AddPolicyHandler(
                HttpTransientErrorPolicyExtensions.GetCircuitBreakerPolicy(handledEventsAllowedBeforeBreaking));          
        }   


    }
}
