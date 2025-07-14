using MotqenIslamicLearningPlatform_API.Models;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class IslamicSubjectsProgressTrackingRepository : GenericRepository<IslamicSubjectsProgressTracking>
    {
        public IslamicSubjectsProgressTrackingRepository(MotqenDbContext db) : base(db)
        { }
    }
}
