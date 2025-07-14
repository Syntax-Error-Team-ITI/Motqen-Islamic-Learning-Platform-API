namespace MotqenIslamicLearningPlatform_API.Models
{
    public class Teacher : BaseEntity
    {
        public string Pic { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Specialization { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual IList<HalaqaTeacher> HalaqaTeachers { get; set; }
        public virtual IList<TeacherAttendance> TeacherAttendances { get; set; } 
    }
}
