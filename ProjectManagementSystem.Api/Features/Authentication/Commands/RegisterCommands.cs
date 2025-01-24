using HotelManagement.Core.ViewModels.Response;
using HotelManagement.Service.PasswordHasherServices;
using MediatR;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response;

namespace ProjectManagementSystem.Api.Features.Authentication.Commands
{
    public record RegisterCommand(string username, string email, string password): IRequest<ResponseViewModel<bool>> ;
    public class RegisterHandler : IRequestHandler<RegisterCommand, ResponseViewModel<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegisterHandler(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseViewModel<bool>> Handle(RegisterCommand command, CancellationToken cancellation) 
        {
            var repo = _unitOfWork.GetRepository<User>();
            var EmailAlreadyRegs = await repo.AnyAsync(u=> u.Email == command.email);

            if (EmailAlreadyRegs) 
            {
                return new FailureResponseViewModel<bool>(Response.ErrorCode.UserEmailExist);
            }

            var password = PasswordHasherService.HashPassord(command.password);
            var role = Role.User;
             await repo.AddAsync(new User { Email = command.email, Password = password, Role = role, Username = command.username });

            return new SuccessResponseViewModel<bool>(Response.SuccessCode.UserCreated);

        }
    }
}
