using HotelManagement.Service.PasswordHasherServices;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Extensions;
using ProjectManagementSystem.Api.Features.Authentication.Login;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.ImageService;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response.RequestResult;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectManagementSystem.Api.Features.Authentication.Registration.Command
{
    public record RegisterCommand(string username, string email, string password, IFormFile imageFile) : IRequest<RequestResult<AuthModel>>;
    public class RegisterHandler : BaseRequestHandler<RegisterCommand, RequestResult<AuthModel>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly JWT _jwt;
        private readonly IImageService _imageService;

        public RegisterHandler(BaseRequestHandlerParam requestHandlerParam, IUnitOfWork unitOfWork, IOptions<JWT> jwt, IImageService imageService) : base(requestHandlerParam)
        {
            _unitOfWork = unitOfWork;
            _jwt = jwt.Value;
            _imageService = imageService;
        }

        public override async Task<RequestResult<AuthModel>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var authModel = new AuthModel();

            var repo = _unitOfWork.GetRepository<User>();
            var EmailAlreadyRegs = await repo.AnyAsync(u => u.Email == request.email);

            if (EmailAlreadyRegs)
            {
                return RequestResult<AuthModel>.Failure(Response.ErrorCode.UserEmailExist, "f");
            }

            var password = PasswordHasherService.HashPassord(request.password);
            var role = Role.User;
            var imagePath = await _imageService.UploadImage(request.imageFile, "users");
            var user = new User { Email = request.email, Password = password, Role = role, Username = request.username, ImagePath = imagePath, Phone = "1000000" };
            await repo.AddAsync(entity: user);
            var createCode = new TempAuthCode { Email = user.Email, Code = new Random().Next(10000, 99999), IsUsed = false, CreatedAt = DateTime.UtcNow, ExpiresOn = DateTime.Now.AddSeconds(120) };
            await _unitOfWork.GetRepository<TempAuthCode>().AddAsync(createCode);
            await _unitOfWork.SaveChangesAsync();

            var claims = GenerateClaims(user);

            var jwtSecurityToken = CreateAccessToken(claims);
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.IsAuthenticated = true;
            authModel.Email = user.Email;
            authModel.UserName = user.Username;

            return RequestResult<AuthModel>.Success(authModel, "s");

        }
        private Claim[] GenerateClaims(User user)
        {
            var identifier = user.Id.ToString();

            return
            [
                new Claim(ClaimTypes.NameIdentifier, identifier)
,
            new Claim(JwtRegisteredClaimNames.UniqueName, identifier),
            new Claim(JwtRegisteredClaimNames.Sub, user.Username, ClaimValueTypes.String),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role,user.Role.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
            ];
        }

        private JwtSecurityToken CreateAccessToken(Claim[] claims)
        {
            var createdAt = DateTime.UtcNow;

            var expiresAt = createdAt.AddDays(_jwt.DurationInDays);

            var keyBytes = Encoding.UTF8.GetBytes(_jwt.Key);

            var symmetricSecurityKey = new SymmetricSecurityKey(keyBytes);

            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}
