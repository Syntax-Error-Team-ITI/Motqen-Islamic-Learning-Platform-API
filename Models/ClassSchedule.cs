namespace MotqenIslamicLearningPlatform_API.Models
{
    public enum DayOfWeek
    {
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday
    }
    public class ClassSchedule : BaseEntity
    {
        public DayOfWeek Day { get; set; } 
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int HalaqaId { get; set; }
        public virtual Halaqa Halaqa { get; set; }

    }
}
