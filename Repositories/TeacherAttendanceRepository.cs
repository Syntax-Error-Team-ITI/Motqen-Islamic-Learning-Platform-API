using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.TeacherModel;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class TeacherAttendanceRepository: GenericRepository<TeacherAttendance>
    {
        public TeacherAttendanceRepository(MotqenDbContext db) : base(db)
        {
        }
  

    }
}
