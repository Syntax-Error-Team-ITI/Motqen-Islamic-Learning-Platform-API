using MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.QuranProgress;

namespace MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.TeacherDtos
{
    public class HalaqaComparisonDto
    {
        public int HalaqaId { get; set; }
        public string HalaqaName { get; set; }
        public QuranSummaryCountersDto QuranProgress { get; set; }
        public int TotalStudents { get; set; }
        public decimal AverageLinesPerStudent =>
            QuranProgress.TotalLinesMemorized / (TotalStudents > 0 ? TotalStudents : 1);
    }
}
