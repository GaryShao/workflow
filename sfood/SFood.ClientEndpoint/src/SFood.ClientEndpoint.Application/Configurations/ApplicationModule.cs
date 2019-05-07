using Autofac;
using SFood.ClientEndpoint.Application.ServiceImplements;
using SFood.ClientEndpoint.Application.ServiceInterfaces;
using SFood.ClientEndpoint.Application.Validator;
using SFood.ClientEndpoint.Application.Validator.Implements;
using SFood.DataAccess.EFCore;
using SFood.DataAccess.Infrastructure.Implements;
using SFood.DataAccess.Infrastructure.Interfaces;

namespace SFood.ClientEndpoint.Application.Configurations
{
    public class ApplicationModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<DishValidator>().As<IDishValidator>().InstancePerLifetimeScope();
            builder.RegisterType<OrderValidator>().As<IOrderValidator>().InstancePerLifetimeScope();

            builder.RegisterType<OrderService>().As<IOrderService>().InstancePerLifetimeScope();
            builder.RegisterType<GeneralService>().As<IGeneralService>().InstancePerLifetimeScope();
            builder.RegisterType<HawkerCenterService>().As<IHawkerCenterService>().InstancePerLifetimeScope();
            builder.RegisterType<RestaurantService>().As<IRestaurantService>().InstancePerLifetimeScope();
            builder.RegisterType<SmsService>().As<ISmsService>().InstancePerLifetimeScope();

            builder.RegisterType<EntityFrameworkReadOnlyRepository<SFoodDbContext>>().As<IReadOnlyRepository>().InstancePerLifetimeScope();
            builder.RegisterType<EntityFrameworkRepository<SFoodDbContext>>().As<IRepository>().InstancePerLifetimeScope();
        }
    }
}
