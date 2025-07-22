namespace MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.TeacherDtos
{
    public class TeacherDashboardDto
    {
        public int TotalHalaqas { get; set; }
        public int TotalStudents { get; set; }
        public decimal OverallAttendanceRate { get; set; }
        public List<HalaqaProgressSummaryDto> HalaqasProgress { get; set; }
    }

    public class HalaqaProgressSummaryDto
    {
        public int HalaqaId { get; set; }
        public string HalaqaName { get; set; }
        public string SubjectName { get; set; }
        public int StudentsCount { get; set; }
        public double AttendanceRate { get; set; }
    }
}