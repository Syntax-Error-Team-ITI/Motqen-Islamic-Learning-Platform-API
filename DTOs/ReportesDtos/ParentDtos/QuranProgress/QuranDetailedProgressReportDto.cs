namespace MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.QuranProgress
{
    public class QuranDetailedProgressReportDto
    {
        public DateTime Date { get; set; }
        public string FromSurahName { get; set; } // Need to map FromSurah int to its name
        public string ToSurahName { get; set; }   // Need to map ToSurah int to its name
        public int FromAyah { get; set; }
        public int ToAyah { get; set; }
        public int NumberOfLines { get; set; }
        public string Type { get; set; } // "Memorization" or "Review"
        public string Status { get; set; } // From ProgressTracking
        public string Evaluation { get; set; } // From ProgressTracking
        public string Notes { get; set; } // From ProgressTracking
    }
}
