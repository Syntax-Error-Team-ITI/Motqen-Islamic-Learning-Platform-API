namespace MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.AttendanceSummary
{
    public class StudentAttendanceReportDto
    {
        public string StudentName { get; set; }
        public string HalaqaName { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string AttendanceStatus { get; set; }
    }
}
