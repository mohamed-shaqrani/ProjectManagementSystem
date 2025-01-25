using Autofac;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using ProjectManagementSystem.Api.Features.Authentication.Login;
using ProjectManagementSystem.Api.Features.Authentication.Registration.Command;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.Common.Users;
using ProjectManagementSystem.Api.Features.Common.Users.Queries;
using ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.AddProject.Commands;
using ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.GetProject;
using ProjectManagementSystem.Api.Features.TasksManagement.Tasks.AddTask.Commands;
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

        builder.RegisterAssemblyTypes(typeof(RegisterHandler).Assembly)
               .Where(a => a.Name.EndsWith("Handler"))
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

        builder.RegisterType<RegisterHandler>()
               .As<IRequestHandler<RegisterCommand, RequestResult<AuthModel>>>()
               .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(typeof(ImageService.ImageService).Assembly)
          .Where(a => a.Name.EndsWith("Service"))
          .AsImplementedInterfaces().InstancePerLifetimeScope();

        builder.RegisterType<AddTaskCommandHandler>()
             .As<IRequestHandler<AddTaskCommand, RequestResult<bool>>>()
             .InstancePerLifetimeScope();

        //builder.RegisterType<LoginCommand>()
        //    .As<IRequestHandler<LoginCommand, ResponseViewModel<AuthanticationModel>>>()
        //    .InstancePerLifetimeScope();

        builder.RegisterType<IsUserExistQueryHandler>()
             .As<IRequestHandler<IsUserExistQuery, bool>>()
             .InstancePerLifetimeScope();

        builder.RegisterType<GetUserByIDQueryHandler>()
             .As<IRequestHandler<GetUserByIDQuery, UserDTO>>()
             .InstancePerLifetimeScope();
        builder.RegisterType<ProjectAuthorizeHandler>()
              .As<IAuthorizationHandler>()
              .InstancePerDependency();

        builder.RegisterType<GetProjectsQueryHandler>()
             .As<IRequestHandler<GetProjectsQuery, RequestResult<PageList<ProjectResponseViewModel>>>>()
             .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(ThisAssembly)
        .AsClosedTypesOf(typeof(IValidator<>))
        .InstancePerLifetimeScope();


    }
}