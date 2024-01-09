using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PersonInfo.Data.Infrastructure
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        #region Properties
        protected PersonInfoContext? dataContext;
        protected readonly DbSet<T> dbSet;

        protected IDbFactory DbFactory
        {
            get;
            private set;
        }

        protected PersonInfoContext DbContext
        {
            get { return dataContext ?? (dataContext = DbFactory.Init()); }
        }

        #endregion

        #region Constructor

        public BaseRepository(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            dbSet = DbContext.Set<T>();
        }

        #endregion

        #region Implementation

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> filter)
        {
            return await dbSet.Where(filter).ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(params object[] key)
        {
            return await dbSet.FindAsync(key);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await dbSet.AddAsync(entity);
            await dataContext.SaveChangesAsync();

            return entity;
        }

        public virtual async Task DeleteAsync(T entity)
        {
            dbSet.Remove(entity);
            await dataContext.SaveChangesAsync();
        }

        public virtual async Task DeleteManyAsync(Expression<Func<T, bool>> filter)
        {
            IEnumerable<T> objects = dbSet.Where<T>(filter).AsEnumerable();
            foreach (T obj in objects)
                dbSet.Remove(obj);

            await dataContext.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            dbSet.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
            await dataContext.SaveChangesAsync();
        }

        public virtual async Task SaveAsync()
        {
            await dataContext.SaveChangesAsync();
        }

        #endregion
    }
}
