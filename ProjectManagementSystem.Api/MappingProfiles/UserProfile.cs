using AutoMapper;
using ProjectManagementSystem.Api.Entities;

namespace ProjectManagementSystem.Api.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, User>();

    }
}