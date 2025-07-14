using MotqenIslamicLearningPlatform_API.Models;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class StudentAttendanceRepository : GenericRepository<StudentAttendance>
    {
        public StudentAttendanceRepository(MotqenDbContext db) : base(db)
        { }
    }
}
