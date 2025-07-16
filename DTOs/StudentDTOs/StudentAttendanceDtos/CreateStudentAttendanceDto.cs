using MotqenIslamicLearningPlatform_API.Enums;

namespace MotqenIslamicLearningPlatform_API.DTOs.StudentDTOs.StudentAttendanceDtos
{
    public class CreateStudentAttendanceDto
    {
        public int StudentId { get; set; }
        public int HalaqaId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public AttendanceStatus Status { get; set; }
    }
}
