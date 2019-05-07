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
using SFood.ClientEndpoint.Application.Configurations;
using SFood.ClientEndpoint.Common;
using SFood.ClientEndpoint.Common.Consts;
using SFood.ClientEndpoint.Common.Extensions;
using SFood.ClientEndpoint.Host.Filters;
using SFood.DataAccess.EFCore;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Text;
using IdentityServer4.AccessTokenValidation;
using Microsoft.IdentityModel.Logging;
using SFood.ClientEndpoint.Common.Options;

namespace SFood.ClientEndpoint.Host
{
    public class Startup
    {
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly string _connectionString;
        private readonly string corsName = "all";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _mapperConfiguration = new MapperConfiguration(mc => { mc.AddProfile(new ApplicationProfile()); });

            _connectionString = Configuration.GetConnectionString(AppConsts.SFoodDatabaseConnectionString);
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SFoodDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SFoodDatabase"));
            });

            services.AddHttpClient("ShortMsg",
                x => { x.BaseAddress = new Uri(Configuration["SMS:BaseUrl"]); });

            services.AddOptions(Configuration);
            services.Configure<VerificationCodeOption>(Configuration.GetSection("VerificationCode"));
            services.Configure<CommsHubOption>(Configuration.GetSection("CommsHub"));

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

            services.AddMvc(config => {
                config.Filters.Add(typeof(ValidateModelFilter));
                config.Filters.Add(typeof(CustomExceptionFilter));
                config.Filters.Add(typeof(TransactionActionFilter));
                }).
                AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.ContractResolver
                        = new CamelCasePropertyNamesContractResolver();
                }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

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
            builder.Register(cm =>
                    ConnectionMultiplexer.Connect(Configuration["Redis:Configuration"])).As<IConnectionMultiplexer>()
                .SingleInstance();

            //var optionsBuilder = new DbContextOptionsBuilder<SFoodDbContext>();
            //builder.RegisterType<SFoodDbContext>().AsSelf()
            //    .WithParameter("options", optionsBuilder.Options)
            //    .InstancePerLifetimeScope();
        }
    }
}