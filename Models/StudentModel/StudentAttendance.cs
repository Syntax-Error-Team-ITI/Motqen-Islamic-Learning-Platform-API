using Microsoft.EntityFrameworkCore;
using MotqenIslamicLearningPlatform_API.Enums;
using MotqenIslamicLearningPlatform_API.Models.HalaqaModel;

namespace MotqenIslamicLearningPlatform_API.Models.StudentModel
{
    
    public class StudentAttendance
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int HalaqaId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public AttendanceStatus Status { get; set; }
        public virtual Student Student { get; set; }
        public virtual Halaqa Halaqa { get; set; }
    }
}
