using MotqenIslamicLearningPlatform_API.Models;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class QuranProgressTrackingRepository : GenericRepository<QuranProgressTracking>
    {
        public QuranProgressTrackingRepository(MotqenDbContext db) : base(db)
        { }
    }
}
