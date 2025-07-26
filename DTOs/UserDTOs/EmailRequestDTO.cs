namespace MotqenIslamicLearningPlatform_API.DTOs.UserDTOs
{
    public class EmailRequestDTO
    {
        public string ToEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
