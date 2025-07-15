using MotqenIslamicLearningPlatform_API.Enums;
using MotqenIslamicLearningPlatform_API.Models.Shared;

namespace MotqenIslamicLearningPlatform_API.Models.HalaqaModel
{
    public class ClassSchedule : BaseEntity
    {
        public DaysOfWeek Day { get; set; } 
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int HalaqaId { get; set; }
        public virtual Halaqa Halaqa { get; set; }

    }
}
