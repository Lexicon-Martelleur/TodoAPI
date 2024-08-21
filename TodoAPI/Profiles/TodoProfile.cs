using AutoMapper;
using TodoAPI.Entities;
using TodoAPI.Models.DTO;

namespace TodoAPI.Profiles;

public class TodoProfile : Profile
{
    public TodoProfile() 
    {
        CreateMap<TodoEntity, TodoDTO>()
            .ForPath(dest => dest.Todo.Title, opt => opt.MapFrom(src => src.Title))
            .ForPath(dest => dest.Todo.Author, opt => opt.MapFrom(src => src.Author))
            .ForPath(dest => dest.Todo.Description, opt => opt.MapFrom(src => src.Description))
            .ForPath(dest => dest.Todo.Done, opt => opt.MapFrom(src => src.Done))
            .ForMember(dest => dest.TimeStamp, opt => opt.MapFrom(src => src.TimeStamp))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

    }
}
