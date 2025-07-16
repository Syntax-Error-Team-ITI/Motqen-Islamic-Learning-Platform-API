using MotqenIslamicLearningPlatform_API.Enums;
using MotqenIslamicLearningPlatform_API.Models.HalaqaModel;
using MotqenIslamicLearningPlatform_API.Models.StudentModel;

namespace MotqenIslamicLearningPlatform_API.DTOs.StudentDTOs.StudentAttendanceDtos
{
    public class StudentAttendanceDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int HalaqaId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string HalaqaName { get; set; } = string.Empty;
        public DateTime AttendanceDate { get; set; }
        public AttendanceStatus Status { get; set; }
    }
}
