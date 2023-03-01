using System;
using System.Net.Http;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;

namespace Ecubytes.AspNetCore.Http
{
    public static class HttpTransientErrorPolicyExtensions
    {
        public static IAsyncPolicy<HttpResponseMessage> GetWaitAndRetryPolicy(int retryCount = 5)
        {
            var delay = Backoff.AwsDecorrelatedJitterBackoff(minDelay: TimeSpan.FromMilliseconds(10), maxDelay: TimeSpan.FromMilliseconds(100), retryCount: 5);

            // var retryPolicy = Policy
            //     .Handle<FooException>()
            //     .WaitAndRetryAsync(delay);


            return HttpPolicyExtensions
                // Handle HttpRequestExceptions, 408 and 5xx status codes
                .HandleTransientHttpError()
                // // Handle 404 not found
                // .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                // // Handle 401 Unauthorized
                // .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                // What to do if any of the above erros occur:
                // Retry 3 times, each time wait 1,2 and 4 seconds before retrying.
                .WaitAndRetryAsync(delay);
            //.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(
            int handledEventsAllowedBeforeBreaking = 10)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking, TimeSpan.FromSeconds(10));
        }

        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(
            int handledEventsAllowedBeforeBreaking, TimeSpan durationOfBreak)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking, durationOfBreak);
        }
    }
}
