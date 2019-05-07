using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SFood.MerchantEndpoint.Common.Options
{
    public static class OptionsRegistration
    {
        public static void RegistryOptions(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<QiNiuOption>(configuration.GetSection("QiNiuOptions"));
            services.Configure<CommsHubOption>(configuration.GetSection("CommsHub"));
            services.Configure<VerificationCodeOption>(configuration.GetSection("VerificationCode"));
        }
    }
}
