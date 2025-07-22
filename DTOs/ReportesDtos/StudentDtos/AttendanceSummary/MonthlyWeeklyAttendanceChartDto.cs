namespace MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.AttendanceSummary
{
    public class MonthlyWeeklyAttendanceChartDto
    {
        public string Period { get; set; } // e.g., "January", "Week 1"
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public int ExcusedCount { get; set; } // If applicable
    }
}
