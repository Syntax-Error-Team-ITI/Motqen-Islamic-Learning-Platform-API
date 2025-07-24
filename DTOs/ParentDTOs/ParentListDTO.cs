namespace MotqenIslamicLearningPlatform_API.DTOs.ParentDTOs
{
    public class ParentListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public List<string> Children { get; set; } = new List<string>();
    }
}
