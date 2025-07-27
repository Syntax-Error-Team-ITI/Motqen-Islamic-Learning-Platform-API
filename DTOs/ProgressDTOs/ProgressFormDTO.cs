using MotqenIslamicLearningPlatform_API.Enums;

namespace MotqenIslamicLearningPlatform_API.DTOs.ProgressDTOs
{
    public class ProgressFormDTO
    {
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string Status { get; set; }
        public string Notes { get; set; }
        public string Evaluation { get; set; }
        public int StudentId { get; set; }
        public int HalaqaId { get; set; }
        public bool IsQuranTracking { get; set; } = false;


        // QuranProgressTracking
        public int? FromAyah { get; set; }
        public int? ToAyah { get; set; }
        public int? FromSurah { get; set; }
        public int? ToSurah { get; set; }
        public ProgressType? Type { get; set; }
        public int? NumberOfLines { get; set; }


        // IslamicSubjectsProgressTracking
        public int? FromPage { get; set; }
        public int? ToPage { get; set; }
        public int? ProgressTrackingId { get; set; }
        public string? LessonName { get; set; }

    }
}
