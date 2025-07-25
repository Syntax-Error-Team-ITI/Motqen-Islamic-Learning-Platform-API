using Microsoft.EntityFrameworkCore;
using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.HalaqaModel;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class ClassScheduleRepository : GenericRepository<ClassSchedule>
    {
        public ClassScheduleRepository(MotqenDbContext db) : base(db)
        { 
        }

        public IList<ClassSchedule> GetAllIncludeHalaqa(bool includeDeleted = false)
        {
            if (!includeDeleted)
            {
                return Db.ClassSchedules
                    .Include(cs => cs.Halaqa)
                    .Where(cs => !cs.IsDeleted)
                    .ToList();
            }
            return Db.ClassSchedules
                .Include(cs => cs.Halaqa)
                .ToList();
        }

        public ClassSchedule GetByIdIncludeHalaqa(int id, bool includeDeleted = false)
        {
            if (!includeDeleted)
            {
                return Db.ClassSchedules
                    .Include(cs => cs.Halaqa)
                    .FirstOrDefault(cs => cs.Id == id && !cs.IsDeleted);
            }
            return Db.ClassSchedules
                .Include(cs => cs.Halaqa)
                .FirstOrDefault(cs => cs.Id == id);
        }
    }
}
