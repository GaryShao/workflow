using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using SFood.Gateway.Host.Extensions;
using System;
using System.Linq;
using System.Text;

namespace SFood.Gateway.Host
{
    public class Startup
    {
        private const string DefaultCorsPolicy = "default";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOcelot();

            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicy, builder =>
                {
                    //App:CorsOrigins in appsettings.json can contain more than one address with splitted by comma.
                    var origins = Configuration["CorsOrigins"]
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(o => o.RemovePostFix("/"))
                        .ToArray();

                    builder.WithOrigins(origins)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            loggerFactory.AddNLog();
            env.ConfigureNLog("nlog.config");

            app.UseCors(DefaultCorsPolicy);

            app.UseOcelot().Wait();
        }
    }
}
