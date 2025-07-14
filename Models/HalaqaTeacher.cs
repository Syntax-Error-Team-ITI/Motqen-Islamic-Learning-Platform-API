using Microsoft.EntityFrameworkCore;

namespace MotqenIslamicLearningPlatform_API.Models
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
