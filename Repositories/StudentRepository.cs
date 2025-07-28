using Microsoft.EntityFrameworkCore;
using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.HalaqaModel;
using MotqenIslamicLearningPlatform_API.Models.StudentModel;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class StudentRepository : GenericRepository<Student>
    {
        public StudentRepository(MotqenDbContext db) : base(db)
        { 
        }
        public ICollection<Student> getStudentByParentId(int parentId)
        {
            return Db.Students.Where(s => s.ParentId == parentId).Include(s => s.User).ToList();
        }

        public Student? GetStudentByUserId(string userId)
        {
            return Db.Students
        .Where(s => s.UserId == userId && !s.IsDeleted)
         
        .Select(s => new Student
        {
            Id = s.Id,
            UserId = s.UserId,
            User = s.User,
            ParentId = s.ParentId,
            Parent = s.Parent,
            ParentNationalId = s.ParentNationalId,
            HalaqaStudents = s.HalaqaStudents.Select(hs => new HalaqaStudent
            {
                StudentId = hs.StudentId,
                HalaqaId = hs.HalaqaId,
                Halaqa = hs.Halaqa
            }).ToList(),

            ProgressTrackings = s.ProgressTrackings.Select(pt => new ProgressTracking
            {
                Id = pt.Id,
                QuranProgressTrackingDetail = pt.QuranProgressTrackingDetail,
                IslamicSubjectsProgressTrackingDetail = pt.IslamicSubjectsProgressTrackingDetail,
                Evaluation = pt.Evaluation,
            }).ToList(),

            StudentAttendances = s.StudentAttendances.Select(sa => new StudentAttendance
            {
                Id = sa.Id,
                AttendanceDate = sa.AttendanceDate,
                HalaqaId = sa.HalaqaId,
                Status = sa.Status,
            }).ToList(),
        })
        .FirstOrDefault();
        }

        public ICollection<Student>? GetAllWithMinimalDataInclude(int halaqaId = 0, bool includeDeleted = false)
        {
            return Db.Students
                .Include(s => s.User)
                .Include(c => c.Parent)
                .ThenInclude(p => p.User)
                .Where(s => (includeDeleted || !s.IsDeleted) && (s.HalaqaStudents.FirstOrDefault(hs => hs.StudentId == s.Id && hs.HalaqaId == halaqaId ) == null))
                .ToList();
        }
        

        public Student? GetSpecificStudentDetailsById(int studentId, bool includeDeleted = false)
        {
            return Db.Students
                .Include(s => s.User)
                .Include(s => s.Parent)
                .ThenInclude(p => p.User)
                .Where(s => includeDeleted || !s.IsDeleted)
                .FirstOrDefault(stud => stud.Id == studentId);
        }
        public ICollection<Student> GetAllWithIncludes(bool includeDeleted = false)
        {
            return Db.Students
                .Include(s => s.User)
                .Where(s => includeDeleted || !s.IsDeleted)
                .ToList();
        }
    }
}
