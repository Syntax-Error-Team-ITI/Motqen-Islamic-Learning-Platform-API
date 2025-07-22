namespace MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.IslamicSubjectsProgress
{
    public class IslamicSubjectsDetailedProgressReportDto
    {
        public string StudentName { get; set; }
        public DateTime Date { get; set; }
        public string Subject { get; set; }
        public string LessonName { get; set; }
        public int FromPage { get; set; }
        public int ToPage { get; set; }
        public string Status { get; set; } // From ProgressTracking
        public string Evaluation { get; set; } // From ProgressTracking
        public string Notes { get; set; } // From ProgressTracking

    }
}
