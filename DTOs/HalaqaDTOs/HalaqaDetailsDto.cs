namespace MotqenIslamicLearningPlatform_API.DTOs.HalaqaDTOs
{
    public class HalaqaDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GuestLiveLink { get; set; }
        public string HostLiveLink { get; set; }
        public string GenderGroup { get; set; }
        public string SubjectName { get; set; }
        public bool IsDeleted { get; set; }

        public IList<ClassScheduleDto> ClassSchedules { get; set; } 

        
    }
}
