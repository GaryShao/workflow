using Autofac;
using SFood.MerchantEndpoint.Common.Providers.Implements;
using SFood.MerchantEndpoint.Common.Providers.Interfaces;
using SFood.MerchantEndpoint.Common.Utilities;
using SFood.MerchantEndpoint.Common.Utilities.Implements;

namespace SFood.MerchantEndpoint.Common
{
    public class CommonModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<QiNiuUtility>().As<IQiNiuUtility>().InstancePerLifetimeScope();
            builder.RegisterType<SecurityUtility>().As<ISecurityUtility>().InstancePerLifetimeScope();
            builder.RegisterType<SmsUtility>().As<ISmsUtility>().InstancePerLifetimeScope();
            //builder.RegisterType<LocalizationUtility>().As<ILocalizationUtility>().InstancePerLifetimeScope();

            //builder.RegisterType<PersistedResourceProvider>().As<IResourceProvider>().InstancePerLifetimeScope();
        }
    }
}
