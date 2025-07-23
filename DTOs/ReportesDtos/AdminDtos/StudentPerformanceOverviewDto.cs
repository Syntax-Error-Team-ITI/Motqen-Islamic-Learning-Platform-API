using System;
using System.Collections.Generic;

namespace MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.AdminDtos
{
    public class StudentPerformanceOverviewDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string Email { get; set; }
        public decimal AttendanceRate { get; set; }
        public int TotalQuranLinesMemorized { get; set; }
        public int TotalQuranLinesReviewed { get; set; }
        public int TotalIslamicPagesCompleted { get; set; }

        // Chart-ready fields
        public List<TimeSeriesPoint>? AttendanceOverTime { get; set; } // e.g., per month
        public List<TimeSeriesPoint>? QuranMemorizationOverTime { get; set; } // e.g., per month
        public List<TimeSeriesPoint>? QuranReviewOverTime { get; set; } // e.g., per month
        public List<TimeSeriesPoint>? IslamicProgressOverTime { get; set; } // e.g., per month
    }
} 