using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Models;
using ToDoBusinessLogic.DTO;

namespace ToDo.Utility.Mapping
{
    public class AppMappingProfile : AutoMapper.Profile
    {
        public AppMappingProfile()
        {
            CreateMap<TodoDTO, TodoViewModel>().ReverseMap()
                .ForMember(dest => dest.TodoId, conf => conf.MapFrom(src => src.TodoId))
            .ForMember(dest => dest.Title, conf => conf.MapFrom(src => src.Title))
            .ForMember(dest => dest.CreationDate, conf => conf.MapFrom(src => src.CreationDate))
            .ForMember(dest => dest.Completed, conf => conf.MapFrom(src => src.Completed));

            CreateMap<TodoViewModel, TodoDTO>().ReverseMap()
                .ForMember(dest => dest.TodoId, conf => conf.MapFrom(src => src.TodoId))
            .ForMember(dest => dest.Title, conf => conf.MapFrom(src => src.Title))
            .ForMember(dest => dest.CreationDate, conf => conf.MapFrom(src => src.CreationDate))
            .ForMember(dest => dest.Completed, conf => conf.MapFrom(src => src.Completed));

            CreateMap<TodoPointDTO, TodoPointViewModel>().ReverseMap()
                .ForMember(dest => dest.PointId, conf => conf.MapFrom(src => src.PointId))
            .ForMember(dest => dest.Description, conf => conf.MapFrom(src => src.Description))
            .ForMember(dest => dest.DateOfComplition, conf => conf.MapFrom(src => src.DateOfComplition))
            .ForMember(dest => dest.IsCompleted, conf => conf.MapFrom(src => src.IsCompleted))
            .ForMember(dest => dest.TodoId, conf => conf.MapFrom(src => src.TodoId));

            CreateMap<TodoPointViewModel, TodoPointDTO>().ReverseMap()
                .ForMember(dest => dest.PointId, conf => conf.MapFrom(src => src.PointId))
            .ForMember(dest => dest.Description, conf => conf.MapFrom(src => src.Description))
            .ForMember(dest => dest.DateOfComplition, conf => conf.MapFrom(src => src.DateOfComplition))
            .ForMember(dest => dest.IsCompleted, conf => conf.MapFrom(src => src.IsCompleted))
            .ForMember(dest => dest.TodoId, conf => conf.MapFrom(src => src.TodoId));
        }
    }
}
