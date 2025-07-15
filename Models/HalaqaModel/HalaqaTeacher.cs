using Microsoft.EntityFrameworkCore;
using MotqenIslamicLearningPlatform_API.Models.TeacherModel;

namespace MotqenIslamicLearningPlatform_API.Models.HalaqaModel
{
    [PrimaryKey(nameof(TeacherId), nameof(HalaqaId))]

    public class HalaqaTeacher
    {
        public int HalaqaId { get; set; }
        public int TeacherId { get; set; }
        public virtual Halaqa Halaqa { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
