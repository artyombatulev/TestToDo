using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoPersistence.EF;
using ToDoPersistence.Entities;
using ToDoPersistence.Interfaces;

namespace ToDoPersistence.Repositories
{
    public class TodoRepository : Repository<Todo>, ITodoRepository
    {
        public TodoRepository(TodoContext repositoryContext)
            : base(repositoryContext)
        {
        }
        public async Task<IEnumerable<Todo>> GetAllTodosAsync()
        {
            return await GetAll()
               .OrderBy(ow => ow.CreationDate)
               .ToListAsync();
        }
        public async Task<Todo> GetTodoByIdAsync(int todoId)
        {
            return await FindByCondition(todo => todo.TodoId.Equals(todoId))
                .FirstOrDefaultAsync();
        }
        public async Task<Todo> GetTodoWithDetailsAsync(int todoId)
        {
            return await FindByCondition(todo => todo.TodoId.Equals(todoId))
                .Include(ac => ac.Points)
                .FirstOrDefaultAsync();
        }
        public void CreateTodo(Todo todo){
            Create(todo);
        }
        public void UpdateTodo(Todo todo) {
            Update(todo);
        }
        public void DeleteTodo(Todo todo) {
            Delete(todo);
        }
        public void DeleteAllTodos(Todo todo)
        {
            DeleteAll(todo);
        }
    }
}
