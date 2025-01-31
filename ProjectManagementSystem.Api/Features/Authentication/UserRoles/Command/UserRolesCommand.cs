using MediatR;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response.RequestResult;

namespace ProjectManagementSystem.Api.Features.Authentication.UserRoles.Command
{
    public record UserRolesCommand(int Projectid,int userid,Role role):IRequest<RequestResult<bool>>;
    public class AddUserRoleHandler : BaseRequestHandler<UserRolesCommand,RequestResult<bool>>
    {
        private readonly IUnitOfWork _unitofwork;

        public AddUserRoleHandler(BaseRequestHandlerParam param, IUnitOfWork unitofwork) : base(param) 
        {
            _unitofwork = unitofwork;
        }

        public override async Task<RequestResult<bool>> Handle(UserRolesCommand command, CancellationToken cancellation) 
        {
            var repo = _unitofwork.GetRepository<ProjectUserRoles>();
            bool any = default;
            if (command.role == Role.Admin) 
            {
                 any = await repo.AnyAsync(
                    ur => ur.Role == Role.Admin &&
                    ur.ProjectId == command.Projectid) ;
                
            }

            if (any)
            {
                return RequestResult<bool>.Failure(Response.ErrorCode.ProjectExist, "There is already admin");
            }

            var userrole = new ProjectUserRoles()
            {
                UserId = command.userid,
                Role = command.role,
                ProjectId = command.Projectid
            };

           await repo.AddAsync(userrole);
            await _unitofwork.SaveChangesAsync();

            return RequestResult<bool>.Success(true, "User role is added successfully");
        }       

    }
}
