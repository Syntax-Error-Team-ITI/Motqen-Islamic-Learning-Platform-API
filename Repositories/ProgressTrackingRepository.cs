using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.StudentModel;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class ProgressTrackingRepository : GenericRepository<ProgressTracking>
    {
        public ProgressTrackingRepository(MotqenDbContext db) : base(db)
        {
        }
    }
}
