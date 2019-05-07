using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Autofac.Extensions.DependencyInjection;

namespace SFood.ClientEndpoint.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {            
            CreateWebHostBuilder(args)
                .Build().Run();
        }

        // The ConfigureServices call here allows for
        // ConfigureContainer to be supported in Startup with
        // a strongly-typed ContainerBuilder.
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .ConfigureServices(services => services.AddAutofac())
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>();
    }
}
