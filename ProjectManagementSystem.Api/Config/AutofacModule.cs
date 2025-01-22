using Autofac;
using FluentValidation;
using MediatR;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.Common.Users;
using ProjectManagementSystem.Api.Features.Common.Users.Queries;
using ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.AddProject.Commands;
using ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.GetProject;
using ProjectManagementSystem.Api.Helpers;
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

        builder.RegisterType<AddTaskCommandHandler>()
             .As<IRequestHandler<AddTaskCommand, RequestResult<bool>>>()
             .InstancePerLifetimeScope();

        builder.RegisterType<IsUserExistQueryHandler>()
             .As<IRequestHandler<IsUserExistQuery, bool>>()
             .InstancePerLifetimeScope();

        builder.RegisterType<GetUserByIDQueryHandler>()
             .As<IRequestHandler<GetUserByIDQuery, UserDTO>>()
             .InstancePerLifetimeScope();


        builder.RegisterAssemblyTypes(typeof(GetProjectsQueryHandler).Assembly);
        builder.RegisterType<GetProjectsQueryHandler>()
             .As<IRequestHandler<GetProjectsQuery, RequestResult<PageList<ProjectResponseViewModel>>>>()
             .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(ThisAssembly)
        .AsClosedTypesOf(typeof(IValidator<>))
        .InstancePerLifetimeScope();


    }
}