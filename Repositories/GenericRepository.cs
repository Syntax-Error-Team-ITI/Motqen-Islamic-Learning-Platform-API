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
        
        public virtual ICollection<TEntity> GetAll(bool includeDeleted = false)
        {
            var prop = typeof(TEntity).GetProperty("IsDeleted");
            if (prop != null && prop.PropertyType == typeof(bool))
            {
                if (!includeDeleted)
                {
                    return Db.Set<TEntity>()
                        .Where(e => EF.Property<bool>(e, "IsDeleted") == false)
                        .ToList();
                }
                else
                {
                   return Db.Set<TEntity>().ToList();
                }
              
            }

            return Db.Set<TEntity>().ToList();
        }

        public virtual TEntity? GetById(int id)
        {
            return Db.Set<TEntity>().Find(id);
        }
        public virtual ICollection<TEntity> GetDeleted()
        {
            var prop = typeof(TEntity).GetProperty("IsDeleted");
            if (prop != null && prop.PropertyType == typeof(bool))
            {
                return Db.Set<TEntity>()
                    .Where(e => EF.Property<bool>(e, "IsDeleted") == true)
                    .ToList();
            }

            return new List<TEntity>();
        }

        public virtual void Add(TEntity entity)
        {
            Db.Set<TEntity>().Add(entity);
        }
        public virtual void Edit(TEntity entity)
        {
            Db.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
        public virtual bool SoftDelete(int id)
        {
            TEntity? entity = GetById(id);
            if (entity != null)
            {
                var prop = typeof(TEntity).GetProperty("IsDeleted");
                if (prop != null && prop.PropertyType == typeof(bool))
                {
                    prop.SetValue(entity, true);
                    Db.Entry(entity).State = EntityState.Modified;
                    return true;
                }
                
                
            }
            return false;
        }
        public virtual void HardDelete(int id) 
        {
            TEntity? entity = GetById(id);
            if (entity != null) 
            { 
                Db.Set<TEntity>().Remove(entity);
            }
        }
        public virtual bool Restor(int id)
        {
            TEntity? entity = GetById(id);
            if (entity != null)
            {
                var prop = typeof(TEntity).GetProperty("IsDeleted");
                if (prop != null && prop.PropertyType == typeof(bool))
                {
                    prop.SetValue(entity, false);
                    Db.Entry(entity).State = EntityState.Modified;
                    return true;
                }
                
            }
            return false;
        }


    }
}
