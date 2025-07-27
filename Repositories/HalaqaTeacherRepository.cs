using Microsoft.EntityFrameworkCore;
using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.HalaqaModel;
using MotqenIslamicLearningPlatform_API.Models.TeacherModel;

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
        public List<Teacher> GetTeachersNotAssignedToHalaqa(int halaqaId)
        {
            var assignedTeacherIds = Db.HalaqaTeacher
                .Where(ht => ht.HalaqaId == halaqaId)
                .Select(ht => ht.TeacherId)
                .ToList();

            return Db.Teachers
                .Include(t => t.User)
                .Where(t => !assignedTeacherIds.Contains(t.Id))
                .ToList();
        }
        //get teacher assigned to halaqa by halaqaId
        public List<Teacher> GetTeachersAssignedToHalaqa(int halaqaId)
        {
            var AssignedTeacherIds = Db.HalaqaTeacher
                .Where(ht => ht.HalaqaId == halaqaId)
                .Select(ht => ht.TeacherId)
                .ToList();

            return Db.Teachers
                .Include(t => t.User)
                .Where(t => AssignedTeacherIds.Contains(t.Id) && !t.IsDeleted)
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
