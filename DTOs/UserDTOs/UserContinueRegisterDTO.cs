using System.ComponentModel.DataAnnotations;

namespace MotqenIslamicLearningPlatform_API.DTOs.UserDTOs
{
    public class UserContinueRegisterDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        // common data 
        public string? Pic { get; set; }

        // parent specific data
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }

        [MaxLength(14), MinLength(14, ErrorMessage = "National id must be 14 digits")]
        public string? NationalId { get; set; }


        // student specific data
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
        //public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? Nationality { get; set; }

        [MaxLength(14), MinLength(14, ErrorMessage = "Parent national id must be 14 digits")]
        public string? ParentNationalId{ get; set; }
    }
}
