using AutoMapper;
using ProjectManagementSystem.Api.Entities;

namespace ProjectManagementSystem.Api.Features.UserManagement.GetUsers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponseViewModel>();
    }
}
