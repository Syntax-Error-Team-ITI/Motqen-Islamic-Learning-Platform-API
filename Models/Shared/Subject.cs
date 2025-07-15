using MotqenIslamicLearningPlatform_API.Models.HalaqaModel;
using MotqenIslamicLearningPlatform_API.Models.StudentModel;
using MotqenIslamicLearningPlatform_API.Models.TeacherModel;

namespace MotqenIslamicLearningPlatform_API.Models.Shared
{
    public class Subject : BaseEntity
    {
        public string Name { get; set; }
        public virtual IList<TeacherSubject> SubjectTeachers { get; set; } = new List<TeacherSubject>();
        public virtual IList<Halaqa> HalaqaSubjects { get; set; } = new List<Halaqa>();
        public virtual IList<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();
    }
}
