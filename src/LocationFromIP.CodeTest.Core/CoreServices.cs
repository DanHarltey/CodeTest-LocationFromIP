using LocationFromIP.CodeTest.Core.Interactors;
using LocationFromIP.CodeTest.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LocationFromIP.CodeTest.Core
{
    public static class CoreServices
    {
        public static void Register(IServiceCollection services) =>
            services.AddScoped<ILocationDetailQueryInteractor, LocationDetailQueryInteractor>();
    }
}
