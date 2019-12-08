using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ToDo.DataAccessModels.BaseModels;

namespace ToDo.BusinessLogic.Interfaces
{
    public interface ITodoPointService
    {
        Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate) where T : BaseEntity;
        Task<List<T>> GetAllAsync<T>() where T : BaseEntity;
        Task DeleteAsync<T>(T entity) where T : BaseEntity;
        Task DeleteAllAsync<T>() where T : BaseEntity;
        Task UpdateAsync<T>(T entity) where T : BaseEntity;
        Task CreateAsync<T>(T entity) where T : BaseEntity;
        Task DeleteAllAsync<T>(IEnumerable<T> entities) where T : BaseEntity;
        Task DeletePointsByTodoId(int id);
    }
}
