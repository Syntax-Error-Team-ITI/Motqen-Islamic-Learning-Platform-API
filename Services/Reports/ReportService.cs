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
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.AdminDtos;
using Microsoft.EntityFrameworkCore;

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

        public List<HalaqaComparisonDto> GetHalaqasComparison(List<int> halaqaIds)
        {
            var comparisonResults = new List<HalaqaComparisonDto>();

            foreach (var halaqaId in halaqaIds)
            {
                var halaqa = Unit.HalaqaRepo.GetById(halaqaId);
                var students = Unit.HalaqaStudentRepo.getAllStudentsByHalaqaId(halaqaId);
                var progress = Unit.ProgressTrackingRepo
                    .GetAllProgressForSpecificHalaqa(halaqaId)
                    .Where(pt => pt.QuranProgressTrackingDetail != null)
                    .ToList();

                var memorizationProgress = progress
                    .Where(pt => pt.QuranProgressTrackingDetail.Type == ProgressType.Memorization)
                    .ToList();

                var reviewProgress = progress
                    .Where(pt => pt.QuranProgressTrackingDetail.Type == ProgressType.Review)
                    .ToList();

                var dto = new HalaqaComparisonDto
                {
                    HalaqaId = halaqaId,
                    HalaqaName = halaqa?.Name ?? "غير معروف",
                    TotalStudents = students.Count,
                    QuranProgress = new QuranSummaryCountersDto
                    {
                        TotalLinesMemorized = memorizationProgress
                            .Sum(pt => pt.QuranProgressTrackingDetail?.NumberOfLines ?? 0),
                        TotalLinesReviewed = reviewProgress
                            .Sum(pt => pt.QuranProgressTrackingDetail?.NumberOfLines ?? 0),
                        TotalSurahsMemorized = GetDistinctSurahs(memorizationProgress).Count,
                        TotalSurahsReviewed = GetDistinctSurahs(reviewProgress).Count,
                        TotalJuzsMemorized = (decimal)memorizationProgress
                            .Sum(pt => pt.QuranProgressTrackingDetail?.NumberOfLines ?? 0) / (20 * 15),
                        TotalJuzsReviewed = (decimal)reviewProgress
                            .Sum(pt => pt.QuranProgressTrackingDetail?.NumberOfLines ?? 0) / (20 * 15)
                    }
                };

                comparisonResults.Add(dto);
            }

            return comparisonResults.OrderByDescending(h => h.QuranProgress.TotalLinesMemorized).ToList();
        }

        public AdminDashboardSummaryDto GetAdminDashboardSummary()
        {
            var db = Unit.StudentRepo.Db;
            var totalUsers = db.Users.Count(u => !u.IsDeleted);
            var totalTeachers = db.Teachers.Count(t => !t.IsDeleted);
            var totalStudents = db.Students.Count(s => !s.IsDeleted);
            var totalHalaqas = db.Halaqas.Count(h => !h.IsDeleted);
            // Attendance rate: average of all student attendance
            var totalAttendance = db.StudentAttendances.Count();
            var presentAttendance = db.StudentAttendances.Count(a => a.Status == Enums.AttendanceStatus.Present);
            decimal overallAttendanceRate = totalAttendance > 0 ? (decimal)presentAttendance / totalAttendance * 100 : 0;
            // New registrations: use earliest DateJoined in HalaqaStudent as proxy
            var thisMonth = DateTime.UtcNow.Month;
            var thisYear = DateTime.UtcNow.Year;
            var newjoinToHalaqa = db.HalaqaStudent
                .Count(hs => hs.DateJoined.Month == thisMonth && hs.DateJoined.Year == thisYear);

            

            return 
                new AdminDashboardSummaryDto {
                    TotalUsers = totalUsers,
                    TotalTeachers = totalTeachers,
                    TotalStudents = totalStudents,
                    TotalHalaqas = totalHalaqas,
                    OverallAttendanceRate = overallAttendanceRate,
                    NewjoinToHalaqaThisMonth = newjoinToHalaqa
                
            };
        }

        public List<UserSummaryDto> GetUserSummaryReport()
        {
            var db = Unit.StudentRepo.Db;
            var users = db.Users.Where(u => !u.IsDeleted).ToList();
            var teacherActiveIds = db.Teachers.Where(t => !t.IsDeleted).Select(t => t.UserId).ToList();
            var studentActiveIds = db.Students.Where(t => !t.IsDeleted).Select(s => s.UserId).ToList();
            var parentActiveIds = db.Parents.Where(t => !t.IsDeleted).Select(p => p.UserId).ToList();
            var teacherDeletedIds = db.Teachers.Where(t => t.IsDeleted).Select(t => t.UserId).ToList();
            var studentDeletedIds = db.Students.Where(t => t.IsDeleted).Select(s => s.UserId).ToList();
            var parentDeletedIds = db.Parents.Where(t => t.IsDeleted).Select(p => p.UserId).ToList();
            var result = new List<UserSummaryDto>();
            foreach (var user in users)
            {
                if (teacherActiveIds.Contains(user.Id))
                {
                    
                    result.Add(new UserSummaryDto
                    {
                        UserId = user.Id,
                        FullName = $"{user.FirstName} {user.LastName}",
                        Email = user.Email,
                        Role = "Teacher",
                        IsActive = true
                    });
                }
                if (teacherDeletedIds.Contains(user.Id))
                {
                    
                    result.Add(new UserSummaryDto
                    {
                        UserId = user.Id,
                        FullName = $"{user.FirstName} {user.LastName}",
                        Email = user.Email,
                        Role = "Teacher",
                        IsActive = false
                    });
                }

                if (studentActiveIds.Contains(user.Id))
                {
                    
                    result.Add(new UserSummaryDto
                    {
                        UserId = user.Id,
                        FullName = $"{user.FirstName} {user.LastName}",
                        Email = user.Email,
                        Role = "Student",
                        IsActive = true
                    });
                }
                if (studentDeletedIds.Contains(user.Id))
                {
                   
                    result.Add(new UserSummaryDto
                    {
                        UserId = user.Id,
                        FullName = $"{user.FirstName} {user.LastName}",
                        Email = user.Email,
                        Role = "Student",
                        IsActive = false
                    });
                }

                if (parentActiveIds.Contains(user.Id))
                {
                    
                    result.Add(new UserSummaryDto
                    {
                        UserId = user.Id,
                        FullName = $"{user.FirstName} {user.LastName}",
                        Email = user.Email,
                        Role = "Parent",
                        IsActive = true
                    });
                }

                if (parentDeletedIds.Contains(user.Id))
                {
                    
                    result.Add(new UserSummaryDto
                    {
                        UserId = user.Id,
                        FullName = $"{user.FirstName} {user.LastName}",
                        Email = user.Email,
                        Role = "Parent",
                        IsActive = false
                    });
                }
            }
            return result;
        }

        public List<TeacherPerformanceDto> GetTeacherPerformanceReport()
        {
            var db = Unit.StudentRepo.Db;
            var teachers = db.Teachers.Where(t => !t.IsDeleted)
                .Include(t =>t.User)
                .ToList();
            var now = DateTime.UtcNow;
            var result = new List<TeacherPerformanceDto>();
            foreach (var teacher in teachers)
            {
                var teacherAttendances = db.TeacherAttendances.Where(a => a.TeacherId == teacher.Id).ToList();
                var teacherHalaqas = db.HalaqaTeacher.Where(ht => ht.TeacherId == teacher.Id).ToList();
                var teacherStudents = db.HalaqaStudent.Where(hs => teacherHalaqas.Select(ht => ht.HalaqaId).Contains(hs.HalaqaId)).ToList();
                var teacherStudentIds = teacherStudents.Select(hs => hs.StudentId).Distinct().ToList();
                var studentProgress = db.ProgressTrackings
                    .Include(pt => pt.QuranProgressTrackingDetail)
                    .Where(pt => teacherStudentIds.Contains(pt.StudentId ?? 0)).ToList();
                // Attendance rate
                var totalAttendance = teacherAttendances.Count;
                var presentAttendance = teacherAttendances.Count(a => a.Status == Enums.AttendanceStatus.Present);
                decimal avgAttendanceRate = totalAttendance > 0 ? (decimal)presentAttendance / totalAttendance * 100 : 0;
                // Student progress: average lines memorized per student
                decimal avgStudentProgress = 0;
                if (teacherStudentIds.Count > 0)
                {
                    var totalLines = studentProgress.Sum(pt => pt.QuranProgressTrackingDetail != null ? pt.QuranProgressTrackingDetail.NumberOfLines : 0);
                    avgStudentProgress = teacherStudentIds.Count > 0 ? (decimal)totalLines / teacherStudentIds.Count : 0;
                }
              
                var user = teacher.User;
                var teacherName = user != null ? $"{user.FirstName} {user.LastName}" : "Unknown";
                var email = user?.Email ?? "No email";

                result.Add(new TeacherPerformanceDto {
                    TeacherId = teacher.Id,
                    TeacherName = teacherName,
                    Email = email,
                    TotalAssignedHalaqas = teacherHalaqas.Count,
                    AverageAttendanceRate = avgAttendanceRate,
                    AverageStudentProgress = avgStudentProgress
                });
            }
            return result;
        }

        public List<StudentPerformanceOverviewDto> GetStudentPerformanceOverview()
        {
            var db = Unit.StudentRepo.Db;
            var students = db.Students.Where(s => !s.IsDeleted)
                .Include(s=>s.User)
                .ToList();
            var now = DateTime.UtcNow;
            var result = new List<StudentPerformanceOverviewDto>();
            foreach (var student in students)
            {
                var attendances = db.StudentAttendances.Where(a => a.StudentId == student.Id).ToList();
                var progress = db.ProgressTrackings.Where(pt => pt.StudentId == student.Id)
                    .Include(pt => pt.QuranProgressTrackingDetail)
                    .Include(pt => pt.IslamicSubjectsProgressTrackingDetail)
                    .ToList();
                // Attendance rate
                var totalAttendance = attendances.Count;
                var presentAttendance = attendances.Count(a => a.Status == Enums.AttendanceStatus.Present);
                decimal attendanceRate = totalAttendance > 0 ? (decimal)presentAttendance / totalAttendance * 100 : 0;
                // Quran memorization/review
                var quranMemorized = progress.Where(pt => pt.QuranProgressTrackingDetail != null && pt.QuranProgressTrackingDetail.Type == Enums.ProgressType.Memorization).Sum(pt => pt.QuranProgressTrackingDetail.NumberOfLines);
                var quranReviewed = progress.Where(pt => pt.QuranProgressTrackingDetail != null && pt.QuranProgressTrackingDetail.Type == Enums.ProgressType.Review).Sum(pt => pt.QuranProgressTrackingDetail.NumberOfLines);
                // Islamic subject progress
                var islamicPages = progress.Where(pt => pt.IslamicSubjectsProgressTrackingDetail != null).Sum(pt => (pt.IslamicSubjectsProgressTrackingDetail.ToPage - pt.IslamicSubjectsProgressTrackingDetail.FromPage + 1));
                
                var studentName = student.User !=null ? $"{student.User.FirstName} {student.User.LastName}" : "";
                var email = student.User?.Email ?? "No email";
                result.Add(new StudentPerformanceOverviewDto {
                    StudentId = student.Id,
                    StudentName = studentName,
                    Email = email,
                    AttendanceRate = attendanceRate,
                    TotalQuranLinesMemorized = quranMemorized,
                    TotalQuranLinesReviewed = quranReviewed,
                    TotalIslamicPagesCompleted = islamicPages
                });
            }
            return result;
        }

        public List<HalaqaHealthReportDto> GetHalaqaHealthReport()
        {
            var db = Unit.StudentRepo.Db;
            var halaqas = db.Halaqas.Where(h => !h.IsDeleted).ToList();
            var now = DateTime.UtcNow;
            var result = new List<HalaqaHealthReportDto>();
            foreach (var halaqa in halaqas)
            {
                var students = db.HalaqaStudent.Where(hs => hs.HalaqaId == halaqa.Id).ToList();
                var teachers = db.HalaqaTeacher.Where(ht => ht.HalaqaId == halaqa.Id).ToList();
                var attendances = db.StudentAttendances.Where(a => a.HalaqaId == halaqa.Id).ToList();
                var progress = db.ProgressTrackings.Where(pt => pt.HalaqaId == halaqa.Id)
                    .Include(pt=> pt.QuranProgressTrackingDetail)
                    .ToList();
                // Attendance rate
                var totalAttendance = attendances.Count;
                var presentAttendance = attendances.Count(a => a.Status == Enums.AttendanceStatus.Present);
                decimal avgAttendanceRate = totalAttendance > 0 ? (decimal)presentAttendance / totalAttendance * 100 : 0;
                // Progress: average lines memorized per student
                decimal avgProgress = 0;
                if (students.Count > 0)
                {
                    var totalLines = progress.Sum(pt => pt.QuranProgressTrackingDetail != null ? pt.QuranProgressTrackingDetail.NumberOfLines : 0);
                    avgProgress = students.Count > 0 ? (decimal)totalLines / students.Count : 0;
                }
                
               
                result.Add(new HalaqaHealthReportDto {
                    HalaqaId = halaqa.Id,
                    HalaqaName = halaqa.Name,
                    StudentCount = students.Count,
                    TeacherCount = teachers.Count,
                    AverageAttendanceRate = avgAttendanceRate,
                    AverageProgress = avgProgress
                });
            }
            return result;
        }

    }
}

