using MotqenIslamicLearningPlatform_API.Enums;

namespace MotqenIslamicLearningPlatform_API.DTOs.HalaqaDTOs
{
    public class CreateClassScheduleDto
    {
        public DaysOfWeek Day { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int HalaqaId { get; set; }
    }
}
