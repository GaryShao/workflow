using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using NLog.Web;
using SFood.Auth.Host.Filters;
using SFood.Auth.Host.Validators;
using SFood.DataAccess.Infrastructure.Implements;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.DataAccess.Models.IdentityModels;
using System;
using System.Reflection;
using System.Text;

namespace SFood.Auth.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var authDatabase = Configuration.GetConnectionString("AuthDb");
            var sfoodDatabase = Configuration.GetConnectionString("SFoodDb");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<ApplicationDbContext>(builder => {
                builder.UseSqlServer(sfoodDatabase, options =>
                    options.MigrationsAssembly(migrationsAssembly));
            });

            services.AddTransient<IProfileService, Services.ProfileService<User>>();
            services.AddScoped<IRepository, EntityFrameworkRepository<ApplicationDbContext>>();
            services.AddScoped<IReadOnlyRepository, EntityFrameworkReadOnlyRepository<ApplicationDbContext>>();
            services.AddHttpClient("FetchToken",
                x => {
                    x.BaseAddress = new Uri(Configuration.GetValue<string>("Authentication:Authority"));
                });


            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });

            services.AddMvc(config => {                    
                    config.Filters.Add(typeof(MvcExceptionFilter));
                }).
                AddJsonOptions(opt => {
                    opt.SerializerSettings.ContractResolver
                        = new CamelCasePropertyNamesContractResolver();
                }).
                SetCompatibilityVersion(CompatibilityVersion.Version_2_1);            

            services.AddIdentity<User, Role>()  
                //.AddUserValidator<NewUserValidator>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()                                
                .AddDeveloperSigningCredential(true, "tempkey.rsa")                   
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(authDatabase,
                    sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(authDatabase,
                    sql => sql.MigrationsAssembly(migrationsAssembly));
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 30;
                })
                .AddAspNetIdentity<User>()                
                .AddResourceOwnerValidator<MerchantPasswordValidator<User>>()
                .AddProfileService<Services.ProfileService<User>>();

            services.AddAuthentication();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseIdentityServer();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            loggerFactory.AddNLog();
            env.ConfigureNLog("nlog.config");            

            app.UseMvc();
        }
    }
}
