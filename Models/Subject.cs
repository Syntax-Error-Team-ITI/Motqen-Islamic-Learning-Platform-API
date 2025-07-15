namespace MotqenIslamicLearningPlatform_API.Models
{
    public class Subject : BaseEntity
    {
        public string Name { get; set; }
        public virtual IList<TeacherSubject> SubjectTeachers { get; set; } = new List<TeacherSubject>();
        public virtual IList<Halaqa> HalaqaSubjects { get; set; } = new List<Halaqa>();
        public virtual IList<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();
    }
}
