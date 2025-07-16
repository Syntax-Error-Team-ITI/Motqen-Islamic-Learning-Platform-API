using Microsoft.EntityFrameworkCore;
using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.StudentModel;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class StudentSubjectRepository : GenericRepository<StudentSubject>
    {
        public StudentSubjectRepository(MotqenDbContext db) : base(db)
        { }
      
        public List<StudentSubject> GetByStudentId(int studentId)
        {
            return Db.StudentSubjects
                .Where(ts => ts.StudentId == studentId)
                .Include(ts => ts.Student)
                .ThenInclude(t => t.User)
                .Include(ts => ts.Subject)
                .ToList();
        }
        public List<StudentSubject> GetBySubjectId(int subjectId)
        {
            return Db.StudentSubjects
                .Where(ts => ts.SubjectId == subjectId)
                .Include(ts => ts.Student)
                .ThenInclude(t => t.User)
                .Include(ts => ts.Subject)
                .ToList();
        }
        public List<StudentSubject> GetAllWithInclude()
        {
            return Db.StudentSubjects
                .Include(ts => ts.Student)
                .ThenInclude(t => t.User)
                .Include(ts => ts.Subject)
                .ToList();
        }
        public StudentSubject? GetByIds(int studentId, int subjectId)
        {
            return Db.StudentSubjects
                .Include(ts => ts.Student)
                .ThenInclude(t => t.User)
                .Include(ts => ts.Subject)
                .FirstOrDefault(ts => ts.StudentId == studentId && ts.SubjectId == subjectId);
        }
        public void Delete(int studentId, int subjectId)
        {
            var studentSubject = GetByIds(studentId, subjectId);
            if (studentSubject != null)
            {
                Db.StudentSubjects.Remove(studentSubject);
            }
        }
    }
}
