using Microsoft.EntityFrameworkCore;
using MotqenIslamicLearningPlatform_API.Enums;
using MotqenIslamicLearningPlatform_API.Models.HalaqaModel;

namespace MotqenIslamicLearningPlatform_API.Models.TeacherModel
{
   
    public class TeacherAttendance
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int HalaqaId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public AttendanceStatus Status { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual Halaqa Halaqa { get; set; }
    }
}
