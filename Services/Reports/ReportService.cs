using AutoMapper;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.AttendanceSummary;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.IslamicSubjectsProgress;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.QuranProgress;
using MotqenIslamicLearningPlatform_API.Enums;
using MotqenIslamicLearningPlatform_API.Migrations;
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



        // ----------------------------------------------------
        // Implementations for New Parent Reports
        // ----------------------------------------------------

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
                    .GroupBy(pt => ISOWeek.GetWeekOfYear(pt.Date) + "-" + pt.Date.Year)
                    .Select(g => new WeeklyMonthlyQuranProgressDto
                    {
                        Period = $"Week {g.Key.Split('-')[0]} {g.Key.Split('-')[1]}",
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
                    .GroupBy(pt => new { pt.Date.Year, pt.Date.Month })
                    .Select(g => new WeeklyMonthlyQuranProgressDto
                    {
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

        private int GetJuzBySurah(int surahNumber)
        {
            return QuranParts.Parts
                .Where(p => p.SurahNumber <= surahNumber)
                .OrderByDescending(p => p.SurahNumber)
                .FirstOrDefault()?.PartNumber ?? 1;
        }

        private int GetJuzCountFromSurahs(List<int> surahNumbers)
        {
            if (surahNumbers == null || surahNumbers.Count == 0)
                return 0;

            surahNumbers.Sort();
            int firstJuz = GetJuzBySurah(surahNumbers.First());
            int lastJuz = GetJuzBySurah(surahNumbers.Last());

            return (lastJuz - firstJuz) + 1;
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
                TotalJuzsMemorized = GetJuzCountFromSurahs(memorizedSurahNumbers),
                TotalJuzsReviewed = GetJuzCountFromSurahs(reviewedSurahNumbers)
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
            var attendanceData = Unit.StudentAttendanceRepo.GetByStudentId(studentId).ToList(); // .ToList() to enable multiple enumerations

            return attendanceData
                .GroupBy(a => a.Status)
                .Select(g => new StudentAttendancePieChartDto
                {
                    Status = g.Key.ToString(),
                    Count = g.Count(),
                    Percentage = attendanceData.Any() ? (decimal)g.Count() / attendanceData.Count * 100 : 0 // Calculate percentage safely
                })
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
                StudentId = studentId,
                HalaqaId = halaqaId,
                Metric = "Attendance Percentage",
                StudentValue = studentAttendancePercentage,
                HalaqaAverageValue = halaqaAttendancePercentage
            });

            return reports;
        }
    }
}

