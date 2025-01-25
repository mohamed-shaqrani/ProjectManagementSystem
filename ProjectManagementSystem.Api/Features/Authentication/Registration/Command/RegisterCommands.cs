using HotelManagement.Service.PasswordHasherServices;
using MediatR;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Authentication.Login;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.ImageService;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response.RequestResult;

namespace ProjectManagementSystem.Api.Features.Authentication.Registration.Command
{
    public record RegisterCommand(string username, string email, string password, IFormFile imageFile) : IRequest<RequestResult<string>>;
    public class RegisterHandler : BaseRequestHandler<RegisterCommand, RequestResult<string>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;

        public RegisterHandler(BaseRequestHandlerParam requestHandlerParam, IUnitOfWork unitOfWork, IImageService imageService) : base(requestHandlerParam)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }

        public override async Task<RequestResult<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var authModel = new AuthModel();

            var repo = _unitOfWork.GetRepository<User>();
            var EmailAlreadyRegs = await repo.AnyAsync(u => u.Email == request.email);

            if (EmailAlreadyRegs)
            {
                return RequestResult<string>.Failure(Response.ErrorCode.UserEmailExist, "f");
            }

            var password = PasswordHasherService.HashPassord(request.password);
            var role = Role.User;
            var imagePath = await _imageService.UploadImage(request.imageFile, "users");
            var user = new User { Email = request.email, Password = password, Role = role, Username = request.username, ImagePath = imagePath, Phone = "1000000" };
            await repo.AddAsync(entity: user);
            var createCode = new TempAuthCode { Email = user.Email, Code = new Random().Next(10000, 99999), IsUsed = false, CreatedAt = DateTime.UtcNow, ExpiresOn = DateTime.Now.AddSeconds(120) };
            await _unitOfWork.GetRepository<TempAuthCode>().AddAsync(createCode);
            await _unitOfWork.SaveChangesAsync();

            //to do send email with code to user email 

            return RequestResult<string>.Success(user.Email, "s");

        }

    }
}
