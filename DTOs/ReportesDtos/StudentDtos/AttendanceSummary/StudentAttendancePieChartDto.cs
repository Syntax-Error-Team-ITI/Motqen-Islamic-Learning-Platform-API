namespace MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.AttendanceSummary
{
    public class StudentAttendancePieChartDto
    {
        public string Status { get; set; } // e.g., "Present", "Absent", "Late"
        public int Count { get; set; }
        public decimal Percentage { get; set; }
        public string HalaqaName { get; set; }
        public int? HalaqaId { get; set; } 
    }
}
