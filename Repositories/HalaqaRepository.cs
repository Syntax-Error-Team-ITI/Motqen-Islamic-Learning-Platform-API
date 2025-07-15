using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.HalaqaModel;
namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class HalaqaRepository : GenericRepository<Halaqa>
    {
        public HalaqaRepository(MotqenDbContext db) : base(db)
        {

        }
    }
}
