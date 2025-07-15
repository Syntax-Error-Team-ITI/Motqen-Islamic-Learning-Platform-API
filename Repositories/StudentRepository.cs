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
        
    }
}
