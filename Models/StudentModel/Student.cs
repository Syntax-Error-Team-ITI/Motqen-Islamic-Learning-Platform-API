using MotqenIslamicLearningPlatform_API.Models.HalaqaModel;
using MotqenIslamicLearningPlatform_API.Models.ParentModel;
using MotqenIslamicLearningPlatform_API.Models.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotqenIslamicLearningPlatform_API.Models.StudentModel
{
    public class Student : BaseEntity
    {
        public string Pic { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public DateTime BirthDate { get; set; }
        public string Nationality { get; set; }
        //public int UserId { get; set; }
        //public virtual User User { get; set; }

        [ForeignKey("Parent")]
        public int? ParentId { get; set; }
        public virtual Parent? Parent { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public virtual User? User { get; set; }

        public virtual IList<ProgressTracking> ProgressTrackings { get; set; } = new List<ProgressTracking>();
        public virtual IList<HalaqaStudent> HalaqaStudents { get; set; }

        public virtual IList<StudentAttendance> StudentAttendances { get; set; }
        public virtual IList<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();
    }
}
