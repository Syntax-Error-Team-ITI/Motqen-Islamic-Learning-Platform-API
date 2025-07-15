using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.Shared;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class SubjectRepository : GenericRepository<Subject>
    {
        public SubjectRepository(MotqenDbContext db) : base(db)
        {
        }
    }
}
