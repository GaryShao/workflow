using Autofac;
using SFood.ClientEndpoint.Common.Utilities;
using SFood.ClientEndpoint.Common.Utilities.Implements;

namespace SFood.ClientEndpoint.Common
{
    public class CommonModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<QiNiuUtility>().As<IQiNiuUtility>().InstancePerLifetimeScope();
            builder.RegisterType<SecurityUtility>().As<ISecurityUtility>().InstancePerLifetimeScope();
            builder.RegisterType<SmsUtility>().As<ISmsUtility>().InstancePerLifetimeScope();
        }
    }
}
