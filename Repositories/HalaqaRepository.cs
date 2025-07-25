using Microsoft.EntityFrameworkCore;
using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.HalaqaModel;
namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class HalaqaRepository : GenericRepository<Halaqa>
    {
        public HalaqaRepository(MotqenDbContext db) : base(db)
        {

        }

        public IList<Halaqa> GetAllIncludeSubject(bool includeDeleted = false)
        {
            if (!includeDeleted)
            {
                return Db.Halaqas
                    .Include(h => h.Subject)
                    .Where(h => !h.IsDeleted)
                    .ToList();
            }
            return Db.Halaqas
                .Include(h => h.Subject)
                .ToList();
        }
        public Halaqa GetByIdIncludeSubject(int halaqaId,bool includeDeleted = false  )
        {
            if (!includeDeleted)
            {
                return Db.Halaqas
                    .Include(h => h.Subject)
                    .FirstOrDefault(h => h.Id == halaqaId && !h.IsDeleted);
            }
            return Db.Halaqas
                .Include(h => h.Subject)
                .FirstOrDefault(h => h.Id == halaqaId);
        }

        public Halaqa GetByIdIncludeSubjectAndClassSchedules(int halaqaId, bool includeDeleted = false)
        {
            if (!includeDeleted)
            {
                return Db.Halaqas
                    .Include(h => h.Subject)
                    .Include(h => h.ClassSchedules)
                    .FirstOrDefault(h => h.Id == halaqaId && !h.IsDeleted);
            }
            return Db.Halaqas
                .Include(h => h.Subject)
                .Include(h => h.ClassSchedules)
                .FirstOrDefault(h => h.Id == halaqaId);
        }
    }
}
