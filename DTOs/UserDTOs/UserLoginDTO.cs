using System.ComponentModel.DataAnnotations;

namespace MotqenIslamicLearningPlatform_API.DTOs.UserDTOs
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "Email is required."),DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
