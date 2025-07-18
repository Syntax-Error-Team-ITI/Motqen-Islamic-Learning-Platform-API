using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.AttendanceSummary;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.IslamicSubjectsProgress;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.QuranProgress;

namespace MotqenIslamicLearningPlatform_API.Services.Reports
{
    public interface IReportService
    {
       
        // Quran Progress Reports
        List<QuranProgressChartPointDto> GetStudentMemorizationProgressChart(int studentId);
        List<QuranProgressChartPointDto> GetStudentReviewProgressChart(int studentId);
        List<WeeklyMonthlyQuranProgressDto> GetStudentWeeklyMonthlyQuranProgress(int studentId, string periodType); // periodType could be "Week" or "Month"
        QuranSummaryCountersDto GetStudentQuranSummaryCounters(int studentId);
        List<QuranDetailedProgressReportDto> GetStudentQuranDetailedProgressReport(int studentId);

        // Islamic Subjects Progress Reports
        List<IslamicSubjectProgressChartDto> GetStudentIslamicSubjectPagesChart(int studentId);
        List<IslamicSubjectProgressOverTimeChartDto> GetStudentIslamicSubjectProgressOverTimeChart(int studentId, int subjectId);
        List<IslamicSubjectsDetailedProgressReportDto> GetStudentIslamicSubjectsDetailedProgressReport(int studentId);

        // Student Attendance Reports (beyond existing basic ones)
        List<StudentAttendancePieChartDto> GetStudentAttendanceSummaryPieChart(int studentId);
        List<MonthlyWeeklyAttendanceChartDto> GetStudentMonthlyWeeklyAttendanceChart(int studentId, string periodType);

        // Student Performance Comparison Report
        List<StudentHalaqaComparisonReportDto> GetStudentPerformanceComparisonReport(int studentId, int halaqaId);

    }
}
