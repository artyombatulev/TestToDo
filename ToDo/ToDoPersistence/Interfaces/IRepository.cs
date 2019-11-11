using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ToDoPersistence.Interfaces
{
    public interface IRepository<T> where T: class
    {
        IQueryable<T> GetAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        //T Get(int id);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteAll(T entity);
    }
}
