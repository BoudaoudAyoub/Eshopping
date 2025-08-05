using Autofac;
using MediatR;
using FluentValidation;
using Eshopping.API.Application.Behaviors;
using Eshopping.API.Application.CommandsValidations;
namespace Eshopping.API.Configuration.Modules;

public class ApplicationModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();

        builder.Register<ServiceFactory>(ctx =>
        {
            var c = ctx.Resolve<IComponentContext>();
            return t => c.Resolve(t);
        });       

        builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
               .AsClosedTypesOf(typeof(IRequestHandler<,>))
               .AsImplementedInterfaces();

        builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
               .AsClosedTypesOf(typeof(INotificationHandler<>))
               .AsImplementedInterfaces();

        builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
               .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
               .AsImplementedInterfaces();

        builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        builder.RegisterGeneric(typeof(TransactionBehavior<,>)).As(typeof(IPipelineBehavior<,>));
    }
}