using MotqenIslamicLearningPlatform_API.Enums;

namespace MotqenIslamicLearningPlatform_API.DTOs.ProgressDTOs
{
    public class ProgressListDTO
    {
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public string Evaluation { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string HalaqaName { get; set; } = string.Empty;
        public string HalaqaSubject { get; set; } = string.Empty;

        // Quran Progress Tracking Details
        public int FromAyah { get; set; }
        public int ToAyah { get; set; }
        public int FromSurah { get; set; }
        public int ToSurah { get; set; }
        public ProgressType Type { get; set; }
        public int NumberOfLines { get; set; }


        // Islamic Subjects Progress Tracking Details
        public string Subject { get; set; } = string.Empty;
        public int FromPage { get; set; }
        public int ToPage { get; set; }
        public string LessonName { get; set; } = string.Empty;


    }
}
