namespace MotqenIslamicLearningPlatform_API.Models
{
    public class Halaqa : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string LiveLink { get; set; }
        public string GenderGroup { get; set; }
        public virtual IList<ProgressTracking> ProgressTrackings { get; set; } 
        public virtual IList<HalaqaStudent> HalaqaStudents { get; set; }
        public virtual IList<HalaqaTeacher> HalaqaTeachers { get; set; }
        public virtual IList<StudentAttendance> StudentAttendances { get; set; }
        public virtual IList<TeacherAttendance> TeacherAttendances { get; set; }
        public virtual IList<ClassSchedule> ClassSchedules { get; set; }
        public int? SubjectId { get; set; }
        public virtual Subject? Subject { get; set; }

    }
}
