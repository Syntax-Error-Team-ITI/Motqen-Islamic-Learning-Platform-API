using MotqenIslamicLearningPlatform_API.Enums;

namespace MotqenIslamicLearningPlatform_API.DTOs.HalaqaDTOs
{
    public class HalaqaDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GuestLiveLink { get; set; }
        public string HostLiveLink { get; set; }
        public GenderGroup GenderGroup { get; set; }
        public string SubjectName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
