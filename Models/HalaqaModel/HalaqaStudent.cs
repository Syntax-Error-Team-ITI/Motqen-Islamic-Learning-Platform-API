using Microsoft.EntityFrameworkCore;
using MotqenIslamicLearningPlatform_API.Models.StudentModel;

namespace MotqenIslamicLearningPlatform_API.Models.HalaqaModel
{
    [PrimaryKey(nameof(HalaqaId), nameof(StudentId))]

    public class HalaqaStudent
    {
        public int HalaqaId { get; set; }
        public int StudentId { get; set; }
        public DateTime DateJoined { get; set; }
        public virtual Halaqa Halaqa { get; set; }
        public virtual Student Student { get; set; }
    }
}
