using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.StudentModel;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class StudentSubjectRepository : GenericRepository<StudentSubject>
    {
        public StudentSubjectRepository(MotqenDbContext db) : base(db)
        { }
    }
}
