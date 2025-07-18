namespace MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.AttendanceSummary
{
    public class StudentHalaqaComparisonReportDto
    {
        public string Metric { get; set; } // e.g., "Average Memorized Lines", "Average Reviewed Lines", "Average Islamic Pages", "Attendance Percentage"
        public decimal StudentValue { get; set; }
        public decimal HalaqaAverageValue { get; set; }
        public int StudentId { get; set; } // To identify the specific student
        public int HalaqaId { get; set; } // To identify the specific halaqa

    }
}
