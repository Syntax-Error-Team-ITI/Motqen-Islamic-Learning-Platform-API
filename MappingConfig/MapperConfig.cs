using AutoMapper;
using MotqenIslamicLearningPlatform_API.DTOs.SubjectDtos;
using MotqenIslamicLearningPlatform_API.Models.Shared;
using MotqenIslamicLearningPlatform_API.DTOs.ParentDTOs;
using MotqenIslamicLearningPlatform_API.DTOs.StudentDTOs;
using MotqenIslamicLearningPlatform_API.Models.HalaqaModel;
using MotqenIslamicLearningPlatform_API.Models.StudentModel;

namespace MotqenIslamicLearningPlatform_API.MappingConfig
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Subject , SubjectDto>().ReverseMap();
            CreateMap<Subject , CreateSubjectDto>().ReverseMap();
            CreateMap<Subject , UpdateSubjectDto>().ReverseMap();
            //////////// Student
            // Student => ParentChildDTO
            CreateMap<Student,ParentChildDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"));

            //////////// HalaqaStudent 
            // HalaqaStudent => StudentHalaqaDisplayDTO
            CreateMap<HalaqaStudent, StudentHalaqaDisplayDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.Student.User.FirstName} {src.Student.User.LastName}"));
            // HalaqaStudent => StudentHalaqaFormDTO
            CreateMap<HalaqaStudent, StudentHalaqaFormDTO>().ReverseMap();
        }
    }
}
