using MotqenIslamicLearningPlatform_API.Enums;

namespace MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.AttendanceSummary
{
    public class StudentAttenndanceDetails
    {
        public DateTime AttendanceDate { get; set; }
        public string Status { get; set; }
        public string Subject { get; set; }
        public string Halaqa { get; set; }
    }

}
