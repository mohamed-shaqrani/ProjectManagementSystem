using ProjectManagementSystem.Api.Features.Common.EmailServices;
using MediatR;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.Common.OTPService;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response.RequestResult;
using ProjectManagementSystem.Api.Features.Common.EmailService;

namespace ProjectManagementSystem.Api.Features.Authentication.ForgetPassword.Commands
{
    public record ForgetPasswordCommand(string Email) : IRequest<RequestResult<bool>>;

    public class ForgetPasswordCommandHandler : BaseRequestHandler<ForgetPasswordCommand, RequestResult<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailServices _emailService;
        private readonly IOTPService _OTPService;

        public ForgetPasswordCommandHandler(
            BaseRequestHandlerParam requestHandlerParam,
            IUnitOfWork unitOfWork, 
            IEmailServices emailService,
            IOTPService OTPService            
            )
            : base(requestHandlerParam)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _OTPService = OTPService;
        }


        public override async Task<RequestResult<bool>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {

            var response = await ValidateRequest(request);
            if (!response.IsSuccess)
            {
                return response;
            }

            var userExist =await _unitOfWork.GetRepository<User>().AnyAsync(x=>x.Email == request.Email);
            if (!userExist)
            {
                return RequestResult<bool>.Failure(Response.ErrorCode.UserNotFound , "User Not Found");
            }


            var otp = _OTPService.GenerateOTP();
            var expiryTime = DateTime.UtcNow.AddMinutes(10);

            
            _emailService.SendEmail(request.Email, "Password Reset OTP", $"Your OTP for password reset is: <b>{otp}</b><br>This OTP will expire in 10 minutes.");

            return RequestResult<bool>.Success(default, "Email sent with otp.");
        }

        private async Task<RequestResult<bool>> ValidateRequest(ForgetPasswordCommand request)
        {
            return RequestResult<bool>.Success(default, "Success");
        }

    }
}
