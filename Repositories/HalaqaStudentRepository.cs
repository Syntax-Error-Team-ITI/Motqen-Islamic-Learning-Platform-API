using MotqenIslamicLearningPlatform_API.Models;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class HalaqaStudentRepository : GenericRepository<HalaqaStudent>
    {
        public HalaqaStudentRepository(MotqenDbContext db) : base(db)
        {
        }
    }
}
