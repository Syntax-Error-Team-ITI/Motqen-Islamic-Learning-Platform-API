using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.StudentModel;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class QuranProgressTrackingRepository : GenericRepository<QuranProgressTracking>
    {
        public QuranProgressTrackingRepository(MotqenDbContext db) : base(db)
        { }
    }
}
