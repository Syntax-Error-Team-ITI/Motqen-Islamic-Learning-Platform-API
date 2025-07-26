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
        #region reports for student 
        #region QuranProgress For student
        List<QuranProgressChartPointDto> GetStudentMemorizationProgressChart(int studentId);
        List<QuranProgressChartPointDto> GetStudentReviewProgressChart(int studentId);
        List<WeeklyMonthlyQuranProgressDto> GetStudentWeeklyMonthlyQuranProgress(int studentId, string periodType); 
        QuranSummaryCountersDto GetStudentQuranSummaryCounters(int studentId);
        List<QuranDetailedProgressReportDto> GetStudentQuranDetailedProgressReport(int studentId);
        #endregion

        #region IslamicProgress For student

        List<IslamicSubjectProgressChartDto> GetStudentIslamicSubjectPagesChart(int studentId);
        List<IslamicSubjectProgressOverTimeChartDto> GetStudentIslamicSubjectProgressOverTimeChart(int studentId, int subjectId);
        List<IslamicSubjectsDetailedProgressReportDto> GetStudentIslamicSubjectsDetailedProgressReport(int studentId);
        #endregion
        #region Attendance For student

        List<StudentAttendancePieChartDto> GetStudentAttendanceSummaryPieChart(int studentId);
        List<StudentAttenndanceDetails> GetStudentAttenndanceDetails(int studentid);
        List<MonthlyWeeklyAttendanceChartDto> GetStudentMonthlyWeeklyAttendanceChart(int studentId, string periodType);
        #endregion

        #endregion

        #region reports for Teacher 
        List<StudentHalaqaComparisonReportDto> GetStudentPerformanceComparisonReport(int studentId, int halaqaId);


        List<QuranProgressChartPointDto> GetHalaqaMemorizationProgress(int halaqaId);
        TeacherQuranSummaryDto GetHalaqaQuranSummary(int halaqaId);

        List<MonthlyWeeklyAttendanceChartDto> GetHalaqaAttendanceTrend(int halaqaId, string periodType);
        List<StudentAttendancePieChartDto> GetHalaqaAttendanceSummary(int halaqaId);

        TeacherDashboardDto GetTeacherDashboard(int teacherId);

        List<IslamicSubjectsDetailedProgressReportDto> GetHalaqaIslamicProgress(int halaqaId);

        List<HalaqaComparisonDto> GetHalaqasComparison(List<int> halaqaIds);
        #endregion

        #region reports For Admin
        AdminDashboardSummaryDto GetAdminDashboardSummary();
        List<UserSummaryDto> GetUserSummaryReport();
        List<TeacherPerformanceDto> GetTeacherPerformanceReport();
        List<StudentPerformanceOverviewDto> GetStudentPerformanceOverview();
        List<HalaqaHealthReportDto> GetHalaqaHealthReport();
        #endregion
    }
}
