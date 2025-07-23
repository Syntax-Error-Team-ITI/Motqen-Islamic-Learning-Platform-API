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
        public int NewRegistrationsThisMonth { get; set; }

        // Chart-ready fields
        public List<TimeSeriesPoint> RegistrationsOverTime { get; set; } // e.g., per month
        public List<TimeSeriesPoint> AttendanceRateOverTime { get; set; } // e.g., per month
        public List<CategoryBreakdown> UserRoleBreakdown { get; set; } // Pie chart: students, teachers, parents, admins
    }
} 