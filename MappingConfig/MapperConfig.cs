using AutoMapper;
using MotqenIslamicLearningPlatform_API.DTOs.SubjectDTOs;
using MotqenIslamicLearningPlatform_API.Models.Shared;
using MotqenIslamicLearningPlatform_API.DTOs.ParentDTOs;
using MotqenIslamicLearningPlatform_API.DTOs.StudentDTOs;
using MotqenIslamicLearningPlatform_API.Models.HalaqaModel;
using MotqenIslamicLearningPlatform_API.Models.StudentModel;
using MotqenIslamicLearningPlatform_API.DTOs.TeacherDTOs;
using MotqenIslamicLearningPlatform_API.Models.TeacherModel;
using MotqenIslamicLearningPlatform_API.DTOs.TeacherDTOs.TeacherAttendanceDtos;

namespace MotqenIslamicLearningPlatform_API.MappingConfig
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            //Subject Mapping
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
            //Teacher  Mapping
            CreateMap<Teacher, TeacherDto>().ReverseMap();
            CreateMap<Teacher,CreateTeacherDto >().ReverseMap();
            CreateMap<Teacher, UpdateTeacherDto>().ReverseMap();
            //TeacherAttendance Mapping
            CreateMap<TeacherAttendance, TeacherAttendanceDto>()
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => $"{src.Teacher.User.FirstName} {src.Teacher.User.LastName}"));
            CreateMap<TeacherAttendance, CreateTeacherAttendanceDto>().ReverseMap();
            CreateMap<TeacherAttendance, UpdateTeacherAttendanceDto>().ReverseMap();

            // student -> student short display

            CreateMap<Student, StudentShortDisplayDTO>()
             .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"));

            //student -> detailed display
            CreateMap<Student, StudentDetailedDisplayDTO>()
                .ForMember(dest => dest.Name, src => src.MapFrom(data => $"{data.User.FirstName} {data.User.LastName}"))
                .ForMember(dest => dest.Parent, src => src.MapFrom(data => $"{data.Parent.User.FirstName} {data.Parent.User.LastName}"));


        }
    }
}
