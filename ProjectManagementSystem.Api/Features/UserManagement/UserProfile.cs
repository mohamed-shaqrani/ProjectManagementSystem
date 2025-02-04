using AutoMapper;
using ProjectManagementSystem.Api.Entities;

namespace ProjectManagementSystem.Api.Features.UserManagement;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponseViewModel>();
    }
}
