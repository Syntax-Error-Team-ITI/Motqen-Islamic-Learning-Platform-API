using Microsoft.EntityFrameworkCore;
using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.ParentModel;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class ParentRepository : GenericRepository<Parent>
    {
        public ParentRepository(MotqenDbContext db) : base(db)
        { }
        public override ICollection<Parent> GetAll(bool includeDeleted = false)
        {
            return Db.Parents.Where(p => !p.IsDeleted || includeDeleted).Include(p => p.User).Include(p => p.Students).ThenInclude(s => s.User).ToList();
        }
    }
}
