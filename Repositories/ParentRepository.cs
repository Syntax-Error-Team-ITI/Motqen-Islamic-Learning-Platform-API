using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.ParentModel;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class ParentRepository : GenericRepository<Parent>
    {
        public ParentRepository(MotqenDbContext db) : base(db)
        { }
    }
}
