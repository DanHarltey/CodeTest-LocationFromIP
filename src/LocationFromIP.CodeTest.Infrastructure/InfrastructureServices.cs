using LocationFromIP.CodeTest.Core.Interfaces;
using LocationFromIP.CodeTest.Infrastructure.WeatherApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;

namespace LocationFromIP.CodeTest.Infrastructure
{
    public static class InfrastructureServices
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<WeatherApiConfiguration>(configuration.GetSection("WeatherApi"));

            services.AddDistributedRedisCache(options => 
            {
                options.Configuration = "localhost"; // might need to change this
                options.InstanceName = "IPLookup";
            });

            services.AddHttpClient<WeatherApiClient>()
               .AddPolicyHandler(PollyPolicy.Create())
               .ConfigureHttpClient();

            services.AddScoped<ILocationDetailQuery>(sp =>
            {
                // wrap the WeatherApiClient in a cache
                var weatherApiClient = sp.GetRequiredService<WeatherApiClient>();
                var queryCache = ActivatorUtilities.CreateInstance<LocationDetailQueryCache>(sp, weatherApiClient);
                return queryCache;
            });
        }

        private static IHttpClientBuilder ConfigureHttpClient(this IHttpClientBuilder builder)
        {
            builder
                .ConfigureHttpClient(client =>
                {
                    client.Timeout = TimeSpan.FromSeconds(5);
                    client.DefaultRequestVersion = HttpVersion.Version20;
                })
                .ConfigurePrimaryHttpMessageHandler(messageHandler =>
                {
                    var handler = new HttpClientHandler();

                    if (handler.SupportsAutomaticDecompression)
                    {
                        handler.AutomaticDecompression = DecompressionMethods.All;
                    }

                    // only use secure tls versions
                    handler.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13;
                    return handler;
                });

            return builder;
        }
    }
}
