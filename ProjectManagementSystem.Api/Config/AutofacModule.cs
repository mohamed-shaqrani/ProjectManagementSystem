using Autofac;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.GetProject.Queries;
using ProjectManagementSystem.Api.Filters;
using ProjectManagementSystem.Api.Helpers;
using ProjectManagementSystem.Api.Repository;

namespace ProjectManagementSystem.Api.Config;
public class AutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UnitOfWork>()
               .As<IUnitOfWork>()
               .InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(BaseEndpointParam<>))
                .AsSelf()
                .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(ThisAssembly)
                .AsClosedTypesOf(typeof(IValidator<>))
                .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(typeof(ImageService.ImageService).Assembly)
                .Where(a => a.Name.EndsWith("Service"))
                .AsImplementedInterfaces().InstancePerLifetimeScope();

        builder.RegisterType<ProjectAuthorizeHandler>()
                .As<IAuthorizationHandler>()
                .InstancePerDependency();


        builder.RegisterType<RoleFeatureService>()
                .As<IRoleFeatureService>()
                .AsImplementedInterfaces();


        builder.RegisterAssemblyTypes(typeof(GetProjectsQueryHandler).Assembly);

        builder.RegisterType<HttpContextAccessor>()
                .As<IHttpContextAccessor>().SingleInstance();

    }
}