namespace MotqenIslamicLearningPlatform_API.DTOs.StudentDTOs
{
    public class StudentListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Nationality { get; set; } = string.Empty;
        public int ParentId { get; set; }
        public string Parent { get; set; } = string.Empty;
    }
}
