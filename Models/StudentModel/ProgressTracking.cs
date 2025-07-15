using MotqenIslamicLearningPlatform_API.Models.Shared;
using MotqenIslamicLearningPlatform_API.Models.HalaqaModel;

namespace MotqenIslamicLearningPlatform_API.Models.StudentModel
{
    public class ProgressTracking : BaseEntity
    {
        public DateTime Date { get; set; }
        public string Status { get; set; } 
        public string Notes { get; set; }
        public string Evaluation { get; set; }

        public int? StudentId { get; set; }
        public virtual Student? Student { get; set; }
        
        public int? HalaqaId { get; set; }
        public virtual Halaqa? Halaqa { get; set; }
        public virtual QuranProgressTracking? QuranProgressTrackingDetail { get; set; }
        public virtual IslamicSubjectsProgressTracking? IslamicSubjectsProgressTrackingDetail { get; set; }
    }
}
