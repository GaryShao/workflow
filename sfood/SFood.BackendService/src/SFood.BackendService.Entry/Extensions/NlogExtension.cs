using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using System;
using System.Reflection;

namespace SFood.BackendService.Entry.Extensions
{
    public static class NLogExtension
    {
        public static IHostBuilder UseNLog(this IHostBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.ConfigureServices(services =>
            {
                ConfigurationItemFactory.Default.RegisterItemsFromAssembly(
                    typeof(NLogExtension).GetTypeInfo().Assembly);

                LogManager.AddHiddenAssembly(typeof(NLogExtension).GetTypeInfo().Assembly);

                services.AddSingleton(new LoggerFactory().AddNLog(new NLogProviderOptions
                {
                    CaptureMessageTemplates = true,
                    CaptureMessageProperties = true
                }));
            });

            return builder;
        }
    }
}
