using AutoMapper;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.TasksManagement.Tasks.GetTask;

namespace ProjectManagementSystem.Api.Features.UserManagement.GetUsers;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<ProjectTask, TaskDTO>()
            .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Title))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username));
    }
}
