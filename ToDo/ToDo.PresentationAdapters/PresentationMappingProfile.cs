using System;
using ToDo.DataAccessModels;
using ToDo.ViewModels;

namespace ToDo.PresentationAdapters
{
    public class PresentationMappingProfile : AutoMapper.Profile
    {
        public PresentationMappingProfile()
        {
            CreateMap<Todo, TodoViewModel>().ReverseMap();
            CreateMap<TodoPoint, TodoPointViewModel>().ReverseMap();
        }
    }
}
