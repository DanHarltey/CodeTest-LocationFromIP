using LocationFromIP.CodeTest.WebApi;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LocationFromIP.CodeTest.Core.IntegrationTests
{
    public class DependencyInjectionTests
    {
        [Fact]
        public async Task DependencyInjectionValidation()
        {
            var builder = Program.CreateHostBuilder(Array.Empty<string>());
            builder.Host.UseDefaultServiceProvider(o =>
            {
                o.ValidateOnBuild = true;
                o.ValidateScopes = true;
            });

            // build will throw if DI is configured incorrect
            await using var host = builder.Build();
        }
    }
}
