using Autofac;
using FluentValidation;
using MediatR;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.AddProject.Commands;
using ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.GetProject;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response.RequestResult;

namespace ProjectManagementSystem.Api.Config;

public class AutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(IMediator).Assembly)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
        builder.RegisterGeneric(typeof(BaseEndpointParam<>))
             .AsSelf()
             .InstancePerLifetimeScope();


        builder.RegisterAssemblyTypes(typeof(AddProjectHandler).Assembly);
        builder.RegisterType<AddProjectHandler>()
             .As<IRequestHandler<AddProjectCommand, RequestResult<bool>>>()
             .InstancePerLifetimeScope();


        builder.RegisterAssemblyTypes(typeof(GetProjectsQueryHandler).Assembly);
        builder.RegisterType<GetProjectsQueryHandler>()
             .As<IRequestHandler<GetProjectsQuery, RequestResult<IEnumerable<ProjectResponseViewModel>>>>()
             .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(ThisAssembly)
        .AsClosedTypesOf(typeof(IValidator<>))
        .InstancePerLifetimeScope();


    }
}