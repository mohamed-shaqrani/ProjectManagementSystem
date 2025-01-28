using HotelManagement.Service.PasswordHasherServices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Authentication.PasswordReset.Command;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.Common.OTPService;
using ProjectManagementSystem.Api.Features.Common.Users.Queries;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response.RequestResult;

namespace ProjectManagementSystem.Api.Features.Authentication.ChangePassword.Command;

public record ChangePasswordCommand(int UserId, string OldPassword, string NewPassword) : IRequest<RequestResult<bool>>;

public class ChangePasswordCommandHandler : BaseRequestHandler<ChangePasswordCommand, RequestResult<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public ChangePasswordCommandHandler(
        BaseRequestHandlerParam requestHandlerParam,
        IUnitOfWork unitOfWork
        )
        : base(requestHandlerParam)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task<RequestResult<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var response = await ValidateRequest(request);
        if (!response.IsSuccess)
            return response;

        var user = new User { Id = request.UserId, Password = request.NewPassword }; 
        var newPassword = PasswordHasherService.HashPassord(request.NewPassword);

        _unitOfWork.GetRepository<User>().SaveInclude(user, a => a.Password);
        await _unitOfWork.SaveChangesAsync();

        return RequestResult<bool>.Success(default, "Password has been changed successfully");
    }
    private async Task<RequestResult<bool>> ValidateRequest(ChangePasswordCommand request)
    {
        var userPassword = await _unitOfWork.GetRepository<User>()
                                      .GetAll(u => u.Id == request.UserId)
                                      .Select(u => u.Password)
                                      .FirstOrDefaultAsync();

        var isOldPasswordCorrect = PasswordHasherService.ValidatePassword(request.NewPassword, userPassword);
        if (!isOldPasswordCorrect)
        {
            return RequestResult<bool>.Failure(default, "Invalid old pasword"));
        }

        return RequestResult<bool>.Success(default, "Success");
    }
}