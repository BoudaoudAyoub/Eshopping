using Autofac;
using Eshopping.Domain.Seedwork;
using Eshopping.Infrastructure.Repositories;
namespace Eshopping.API.Configuration.Modules;

public class MediatorModule() : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(Repository<,>)).As(typeof(IRepository<,>));

        builder.RegisterAssemblyTypes(typeof(OrderRepository).Assembly)
               .Where(t => t.Name.EndsWith("Repository"))
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();
    }
}