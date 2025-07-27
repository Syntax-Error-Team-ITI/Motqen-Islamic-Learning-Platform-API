using System.ComponentModel.DataAnnotations;

namespace MotqenIslamicLearningPlatform_API.DTOs.UserDTOs
{
    public class StudentRegisterDTO : UserRegisterDTO
    {
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        //public int? Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;

        [MaxLength(14), MinLength(14, ErrorMessage = "Parent national id must be 14 digits")]
        public string ParentNationalId { get; set; } = string.Empty;
    }
}
