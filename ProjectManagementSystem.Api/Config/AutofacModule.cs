using Autofac;
using MediatR;

namespace ProjectManagementSystem.Api.Config;

public class AutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(IMediator).Assembly)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
    }
}