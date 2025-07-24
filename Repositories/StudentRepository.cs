using Microsoft.EntityFrameworkCore;
using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.StudentModel;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class StudentRepository : GenericRepository<Student>
    {
        public StudentRepository(MotqenDbContext db) : base(db)
        { 
        }
        public ICollection<Student> getStudentByParentId(int parentId)
        {
            return Db.Students.Where(s => s.ParentId == parentId).Include(s => s.User).ToList();
        }

        public ICollection<Student>? GetAllWithMinimalDataInclude(int halaqaId = 0, bool includeDeleted = false)
        {
            return Db.Students
                .Include(s => s.User)
                .Where(s => (includeDeleted || !s.IsDeleted) && (s.HalaqaStudents.FirstOrDefault(hs => hs.StudentId == s.Id && hs.HalaqaId == halaqaId ) == null))
                .ToList();
        }
        

        public Student? GetSpecificStudentDetailsById(int studentId, bool includeDeleted = false)
        {
            return Db.Students
                .Include(s => s.User)
                .Include(s => s.Parent)
                .ThenInclude(p => p.User)
                .Where(s => includeDeleted || !s.IsDeleted)
                .FirstOrDefault(stud => stud.Id == studentId);
        }
        public ICollection<Student> GetAllWithIncludes(bool includeDeleted = false)
        {
            return Db.Students
                .Include(s => s.User)
                .Where(s => includeDeleted || !s.IsDeleted)
                .ToList();
        }
    }
}
