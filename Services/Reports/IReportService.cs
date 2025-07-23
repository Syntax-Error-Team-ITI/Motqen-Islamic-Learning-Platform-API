using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.AttendanceSummary;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.IslamicSubjectsProgress;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.QuranProgress;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.TeacherDtos;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.AdminDtos;

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
        List<StudentAttenndanceDetails> GetStudentAttenndanceDetails(int studentid);
        List<MonthlyWeeklyAttendanceChartDto> GetStudentMonthlyWeeklyAttendanceChart(int studentId, string periodType);

        // Student Performance Comparison Report
        List<StudentHalaqaComparisonReportDto> GetStudentPerformanceComparisonReport(int studentId, int halaqaId);


        // 1. تقارير القرآن
        List<QuranProgressChartPointDto> GetHalaqaMemorizationProgress(int halaqaId);
        TeacherQuranSummaryDto GetHalaqaQuranSummary(int halaqaId);

        // 2. تقارير الحضور
        List<MonthlyWeeklyAttendanceChartDto> GetHalaqaAttendanceTrend(int halaqaId, string periodType);
        List<StudentAttendancePieChartDto> GetHalaqaAttendanceSummary(int halaqaId);

        // 3. لوحة التحكم
        TeacherDashboardDto GetTeacherDashboard(int teacherId);

        // 4. تقارير المواد الإسلامية
        List<IslamicSubjectsDetailedProgressReportDto> GetHalaqaIslamicProgress(int halaqaId);

        List<HalaqaComparisonDto> GetHalaqasComparison(List<int> halaqaIds);

        List<AdminDashboardSummaryDto> GetAdminDashboardSummary();
        List<UserSummaryDto> GetUserSummaryReport();
        List<TeacherPerformanceDto> GetTeacherPerformanceReport();
        List<StudentPerformanceOverviewDto> GetStudentPerformanceOverview();
        List<HalaqaHealthReportDto> GetHalaqaHealthReport();
    }
}
