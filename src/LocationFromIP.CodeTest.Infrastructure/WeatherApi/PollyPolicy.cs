using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace LocationFromIP.CodeTest.Infrastructure.WeatherApi
{
    internal static class PollyPolicy
    {
        internal static IAsyncPolicy<HttpResponseMessage> Create()
        {
            var circuitBreaker = CreateCircuitBreaker();

            var waitAndRetry = HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(0.5),
                });

            return waitAndRetry.WrapAsync(circuitBreaker);
        }

        private static IAsyncPolicy<HttpResponseMessage> CreateCircuitBreaker()
        {
            var httpErrors = HttpPolicyExtensions.HandleTransientHttpError()
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 10,
                    durationOfBreak: TimeSpan.FromSeconds(10));

            var timeouts = Policy.Handle<TaskCanceledException>().CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: 10,
                    durationOfBreak: TimeSpan.FromSeconds(10));

            return httpErrors.WrapAsync(timeouts);
        }
    }
}
