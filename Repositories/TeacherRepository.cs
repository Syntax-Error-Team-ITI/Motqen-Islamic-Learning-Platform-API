using MotqenIslamicLearningPlatform_API.Models;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class TeacherRepository : GenericRepository<Teacher>
    {
        public TeacherRepository(MotqenDbContext db) : base(db)
        { 
        }
    
    }
}
