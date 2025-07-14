using MotqenIslamicLearningPlatform_API.Models;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class StudentRepository : GenericRepository<Student>
    {
        public StudentRepository(MotqenDbContext db) : base(db)
        { 
        }
    }
}
