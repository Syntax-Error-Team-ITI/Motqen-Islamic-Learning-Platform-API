using MotqenIslamicLearningPlatform_API.Enums;
using MotqenIslamicLearningPlatform_API.Models.Shared;

namespace MotqenIslamicLearningPlatform_API.Models.StudentModel
{
    public class QuranProgressTracking : BaseEntity
    {
        public int FromAyah { get; set; }
        public int ToAyah { get; set; }
        public int FromSurah { get; set; }
        public int ToSurah { get; set; }
        public ProgressType Type { get; set; }
        public int NumberOfLines { get; set; }
        public int? ProgressTrackingId { get; set; }
        public virtual ProgressTracking? ProgressTracking { get; set; } 

    }

}