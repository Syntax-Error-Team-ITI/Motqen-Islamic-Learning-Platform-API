namespace MotqenIslamicLearningPlatform_API.Models
{
    public class IslamicSubjectsProgressTracking : BaseEntity
    {
        public string Subject { get; set; }
        public int FromPage { get; set; }
        public int ToPage { get; set; }
        public string LessonName { get; set; }
        public int? ProgressTrackingId { get; set; }
        public virtual ProgressTracking? ProgressTracking { get; set; } 

    }
}