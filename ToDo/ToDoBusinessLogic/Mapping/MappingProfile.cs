using System;
using System.Collections.Generic;
using System.Text;
using ToDoBusinessLogic.DTO;
using ToDoPersistence.Entities;

namespace ToDoBusinessLogic.Mapping
{
    public class MappingProfile: AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<Todo, TodoDTO>().ReverseMap()
                .ForMember(dest => dest.TodoId, conf => conf.MapFrom(src => src.TodoId))
            .ForMember(dest => dest.Title, conf => conf.MapFrom(src => src.Title))
            .ForMember(dest => dest.CreationDate, conf => conf.MapFrom(src => src.CreationDate))
            .ForMember(dest => dest.Completed, conf => conf.MapFrom(src => src.Completed));

            CreateMap<TodoDTO, Todo>().ReverseMap()
                .ForMember(dest => dest.TodoId, conf => conf.MapFrom(src => src.TodoId))
            .ForMember(dest => dest.Title, conf => conf.MapFrom(src => src.Title))
            .ForMember(dest => dest.CreationDate, conf => conf.MapFrom(src => src.CreationDate))
            .ForMember(dest => dest.Completed, conf => conf.MapFrom(src => src.Completed)); ;


            CreateMap<TodoPoint, TodoPointDTO>().ReverseMap()
                .ForMember(dest => dest.PointId, conf => conf.MapFrom(src => src.PointId))
            .ForMember(dest => dest.Description, conf => conf.MapFrom(src => src.Description))
            .ForMember(dest => dest.DateOfComplition, conf => conf.MapFrom(src => src.DateOfComplition))
            .ForMember(dest => dest.IsCompleted, conf => conf.MapFrom(src => src.IsCompleted))
            .ForMember(dest => dest.TodoId, conf => conf.MapFrom(src => src.TodoId)); 

            CreateMap<TodoPointDTO, TodoPoint>().ReverseMap()
                .ForMember(dest => dest.PointId, conf => conf.MapFrom(src => src.PointId))
            .ForMember(dest => dest.Description, conf => conf.MapFrom(src => src.Description))
            .ForMember(dest => dest.DateOfComplition, conf => conf.MapFrom(src => src.DateOfComplition))
            .ForMember(dest => dest.IsCompleted, conf => conf.MapFrom(src => src.IsCompleted))
            .ForMember(dest => dest.TodoId, conf => conf.MapFrom(src => src.TodoId));
        }
    }
}
