using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using SFood.BackendService.Application.ServiceImplements;
using SFood.BackendService.Application.ServiceInterfaces;
using SFood.BackendService.Common.Options;
using SFood.DataAccess.EFCore;
using SFood.DataAccess.Infrastructure.Implements;
using SFood.DataAccess.Infrastructure.Interfaces;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace SFood.BackendService.Entry
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureLogging(factory =>
                {
                    factory.AddConsole();
                    factory.AddNLog(new NLogProviderOptions {
                        CaptureMessageTemplates = true,
                        CaptureMessageProperties = true
                    });
                    LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();
                })
                .ConfigureHostConfiguration(config => {
                    config.AddEnvironmentVariables();
                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }
                })
                 .ConfigureAppConfiguration((hostContext, config) =>
                 {
                     var environmentVariable = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                     if (!string.IsNullOrEmpty(environmentVariable))
                     {
                         hostContext.HostingEnvironment.EnvironmentName = environmentVariable;
                     }                     
                     var env = hostContext.HostingEnvironment;

                     config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                         .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true);                                         
                 })
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<OrderExpirationOptions>(hostContext.Configuration.GetSection("BackendTasks:OrderExpirationTask"));
                    services.AddOptions();                    

                    services.AddScoped<IOrderService, OrderService>();
                    services.AddScoped<IReadOnlyRepository, EntityFrameworkReadOnlyRepository<SFoodDbContext>>();
                    services.AddScoped<IRepository, EntityFrameworkRepository<SFoodDbContext>>();

                    services.AddDbContext<SFoodDbContext>(options =>
                    {
                        options.UseSqlServer(hostContext.Configuration.GetConnectionString("SFoodDatabase"));
                    }, ServiceLifetime.Scoped);

                    services.AddSingleton(typeof(IConnectionMultiplexer), ConnectionMultiplexer.Connect(hostContext.Configuration["Redis:Configuration"]));

                    services.AddHostedService<OrderHostedService>();                    
                })
                .UseConsoleLifetime()
                .Build();

            await host.RunAsync();
        }
    }
}
