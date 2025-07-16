using MotqenIslamicLearningPlatform_API.Models.Shared;
using MotqenIslamicLearningPlatform_API.Models.StudentModel;

namespace MotqenIslamicLearningPlatform_API.DTOs.StudentDTOs
{
    public class StudentDetailedDisplayDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Pic { get; set; }= string.Empty;
        public string Gender { get; set; }= string.Empty;
        public int Age { get; set; }
        public DateTime BirthDate { get; set; }
        public string Nationality { get; set; } = string.Empty;
        public string Parent { get; set; } = string.Empty;
    }
}
