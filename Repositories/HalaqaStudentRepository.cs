using Microsoft.EntityFrameworkCore;
using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.HalaqaModel;
using MotqenIslamicLearningPlatform_API.Models.StudentModel;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class HalaqaStudentRepository : GenericRepository<HalaqaStudent>
    {
        public HalaqaStudentRepository(MotqenDbContext db) : base(db)
        {
        }
        public HalaqaStudent? GetHalaqaStudent(int studentId, int halaqaId)
        {
            return Db.HalaqaStudent.Find(new HalaqaStudent {StudentId= studentId, HalaqaId = halaqaId });
        }
        public ICollection<HalaqaStudent> getAllStudentsByHalaqaId(int parentId, bool includeDeleted = false)
        {
            var halaqaStudents = Db.HalaqaStudent.Where(hs => hs.HalaqaId == parentId).Include(hs => hs.Student).ThenInclude(s => s.User).ToList();
            if (!includeDeleted)
                return halaqaStudents.Where(hs => hs.Student.IsDeleted == true).ToList();
            return halaqaStudents;
        }
        public HalaqaStudent? getStudentByHalaqaId(int studentId,int halaqaId)
        {
            return Db.HalaqaStudent.FirstOrDefault(hs => hs.StudentId == studentId && hs.HalaqaId == halaqaId);
        }
        public void RemoveStudentFromHalaqa(int studentId,int halaqaId)
        {
            var halaqaStudent = GetHalaqaStudent(studentId, halaqaId);
            if (halaqaStudent != null) 
                Db.HalaqaStudent.Remove(halaqaStudent);
        }
    }
}
