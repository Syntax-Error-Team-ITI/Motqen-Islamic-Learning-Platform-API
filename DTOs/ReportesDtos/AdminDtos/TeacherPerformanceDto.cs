using System.Collections.Generic;

namespace MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.AdminDtos
{
    public class TeacherPerformanceDto
    {
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public string Email { get; set; }
        public int TotalAssignedHalaqas { get; set; }
        public decimal AverageAttendanceRate { get; set; }
        public decimal AverageStudentProgress { get; set; }

        public List<TimeSeriesPoint>? AttendanceRateOverTime { get; set; } 
        public List<TimeSeriesPoint>? StudentProgressOverTime { get; set; } 
        public List<CategoryBreakdown>? HalaqaAssignmentBreakdown { get; set; } 
    }
} 