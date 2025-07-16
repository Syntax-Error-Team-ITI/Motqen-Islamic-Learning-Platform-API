using MotqenIslamicLearningPlatform_API.Enums;

namespace MotqenIslamicLearningPlatform_API.DTOs.TeacherDTOs.TeacherAttendanceDtos
{
    public class UpdateTeacherAttendanceDto
    {
        public int Id { get; set; }

        public AttendanceStatus Status { get; set; }
    }
}
