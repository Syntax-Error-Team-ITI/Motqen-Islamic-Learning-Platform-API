using AutoMapper;
using MotqenIslamicLearningPlatform_API.DTOs.SubjectDtos;
using MotqenIslamicLearningPlatform_API.Models.Shared;

namespace MotqenIslamicLearningPlatform_API.MappingConfig
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Subject , SubjectDto>().ReverseMap();
            CreateMap<Subject , CreateSubjectDto>().ReverseMap();
            CreateMap<Subject , UpdateSubjectDto>().ReverseMap();
        }
    }
}
