using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ToDo.DataAccessModels.BaseModels;
using ToDo.Persistence.DatabaseContext;

namespace ToDo.BusinessLogic.Base
{
    public class BaseService : IBaseService, IDisposable
    {
        protected ITodoContext dbContext;

        public BaseService(ITodoContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate) where T : BaseEntity
        {
            return await dbContext.DbSet<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<List<T>> GetAllAsync<T>() where T : BaseEntity
        {
            return await dbContext.DbSet<T>().ToListAsync();
        }

        public async Task DeleteAsync<T>(T entity) where T : BaseEntity
        {
            dbContext.DbSet<T>().Remove(entity);
            await dbContext.SaveAsync();
        }

        public async Task DeleteAllAsync<T>() where T : BaseEntity
        {
            var dbSet = dbContext.DbSet<T>();
            dbSet.RemoveRange(dbSet);
            await dbContext.SaveAsync();
        }

        public async Task DeleteAllAsync<T>(IEnumerable<T> entities) where T : BaseEntity
        {
            dbContext.DbSet<T>().RemoveRange(entities);
            await dbContext.SaveAsync();
        }

        public async Task UpdateAsync<T>(T entity) where T : BaseEntity
        {
            dbContext.DbSet<T>().Update(entity);
            await dbContext.SaveAsync();
        }

        public async Task CreateAsync<T>(T entity) where T : BaseEntity
        {
            dbContext.DbSet<T>().Add(entity);
            await dbContext.SaveAsync();
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
