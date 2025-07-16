using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.TeacherModel;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class TeacherAttendanceRepository: GenericRepository<TeacherAttendance>
    {
        public TeacherAttendanceRepository(MotqenDbContext db) : base(db)
        {
        }
  
        public ICollection<TeacherAttendance> GetByTeacherId(int teacherId)
        {
            return Db.TeacherAttendances
                .Include(t => t.Teacher)
                    .ThenInclude(u => u.User)
                .Include(t => t.Halaqa)
                .Where(t => t.TeacherId == teacherId)
                .ToList();
        }
        public ICollection<TeacherAttendance> GetByHalaqaId(int HalaqaId)
        {
            return Db.TeacherAttendances
                .Include(t => t.Teacher)
                    .ThenInclude(u => u.User)
                .Include(t => t.Halaqa)
                .Where(t => t.HalaqaId == HalaqaId)
                .ToList();
        }
        public ICollection<TeacherAttendance> GetByTeacherIdAndHalaqaId(int teacherId, int halaqaId)
        {
            return Db.TeacherAttendances
                .Include(t => t.Teacher)
                    .ThenInclude(u => u.User)
                .Include(t => t.Halaqa)
                .Where(t => t.TeacherId == teacherId && t.HalaqaId == halaqaId)
                .ToList();

        }
       
        public TeacherAttendance? GetByComposite(int teacherId ,int halaqaId , DateTime date)
        {
            var targetDate = date.Date;

            return Db.TeacherAttendances
                .Include(t => t.Teacher)
                    .ThenInclude(u => u.User)
                .Include(t => t.Halaqa)
                .FirstOrDefault(t =>
                    t.TeacherId == teacherId &&
                    t.HalaqaId == halaqaId &&
                    t.AttendanceDate.Date == targetDate
                );
        }
    }
}
