using System.ComponentModel.DataAnnotations;

namespace MotqenIslamicLearningPlatform_API.DTOs.UserDTOs
{
    public class ParentRegisterDTO : UserRegisterDTO
    {
        [Required(ErrorMessage = "PhoneNumber is required")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; } = string.Empty;

        [MaxLength(14), MinLength(14, ErrorMessage = "National id must be 14 digits")]
        public string NationalId { get; set; } = string.Empty;
    }
}
