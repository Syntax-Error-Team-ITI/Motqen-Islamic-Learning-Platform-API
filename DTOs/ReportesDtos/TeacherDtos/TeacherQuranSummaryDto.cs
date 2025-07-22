namespace MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.TeacherDtos
{
    public class TeacherQuranSummaryDto
    {
        public int HalaqaId { get; set; }
        public string HalaqaName { get; set; }
        public int TotalStudents { get; set; }
        public int TotalLinesMemorized { get; set; }
        public int TotalLinesReviewed { get; set; }
        public decimal AverageLinesPerStudent { get; set; }
    }
}
