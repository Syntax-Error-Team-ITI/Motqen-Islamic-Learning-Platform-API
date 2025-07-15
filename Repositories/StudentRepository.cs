using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.StudentModel;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class StudentRepository : GenericRepository<Student>
    {
        public StudentRepository(MotqenDbContext db) : base(db)
        { 
        }
    }
}
