using Microsoft.EntityFrameworkCore;
using MotqenIslamicLearningPlatform_API.Models.Shared;

namespace MotqenIslamicLearningPlatform_API.Models.StudentModel
{
    [PrimaryKey(nameof(StudentId), nameof(SubjectId))]
    public class StudentSubject
    {
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
