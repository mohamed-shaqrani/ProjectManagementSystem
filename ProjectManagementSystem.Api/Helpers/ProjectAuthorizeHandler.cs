using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Repository;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

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

            
            if (context.Resource is HttpContext httpContext) 
            {
                httpContext.Request.EnableBuffering();


                var reader = await httpContext.Request.BodyReader.ReadAsync();
                var body = reader.Buffer;
                var bodyconverter = Encoding.UTF8.GetString(body);
                
                    
                    
                    
                    var json = JsonSerializer.Deserialize<BaseCommand>(bodyconverter);
                 httpContext.Request.Body.Position = 0;
                    if(json is not null) 
                    {
                        var projectid = json.ProjectId;
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
                        var ProjectId = json.ProjectId ;
                        if (ProjectId <0)
                        {
                            return;
                        }
                        var Any = await UserRolesRepo.AnyAsync(up => up.UserId == user.Id && up.ProjectId == ProjectId && up.Role == Role.Admin);

                        if (Any)
                        {
                            context.Succeed(requirement);
                        }

                    }
                }
            }
            

        }
    }


