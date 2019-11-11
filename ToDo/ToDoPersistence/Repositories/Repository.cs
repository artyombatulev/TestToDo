using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ToDoPersistence.EF;
using ToDoPersistence.Interfaces;

namespace ToDoPersistence.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected TodoContext RepositoryContext { get; set; }

        public Repository(TodoContext repositoryContext)
        {
            this.RepositoryContext = repositoryContext;
        }

        public IQueryable<T> GetAll()
        {
            return this.RepositoryContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.RepositoryContext.Set<T>().Where(expression).AsNoTracking();
        }

        /*public T Get(int id)
        {
            return this.RepositoryContext.Set<T>().Find(id);
        }*/

        public void Create(T entity)
        {
            this.RepositoryContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            this.RepositoryContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            this.RepositoryContext.Set<T>().Remove(entity);
        }
        public void DeleteAll(T entity)
        {
            var ent = this.RepositoryContext.Set<T>().AsNoTracking();
            this.RepositoryContext.RemoveRange(ent);
        }
    }
}
