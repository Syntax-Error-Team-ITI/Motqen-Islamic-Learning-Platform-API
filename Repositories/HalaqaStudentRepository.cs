using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.HalaqaModel;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class HalaqaStudentRepository : GenericRepository<HalaqaStudent>
    {
        public HalaqaStudentRepository(MotqenDbContext db) : base(db)
        {
        }
    }
}
