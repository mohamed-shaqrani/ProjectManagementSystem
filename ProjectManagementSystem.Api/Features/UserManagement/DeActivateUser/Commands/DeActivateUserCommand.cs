using MediatR;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response;
using ProjectManagementSystem.Api.Response.RequestResult;

namespace ProjectManagementSystem.Api.Features.UserManagement.DeActivateUser.Commands;

public record DeActivateUserCommand(int UserId, bool DeActive) : IRequest<RequestResult<bool>>;
public class DeActivateUserHandler : BaseRequestHandler<DeActivateUserCommand, RequestResult<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeActivateUserHandler(BaseRequestHandlerParam param, IUnitOfWork unitOfWork) : base(param)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task<RequestResult<bool>> Handle(DeActivateUserCommand request, CancellationToken cancellationToken)
    {
        var doesUserExist = await _unitOfWork.GetRepository<User>().AnyAsync(x => x.Id == request.UserId);
        if (!doesUserExist)
            return RequestResult<bool>.Failure(ErrorCode.UserNotFound, "User not found");
        var alreadyDeactivated = await _unitOfWork.GetRepository<User>().AnyAsync(x => x.Id == request.UserId && x.IsActive == false);

        if (alreadyDeactivated)
            return RequestResult<bool>.Failure(ErrorCode.UserAlreadyDeactivated, "User has been DeActivated before");
        var user = new User
        {
            Id = request.UserId,
            IsActive = request.DeActive,
        };
        _unitOfWork.GetRepository<User>().SaveInclude(user, a => a.IsActive, a => a.UpdatedAt);
        int res = await _unitOfWork.SaveChangesAsync();
        return res > 0 ? RequestResult<bool>.Success(default, "User has been Deactivated successfully")
                       : RequestResult<bool>.Failure(ErrorCode.DataBaseError, "Operation failed");
    }
}
