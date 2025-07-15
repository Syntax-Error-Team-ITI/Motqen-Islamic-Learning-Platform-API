using Microsoft.EntityFrameworkCore;

namespace MotqenIslamicLearningPlatform_API.Models
{
    [PrimaryKey(nameof(TeacherId) , nameof(SubjectId))]

    public class TeacherSubject 
    {
        public int TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }
        public bool IsDeleted { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

