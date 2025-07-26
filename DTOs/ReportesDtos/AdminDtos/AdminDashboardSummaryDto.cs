using System;
using System.Collections.Generic;

namespace MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.AdminDtos
{
    public class AdminDashboardSummaryDto
    {
        public int TotalUsers { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalStudents { get; set; }
        public int TotalHalaqas { get; set; }
        public decimal OverallAttendanceRate { get; set; }
        public int NewjoinToHalaqaThisMonth { get; set; }

    }
} 