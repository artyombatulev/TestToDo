using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDoBusinessLogic.DTO;

namespace ToDoBusinessLogic.Interfaces
{
    public interface ITodoService
    {
        Task<TodoDTO> GetTodo(int? id);
        Task<IEnumerable<TodoDTO>> GetTodos();
        Task<TodoDTO> GetTodoWithPoints(int? id);
        void AddTodo(TodoDTO todoDto);

        void EditTodo(TodoDTO todoDto);

        void DeleteTodo(TodoDTO todoDto);

        void DeleteAllTodos();
        void Dispose();
    }
}
