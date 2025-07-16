using MotqenIslamicLearningPlatform_API.Models.Shared;
using MotqenIslamicLearningPlatform_API.Models.StudentModel;

namespace MotqenIslamicLearningPlatform_API.DTOs.StudentDTOs.StudentSubjectDtos
{
    public class StudentSubjectDto
    {
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string StudentName { get; set; }

    }
}
