using System.ComponentModel.DataAnnotations;

namespace MotqenIslamicLearningPlatform_API.DTOs.UserDTOs
{
    public class UserRegisterDTO
    {
        //public string? GoogleIdToken { get; set; }

        [Required(ErrorMessage = "Email is required."), DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "FirstName is required.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "LastName is required.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required."), DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Compare("Password"), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
