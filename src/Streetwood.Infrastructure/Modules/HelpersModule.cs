using Autofac;
using Streetwood.Infrastructure.Services.Abstract.Helpers;
using Streetwood.Infrastructure.Services.Implementations.Helpers;

namespace Streetwood.Infrastructure.Modules
{
    public class HelpersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProductOrderHelper>().As<IProductOrderHelper>().InstancePerLifetimeScope();
        }
    }
}
