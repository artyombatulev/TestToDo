using AutoMapper;
using System;
using ToDo.PresentationAdapters;

namespace ToDo.Common
{
    public class AutoMapperConfiguration
    {
        public static IMapper Configure()
        {
            var config = new MapperConfiguration(c =>
            {
                c.AddProfile(new PresentationMappingProfile());
            });

            return config.CreateMapper();
        }
    }
}
