namespace MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.IslamicSubjectsProgress
{
    public class IslamicSubjectProgressOverTimeChartDto
    {
        public DateTime Date { get; set; }
        public int PagesOrLessonsCompleted { get; set; } // Can be pages or lessons
        public string SubjectName { get; set; } // To identify which subject is being tracked

    }
}
