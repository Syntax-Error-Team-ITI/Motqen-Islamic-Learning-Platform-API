using MotqenIslamicLearningPlatform_API.Models.Shared;
using MotqenIslamicLearningPlatform_API.Models.StudentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotqenIslamicLearningPlatform_API.Models.ParentModel
{
    public class Parent : BaseEntity
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Pic { get; set; } = string.Empty;

        [MaxLength(14), MinLength(14, ErrorMessage = "National number must be 14 digits")]
        [Required(ErrorMessage = "National ID is required.")]
        public string NationalId { get; set; } = string.Empty;


        [ForeignKey("User")]
        public string? UserId { get; set; }
        public virtual User? User { get; set; }

        public IList<Student> Students { get; set; } = new List<Student>();
    }
}
