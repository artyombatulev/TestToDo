using System;
using System.Collections.Generic;
using System.Text;
using ToDoBusinessLogic.DTO;

namespace ToDoBusinessLogic.Interfaces
{
    public interface ITodoService
    {
        TodoDTO GetTodo(int? id);
        IEnumerable<TodoDTO> GetTodos();
        void AddTodo(TodoDTO todoDto);

        void EditTodo(TodoDTO todoDto);

        void DeleteTodo(int? id);

        void DeleteAllTodos();
        void Dispose();
    }
}
