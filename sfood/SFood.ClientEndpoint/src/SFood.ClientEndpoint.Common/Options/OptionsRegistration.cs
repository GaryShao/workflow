using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SFood.ClientEndpoint.Common.Options
{
    public static class OptionsRegistration
    {
        public static void RegistryOptions(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<QiNiuOption>(configuration.GetSection("QiNiuOptions"));
        }
    }
}
