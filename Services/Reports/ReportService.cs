using AutoMapper;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.AttendanceSummary;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.IslamicSubjectsProgress;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.QuranProgress;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.TeacherDtos;
using MotqenIslamicLearningPlatform_API.Enums;
using MotqenIslamicLearningPlatform_API.Models.Shared;
using MotqenIslamicLearningPlatform_API.Models.StudentModel;
using MotqenIslamicLearningPlatform_API.UnitOfWorks;
using System.Globalization;

namespace MotqenIslamicLearningPlatform_API.Services.Reports
{
    public class ReportService : IReportService
    {
        IMapper Mapper;
        UnitOfWork Unit;
        public ReportService( IMapper Mapper , UnitOfWork Unit)
        {
            this.Mapper = Mapper;
            this.Unit = Unit;
        }



        // Quran Progress Reports
        public List<QuranProgressChartPointDto> GetStudentMemorizationProgressChart(int studentId)
        {
            var progressData = Unit.ProgressTrackingRepo.GetAllProgressForSpecificStudent(studentId)
                                   .Where(pt => pt.QuranProgressTrackingDetail != null && pt.QuranProgressTrackingDetail.Type == ProgressType.Memorization)
                                   .OrderBy(pt => pt.Date)
                                   .ToList();

            // Using AutoMapper for direct mapping
            return Mapper.Map<List<QuranProgressChartPointDto>>(progressData);
        }

        public List<QuranProgressChartPointDto> GetStudentReviewProgressChart(int studentId)
        {
            var progressData = Unit.ProgressTrackingRepo.GetAllProgressForSpecificStudent(studentId)
                                   .Where(pt => pt.QuranProgressTrackingDetail != null && pt.QuranProgressTrackingDetail.Type == ProgressType.Review)
                                   .OrderBy(pt => pt.Date)
                                   .ToList();

            // Using AutoMapper for direct mapping
            return Mapper.Map<List<QuranProgressChartPointDto>>(progressData);
        }

        public List<WeeklyMonthlyQuranProgressDto> GetStudentWeeklyMonthlyQuranProgress(int studentId, string periodType)
        {
            var progressData = Unit.ProgressTrackingRepo.GetAllProgressForSpecificStudent(studentId)
                                   .Where(pt => pt.QuranProgressTrackingDetail != null)
                                   .ToList();

            if (periodType.ToLower() == "week")
            {
                return progressData
                    .GroupBy(pt => new { Weekkey =ISOWeek.GetWeekOfYear(pt.Date) + "-" + pt.Date.Year , halaqaid =pt.HalaqaId }) 
                    .Select(g => new WeeklyMonthlyQuranProgressDto
                    {
                        HalaqaId = g.Key.halaqaid,
                        HalaqaName = g.First().Halaqa.Name,
                        Period = $"Week {g.Key.Weekkey.Split('-')[0]} {g.Key.Weekkey.Split('-')[1]}",
                        TotalMemorizedLines = g.Where(x => x.QuranProgressTrackingDetail.Type == ProgressType.Memorization)
                                               .Sum(x => x.QuranProgressTrackingDetail?.NumberOfLines ?? 0),
                        TotalReviewedLines = g.Where(x => x.QuranProgressTrackingDetail.Type == ProgressType.Review)
                                              .Sum(x => x.QuranProgressTrackingDetail?.NumberOfLines ?? 0)
                    })
                    .OrderBy(x => x.Period)
                    .ToList();
            }
            else if (periodType.ToLower() == "month")
            {
                return progressData
                    .GroupBy(pt => new { pt.Date.Year, pt.Date.Month, halaqaid = pt.HalaqaId })
                    .Select(g => new WeeklyMonthlyQuranProgressDto
                    {
                        HalaqaId = g.Key.halaqaid,
                        HalaqaName = g.First().Halaqa.Name,
                        Period = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM yyyy"),
                        TotalMemorizedLines = g.Where(x => x.QuranProgressTrackingDetail.Type == ProgressType.Memorization)
                                               .Sum(x => x.QuranProgressTrackingDetail?.NumberOfLines ?? 0),
                        TotalReviewedLines = g.Where(x => x.QuranProgressTrackingDetail.Type == ProgressType.Review)
                                              .Sum(x => x.QuranProgressTrackingDetail?.NumberOfLines ?? 0)
                    })
                    .OrderBy(x => x.Period)
                    .ToList();
            }
            return new List<WeeklyMonthlyQuranProgressDto>();
        }

      

        public QuranSummaryCountersDto GetStudentQuranSummaryCounters(int studentId)
        {
            var allQuranProgress = Unit.ProgressTrackingRepo
                .GetAllProgressForSpecificStudent(studentId)
                .Where(pt => pt.QuranProgressTrackingDetail != null)
                .ToList();

            var memorizationProgress = allQuranProgress
                .Where(pt => pt.QuranProgressTrackingDetail?.Type == ProgressType.Memorization)
                .ToList();

            var reviewProgress = allQuranProgress
                .Where(pt => pt.QuranProgressTrackingDetail?.Type == ProgressType.Review)
                .ToList();

            int totalMemorizedLines = memorizationProgress
                .Sum(pt => pt.QuranProgressTrackingDetail?.NumberOfLines ?? 0);

            int totalReviewedLines = reviewProgress
                .Sum(pt => pt.QuranProgressTrackingDetail?.NumberOfLines ?? 0);

            var memorizedSurahNumbers = GetDistinctSurahs(memorizationProgress);
            var reviewedSurahNumbers = GetDistinctSurahs(reviewProgress);

            return new QuranSummaryCountersDto
            {
                TotalLinesMemorized = totalMemorizedLines,
                TotalLinesReviewed = totalReviewedLines,
                TotalSurahsMemorized = memorizedSurahNumbers.Count,
                TotalSurahsReviewed = reviewedSurahNumbers.Count,
                TotalJuzsMemorized = (decimal)totalMemorizedLines / (20 * 15)  , // Assuming 15 lines per page and 20 pages per Juz
                TotalJuzsReviewed = (decimal)totalReviewedLines / (20*15) // Assuming 15 lines per page and 20 pages per Juz

            };
        }

        private List<int> GetDistinctSurahs(List<ProgressTracking> progressList)
        {
            return progressList
                .SelectMany(pt => Enumerable.Range(
                    pt.QuranProgressTrackingDetail.FromSurah,
                    pt.QuranProgressTrackingDetail.ToSurah - pt.QuranProgressTrackingDetail.FromSurah + 1))
                .Distinct()
                .ToList();
        }


        public List<QuranDetailedProgressReportDto> GetStudentQuranDetailedProgressReport(int studentId)
        {
            var detailedProgress = Unit.ProgressTrackingRepo.GetAllProgressForSpecificStudent(studentId)
                                       .Where(pt => pt.QuranProgressTrackingDetail != null)
                                       .ToList();

            // Using AutoMapper for direct mapping
            return Mapper.Map<List<QuranDetailedProgressReportDto>>(detailedProgress);
        }

        /// 
        // Islamic Subjects Progress Reports
        public List<IslamicSubjectProgressChartDto> GetStudentIslamicSubjectPagesChart(int studentId)
        {
            var islamicProgress = Unit.ProgressTrackingRepo.GetAllProgressForSpecificStudent(studentId)
                                      .Where(pt => pt.IslamicSubjectsProgressTrackingDetail != null)
                                      .ToList();

            // This is an aggregation, so manual projection is appropriate.
            return islamicProgress
                .GroupBy(pt => pt.IslamicSubjectsProgressTrackingDetail.Subject)
                .Select(g => new IslamicSubjectProgressChartDto
                {
                    SubjectName = g.Key,
                    TotalPagesCompleted = g.Sum(x => (x.IslamicSubjectsProgressTrackingDetail?.ToPage ?? 0) - (x.IslamicSubjectsProgressTrackingDetail?.FromPage ?? 0) + 1)
                })
                .ToList();
        }

        public List<IslamicSubjectProgressOverTimeChartDto> GetStudentIslamicSubjectProgressOverTimeChart(int studentId, int subjectId)
        {
            var subjectName = Unit.SubjectRepo.GetById(subjectId)?.Name;
            if (subjectName == null) return new List<IslamicSubjectProgressOverTimeChartDto>();

            var progressData = Unit.ProgressTrackingRepo.GetAllProgressForSpecificStudent(studentId)
                                   .Where(pt => pt.IslamicSubjectsProgressTrackingDetail != null && pt.IslamicSubjectsProgressTrackingDetail.Subject == subjectName)
                                   .OrderBy(pt => pt.Date)
                                   .ToList();

            // Using AutoMapper for direct mapping
            return Mapper.Map<List<IslamicSubjectProgressOverTimeChartDto>>(progressData);
        }

        public List<IslamicSubjectsDetailedProgressReportDto> GetStudentIslamicSubjectsDetailedProgressReport(int studentId)
        {
            var detailedProgress = Unit.ProgressTrackingRepo.GetAllProgressForSpecificStudent(studentId)
                                       .Where(pt => pt.IslamicSubjectsProgressTrackingDetail != null)
                                       .ToList();

            // Using AutoMapper for direct mapping
            return Mapper.Map<List<IslamicSubjectsDetailedProgressReportDto>>(detailedProgress);
        }

        // Student Attendance Reports
        public List<StudentAttendancePieChartDto> GetStudentAttendanceSummaryPieChart(int studentId)
        {
            var attendanceData = Unit.StudentAttendanceRepo.GetByStudentId(studentId).ToList();
            var studentName = attendanceData[0].Student?.User != null ? $"{attendanceData[0].Student.User.FirstName} {attendanceData[0].Student.User.LastName}" : "";
            return attendanceData
                .GroupBy(a => new {a.HalaqaId , a.Status})
                .Select(g => new StudentAttendancePieChartDto
                {
                     HalaqaName = g.First().Halaqa?.Name,
                     HalaqaId = g.Key.HalaqaId,
                     Status = g.Key.Status.ToString(),
                     Count = g.Count(),
                     Percentage = attendanceData.Where(a => a.HalaqaId == g.Key.HalaqaId ).Count() > 0 ?
                                 (decimal)g.Count() / attendanceData.Where(a => a.HalaqaId == g.Key.HalaqaId).Count() * 100 : 0

                })
                .ToList();
        }
        public List<StudentAttenndanceDetails> GetStudentAttenndanceDetails(int studentid)
        {
            var attendanceData = Unit.StudentAttendanceRepo.GetByStudentIdWithSubject(studentid).ToList(); 
            return attendanceData
                .Select(a => new StudentAttenndanceDetails
                {
                    AttendanceDate = a.AttendanceDate,
                    Status = a.Status.ToString(),
                    Halaqa = a.Halaqa?.Name ?? "",
                    Subject = a.Halaqa?.Subject?.Name ?? ""
                })
                .OrderByDescending(a => a.AttendanceDate)
                .ToList();
        }

        public List<MonthlyWeeklyAttendanceChartDto> GetStudentMonthlyWeeklyAttendanceChart(int studentId, string periodType)
        {
            var attendanceData = Unit.StudentAttendanceRepo.GetByStudentId(studentId);

            if (periodType.ToLower() == "month")
            {
                return attendanceData
                    .GroupBy(a => new { a.AttendanceDate.Year, a.AttendanceDate.Month })
                    .Select(g => new MonthlyWeeklyAttendanceChartDto
                    {
                        Period = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM yyyy"),
                        PresentCount = g.Count(x => x.Status == AttendanceStatus.Present),
                        AbsentCount = g.Count(x => x.Status == AttendanceStatus.Absent),
                        ExcusedCount = g.Count(x => x.Status == AttendanceStatus.Excused)
                    })
                    .OrderBy(x => x.Period)
                    .ToList();
            }
            else if (periodType.ToLower() == "week")
            {
                return attendanceData
                    .GroupBy(a => ISOWeek.GetWeekOfYear(a.AttendanceDate) + "-" + a.AttendanceDate.Year)
                    .Select(g => new MonthlyWeeklyAttendanceChartDto
                    {
                        Period = $"Week {g.Key.Split('-')[0]} {g.Key.Split('-')[1]}",
                        PresentCount = g.Count(x => x.Status == AttendanceStatus.Present),
                        AbsentCount = g.Count(x => x.Status == AttendanceStatus.Absent),
                        ExcusedCount = g.Count(x => x.Status == AttendanceStatus.Excused)
                    })
                    .OrderBy(x => x.Period)
                    .ToList();
            }
            return new List<MonthlyWeeklyAttendanceChartDto>();
        }

        // Student Performance Comparison Report
        public List<StudentHalaqaComparisonReportDto> GetStudentPerformanceComparisonReport(int studentId, int halaqaId)
        {
            var studentProgress = Unit.ProgressTrackingRepo.GetAllProgressForSpecificStudent(studentId)
                                      .Where(pt => pt.HalaqaId == halaqaId)
                                      .ToList();
            var _StudentName = $"{studentProgress[0].Student.User.FirstName} {studentProgress[0].Student.User.LastName}";
            
            var halaqaProgress = Unit.ProgressTrackingRepo.GetAllProgressForSpecificHalaqa(halaqaId)
                                     .ToList();
            var studentAttendance = Unit.StudentAttendanceRepo.GetByStudentIdAndHalaqaId(studentId, halaqaId).ToList(); // .ToList()
            var halaqaAttendance = Unit.StudentAttendanceRepo.GetByHalaqaId(halaqaId).ToList(); // .ToList()

            var reports = new List<StudentHalaqaComparisonReportDto>();

            // Metric 1: Average Memorized Lines
            decimal? studentAvgMemorizedLines = studentProgress
                .Where(pt => pt.QuranProgressTrackingDetail?.Type == ProgressType.Memorization)
                .Average(pt => (decimal?)pt.QuranProgressTrackingDetail?.NumberOfLines); // Removed ?? 0 for proper null handling
            decimal? halaqaAvgMemorizedLines = halaqaProgress
                .Where(pt => pt.QuranProgressTrackingDetail?.Type == ProgressType.Memorization)
                .Average(pt => (decimal?)pt.QuranProgressTrackingDetail?.NumberOfLines); // Removed ?? 0
            reports.Add(new StudentHalaqaComparisonReportDto
            {
                StudentName = _StudentName,
                StudentId = studentId,
                HalaqaId = halaqaId,
                Metric = "Average Memorized Lines",
                StudentValue = studentAvgMemorizedLines ?? 0,
                HalaqaAverageValue = halaqaAvgMemorizedLines ?? 0
            });

            // Metric 2: Average Reviewed Lines
            decimal? studentAvgReviewedLines = studentProgress
                .Where(pt => pt.QuranProgressTrackingDetail?.Type == ProgressType.Review)
                .Average(pt => (decimal?)pt.QuranProgressTrackingDetail?.NumberOfLines); // Removed ?? 0
            decimal? halaqaAvgReviewedLines = halaqaProgress
                .Where(pt => pt.QuranProgressTrackingDetail?.Type == ProgressType.Review)
                .Average(pt => (decimal?)pt.QuranProgressTrackingDetail?.NumberOfLines); // Removed ?? 0
            reports.Add(new StudentHalaqaComparisonReportDto
            {
                StudentName = _StudentName,
                StudentId = studentId,
                HalaqaId = halaqaId,
                Metric = "Average Reviewed Lines",
                StudentValue = studentAvgReviewedLines ?? 0,
                HalaqaAverageValue = halaqaAvgReviewedLines ?? 0
            });

            // Metric 3: Average Islamic Pages Completed
            decimal? studentAvgIslamicPages = studentProgress
                .Where(pt => pt.IslamicSubjectsProgressTrackingDetail != null)
                .Average(pt => (decimal?)((pt.IslamicSubjectsProgressTrackingDetail?.ToPage ?? 0) - (pt.IslamicSubjectsProgressTrackingDetail?.FromPage ?? 0) + 1)); // Corrected null handling
            decimal? halaqaAvgIslamicPages = halaqaProgress
                .Where(pt => pt.IslamicSubjectsProgressTrackingDetail != null)
                .Average(pt => (decimal?)((pt.IslamicSubjectsProgressTrackingDetail?.ToPage ?? 0) - (pt.IslamicSubjectsProgressTrackingDetail?.FromPage ?? 0) + 1)); // Corrected null handling
            reports.Add(new StudentHalaqaComparisonReportDto
            {
                StudentName = _StudentName,
                StudentId = studentId,
                HalaqaId = halaqaId,
                Metric = "Average Islamic Pages Completed",
                StudentValue = studentAvgIslamicPages ?? 0,
                HalaqaAverageValue = halaqaAvgIslamicPages ?? 0
            });


            // Metric 4: Attendance Percentage
            var totalStudentAttendanceDays = studentAttendance.Count();
            var studentPresentDays = studentAttendance.Count(a => a.Status == AttendanceStatus.Present);
            decimal studentAttendancePercentage = totalStudentAttendanceDays > 0 ? (decimal)studentPresentDays / totalStudentAttendanceDays * 100 : 0;

            var totalHalaqaAttendanceDays = halaqaAttendance.Count();
            var halaqaPresentDays = halaqaAttendance.Count(a => a.Status == AttendanceStatus.Present);
            decimal halaqaAttendancePercentage = totalHalaqaAttendanceDays > 0 ? (decimal)halaqaPresentDays / totalHalaqaAttendanceDays * 100 : 0;

            reports.Add(new StudentHalaqaComparisonReportDto
            {
                StudentName = _StudentName,
                StudentId = studentId,
                HalaqaId = halaqaId,
                Metric = "Attendance Percentage",
                StudentValue = studentAttendancePercentage,
                HalaqaAverageValue = halaqaAttendancePercentage
            });

            return reports;
        }

        public List<QuranProgressChartPointDto> GetHalaqaMemorizationProgress(int halaqaId)
        {
            var progressData = Unit.ProgressTrackingRepo
                .GetAllProgressForSpecificHalaqa(halaqaId)
                .Where(pt => pt.QuranProgressTrackingDetail != null)
                .OrderBy(pt => pt.Date)
                .ToList();

            return Mapper.Map<List<QuranProgressChartPointDto>>(progressData);
        }

        public TeacherQuranSummaryDto GetHalaqaQuranSummary(int halaqaId)
        {
            var progress = Unit.ProgressTrackingRepo
                .GetAllProgressForSpecificHalaqa(halaqaId)
                .Where(pt => pt.QuranProgressTrackingDetail != null)
                .ToList();

            var students = Unit.HalaqaStudentRepo.getAllStudentsByHalaqaId(halaqaId);

            return new TeacherQuranSummaryDto
            {
                HalaqaId = halaqaId,
                HalaqaName = Unit.HalaqaRepo.GetById(halaqaId)?.Name,
                TotalStudents = students.Count,
                TotalLinesMemorized = progress
                    .Where(p => p.QuranProgressTrackingDetail.Type == ProgressType.Memorization)
                    .Sum(p => p.QuranProgressTrackingDetail.NumberOfLines),
                TotalLinesReviewed = progress
                    .Where(p => p.QuranProgressTrackingDetail.Type == ProgressType.Review)
                    .Sum(p => p.QuranProgressTrackingDetail.NumberOfLines),
                AverageLinesPerStudent = students.Count > 0 ?
                    (decimal)progress.Sum(p => p.QuranProgressTrackingDetail.NumberOfLines) / students.Count : 0
            };
        }



        /// <summary>
        /// </summary>
        /// <param name="halaqaId"></param>
        /// <param name="periodType"></param>
        /// <returns></returns>
        public List<MonthlyWeeklyAttendanceChartDto> GetHalaqaAttendanceTrend(int halaqaId, string periodType)
        {
            var attendanceData = Unit.StudentAttendanceRepo.GetByHalaqaId(halaqaId);

            if (periodType.ToLower() == "month")
            {
                return attendanceData
                    .GroupBy(a => new { a.AttendanceDate.Year, a.AttendanceDate.Month })
                    .Select(g => new MonthlyWeeklyAttendanceChartDto
                    {
                        Period = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM yyyy"),
                        PresentCount = g.Count(x => x.Status == AttendanceStatus.Present),
                        AbsentCount = g.Count(x => x.Status == AttendanceStatus.Absent),
                        ExcusedCount = g.Count(x => x.Status == AttendanceStatus.Excused)
                    })
                    .OrderBy(x => x.Period)
                    .ToList();
            } else if (periodType.ToLower() == "week")
            {
                return attendanceData
                    .GroupBy(a => ISOWeek.GetWeekOfYear(a.AttendanceDate) + "-" + a.AttendanceDate.Year)
                    .Select(g => new MonthlyWeeklyAttendanceChartDto
                    {
                        Period = $"Week {g.Key.Split('-')[0]} {g.Key.Split('-')[1]}",
                        PresentCount = g.Count(x => x.Status == AttendanceStatus.Present),
                        AbsentCount = g.Count(x => x.Status == AttendanceStatus.Absent),
                        ExcusedCount = g.Count(x => x.Status == AttendanceStatus.Excused)
                    })
                    .OrderBy(x => x.Period)
                    .ToList();
            }
            return new List<MonthlyWeeklyAttendanceChartDto>();
        }

        public List<StudentAttendancePieChartDto> GetHalaqaAttendanceSummary(int halaqaId)
        {
            var attendanceData = Unit.StudentAttendanceRepo.GetByHalaqaId(halaqaId).ToList();

            return attendanceData
                .GroupBy(a => a.Status)
                .Select(g => new StudentAttendancePieChartDto
                {
                    HalaqaName = attendanceData.FirstOrDefault()?.Halaqa?.Name ?? "",
                    HalaqaId = halaqaId,
                    Status = g.Key.ToString(),
                    Count = g.Count(),
                    Percentage = (decimal)g.Count() / attendanceData.Count * 100
                })
                .ToList();
        }



        public TeacherDashboardDto GetTeacherDashboard(int teacherId)
        {
            var halaqas = Unit.HalaqaTeacherRepo.GetByTeacherId(teacherId);
            var dashboard = new TeacherDashboardDto
            {
                TotalHalaqas = halaqas.Count,
                TotalStudents = halaqas.Sum(h =>
                    Unit.HalaqaStudentRepo.getAllStudentsByHalaqaId(h.HalaqaId).Count),
                HalaqasProgress = new List<HalaqaProgressSummaryDto>()
            };

            // حساب مجموع الحضور لجميع الحلقات
            decimal totalPresent = 0;
            decimal totalSessions = 0;

            foreach (var halaqa in halaqas)
            {
                var students = Unit.HalaqaStudentRepo.getAllStudentsByHalaqaId(halaqa.HalaqaId);
                var attendance = Unit.StudentAttendanceRepo.GetByHalaqaId(halaqa.HalaqaId);

                var presentCount = attendance.Count(a => a.Status == AttendanceStatus.Present);
                var totalCount = attendance.Count;
                var attendanceRate = totalCount > 0 ?
                    (double)presentCount / totalCount * 100 : 0;

                dashboard.HalaqasProgress.Add(new HalaqaProgressSummaryDto
                {
                    HalaqaId = halaqa.HalaqaId,
                    HalaqaName = halaqa.Halaqa?.Name,
                    SubjectName = halaqa.Halaqa?.Subject?.Name,
                    StudentsCount = students.Count,
                    AttendanceRate = attendanceRate
                });

                totalPresent += presentCount;
                totalSessions += totalCount;
            }

            // حساب معدل الحضور الكلي
            dashboard.OverallAttendanceRate = totalSessions > 0 ?
                (decimal)totalPresent / totalSessions * 100 : 0;

            return dashboard;
        }

        public List<IslamicSubjectsDetailedProgressReportDto> GetHalaqaIslamicProgress(int halaqaId)
        {
            // 1. الحصول على جميع تتبع التقدم للحلقة
            var progressData = Unit.ProgressTrackingRepo
                .GetAllProgressForSpecificHalaqa(halaqaId)
                .Where(pt => pt.IslamicSubjectsProgressTrackingDetail != null)
                .OrderByDescending(pt => pt.Date)
                .ToList();

            // 2. تحويل البيانات إلى DTO مع إضافة معلومات الطالب
            var result = new List<IslamicSubjectsDetailedProgressReportDto>();

            foreach (var progress in progressData)
            {
                var dto = Mapper.Map<IslamicSubjectsDetailedProgressReportDto>(progress);

                // إضافة معلومات الطالب للمدرس

                dto.StudentName = progress.Student?.User != null ? $"{progress.Student?.User?.FirstName} ${progress.Student?.User?.LastName}" : "";

                result.Add(dto);
            }

            return result;
        }
    }
}

