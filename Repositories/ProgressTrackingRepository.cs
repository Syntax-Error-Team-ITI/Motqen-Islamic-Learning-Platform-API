using MotqenIslamicLearningPlatform_API.Models;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class ProgressTrackingRepository : GenericRepository<ProgressTracking>
    {
        public ProgressTrackingRepository(MotqenDbContext db) : base(db)
        {
        }
    }
}
