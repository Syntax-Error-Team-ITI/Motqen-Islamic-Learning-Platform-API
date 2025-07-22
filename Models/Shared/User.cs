using Microsoft.AspNetCore.Identity;
using MotqenIslamicLearningPlatform_API.Models.ParentModel;
using MotqenIslamicLearningPlatform_API.Models.StudentModel;
using MotqenIslamicLearningPlatform_API.Models.TeacherModel;
using System.ComponentModel.DataAnnotations;

namespace MotqenIslamicLearningPlatform_API.Models.Shared
{
    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public bool IsDeleted { get; set; } = false;

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public virtual Student? Student { get; set; }
        public virtual Teacher? Teacher { get; set; }
        public virtual Parent? Parent { get; set; }
    }
}
