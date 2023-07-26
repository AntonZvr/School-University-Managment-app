using AutoMapper;
using WebApp.Data.ViewModels;
using WebApp.Models;
using WebApp.Data.ViewModels; 

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<StudentsModel, StudentsViewModel>().ReverseMap();
        CreateMap<CoursesModel, CourseViewModel>().ReverseMap();
        CreateMap<GroupsModel, GroupViewModel>().ReverseMap();
    }
}

