namespace MotqenIslamicLearningPlatform_API.DTOs.StudentDTOs
{
    public class StudentHalaqaFormDTO
    {
        public int HalaqaId { get; set; }
        public int StudentId { get; set; }
        public DateTime DateJoined { get; set; } = DateTime.UtcNow;
    }
}
