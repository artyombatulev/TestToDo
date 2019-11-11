using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToDoPersistence.Entities;

namespace ToDoPersistence.Interfaces
{
    public interface ITodoRepository: IRepository<Todo>
    {
        Task<IEnumerable<Todo>> GetAllTodosAsync();
        Task<Todo> GetTodoByIdAsync(int todoId);
        Task<Todo> GetTodoWithDetailsAsync(int todoId);
        void CreateTodo(Todo todo);
        void UpdateTodo(Todo todo);
        void DeleteTodo(Todo todo);
        void DeleteAllTodos(Todo todo);
    }
}
