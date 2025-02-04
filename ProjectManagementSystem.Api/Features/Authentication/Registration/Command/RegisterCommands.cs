using MediatR;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Authentication.Login;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.Common.EmailService;
using ProjectManagementSystem.Api.Features.Common.OTPService;
using ProjectManagementSystem.Api.ImageService;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response.RequestResult;

namespace ProjectManagementSystem.Api.Features.Authentication.Registration.Command
{
    public record RegisterCommand(string username, string email, string password, string phone, IFormFile imageFile) : IRequest<RequestResult<string>>;
    public class RegisterHandler : BaseRequestHandler<RegisterCommand, RequestResult<string>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;

        private readonly IOTPService _otpService;
        private readonly IEmailServices _emailService;

        public RegisterHandler(BaseRequestHandlerParam requestHandlerParam, IUnitOfWork unitOfWork, IOTPService oTPService, IEmailServices emailService, IImageService imageService) : base(requestHandlerParam)
        {
            _unitOfWork = unitOfWork;

            _otpService = oTPService;
            _emailService = emailService;
            _imageService = imageService;
        }

        public override async Task<RequestResult<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var authModel = new AuthModel();

            var repo = _unitOfWork.GetRepository<User>();
            var EmailAlreadyRegs = await repo.AnyAsync(u => u.Email == request.email);

            if (EmailAlreadyRegs)
            {
                return RequestResult<string>.Failure(Response.ErrorCode.UserEmailExist, "Email already exists");
            }

            var password = PasswordHasherService.HashPassord(request.password);
            var role = Role.User;
            string imagePath = string.Empty;
            if (request.imageFile is not null)
                imagePath = await _imageService.UploadImage(request.imageFile, "users");


            var user = new UserTempData { Email = request.email, Password = password, Role = role, UserName = request.username, Phone = request.phone, ImagePath = imagePath };


            var otp = _otpService.GenerateOTP();

            _otpService.SaveOTP(user, otp);
            var body = $"Please use this code to verify your account \n {otp}";
            _emailService.SendEmail(user.Email, "verification", body);
            //to do send email with code to user email 

            return RequestResult<string>.Success(user.Email, "Verification code has been sent to your email");

        }

    }
}
