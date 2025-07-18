namespace MotqenIslamicLearningPlatform_API.DTOs.ReportesDtos.ParentDtos.IslamicSubjectsProgress
{
    public class IslamicSubjectProgressChartDto
    {
        public string SubjectName { get; set; }
        public int TotalPagesCompleted { get; set; }
        // Or if tracking by lessons: public int TotalLessonsCompleted { get; set; }

    }
}
