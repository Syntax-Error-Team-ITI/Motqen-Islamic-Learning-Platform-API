using MotqenIslamicLearningPlatform_API.Models.HalaqaModel;
using MotqenIslamicLearningPlatform_API.Models.Shared;
using MotqenIslamicLearningPlatform_API.Models.TeacherModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotqenIslamicLearningPlatform_API.DTOs.UserDTOs
{
    public class AddTeacherDTO : UserRegisterDTO
    {
        public string Gender { get; set; }
        public int Age { get; set; }
    }
}
