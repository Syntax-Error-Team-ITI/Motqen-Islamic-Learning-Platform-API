using Microsoft.EntityFrameworkCore;
using MotqenIslamicLearningPlatform_API.Models;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class GenericRepository<TEntity> where TEntity : class, new()
    {
        public MotqenDbContext Db { get; }
        public GenericRepository(MotqenDbContext db)
        {
            Db = db;
        }
        public virtual ICollection<TEntity> GetAll()
        {
            return Db.Set<TEntity>().ToList();
        }
        public virtual TEntity? GetById(int id)
        {
            return Db.Set<TEntity>().Find(id);
        }
        public virtual void Add(TEntity entity)
        {
            Db.Set<TEntity>().Add(entity);
        }
        public virtual void Edit(TEntity entity)
        {
            Db.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
        public virtual void Delete(int id)
        {
            TEntity? entity = GetById(id);
            if (entity != null)
            {
                var prop = typeof(TEntity).GetProperty("IsDeleted");
                if (prop != null && prop.PropertyType == typeof(bool))
                {
                    prop.SetValue(entity, true);
                    Db.Entry(entity).State = EntityState.Modified;
                }
                else
                {
                    Db.Set<TEntity>().Remove(entity); 
                }
                
            }
        }


    }
}
