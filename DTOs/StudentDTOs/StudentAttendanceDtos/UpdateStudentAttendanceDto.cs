using MotqenIslamicLearningPlatform_API.Enums;

namespace MotqenIslamicLearningPlatform_API.DTOs.StudentDTOs.StudentAttendanceDtos
{
    public class UpdateStudentAttendanceDto
    {
        public int Id { get; set; }
        public AttendanceStatus Status { get; set; }
    }
}
