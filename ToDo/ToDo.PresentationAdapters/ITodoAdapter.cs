using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDo.ViewModels;

namespace ToDo.PresentationAdapters
{
    public interface ITodoAdapter
    {
        Task<List<TodoViewModel>> GetTodos();
        Task<TodoViewModel> GetTodo(int id);
        Task AddTodo(TodoViewModel todo);
        Task UpdateTodo(TodoViewModel todo);
        Task DeleteTodo(int id);
        Task DeleteAllTodos();
    }
}
