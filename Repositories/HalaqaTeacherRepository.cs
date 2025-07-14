using MotqenIslamicLearningPlatform_API.Models;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class HalaqaTeacherRepository : GenericRepository<HalaqaTeacher>
    {
        public HalaqaTeacherRepository(MotqenDbContext db) : base(db)
        { }
    }
}
