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
using MotqenIslamicLearningPlatform_API.DTOs.HalaqaDTOs;
using MotqenIslamicLearningPlatform_API.DTOs.ProgressDTOs;
using MotqenIslamicLearningPlatform_API.DTOs.TeacherDTOs.TeacherSubjectDtos;
using MotqenIslamicLearningPlatform_API.DTOs.StudentDTOs.StudentAttendanceDtos;
using MotqenIslamicLearningPlatform_API.DTOs.StudentDTOs.StudentSubjectDtos;
using MotqenIslamicLearningPlatform_API.DTOs.TeacherDTOs.HalaqaTeacherDtos;

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
            // HalaqaStudent => HalaqaStudentDisplayHalaqaDTO
            CreateMap<HalaqaStudent, HalaqaStudentDisplayHalaqaDTO>()
                .AfterMap(
                (src, dest,context) =>
                {
                    var teacher = src.Halaqa.HalaqaTeachers.FirstOrDefault(ht => ht.HalaqaId == src.HalaqaId).Teacher.User;
                    dest.Name = $"{teacher.FirstName} {teacher.LastName}";
                    dest.SubjectName = src.Halaqa.Subject.Name;
                    dest.LiveLink = src.Halaqa.LiveLink;
                    dest.Name = src.Halaqa.Name;
                    dest.Description = src.Halaqa.Description;
                }
                );
            //HalaqaTeacher Mapping
            CreateMap<HalaqaTeacher, HalaqaTeacherDto>()
                .ForMember(dest => dest.HalaqaName, opt => opt.MapFrom(src => src.Halaqa.Name))
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => $"{src.Teacher.User.FirstName} {src.Teacher.User.LastName}"))
                .ForMember(dest => dest.SubjectName , opt => opt.MapFrom(src => src.Halaqa.Subject.Name));
            CreateMap<HalaqaTeacher, CreateHalaqaTeacherDto>().ReverseMap();
            // StudentSubject Mapping
            CreateMap<StudentSubject, StudentSubjectDto>()
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name))
                //.ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => $"{src.Teacher.User.FirstName} {src.Teacher.User.LastName}"));
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => $"{src.Student.User.FirstName} {src.Student.User.LastName}"));

            CreateMap<StudentSubject, CreateStudentSubjectDto>().ReverseMap();

            //Teacher  Mapping
            CreateMap<Teacher, TeacherDto>().ReverseMap();
            CreateMap<Teacher,CreateTeacherDto >().ReverseMap();
            CreateMap<Teacher, UpdateTeacherDto>().ReverseMap();
            //TeacherAttendance Mapping
            CreateMap<TeacherAttendance, TeacherAttendanceDto>()
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => $"{src.Teacher.User.FirstName} {src.Teacher.User.LastName}"))
                .ForMember(desc => desc.HalaqaName, opt => opt.MapFrom(src => src.Halaqa.Name));
            CreateMap<TeacherAttendance, CreateTeacherAttendanceDto>().ReverseMap();
            CreateMap<TeacherAttendance, UpdateTeacherAttendanceDto>().ReverseMap();
            // TeacherSubject Mapping
            CreateMap<TeacherSubject, TeacherSubjectDto>()
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name))
                //.ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => $"{src.Teacher.User.FirstName} {src.Teacher.User.LastName}"));
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => $"{src.Teacher.User.FirstName} {src.Teacher.User.LastName}"));

            CreateMap<TeacherSubject, CreateTeacherSubjectDto>().ReverseMap();

            // Halaqa Mapping

            CreateMap<Halaqa, HalaqaDto>()
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name));
            CreateMap<Halaqa, CreateHalaqaDto>().ReverseMap();
            CreateMap<Halaqa, UpdateHalaqaDto>()
                .ReverseMap();

            CreateMap<ClassSchedule , ClassScheduleDto>()
                .ForMember(dest => dest.HalaqaName, opt => opt.MapFrom(src => src.Halaqa.Name))
                .ReverseMap();
            CreateMap<ClassSchedule , CreateClassScheduleDto>()
                .ReverseMap();
            CreateMap<ClassSchedule ,UpdateClassScheduleDto>()
                .ReverseMap();

            // student -> student short display

            CreateMap<Student, StudentShortDisplayDTO>()
             .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"));

            //student -> detailed display
            CreateMap<Student, StudentDetailedDisplayDTO>()
                .ForMember(dest => dest.Name, src => src.MapFrom(data => $"{data.User.FirstName} {data.User.LastName}"))
                .ForMember(dest => dest.Parent, src => src.MapFrom(data => $"{data.Parent.User.FirstName} {data.Parent.User.LastName}"));
            //studentAttendance Mapping
            CreateMap<StudentAttendance, StudentAttendanceDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => $"{src.Student.User.FirstName} {src.Student.User.LastName}"))
                .ForMember(desc => desc.HalaqaName, opt => opt.MapFrom(src => src.Halaqa.Name));
            CreateMap<StudentAttendance, CreateStudentAttendanceDto>().ReverseMap();
            CreateMap<StudentAttendance, UpdateStudentAttendanceDto>().ReverseMap();
            // ProgressTracking Mapping
            CreateMap<ProgressTracking, ProgressListDTO>()
                .ForMember(ProgrssListDTO => ProgrssListDTO.StudentName, opt => opt.MapFrom(src => $"{src.Student.User.FirstName} {src.Student.User.LastName}"))
                .ForMember(ProgrssListDTO => ProgrssListDTO.HalaqaName, opt => opt.MapFrom(src => src.Halaqa.Name))
                .ForMember(ProgrssListDTO => ProgrssListDTO.HalaqaSubject, opt => opt.MapFrom(src => src.Halaqa.Subject.Name))
                .ForMember(ProgrssListDTO => ProgrssListDTO.FromAyah, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.FromAyah))
                .ForMember(ProgrssListDTO => ProgrssListDTO.ToAyah, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.ToAyah))
                .ForMember(ProgrssListDTO => ProgrssListDTO.FromSurah, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.FromSurah))
                .ForMember(ProgrssListDTO => ProgrssListDTO.ToSurah, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.ToSurah))
                .ForMember(ProgrssListDTO => ProgrssListDTO.Subject, opt => opt.MapFrom(src => src.IslamicSubjectsProgressTrackingDetail.Subject))
                .ForMember(ProgrssListDTO => ProgrssListDTO.LessonName, opt => opt.MapFrom(src => src.IslamicSubjectsProgressTrackingDetail.LessonName))
                .ForMember(ProgrssListDTO => ProgrssListDTO.FromPage, opt => opt.MapFrom(src => src.IslamicSubjectsProgressTrackingDetail.FromPage))
                .ForMember(ProgrssListDTO => ProgrssListDTO.ToPage, opt => opt.MapFrom(src => src.IslamicSubjectsProgressTrackingDetail.ToPage));



        }
    }
}
