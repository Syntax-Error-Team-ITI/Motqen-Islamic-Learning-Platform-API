using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.AttendanceSummary;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.IslamicSubjectsProgress;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.QuranProgress;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.TeacherDtos;
using MotqenIslamicLearningPlatform_API.Services.Reports;

namespace MotqenIslamicLearningPlatform_API.Controllers.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentReportsController : ControllerBase
    {
        IReportService ReportService;
        public ParentReportsController(IReportService _report)
        {
            this. ReportService = _report;
        }
        #region Quran

        [HttpGet("Quran/MemorizationChart/{studentId}")]
        public ActionResult<List<QuranProgressChartPointDto>> GetQuranMemorizationChart(int studentId)
        {
            var chartData = ReportService.GetStudentMemorizationProgressChart(studentId);
            if (chartData == null || !chartData.Any())
            {
                return NotFound("No memorization progress data found for this student.");
            }
            return Ok(chartData);
        }

        [HttpGet("Quran/ReviewChart/{studentId}")]
        public ActionResult<List<QuranProgressChartPointDto>> GetQuranReviewChart(int studentId)
        {
            var chartData = ReportService.GetStudentReviewProgressChart(studentId);
            if (chartData == null || !chartData.Any())
            {
                return NotFound("No review progress data found for this student.");
            }
            return Ok(chartData);
        }

        [HttpGet("Quran/WeeklyMonthlyProgress/{studentId}")]
        public ActionResult<List<WeeklyMonthlyQuranProgressDto>> GetQuranWeeklyMonthlyProgress(int studentId, [FromQuery] string periodType = "month")
        {
            if (periodType.ToLower() != "week" && periodType.ToLower() != "month")
            {
                return BadRequest("Invalid periodType. Must be 'week' or 'month'.");
            }
            var data = ReportService.GetStudentWeeklyMonthlyQuranProgress(studentId, periodType);
            if (data == null || !data.Any())
            {
                return NotFound($"No {periodType}ly Quran progress data found for this student.");
            }
            return Ok(data);
        }

        [HttpGet("Quran/SummaryCounters/{studentId}")]
        public ActionResult<QuranSummaryCountersDto> GetQuranSummaryCounters(int studentId)
        {
            var counters = ReportService.GetStudentQuranSummaryCounters(studentId);
            if (counters == null) 
            {
                return NotFound("No Quran summary data found for this student.");
            }
            return Ok(counters);
        }

        [HttpGet("Quran/DetailedProgressReport/{studentId}")]
        public ActionResult<List<QuranDetailedProgressReportDto>> GetQuranDetailedProgressReport(int studentId)
        {
            var report = ReportService.GetStudentQuranDetailedProgressReport(studentId);
            if (report == null || !report.Any())
            {
                return NotFound("No detailed Quran progress report found for this student.");
            }
            return Ok(report);
        }

        #endregion

        #region Islamic 
        [HttpGet("IslamicSubjects/PagesChart/{studentId}")]
        public ActionResult<List<IslamicSubjectProgressChartDto>> GetIslamicSubjectPagesChart(int studentId)
        {
            var chartData = ReportService.GetStudentIslamicSubjectPagesChart(studentId);
            if (chartData == null || !chartData.Any())
            {
                return NotFound("No Islamic subjects pages data found for this student.");
            }
            return Ok(chartData);
        }

        [HttpGet("IslamicSubjects/ProgressOverTimeChart/{studentId}/{subjectId}")]
        public ActionResult<List<IslamicSubjectProgressOverTimeChartDto>> GetIslamicSubjectProgressOverTimeChart(int studentId, int subjectId)
        {
            var chartData = ReportService.GetStudentIslamicSubjectProgressOverTimeChart(studentId, subjectId);
            if (chartData == null || !chartData.Any())
            {
                return NotFound("No Islamic subject progress over time data found for this student.");
            }
            return Ok(chartData);
        }

        [HttpGet("IslamicSubjects/DetailedProgressReport/{studentId}")]
        public ActionResult<List<IslamicSubjectsDetailedProgressReportDto>> GetIslamicSubjectsDetailedProgressReport(int studentId)
        {
            var report = ReportService.GetStudentIslamicSubjectsDetailedProgressReport(studentId);
            if (report == null || !report.Any())
            {
                return NotFound("No detailed Islamic subjects progress report found for this student.");
            }
            return Ok(report);
        }

        #endregion

        #region Attendance

        [HttpGet("Attendance/SummaryPieChart/{studentId}")]
        public ActionResult<List<StudentAttendancePieChartDto>> GetStudentAttendanceSummaryPieChart(int studentId)
        {
            var chartData = ReportService.GetStudentAttendanceSummaryPieChart(studentId);
            if (chartData == null || !chartData.Any())
            {
                return NotFound("No attendance summary data found for this student.");
            }
            return Ok(chartData);
        }
        [HttpGet("Attendance/datails/{studentid:int}")]
        public IActionResult GetStudentAttenndanceDetails(int studentid)
        {
            var data = ReportService.GetStudentAttenndanceDetails(studentid);
            if (data == null || !data.Any())
            {
                return NotFound("No attendance details found for this student.");
            }
            return Ok(data);
        }

        [HttpGet("Attendance/MonthlyWeeklyChart/{studentId}")]
        public ActionResult<List<MonthlyWeeklyAttendanceChartDto>> GetStudentMonthlyWeeklyAttendanceChart(int studentId, [FromQuery] string periodType = "month")
        {
            if (periodType.ToLower() != "week" && periodType.ToLower() != "month")
            {
                return BadRequest("Invalid periodType. Must be 'week' or 'month'.");
            }
            var chartData = ReportService.GetStudentMonthlyWeeklyAttendanceChart(studentId, periodType);
            if (chartData == null || !chartData.Any())
            {
                return NotFound($"No {periodType}ly attendance data found for this student.");
            }
            return Ok(chartData);
        }

        [HttpGet("Performance/Comparison/{studentId}/{halaqaId}")]
        public ActionResult<List<StudentHalaqaComparisonReportDto>> GetStudentPerformanceComparisonReport(int studentId, int halaqaId)
        {
            var report = ReportService.GetStudentPerformanceComparisonReport(studentId, halaqaId);
            if (report == null || !report.Any())
            {
                return NotFound("No performance comparison data found for this student in this halaqa.");
            }
            return Ok(report);
        }

        #endregion
    }
}
