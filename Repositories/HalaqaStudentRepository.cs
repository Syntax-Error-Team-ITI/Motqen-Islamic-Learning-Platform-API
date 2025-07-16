using Microsoft.EntityFrameworkCore;
using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.HalaqaModel;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class HalaqaStudentRepository : GenericRepository<HalaqaStudent>
    {
        public HalaqaStudentRepository(MotqenDbContext db) : base(db)
        {
        }
        public HalaqaStudent? GetHalaqaStudent(int studentId, int halaqaId)
        {
            return Db.HalaqaStudent.FirstOrDefault(hs => hs.StudentId == studentId && hs.HalaqaId == halaqaId);
        }
        public ICollection<HalaqaStudent> getAllStudentsByHalaqaId(int halaqaId, bool includeDeleted = false)
        {
            var halaqaStudents = Db.HalaqaStudent.Where(hs => hs.HalaqaId == halaqaId).Include(hs => hs.Student).ThenInclude(s => s.User).ToList();
            if (!includeDeleted)
                return halaqaStudents.Where(hs => hs.Student.IsDeleted == false).ToList();
            return halaqaStudents;
        }
        public ICollection<HalaqaStudent> getAllHalaqaByStudentId(int studentId, bool includeDeleted = false)
        {
            var halaqaStudents = Db.
                HalaqaStudent.Where(hs => hs.StudentId == studentId)
                .Include(hs => hs.Halaqa)
                .ThenInclude(hs => hs.HalaqaTeachers)
                .ThenInclude(ht => ht.Teacher)
                .ThenInclude(t => t.User)
                .Include(hs => hs.Halaqa)
                .ThenInclude(h => h.Subject).ToList();
            if (!includeDeleted)
                return halaqaStudents.Where(hs => !hs.Halaqa.IsDeleted).ToList();
            return halaqaStudents;
        }
        public HalaqaStudent? getStudentByHalaqaId(int studentId, int halaqaId)
        {
            return Db.HalaqaStudent.FirstOrDefault(hs => hs.StudentId == studentId && hs.HalaqaId == halaqaId);
        }
        public void RemoveStudentFromHalaqa(int studentId, int halaqaId)
        {
            var halaqaStudent = GetHalaqaStudent(studentId, halaqaId);
            if (halaqaStudent != null)
                Db.HalaqaStudent.Remove(halaqaStudent);
        }
    }
}
