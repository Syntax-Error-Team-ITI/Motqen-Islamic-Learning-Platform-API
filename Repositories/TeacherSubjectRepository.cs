using Microsoft.EntityFrameworkCore;
using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.TeacherModel;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class TeacherSubjectRepository : GenericRepository<TeacherSubject>
    {
        public TeacherSubjectRepository(MotqenDbContext db) : base(db)
        { }
       
        public List<TeacherSubject> GetByTeacherId(int teacherId)
        {
            return Db.TeacherSubjects
                .Where(ts => ts.TeacherId == teacherId)
                .Include(ts => ts.Teacher)
                .ThenInclude(t => t.User)
                .Include(ts => ts.Subject)
                .ToList();
        }
        public List<TeacherSubject> GetBySubjectId(int subjectId)
        {
            return Db.TeacherSubjects
                .Where(ts => ts.SubjectId == subjectId)
                .Include(ts => ts.Teacher)
                .ThenInclude(t => t.User)
                .Include(ts => ts.Subject)
                .ToList();
        }
        public List<TeacherSubject> GetAllWithInclude()
        {
            return Db.TeacherSubjects
                .Include(ts => ts.Teacher)
                .ThenInclude(t => t.User)
                .Include(ts => ts.Subject)
                .ToList();
        }
        public TeacherSubject? GetByIds(int teacherId, int subjectId)
        {
            return Db.TeacherSubjects
                .Include(ts => ts.Teacher)
                .ThenInclude(t => t.User)
                .Include(ts => ts.Subject)
                .FirstOrDefault(ts => ts.TeacherId == teacherId && ts.SubjectId == subjectId);
        }
        public void Delete(int teacherId, int subjectId)
        {
            var teacherSubject = GetByIds(teacherId, subjectId);
            if (teacherSubject != null)
            {
                Db.TeacherSubjects.Remove(teacherSubject);
            }
        }

    }

}
