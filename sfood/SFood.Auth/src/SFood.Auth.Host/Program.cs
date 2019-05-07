using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace SFood.Auth.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var host = CreateWebHostBuilder(args).Build();
            //SeedIdentityServiceData.EnsureSeedData(host.Services);
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
