using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Repository;
using System.Security.Claims;

namespace ProjectManagementSystem.Api.Helpers
{
    public class ProjectAdminRequirement : IAuthorizationRequirement { }
    public class ProjectAuthorizeHandler : AuthorizationHandler<ProjectAdminRequirement>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectAuthorizeHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ProjectAdminRequirement requirement)
        {
            var UserRepo = _unitOfWork.GetRepository<User>();
            var UserEmail = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            var UserEmailValue = UserEmail?.Value;
            if (UserEmailValue == null)
            {
                return;
            }

            var user = await UserRepo.GetAll(u => u.Email == UserEmailValue).FirstOrDefaultAsync();
            if (user is null)
            {
                return;
            }

            var UserRolesRepo = _unitOfWork.GetRepository<ProjectUserRoles>();
            var ProjectId = context.Resource as int?;
            if (ProjectId is null)
            {
                return;
            }
            var Any = await UserRolesRepo.AnyAsync(up => up.UserId == user.Id && up.ProjectId == ProjectId && up.Role == Role.Admin);

            if (!Any)
            {
                context.Succeed(requirement);
            }

        }
    }
}
