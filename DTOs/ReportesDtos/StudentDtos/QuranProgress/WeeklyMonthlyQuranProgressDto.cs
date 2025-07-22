namespace MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.QuranProgress
{
    public class WeeklyMonthlyQuranProgressDto
    {
        public string Period { get; set; } // e.g., "Week 1", "January", "2024-W01", "2024-01"
        public int TotalMemorizedLines { get; set; }
        public int TotalReviewedLines { get; set; }

        public int? HalaqaId { get; set; }
        public string? HalaqaName { get; set; }
    }
}
