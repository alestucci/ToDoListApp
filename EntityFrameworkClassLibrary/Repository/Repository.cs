using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EntityFrameworkClassLibrary.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }
    
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T GetFirstOrDefault(int id)
        {
            return _db.Set<T>().Find(id);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
