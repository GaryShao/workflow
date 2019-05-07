using Autofac;
using SFood.DataAccess.EFCore;
using SFood.DataAccess.Infrastructure.Implements;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.MerchantEndpoint.Application.ServiceImplements;
using SFood.MerchantEndpoint.Application.Validator;
using SFood.MerchantEndpoint.Application.Validator.Implements;

namespace SFood.MerchantEndpoint.Application.Configurations
{
    public class ApplicationModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<DishValidator>().As<IDishValidator>().InstancePerLifetimeScope();
            builder.RegisterType<CustomizationValidator>().As<ICustomizationValidator>().InstancePerLifetimeScope();

            builder.RegisterType<SmsService>().As<ISmsService>().InstancePerLifetimeScope();
            builder.RegisterType<DishService>().As<IDishService>().InstancePerLifetimeScope();
            builder.RegisterType<ImageService>().As<IImageService>().InstancePerLifetimeScope();
            builder.RegisterType<MenuService>().As<IMenuService>().InstancePerLifetimeScope();
            builder.RegisterType<RestaurantService>().As<IRestaurantService>().InstancePerLifetimeScope();
            builder.RegisterType<AccountService>().As<IAccountService>().InstancePerLifetimeScope();
            builder.RegisterType<RestaurantService>().As<IRestaurantService>().InstancePerLifetimeScope();            
            builder.RegisterType<HawkerCenterService>().As<IHawkerCenterService>().InstancePerLifetimeScope();
            builder.RegisterType<RestaurantCategoryService>().As<IRestaurantCategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<DishCategoryService>().As<IDishCategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<OrderService>().As<IOrderService>().InstancePerLifetimeScope();
            builder.RegisterType<StatisticService>().As<IStatisticService>().InstancePerLifetimeScope();
            builder.RegisterType<StaffService>().As<IStaffService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomizationService>().As<ICustomizationService>().InstancePerDependency();


            builder.RegisterType<EntityFrameworkReadOnlyRepository<SFoodDbContext>>().As<IReadOnlyRepository>().InstancePerLifetimeScope();
            builder.RegisterType<EntityFrameworkRepository<SFoodDbContext>>().As<IRepository>().InstancePerLifetimeScope();
        }
    }
}
