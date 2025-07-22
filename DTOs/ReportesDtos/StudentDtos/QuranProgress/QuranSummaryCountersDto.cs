namespace MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.QuranProgress
{
    public class QuranSummaryCountersDto
    {
        public int TotalSurahsMemorized { get; set; }
        public decimal TotalJuzsMemorized { get; set; } // Assuming Juz is equivalent to Part
        public int TotalSurahsReviewed { get; set; }
        public decimal TotalJuzsReviewed { get; set; }
        public int TotalLinesMemorized { get; set; }
        public int TotalLinesReviewed { get; set; }
    }
}
