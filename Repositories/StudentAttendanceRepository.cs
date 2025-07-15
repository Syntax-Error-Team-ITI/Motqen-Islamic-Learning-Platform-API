using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.StudentModel;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class StudentAttendanceRepository : GenericRepository<StudentAttendance>
    {
        public StudentAttendanceRepository(MotqenDbContext db) : base(db)
        { }
    }
}
