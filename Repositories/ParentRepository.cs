using MotqenIslamicLearningPlatform_API.Models;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class ParentRepository : GenericRepository<Parent>
    {
        public ParentRepository(MotqenDbContext db) : base(db)
        { }
    }
}
