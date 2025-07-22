using Microsoft.EntityFrameworkCore;
using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.TeacherModel;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class TeacherRepository : GenericRepository<Teacher>
    {
        public TeacherRepository(MotqenDbContext db) : base(db)
        { }
             public virtual ICollection<Teacher> GetAll(bool includeDeleted = false)
            {
                if (!includeDeleted)
                {
                    return Db.Teachers
                        .Include(t => t.User)
                        .Where(e => EF.Property<bool>(e, "IsDeleted") == false)
                        .ToList();
                }
                else
                {
                    return Db.Teachers
                        .Include(t => t.User)
                        .ToList();
                }

             }
    
    
    }
}
