using System.ComponentModel.DataAnnotations;

namespace MotqenIslamicLearningPlatform_API.DTOs.UserDTOs
{
    public class UserRegisterDTO
    {
        [Required(ErrorMessage = "Email is required."), DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "FirstName is required.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "LastName is required.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required.")]
        public string Role { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "BirthDate is required."), DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Age is required.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Password is required."), DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Compare("Password"), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
