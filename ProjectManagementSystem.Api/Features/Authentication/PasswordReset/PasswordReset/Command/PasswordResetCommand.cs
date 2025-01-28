using HotelManagement.Service.PasswordHasherServices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.Common.OTPService;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response.RequestResult;

namespace ProjectManagementSystem.Api.Features.Authentication.PasswordReset.PasswordReset.Command;

public record PasswordResetCommand(string Email, string NewPassword, string OTP) : IRequest<RequestResult<bool>>;

public class PasswordResetCommandHandler : BaseRequestHandler<PasswordResetCommand, RequestResult<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOTPService _OTPService;

    public PasswordResetCommandHandler(
        BaseRequestHandlerParam requestHandlerParam,
        IUnitOfWork unitOfWork,
        IOTPService OTPService
        )
        : base(requestHandlerParam)
    {
        _unitOfWork = unitOfWork;
        _OTPService = OTPService;
    }

    public override async Task<RequestResult<bool>> Handle(PasswordResetCommand request, CancellationToken cancellationToken)
    {
        var response = await ValidateRequest(request);
        if (!response.IsSuccess)
            return response;

        var userid = await _unitOfWork.GetRepository<User>()
                                    .GetAll(u => u.Email == request.Email)
                                    .Select(u => u.Id)
                                    .FirstOrDefaultAsync();

        var user = new User { Id = userid, Password = request.NewPassword };
        var newPassword = PasswordHasherService.HashPassord(request.NewPassword);

        _unitOfWork.GetRepository<User>().SaveInclude(user, a => a.Password);
        await _unitOfWork.SaveChangesAsync();


        return RequestResult<bool>.Success(default, "Password has been changed successfully");
    }

    private Task<RequestResult<bool>> ValidateRequest(PasswordResetCommand request)
    {
        var savedOTP = _OTPService.GetTempUser(request.OTP);

        if (savedOTP is null)
        {
            return Task.FromResult(RequestResult<bool>.Failure(default, "Invalid OTP"));
        }

        return Task.FromResult(RequestResult<bool>.Success(default, "Success"));
    }
}