using MotqenIslamicLearningPlatform_API.Models.HalaqaModel;
using MotqenIslamicLearningPlatform_API.Models.TeacherModel;

namespace MotqenIslamicLearningPlatform_API.DTOs.TeacherDTOs.HalaqaTeacherDtos
{
    public class HalaqaTeacherDto
    {

        public int HalaqaId { get; set; }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public string HalaqaName { get; set; }
        public string SubjectName { get; set; }

    }
}
