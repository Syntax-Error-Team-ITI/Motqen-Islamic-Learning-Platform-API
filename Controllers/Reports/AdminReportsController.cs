using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotqenIslamicLearningPlatform_API.Services.Reports;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.AdminDtos;
using Microsoft.AspNetCore.Authorization;

namespace MotqenIslamicLearningPlatform_API.Controllers.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        public AdminReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        #region Summary

        [Authorize (Roles = "Admin")]
        [HttpGet("dashboard-summary")]
        public ActionResult<AdminDashboardSummaryDto> GetDashboardSummary()
        {
            var summary = _reportService.GetAdminDashboardSummary();
            return Ok(summary);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("user-summary")]
        public ActionResult<List<UserSummaryDto>> GetUserSummary()
        {
            var users = _reportService.GetUserSummaryReport();
            return Ok(users);
        }
        #endregion

        #region  Performance
        [Authorize(Roles = "Admin")]

        [HttpGet("teacher-performance")]
        public ActionResult<List<TeacherPerformanceDto>> GetTeacherPerformance()
        {
            var data = _reportService.GetTeacherPerformanceReport();
            return Ok(data);
        }
        [Authorize(Roles = "Admin")]

        [HttpGet("student-performance")]
        public ActionResult<List<StudentPerformanceOverviewDto>> GetStudentPerformance()
        {
            var data = _reportService.GetStudentPerformanceOverview();
            return Ok(data);
        }

        #endregion

        #region Halaqa Health
        [Authorize(Roles = "Admin")]

        [HttpGet("halaqa-health")]
        public ActionResult<List<HalaqaHealthReportDto>> GetHalaqaHealth()
        {
            var data = _reportService.GetHalaqaHealthReport();
            return Ok(data);
        }
        #endregion
    }
}
