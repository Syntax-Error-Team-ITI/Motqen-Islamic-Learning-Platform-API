using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.HalaqaModel;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class ClassScheduleRepository : GenericRepository<ClassSchedule>
    {
        public ClassScheduleRepository(MotqenDbContext db) : base(db)
        { 
        }
    }
}
