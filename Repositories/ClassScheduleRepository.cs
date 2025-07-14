using MotqenIslamicLearningPlatform_API.Models;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class ClassScheduleRepository : GenericRepository<ClassSchedule>
    {
        public ClassScheduleRepository(MotqenDbContext db) : base(db)
        { 
        }
    }
}
