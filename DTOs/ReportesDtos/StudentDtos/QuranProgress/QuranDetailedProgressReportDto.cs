using MotqenIslamicLearningPlatform_API.Enums;

namespace MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.QuranProgress
{
    public class QuranDetailedProgressReportDto
    {
        public DateTime Date { get; set; }
        public int FromSurahNumber { get; set; } // Need to map FromSurah int to its name
        public int ToSurahNumber { get; set; }   // Need to map ToSurah int to its name
        public int FromAyah { get; set; }
        public int ToAyah { get; set; }
        public int NumberOfLines { get; set; }
        public ProgressType Type { get; set; } // "Memorization" or "Review"
        public string Status { get; set; } // From ProgressTracking
        public string Evaluation { get; set; } // From ProgressTracking
        public string Notes { get; set; } // From ProgressTracking
    }
}
