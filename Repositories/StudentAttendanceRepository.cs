using Microsoft.EntityFrameworkCore;
using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.StudentModel;
using MotqenIslamicLearningPlatform_API.Models.TeacherModel;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class StudentAttendanceRepository : GenericRepository<StudentAttendance>
    {
        public StudentAttendanceRepository(MotqenDbContext db) : base(db)
        { }
        public StudentAttendance? GetByIdWithInclude(int id)
        {
            return Db.StudentAttendances
                .Include(t => t.Student)
                    .ThenInclude(u => u.User)
                .Include(t => t.Halaqa)
                .FirstOrDefault(t => t.Id == id);
        }

        public List<StudentAttendance> GetAllWithInclude()
        {
            return Db.StudentAttendances
                .Include(t => t.Student)
                    .ThenInclude(u => u.User)
                .Include(t => t.Halaqa)
                .ToList();
        }
        public List<StudentAttendance> GetByStudentId(int StudentId)
        {
            return Db.StudentAttendances
                .Include(t => t.Student)
                    .ThenInclude(u => u.User)
                .Include(t => t.Halaqa)
                .Where(t => t.StudentId == StudentId)
                .ToList();
        }
        public List<StudentAttendance> GetByHalaqaId(int HalaqaId)
        {
            return Db.StudentAttendances
                .Include(t => t.Student)
                    .ThenInclude(u => u.User)
                .Include(t => t.Halaqa)
                .Where(t => t.HalaqaId == HalaqaId)
                .ToList();
        }
        public List<StudentAttendance> GetByStudentIdAndHalaqaId(int StudentId, int halaqaId)
        {
            return Db.StudentAttendances
                .Include(t => t.Student)
                    .ThenInclude(u => u.User)
                .Include(t => t.Halaqa)
                .Where(t => t.StudentId == StudentId && t.HalaqaId == halaqaId)
                .ToList();

        }

        public StudentAttendance? GetByComposite(int StudentId, int halaqaId, DateTime date)
        {
            var targetDate = date.Date;

            return Db.StudentAttendances
                .Include(t => t.Student)
                    .ThenInclude(u => u.User)
                .Include(t => t.Halaqa)
                .FirstOrDefault(t =>
                    t.StudentId == StudentId &&
                    t.HalaqaId == halaqaId &&
                    t.AttendanceDate.Date == targetDate
                );
        }
    }
}
