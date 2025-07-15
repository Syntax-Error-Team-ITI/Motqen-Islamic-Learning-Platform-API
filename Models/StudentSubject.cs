using Microsoft.EntityFrameworkCore;

namespace MotqenIslamicLearningPlatform_API.Models
{
    [PrimaryKey(nameof(StudentId), nameof(SubjectId))]
    public class StudentSubject
    {
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
