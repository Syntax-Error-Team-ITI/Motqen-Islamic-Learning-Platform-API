using MotqenIslamicLearningPlatform_API.Enums;

namespace MotqenIslamicLearningPlatform_API.DTOs.HalaqaDTOs
{
    public class CreateHalaqaDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public GenderGroup GenderGroup { get; set; }
        public int? SubjectId { get; set; }
    }
}
