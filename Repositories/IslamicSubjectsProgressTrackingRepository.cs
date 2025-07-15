using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.StudentModel;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class IslamicSubjectsProgressTrackingRepository : GenericRepository<IslamicSubjectsProgressTracking>
    {
        public IslamicSubjectsProgressTrackingRepository(MotqenDbContext db) : base(db)
        { }
    }
}
