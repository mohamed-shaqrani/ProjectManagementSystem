using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Extensions;
using ProjectManagementSystem.Api.Features.Authentication.Login;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.Common.OTPService;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response.RequestResult;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectManagementSystem.Api.Features.Authentication.ConfirmAccount.command
{
    public record ConfirmAccountCommand( string code) : IRequest<RequestResult<AuthModel>>;
    public class ConfirmAccountHandler : BaseRequestHandler<ConfirmAccountCommand, RequestResult<AuthModel>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly JWT _jwt;
        
        private readonly IOTPService _otpservice;

        public ConfirmAccountHandler(BaseRequestHandlerParam requestHandlerParam, IUnitOfWork unitOfWork, IOptions<JWT> jwt, IOTPService otpservice) : base(requestHandlerParam)
        {
            _unitOfWork = unitOfWork;
            _jwt = jwt.Value;
           _otpservice = otpservice;
        }

        public override async Task<RequestResult<AuthModel>> Handle(ConfirmAccountCommand request, CancellationToken cancellationToken)
        {
           
           
            var authModel = new AuthModel();

            if(string.IsNullOrEmpty(request.code)) 
            {
                return RequestResult<AuthModel>.Failure(Response.ErrorCode.ValidationError, "validation error code is wrong");
            }

            

            var Tempuser = _otpservice.GetTempUser(request.code);
            if (Tempuser == null) 
            {
                return RequestResult<AuthModel>.Failure(Response.ErrorCode.ValidationError, "validation error code is wrong");

            }
            var user = new User()
            {
              Email = Tempuser.Email,
              Password = Tempuser.Password,
              Username = Tempuser.UserName,
              Role = Tempuser.Role,
              Phone = Tempuser.Phone,
            };

            var repo = _unitOfWork.GetRepository<User>();
           await repo.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();


            var claims = GenerateClaims(user);

            var jwtSecurityToken = CreateAccessToken(claims);
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.IsAuthenticated = true;
            authModel.Email = user.Email;
            authModel.UserName = user.Username;

            return RequestResult<AuthModel>.Success(authModel, "success");

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
