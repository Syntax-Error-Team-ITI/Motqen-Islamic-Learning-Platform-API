using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.TeacherModel;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class TeacherRepository : GenericRepository<Teacher>
    {
        public TeacherRepository(MotqenDbContext db) : base(db)
        {
        }
    }
}
