using AutoMapper;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.GetProject;

namespace ProjectManagementSystem.Api.MappingProfiles;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<Project, ProjectResponseViewModel>();
        CreateMap<IGrouping<ProjectGroupKey, Project>, ProjectResponseViewModel>()
         .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Key.Title))
         .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Key.Status))
         .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.Key.CreatedAt))
         .ForMember(dest => dest.NumTasks, opt => opt.MapFrom(src => src.Count())).ReverseMap();
    }
}
