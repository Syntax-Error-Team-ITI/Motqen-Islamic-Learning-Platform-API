using MotqenIslamicLearningPlatform_API.Models.Shared;
using MotqenIslamicLearningPlatform_API.Models.TeacherModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotqenIslamicLearningPlatform_API.DTOs.TeacherDTOs.TeacherSubjectDtos
{
    public class TeacherSubjectDto
    {
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
    }
}
