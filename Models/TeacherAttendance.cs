using Microsoft.EntityFrameworkCore;

namespace MotqenIslamicLearningPlatform_API.Models
{
    [PrimaryKey(nameof(TeacherId), nameof(HalaqaId))]

    public class TeacherAttendance
    {
        public int TeacherId { get; set; }
        public int HalaqaId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public AttendanceStatus Status { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual Halaqa Halaqa { get; set; }
    }
}
