using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.TeacherModel;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class TeacherSubjectRepository : GenericRepository<TeacherSubject>
    {
        public TeacherSubjectRepository(MotqenDbContext db) : base(db)
        { }
    }
}
