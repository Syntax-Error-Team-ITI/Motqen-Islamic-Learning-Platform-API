namespace MotqenIslamicLearningPlatform_API.DTOs.HalaqaDTOs
{
    public class CreateHalaqaDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string LiveLink { get; set; }
        public string GenderGroup { get; set; }
        public int? SubjectId { get; set; }
    }
}
