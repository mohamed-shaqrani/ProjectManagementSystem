using Autofac;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using ProjectManagementSystem.Api.Features.Authentication.ForgetPassword.Commands;
using ProjectManagementSystem.Api.Features.Authentication.Login;
using ProjectManagementSystem.Api.Features.Authentication.Login.Command;
using ProjectManagementSystem.Api.Features.Authentication.Registration.Command;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.Common.Users;
using ProjectManagementSystem.Api.Features.Common.Users.Queries;
using ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.AddProject.Commands;
using ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.GetProject.Queries;
using ProjectManagementSystem.Api.Helpers;
using ProjectManagementSystem.Api.Repository;

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

        builder.RegisterAssemblyTypes(ThisAssembly)
      .AsClosedTypesOf(typeof(IValidator<>))
      .InstancePerLifetimeScope();


        builder.RegisterAssemblyTypes(typeof(AddProjectHandler).Assembly);
        builder.RegisterAssemblyTypes(typeof(GetProjectsQuery).Assembly);

        // Register all handlers in the assembly
        builder.RegisterAssemblyTypes(typeof(AddProjectHandler).Assembly)
                .Where(x => x.Name.EndsWith("Handler")).AsImplementedInterfaces().InstancePerLifetimeScope();



        // Register all handlers in the assembly
        builder.RegisterAssemblyTypes(typeof(RegisterHandler).Assembly)
               .Where(t => t.Name.EndsWith("Handler"))
               .AsClosedTypesOf(typeof(IRequestHandler<,>))
               .InstancePerLifetimeScope();



        builder.RegisterAssemblyTypes(typeof(ImageService.ImageService).Assembly)
          .Where(a => a.Name.EndsWith("Service"))
          .AsImplementedInterfaces().InstancePerLifetimeScope();


        builder.RegisterType<LoginCommand>()
            .As<IRequestHandler<LoginCommand, ResponseViewModel<AuthanticationModel>>>()
            .InstancePerLifetimeScope();

        builder.RegisterType<IsUserExistQueryHandler>()
             .As<IRequestHandler<IsUserExistQuery, bool>>()
             .InstancePerLifetimeScope();

        builder.RegisterType<GetUserByIDQueryHandler>()
             .As<IRequestHandler<GetUserByIDQuery, UserDTO>>()
             .InstancePerLifetimeScope();

        builder.RegisterType<ProjectAuthorizeHandler>()
              .As<IAuthorizationHandler>()
              .InstancePerDependency();




        builder.RegisterAssemblyTypes(ThisAssembly)
        .AsClosedTypesOf(typeof(IValidator<>))
        .InstancePerLifetimeScope();



    }
}