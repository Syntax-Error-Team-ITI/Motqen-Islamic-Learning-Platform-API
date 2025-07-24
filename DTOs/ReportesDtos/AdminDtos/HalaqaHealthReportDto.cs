using System.Collections.Generic;

namespace MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.AdminDtos
{
    public class HalaqaHealthReportDto
    {
        public int HalaqaId { get; set; }
        public string HalaqaName { get; set; }
        public int StudentCount { get; set; }
        public int TeacherCount { get; set; }
        public decimal AverageAttendanceRate { get; set; }
        public decimal AverageProgress { get; set; }

    }
} 