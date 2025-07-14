using MotqenIslamicLearningPlatform_API.Models;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class TeacherAttendanceRepository: GenericRepository<TeacherAttendance>
    {
        public TeacherAttendanceRepository(MotqenDbContext db) : base(db)
        {
        }
  

    }
}
