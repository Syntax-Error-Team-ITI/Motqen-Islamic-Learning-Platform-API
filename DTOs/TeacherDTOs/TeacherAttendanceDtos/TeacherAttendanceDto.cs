using MotqenIslamicLearningPlatform_API.Enums;

namespace MotqenIslamicLearningPlatform_API.DTOs.TeacherDTOs.TeacherAttendanceDtos
{
    public class TeacherAttendanceDto
    {
        public int Id { get; set; }

        public int TeacherId { get; set; }
        public int HalaqaId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public string HalaqaName { get; set; } = string.Empty;
        public DateTime AttendanceDate { get; set; }
        public AttendanceStatus Status { get; set; }
    }
}
