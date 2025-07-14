using MotqenIslamicLearningPlatform_API.Models;
namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class HalaqaRepository : GenericRepository<Halaqa>
    {
        public HalaqaRepository(MotqenDbContext db) : base(db)
        {

        }
    }
}
