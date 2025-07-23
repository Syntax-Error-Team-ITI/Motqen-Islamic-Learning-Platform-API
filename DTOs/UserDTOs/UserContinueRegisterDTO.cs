using System.ComponentModel.DataAnnotations;

namespace MotqenIslamicLearningPlatform_API.DTOs.UserDTOs
{
    public class UserContinueRegisterDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        // common data 
        public string? Pic { get; set; }
        public string? Gender { get; set; }
        public int? Age { get; set; }


        // parent specific data
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }


        // student specific data
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
        public string? Nationality { get; set; }


        // teacher specific data
        // - pic 
        // - gender
        // - age
    }
}
