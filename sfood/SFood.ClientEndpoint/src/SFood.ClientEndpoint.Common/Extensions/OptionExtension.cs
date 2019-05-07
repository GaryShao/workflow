using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFood.ClientEndpoint.Common.Options;

namespace SFood.ClientEndpoint.Common.Extensions
{
    /// <summary>
    /// 添加系统中需要的所有options
    /// </summary>
    public static class OptionExtension
    {
        public static void AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            OptionsRegistration.RegistryOptions(services, configuration);
        }
    }
}
