using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Extensions;
using ProjectManagementSystem.Api.Features.Authentication.Login;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response.RequestResult;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectManagementSystem.Api.Features.Authentication.ConfirmAccount.command
{
    public record ConfirmAccountCommand(string email, int code) : IRequest<RequestResult<AuthModel>>;
    public class ConfirmAccountHandler : BaseRequestHandler<ConfirmAccountCommand, RequestResult<AuthModel>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly JWT _jwt;

        public ConfirmAccountHandler(BaseRequestHandlerParam requestHandlerParam, IUnitOfWork unitOfWork, IOptions<JWT> jwt) : base(requestHandlerParam)
        {
            _unitOfWork = unitOfWork;
            _jwt = jwt.Value;
        }

        public override async Task<RequestResult<AuthModel>> Handle(ConfirmAccountCommand request, CancellationToken cancellationToken)
        {

            var checkCode = await _unitOfWork.GetRepository<TempAuthCode>()
                                                                        .GetAll(e => e.Email == request.email && e.Code == request.code && e.IsUsed == false && e.ExpiresOn > DateTime.Now)
                                                                        .FirstOrDefaultAsync();
            if (checkCode == null)
            {
                return RequestResult<AuthModel>.Failure(Response.ErrorCode.InvalidCode, " Invalid Confirmation Code");

            }
            var authModel = new AuthModel();

            var user = await _unitOfWork.GetRepository<User>().GetAll(e => e.Email == request.email).FirstOrDefaultAsync();
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
