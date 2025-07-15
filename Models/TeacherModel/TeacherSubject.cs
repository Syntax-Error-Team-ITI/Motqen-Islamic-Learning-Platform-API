using Microsoft.EntityFrameworkCore;
using MotqenIslamicLearningPlatform_API.Models.Shared;

namespace MotqenIslamicLearningPlatform_API.Models.TeacherModel
{
    [PrimaryKey(nameof(TeacherId) , nameof(SubjectId))]

    public class TeacherSubject 
    {
        public int TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }
    }
}

