using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotqenIslamicLearningPlatform_API.Services.Reports;
using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.AdminDtos;

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

        [HttpGet("dashboard-summary")]
        public ActionResult<AdminDashboardSummaryDto> GetDashboardSummary()
        {
            var summary = _reportService.GetAdminDashboardSummary();
            return Ok(summary);
        }

        [HttpGet("user-summary")]
        public ActionResult<List<UserSummaryDto>> GetUserSummary()
        {
            var users = _reportService.GetUserSummaryReport();
            return Ok(users);
        }

        [HttpGet("teacher-performance")]
        public ActionResult<List<TeacherPerformanceDto>> GetTeacherPerformance()
        {
            var data = _reportService.GetTeacherPerformanceReport();
            return Ok(data);
        }

        [HttpGet("student-performance")]
        public ActionResult<List<StudentPerformanceOverviewDto>> GetStudentPerformance()
        {
            var data = _reportService.GetStudentPerformanceOverview();
            return Ok(data);
        }

        [HttpGet("halaqa-health")]
        public ActionResult<List<HalaqaHealthReportDto>> GetHalaqaHealth()
        {
            var data = _reportService.GetHalaqaHealthReport();
            return Ok(data);
        }
    }
}
