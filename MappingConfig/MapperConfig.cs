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
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.IslamicSubjectsProgress;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.QuranProgress;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.AttendanceSummary;

namespace MotqenIslamicLearningPlatform_API.MappingConfig
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            /// Parent 
            CreateMap<Parent, ParentListDTO>().ReverseMap();
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
     .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.Student.User.FirstName} {src.Student.User.LastName}"))
     .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StudentId))
     .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Student.User.Email));
            // HalaqaStudent => StudentHalaqaFormDTO
            CreateMap<HalaqaStudent, StudentHalaqaFormDTO>().ReverseMap();
            // HalaqaStudent => HalaqaStudentDisplayHalaqaDTO
            CreateMap<HalaqaStudent, HalaqaStudentDisplayHalaqaDTO>()
                .AfterMap(
                (src, dest,context) =>
                {
                    var teacher = src.Halaqa.HalaqaTeachers.FirstOrDefault(ht => ht.HalaqaId == src.HalaqaId).Teacher.User;
                    dest.TeacherName = $"{teacher.FirstName} {teacher.LastName}";
                    dest.SubjectName = src.Halaqa.Subject.Name;
                    dest.GuestLiveLink = src.Halaqa.GuestLiveLink;
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
            CreateMap<HalaqaTeacher, TeacherDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.Teacher.User.FirstName} {src.Teacher.User.LastName}"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Teacher.User.Email))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TeacherId))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Teacher.Gender))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.Teacher.IsDeleted))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Teacher.Age))
                .ForMember(dest => dest.Pic, opt => opt.MapFrom(src => src.Teacher.Pic));


            CreateMap<StudentSubject, CreateStudentSubjectDto>().ReverseMap();

            //Teacher  Mapping
            CreateMap<Teacher, TeacherDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));
            
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
                .ForMember(ProgrssListDTO => ProgrssListDTO.NumberOfLines, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.NumberOfLines))
                .ForMember(ProgrssListDTO => ProgrssListDTO.Type, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.Type))
                .ForMember(ProgrssListDTO => ProgrssListDTO.FromSurah, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.FromSurah))
                .ForMember(ProgrssListDTO => ProgrssListDTO.ToSurah, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.ToSurah))
                .ForMember(ProgrssListDTO => ProgrssListDTO.Subject, opt => opt.MapFrom(src => src.IslamicSubjectsProgressTrackingDetail.Subject))
                .ForMember(ProgrssListDTO => ProgrssListDTO.LessonName, opt => opt.MapFrom(src => src.IslamicSubjectsProgressTrackingDetail.LessonName))
                .ForMember(ProgrssListDTO => ProgrssListDTO.FromPage, opt => opt.MapFrom(src => src.IslamicSubjectsProgressTrackingDetail.FromPage))
                .ForMember(ProgrssListDTO => ProgrssListDTO.ToPage, opt => opt.MapFrom(src => src.IslamicSubjectsProgressTrackingDetail.ToPage));
            // //////////////
            CreateMap<ProgressFormDTO, ProgressTracking>();

            /////// Reports

            // StudentAttendance => StudentAttendanceReportDto
            CreateMap<StudentAttendance, StudentAttendanceReportDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => $"{src.Student.User.FirstName} {src.Student.User.LastName}"))
                .ForMember(dest => dest.HalaqaName, opt => opt.MapFrom(src => src.Halaqa.Name))
                .ForMember(dest => dest.AttendanceStatus, opt => opt.MapFrom(src => src.Status.ToString())); // Convert enum to string

            // StudentAttendance => StudentAttendancePieChartDto (You'll likely aggregate this in service layer)
            // This mapping is more about how individual attendance records might contribute, but the DTO itself
            // is for aggregated data (Status, Count, Percentage). No direct 1-to-1 mapping for this one here.

            // StudentAttendance => MonthlyWeeklyAttendanceChartDto (You'll likely aggregate this in service layer)
            // Similar to PieChartDto, this is for aggregated data.

            // ProgressTracking for QuranDetailedProgressReportDto
            CreateMap<ProgressTracking, QuranDetailedProgressReportDto>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.FromSurahNumber, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.FromSurah))// Requires a lookup for Surah name from int, handle in service or a custom resolver
                .ForMember(dest => dest.ToSurahNumber, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.ToSurah))   // Requires a lookup for Surah name from int, handle in service or a custom resolver
                .ForMember(dest => dest.FromAyah, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.FromAyah))
                .ForMember(dest => dest.ToAyah, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.ToAyah))
                .ForMember(dest => dest.NumberOfLines, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.NumberOfLines))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.Type.ToString())) // Convert enum to string
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Evaluation, opt => opt.MapFrom(src => src.Evaluation))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes));

            // ProgressTracking for QuranProgressChartPointDto
            CreateMap<ProgressTracking, QuranProgressChartPointDto>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.NumberOfLines, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.NumberOfLines))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.QuranProgressTrackingDetail.Type.ToString())); // Convert enum to string

            // QuranProgressTracking for WeeklyMonthlyQuranProgressDto (This will be aggregated in service)
            // CreateMap<QuranProgressTracking, WeeklyMonthlyQuranProgressDto>(); // Direct mapping is not suitable as it's aggregated

            // ProgressTracking for IslamicSubjectsDetailedProgressReportDto
            CreateMap<ProgressTracking, IslamicSubjectsDetailedProgressReportDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => $"{src.Student.User.FirstName} {src.Student.User.LastName}" ))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.IslamicSubjectsProgressTrackingDetail.Subject))
                .ForMember(dest => dest.LessonName, opt => opt.MapFrom(src => src.IslamicSubjectsProgressTrackingDetail.LessonName))
                .ForMember(dest => dest.FromPage, opt => opt.MapFrom(src => src.IslamicSubjectsProgressTrackingDetail.FromPage))
                .ForMember(dest => dest.ToPage, opt => opt.MapFrom(src => src.IslamicSubjectsProgressTrackingDetail.ToPage))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Evaluation, opt => opt.MapFrom(src => src.Evaluation))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes));

            // IslamicSubjectsProgressTracking for IslamicSubjectProgressOverTimeChartDto
            // This DTO expects aggregation of pages/lessons per subject over time.
            // A direct mapping from IslamicSubjectsProgressTracking might map `Subject` and `Date`,
            // but `PagesOrLessonsCompleted` needs calculation (ToPage - FromPage + 1) or lessons count.
            CreateMap<IslamicSubjectsProgressTracking, IslamicSubjectProgressOverTimeChartDto>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.ProgressTracking.Date))
                .ForMember(dest => dest.PagesOrLessonsCompleted, opt => opt.MapFrom(src => src.ToPage - src.FromPage + 1)) // Assuming pages, adjust if lessons
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject));
            //test
            CreateMap<ProgressTracking, IslamicSubjectProgressOverTimeChartDto>()
                           .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.IslamicSubjectsProgressTrackingDetail.ProgressTracking.Date))
                           .ForMember(dest => dest.PagesOrLessonsCompleted, opt => opt.MapFrom(src => src.IslamicSubjectsProgressTrackingDetail.ToPage - src.IslamicSubjectsProgressTrackingDetail.FromPage + 1)) // Assuming pages, adjust if lessons
                           .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.IslamicSubjectsProgressTrackingDetail.Subject));

            // IslamicSubjectsProgressTracking for IslamicSubjectProgressChartDto
            // This DTO expects aggregated total pages/lessons per subject.
            // No direct 1-to-1 mapping here, will be handled in service layer.

            // StudentHalaqaComparisonReportDto - This DTO is for aggregated comparison data
            // It will be constructed in the service layer, not via a direct AutoMapper profile.
            // As it involves comparison between student's values and halaqa averages.

            // QuranSummaryCountersDto - This DTO is for aggregated summary counters.
            // It will be constructed in the service layer, not via a direct AutoMapper profile.

            // Admin Report DTOs: No direct entity-to-DTO mapping needed for AdminDashboardSummaryDto or UserSummaryDto (constructed manually in service).


        }
    }
}
