using AutoMapper;
using MotqenIslamicLearningPlatform_API.DTOs.HalaqaDTOs;
using MotqenIslamicLearningPlatform_API.DTOs.ParentDTOs;
using MotqenIslamicLearningPlatform_API.DTOs.ProgressDTOs;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.AttendanceSummary;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.IslamicSubjectsProgress;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.QuranProgress;
using MotqenIslamicLearningPlatform_API.DTOs.StudentDTOs;
using MotqenIslamicLearningPlatform_API.DTOs.StudentDTOs.StudentAttendanceDtos;
using MotqenIslamicLearningPlatform_API.DTOs.StudentDTOs.StudentSubjectDtos;
using MotqenIslamicLearningPlatform_API.DTOs.SubjectDTOs;
using MotqenIslamicLearningPlatform_API.DTOs.TeacherDTOs;
using MotqenIslamicLearningPlatform_API.DTOs.TeacherDTOs.HalaqaTeacherDtos;
using MotqenIslamicLearningPlatform_API.DTOs.TeacherDTOs.TeacherAttendanceDtos;
using MotqenIslamicLearningPlatform_API.DTOs.TeacherDTOs.TeacherSubjectDtos;
using MotqenIslamicLearningPlatform_API.Models.HalaqaModel;
using MotqenIslamicLearningPlatform_API.Models.ParentModel;
using MotqenIslamicLearningPlatform_API.Models.Shared;
using MotqenIslamicLearningPlatform_API.Models.StudentModel;
using MotqenIslamicLearningPlatform_API.Models.TeacherModel;

namespace MotqenIslamicLearningPlatform_API.MappingConfig
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {


            #region Parent Mapping

            CreateMap<Parent, ParentListDTO>()
                .AfterMap((src, dest) => {
                    dest.Name = $"{src.User.FirstName} {src.User.LastName}";
                    dest.Children = src.Students.Select(s => $"{s.User.FirstName} {s.User.LastName}").ToList();
                });

            #endregion

            #region Subject Mapping

            CreateMap<Subject, SubjectDto>().ReverseMap();
            CreateMap<Subject, CreateSubjectDto>().ReverseMap();
            CreateMap<Subject, UpdateSubjectDto>().ReverseMap();

            #endregion

            #region Global Student Mapping
            #region Student Mapping

            CreateMap<Student, ParentChildDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"));

            CreateMap<Student, StudentShortDisplayDTO>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"));

            CreateMap<Student, StudentDetailedDisplayDTO>()
               .ForMember(dest => dest.Name, src => src.MapFrom(data => $"{data.User.FirstName} {data.User.LastName}"))
               .ForMember(dest => dest.Parent, src => src.MapFrom(data => $"{data.Parent.User.FirstName} {data.Parent.User.LastName}"));

            #endregion

            #region StudentSubject Mapping

            CreateMap<StudentSubject, StudentSubjectDto>()
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => $"{src.Student.User.FirstName} {src.Student.User.LastName}"));


            CreateMap<StudentSubject, CreateStudentSubjectDto>().ReverseMap();
            #endregion

            #region StudentAttendance Mapping

            CreateMap<StudentAttendance, StudentAttendanceDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => $"{src.Student.User.FirstName} {src.Student.User.LastName}"))
                .ForMember(desc => desc.HalaqaName, opt => opt.MapFrom(src => src.Halaqa.Name));

            CreateMap<StudentAttendance, CreateStudentAttendanceDto>().ReverseMap();

            CreateMap<StudentAttendance, UpdateStudentAttendanceDto>().ReverseMap();

            #endregion

            #region ProgressTracking Mapping
            CreateMap<ProgressTracking, ProgressListDTO>()
                .ForMember(ProgrssListDTO => ProgrssListDTO.StudentName, opt => opt.MapFrom(src => $"{src.Student.User.FirstName} {src.Student.User.LastName}"))
                .ForMember(ProgrssListDTO => ProgrssListDTO.HalaqaName, opt => opt.MapFrom(src => src.Halaqa.Name))
                .ForMember(ProgrssListDTO => ProgrssListDTO.HalaqaSubject, opt => opt.MapFrom(src => src.Halaqa.Subject.Name))
                .ForMember(ProgrssListDTO => ProgrssListDTO.FromAyah, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.FromAyah))
                .ForMember(ProgrssListDTO => ProgrssListDTO.ToAyah, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.ToAyah))
                .ForMember(ProgrssListDTO => ProgrssListDTO.NumberOfLines, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.NumberOfLines))
                .ForMember(ProgrssListDTO => ProgrssListDTO.Type, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.Type))
                .ForMember(ProgrssListDTO => ProgrssListDTO.FromSurah, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.FromSurah))
                .ForMember(ProgrssListDTO => ProgrssListDTO.ToSurah, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.ToSurah))
                .ForMember(ProgrssListDTO => ProgrssListDTO.Subject, opt => opt.MapFrom(src => src.IslamicSubjectsProgressTrackingDetail.Subject))
                .ForMember(ProgrssListDTO => ProgrssListDTO.LessonName, opt => opt.MapFrom(src => src.IslamicSubjectsProgressTrackingDetail.LessonName))
                .ForMember(ProgrssListDTO => ProgrssListDTO.FromPage, opt => opt.MapFrom(src => src.IslamicSubjectsProgressTrackingDetail.FromPage))
                .ForMember(ProgrssListDTO => ProgrssListDTO.ToPage, opt => opt.MapFrom(src => src.IslamicSubjectsProgressTrackingDetail.ToPage));

            CreateMap<ProgressFormDTO, ProgressTracking>();

            #endregion


            #endregion

            #region Global Halaqa Mapping
            #region Halaqa Mapping

            CreateMap<Halaqa, HalaqaDto>()
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name));

            CreateMap<Halaqa, CreateHalaqaDto>().ReverseMap();

            CreateMap<Halaqa, UpdateHalaqaDto>().ReverseMap();

            CreateMap<Halaqa, HalaqaNamesListDTO>();

            #endregion

            #region HalaqaStudent Mapping

            CreateMap<HalaqaStudent, StudentHalaqaDisplayDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.Student.User.FirstName} {src.Student.User.LastName}"))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StudentId))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Student.User.Email));

            CreateMap<HalaqaStudent, StudentHalaqaFormDTO>().ReverseMap();

            CreateMap<HalaqaStudent, HalaqaStudentDisplayHalaqaDTO>()
                .AfterMap(
                (src, dest, context) =>
                {
                    var teacher = src.Halaqa.HalaqaTeachers?.FirstOrDefault(ht => ht.HalaqaId == src.HalaqaId)?.Teacher.User;
                    if (teacher != null)
                        dest.TeacherName = $"{teacher.FirstName} {teacher.LastName}";
                    dest.SubjectName = src.Halaqa.Subject.Name;
                    dest.GuestLiveLink = src.Halaqa.GuestLiveLink;
                    dest.Name = src.Halaqa.Name;
                    dest.Description = src.Halaqa.Description;
                }
                );
            #endregion

            #region HalaqaTeacher Mapping

            CreateMap<HalaqaTeacher, HalaqaTeacherDto>()
                .ForMember(dest => dest.HalaqaName, opt => opt.MapFrom(src => src.Halaqa.Name))
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => $"{src.Teacher.User.FirstName} {src.Teacher.User.LastName}"))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Halaqa.Subject.Name));

            CreateMap<HalaqaTeacher, CreateHalaqaTeacherDto>().ReverseMap();

            CreateMap<HalaqaTeacher, TeacherDto>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.Teacher.User.FirstName} {src.Teacher.User.LastName}"))
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Teacher.User.Email))
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TeacherId))
               .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Teacher.Gender))
               .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.Teacher.IsDeleted))
               .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Teacher.Age))
               .ForMember(dest => dest.Pic, opt => opt.MapFrom(src => src.Teacher.Pic));
            #endregion

            #region ClaasSchedule Mapping

            CreateMap<ClassSchedule, ClassScheduleDto>()
                .ForMember(dest => dest.HalaqaName, opt => opt.MapFrom(src => src.Halaqa.Name)).ReverseMap();

            CreateMap<ClassSchedule, CreateClassScheduleDto>().ReverseMap();

            CreateMap<ClassSchedule, UpdateClassScheduleDto>().ReverseMap();

            #endregion

            #endregion

            #region Global Teacher Mapping

            #region Teacher Mapping
            CreateMap<Teacher, TeacherDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));

            CreateMap<Teacher, CreateTeacherDto>().ReverseMap();
            CreateMap<Teacher, UpdateTeacherDto>().ReverseMap();

            #endregion

            #region TeacherAttendance Mapping

            CreateMap<TeacherAttendance, TeacherAttendanceDto>()
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => $"{src.Teacher.User.FirstName} {src.Teacher.User.LastName}"))
                .ForMember(desc => desc.HalaqaName, opt => opt.MapFrom(src => src.Halaqa.Name));

            CreateMap<TeacherAttendance, CreateTeacherAttendanceDto>().ReverseMap();

            CreateMap<TeacherAttendance, UpdateTeacherAttendanceDto>().ReverseMap();

            #endregion

            #region TeacherSubject Mapping
            CreateMap<TeacherSubject, TeacherSubjectDto>()
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name))
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => $"{src.Teacher.User.FirstName} {src.Teacher.User.LastName}"));

            CreateMap<TeacherSubject, CreateTeacherSubjectDto>().ReverseMap();

            #endregion

            #endregion

            #region Reports

            CreateMap<StudentAttendance, StudentAttendanceReportDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => $"{src.Student.User.FirstName} {src.Student.User.LastName}"))
                .ForMember(dest => dest.HalaqaName, opt => opt.MapFrom(src => src.Halaqa.Name))
                .ForMember(dest => dest.AttendanceStatus, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<ProgressTracking, QuranDetailedProgressReportDto>()
                .ForMember(dest => dest.FromSurahNumber, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.FromSurah))
                .ForMember(dest => dest.ToSurahNumber, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.ToSurah))
                .ForMember(dest => dest.FromAyah, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.FromAyah))
                .ForMember(dest => dest.ToAyah, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.ToAyah))
                .ForMember(dest => dest.NumberOfLines, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.NumberOfLines))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.Type.ToString()));

            CreateMap<ProgressTracking, QuranProgressChartPointDto>()
                .ForMember(dest => dest.NumberOfLines, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.NumberOfLines))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.Type.ToString()));


            CreateMap<ProgressTracking, IslamicSubjectsDetailedProgressReportDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => $"{src.Student.User.FirstName} {src.Student.User.LastName}"))
                .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.IslamicSubjectsProgressTrackingDetail.Subject))
                .ForMember(dest => dest.LessonName, opt => opt.MapFrom(src => src.IslamicSubjectsProgressTrackingDetail.LessonName))
                .ForMember(dest => dest.FromPage, opt => opt.MapFrom(src => src.IslamicSubjectsProgressTrackingDetail.FromPage))
                .ForMember(dest => dest.ToPage, opt => opt.MapFrom(src => src.IslamicSubjectsProgressTrackingDetail.ToPage));

            CreateMap<IslamicSubjectsProgressTracking, IslamicSubjectProgressOverTimeChartDto>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.ProgressTracking.Date))
                .ForMember(dest => dest.PagesOrLessonsCompleted, opt => opt.MapFrom(src => src.ToPage - src.FromPage + 1))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject));

            CreateMap<ProgressTracking, IslamicSubjectProgressOverTimeChartDto>()
                           .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.IslamicSubjectsProgressTrackingDetail.ProgressTracking.Date))
                           .ForMember(dest => dest.PagesOrLessonsCompleted, opt => opt.MapFrom(src => src.IslamicSubjectsProgressTrackingDetail.ToPage - src.IslamicSubjectsProgressTrackingDetail.FromPage + 1))
                           .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.IslamicSubjectsProgressTrackingDetail.Subject));
            #endregion

        }
    }
}
