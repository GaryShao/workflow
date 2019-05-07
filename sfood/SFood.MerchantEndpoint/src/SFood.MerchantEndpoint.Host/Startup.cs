using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using NLog.Web;
using SFood.DataAccess.EFCore;
using SFood.MerchantEndpoint.Application.Configurations;
using SFood.MerchantEndpoint.Common;
using SFood.MerchantEndpoint.Common.Consts;
using SFood.MerchantEndpoint.Common.Extensions;
using SFood.MerchantEndpoint.Host.Filters;
using System;
using System.Linq;
using System.Text;

namespace SFood.MerchantEndpoint.Host
{
    public class Startup
    {
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly string _connectionString;
        private readonly string _redisConfig;
        private readonly string corsName = "all";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _mapperConfiguration = new MapperConfiguration(mc => {
                mc.AddProfile(new ApplicationProfile());
            });

            _connectionString = Configuration.GetConnectionString(AppConsts.SFoodDatabaseConnectionString);
            _redisConfig = Configuration.GetValue<string>("Redis:Configuration");
        }

        public IConfiguration Configuration { get; }        

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient("CommsHub",
                x => {
                    x.BaseAddress = new Uri(Configuration["CommsHub:BaseUrl"]);
                });

            services.AddOptions(Configuration);

            services.AddAuthentication((options) =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters();
                options.RequireHttpsMetadata = false;
                options.Audience = Configuration.GetValue<string>("Authentication:Audience");
                options.Authority = Configuration.GetValue<string>("Authentication:Authority");
            });

            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });

            services.AddMvc(options => {                
                options.Filters.Add(typeof(ValidateModelFilter));
                options.Filters.Add(typeof(CustomExceptionFilter));
                options.Filters.Add(typeof(TransactionActionFilter));
                }).
                AddJsonOptions(opt => {                    
                    opt.SerializerSettings.ContractResolver
                        = new CamelCasePropertyNamesContractResolver();
                }).
                SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddCors(
                options => options.AddPolicy(
                    corsName,
                    coreBuilder => coreBuilder
                        .WithOrigins(
                            Configuration["CorsOrigins"]
                                .Split(" | ", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostfix("/"))
                                .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                )
            );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAuthentication();
            
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            loggerFactory.AddNLog();
            env.ConfigureNLog("nlog.config");

            app.UseCors(corsName);
            app.UseMvc();
        }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you. 
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<ApplicationModule>();
            builder.RegisterModule<CommonModule>();
            builder.Register(_ => _mapperConfiguration.CreateMapper()).As<IMapper>().SingleInstance();
            //builder.Register(_ => ConnectionMultiplexer.Connect(_redisConfig)).As<IConnectionMultiplexer>().SingleInstance();

            var optionsBuilder = new DbContextOptionsBuilder<SFoodDbContext>();
            optionsBuilder.UseSqlServer(_connectionString);

            builder.RegisterType<SFoodDbContext>().AsSelf()
                .WithParameter("options", optionsBuilder.Options)
                .InstancePerLifetimeScope();
        }
    }
}
