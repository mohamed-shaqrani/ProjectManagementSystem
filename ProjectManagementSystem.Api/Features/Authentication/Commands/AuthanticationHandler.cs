﻿using HotelManagement.Core.ViewModels.Response;
using HotelManagement.Service.PasswordHasherServices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Extensions;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response;
using ProjectManagementSystem.Api.Response.RequestResult;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectManagementSystem.Api.Features.Authentication.Commands
{
    public record LoginCommand(  string Username,
    string Email,
   string Password, 

    string phone ) : IRequest<ResponseViewModel<AuthanticationModel>>;


    public class AuthanticationHandler : IRequestHandler<LoginCommand, ResponseViewModel<AuthanticationModel>>
    {
        private IUnitOfWork unitofwork;

        private JWT jwt;
        public AuthanticationHandler(IUnitOfWork unitOfWork, IOptions<JWT> jwt)
        {
            unitofwork = unitOfWork;
            this.jwt = jwt.Value;
        }

        public async Task<ResponseViewModel<AuthanticationModel>> Handle(LoginCommand loginCommand, CancellationToken token) 
        {
            var authModel = new AuthanticationModel();


            var user = await unitofwork.GetRepository<User>().GetAll(e => e.Email == loginCommand.Email).FirstOrDefaultAsync();

            if (user == null)
            {
                return new FailureResponseViewModel<AuthanticationModel>(ErrorCode.UserNotFound);

            }

            var correctPassword = PasswordHasherService.ValidatePassword(loginCommand.Password, user.Password);
            if (correctPassword)
            {
                var userRole = user.Role;
                var claims = GenerateClaims(user);
                var jwtSecurityToken = CreateAccessToken(claims);
                authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                authModel.ExpiresOn = jwtSecurityToken.ValidTo;
                authModel.IsAuthenticated = true;
                authModel.Email = user.Email;
                authModel.UserName = user.Username;

                return new SuccessResponseViewModel<AuthanticationModel>(SuccessCode.LoginCorrectly, authModel);

            }
            return new FailureResponseViewModel<AuthanticationModel>(ErrorCode.IncorrectPassword);

        }


        private static Claim[] GenerateClaims(User user)
        {
            var identifier = user.Id.ToString();

            return
            [
                new Claim(ClaimTypes.NameIdentifier, identifier)
,
            new Claim(JwtRegisteredClaimNames.UniqueName, identifier),
            new Claim(JwtRegisteredClaimNames.Sub, user.Username, ClaimValueTypes.String),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role,((int)user.Role).ToString())
            ];
        }

        private JwtSecurityToken CreateAccessToken(Claim[] claims)
        {
            var createdAt = DateTime.UtcNow;

            var expiresAt = createdAt.AddDays(jwt.DurationInDays);

            var keyBytes = Encoding.UTF8.GetBytes(jwt.Key);

            var symmetricSecurityKey = new SymmetricSecurityKey(keyBytes);

            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jwt.Issuer,
                audience: jwt.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}
