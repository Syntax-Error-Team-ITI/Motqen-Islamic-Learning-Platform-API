using MotqenIslamicLearningPlatform_API.Enums;

namespace MotqenIslamicLearningPlatform_API.DTOs.TeacherDTOs.TeacherAttendanceDtos
{
    public class CreateTeacherAttendanceDto
    {
        public int TeacherId { get; set; }
        public int HalaqaId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public AttendanceStatus Status { get; set; }
    }
}
