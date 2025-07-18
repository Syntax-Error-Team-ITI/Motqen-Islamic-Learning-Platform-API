using Microsoft.EntityFrameworkCore;
using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.HalaqaModel;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class HalaqaTeacherRepository : GenericRepository<HalaqaTeacher>
    {
        public HalaqaTeacherRepository(MotqenDbContext db) : base(db)
        { }
        public List<HalaqaTeacher> GetAllWithIncludes()
        {
            return Db.HalaqaTeacher
                .Include(ht => ht.Halaqa)
                    .ThenInclude(h => h.Subject)
                .Include(ht => ht.Teacher)
                    .ThenInclude(t => t.User)
                    .Where(ht => !ht.Halaqa.IsDeleted && !ht.Teacher.IsDeleted)
                .ToList();
        }

        public HalaqaTeacher? GetByIds(int teacherId, int halaqaId)
        {
            return Db.HalaqaTeacher
                .Include(ht => ht.Halaqa)
                .ThenInclude(h => h.Subject)
                .Include(ht => ht.Teacher)
                .ThenInclude(t => t.User)
                .FirstOrDefault(ht => !ht.Halaqa.IsDeleted && !ht.Teacher.IsDeleted && ht.TeacherId == teacherId && ht.HalaqaId == halaqaId);
                
        }
        public bool Exists(int teacherId, int halaqaId)
        {
            return Db.HalaqaTeacher
                .Any(ht => ht.TeacherId == teacherId && ht.HalaqaId == halaqaId && !ht.Halaqa.IsDeleted && !ht.Teacher.IsDeleted);
        }

        public List< HalaqaTeacher>? GetByTeacherId(int teacherId)
        {
            return Db.HalaqaTeacher
                .Include(ht => ht.Halaqa)
                    .ThenInclude(h => h.Subject)
                .Include(ht => ht.Teacher)
                    .ThenInclude(t => t.User)
                    .Where(ht => !ht.Halaqa.IsDeleted && !ht.Teacher.IsDeleted && ht.TeacherId == teacherId)
                .ToList();
        }
        public List<HalaqaTeacher>? GetByHalaqaId(int halaqaId)
        {
            return Db.HalaqaTeacher
                .Include(ht => ht.Halaqa)
                    .ThenInclude(h => h.Subject)
                .Include(ht => ht.Teacher)
                    .ThenInclude(t => t.User)
                    .Where(ht => !ht.Halaqa.IsDeleted && !ht.Teacher.IsDeleted && ht.HalaqaId == halaqaId)
                .ToList();
        }
        public void Delete(int teacherId, int halaqaId)
        {
            if (Exists(teacherId, halaqaId))
            {
                var entity = GetByIds(teacherId, halaqaId);
                if (entity != null)
                    Db.HalaqaTeacher.Remove(entity);
            }

        }

    }
}
