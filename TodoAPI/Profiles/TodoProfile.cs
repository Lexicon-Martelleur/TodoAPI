﻿using AutoMapper;
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
            .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.TimeStamp))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

        CreateMap<TodoDTO, TodoEntity>()
            .ConstructUsing(dto => new TodoEntity(
                dto.Timestamp,
                dto.Todo.Title,
                dto.Todo.Author,
                dto.Todo.Description,
                dto.Todo.Done))
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
