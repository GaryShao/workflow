using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using NLog.Web;
using SFood.BusinessInfo.Application.AutomapProfiles;
using SFood.BusinessInfo.Host.Filters;

namespace SFood.BusinessInfo.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Automapper
            var mappingConfig = new MapperConfiguration(mc => {
                mc.AddProfile(new MappingProfile());
            });
            services.AddSingleton(mappingConfig.CreateMapper());
            #endregion

            services.AddMvc(config => {
                    config.Filters.Add(typeof(MvcExceptionFilter));
                }).
                AddJsonOptions(opt => {
                    opt.SerializerSettings.ContractResolver
                        = new CamelCasePropertyNamesContractResolver();
                }).
                SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseHsts();

            loggerFactory.AddNLog();
            env.ConfigureNLog("nlog.config");

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
