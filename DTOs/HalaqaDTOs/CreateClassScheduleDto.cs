using MotqenIslamicLearningPlatform_API.Enums;

namespace MotqenIslamicLearningPlatform_API.DTOs.HalaqaDTOs
{
    public class CreateClassScheduleDto
    {
        public DaysOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public int HalaqaId { get; set; }
    }
}
