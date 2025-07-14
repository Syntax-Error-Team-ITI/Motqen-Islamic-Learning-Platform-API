namespace MotqenIslamicLearningPlatform_API.Models
{
    public class QuranProgressTracking : BaseEntity
    {
        public int FromAyah { get; set; }
        public int ToAyah { get; set; }
        public int FromSurah { get; set; }
        public int ToSurah { get; set; }
        public int? ProgressTrackingId { get; set; }
        public virtual ProgressTracking? ProgressTracking { get; set; } 

    }
}