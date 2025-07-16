using Microsoft.EntityFrameworkCore;
using MotqenIslamicLearningPlatform_API.Models.HalaqaModel;
using MotqenIslamicLearningPlatform_API.Models.Shared;

namespace MotqenIslamicLearningPlatform_API.Models.TeacherModel
{
    public class Teacher : BaseEntity
    {
        public string Pic { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual IList<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();
        public virtual IList<HalaqaTeacher> HalaqaTeachers { get; set; }
        public virtual IList<TeacherAttendance> TeacherAttendances { get; set; } 
    }
}
