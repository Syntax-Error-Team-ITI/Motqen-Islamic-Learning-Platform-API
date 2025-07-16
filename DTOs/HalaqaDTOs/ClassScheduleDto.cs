using MotqenIslamicLearningPlatform_API.Enums;

namespace MotqenIslamicLearningPlatform_API.DTOs.HalaqaDTOs
{
    public class ClassScheduleDto
    {
        public DaysOfWeek Day { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string HalaqaName { get; set; }
        public bool IsDeleted { get; set; }


    }
}
