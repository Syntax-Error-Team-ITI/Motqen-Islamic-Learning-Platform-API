using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.AttendanceSummary;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.IslamicSubjectsProgress;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.QuranProgress;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.TeacherDtos;
using MotqenIslamicLearningPlatform_API.Services.Reports;

namespace MotqenIslamicLearningPlatform_API.Controllers.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherReportsController : ControllerBase
    {

        private readonly IReportService _reportService;

        public TeacherReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }
        #region Quran
        [Authorize(Roles = "Admin , Parent")]
        [HttpGet("Quran/MemorizationProgress/{halaqaId}")]
        public ActionResult<List<QuranProgressChartPointDto>> GetHalaqaMemorizationProgress(int halaqaId)
        {
            var progressData = _reportService.GetHalaqaMemorizationProgress(halaqaId);
            if (progressData == null || !progressData.Any())
            {
                //return NotFound("No memorization progress data found for this halaqa.");
                return Ok();

            }
            return Ok(progressData);
        }
        [Authorize(Roles = "Admin , Parent")]
        [HttpGet("Quran/Summary/{halaqaId}")]
        public ActionResult<TeacherQuranSummaryDto> GetHalaqaQuranSummary(int halaqaId)
        {
            var summary = _reportService.GetHalaqaQuranSummary(halaqaId);
            if (summary == null)
            {
                //return NotFound("No Quran summary data found for this halaqa.");
                return Ok();

            }
            return Ok(summary);
        }

        #endregion

        #region Attendance
        [Authorize(Roles = "Admin , Parent")]

        [HttpGet("Attendance/Trend/{halaqaId}")]
        public ActionResult<List<MonthlyWeeklyAttendanceChartDto>> GetHalaqaAttendanceTrend(
            int halaqaId, [FromQuery] string periodType = "month")
        {
            if (periodType.ToLower() != "week" && periodType.ToLower() != "month")
            {
                //return BadRequest("Invalid periodType. Must be 'week' or 'month'.");
                return Ok();

            }

            var trendData = _reportService.GetHalaqaAttendanceTrend(halaqaId, periodType);
            if (trendData == null || !trendData.Any())
            {
                //return NotFound($"No {periodType}ly attendance trend data found for this halaqa.");
                return Ok();

            }
            return Ok(trendData);
        }
        [Authorize(Roles = "Admin , Parent")]

        [HttpGet("Attendance/Summary/{halaqaId}")]
        public ActionResult<List<StudentAttendancePieChartDto>> GetHalaqaAttendanceSummary(int halaqaId)
        {
            var summaryData = _reportService.GetHalaqaAttendanceSummary(halaqaId);
            if (summaryData == null || !summaryData.Any())
            {
                //return NotFound("No attendance summary data found for this halaqa.");
                return Ok();

            }
            return Ok(summaryData);
        }


        #endregion

        #region Dashboard / IslamicProgress / Comparison
        [Authorize(Roles = "Admin , Parent")]

        [HttpGet("Dashboard/{teacherId}")]
        public ActionResult<TeacherDashboardDto> GetTeacherDashboard(int teacherId)
        {
            var dashboard = _reportService.GetTeacherDashboard(teacherId);
            if (dashboard == null)
            {
                //return NotFound("No dashboard data found for this teacher.");
                return Ok();

            }
            return Ok(dashboard);
        }
        [Authorize(Roles = "Admin , Parent")]

        [HttpGet("IslamicProgress/{halaqaId}")]
        public ActionResult<List<IslamicSubjectsDetailedProgressReportDto>> GetHalaqaIslamicProgress(int halaqaId)
        {
            var progressData = _reportService.GetHalaqaIslamicProgress(halaqaId);
            if (progressData == null || !progressData.Any())
            {
                //return NotFound("No Islamic subjects progress data found for this halaqa.");
                return Ok();

            }
            return Ok(progressData);
        }
        [Authorize(Roles = "Admin , Parent")]

        [HttpGet("Comparison")]
        public ActionResult<List<HalaqaComparisonDto>> GetHalaqasComparison([FromQuery] List<int> halaqaIds)
        {
            if (halaqaIds == null || !halaqaIds.Any())
            {
                //return BadRequest("You must send ids");
                return Ok();

            }

            var comparison = _reportService.GetHalaqasComparison(halaqaIds);
            return Ok(comparison);
        }
        #endregion
    }
}
