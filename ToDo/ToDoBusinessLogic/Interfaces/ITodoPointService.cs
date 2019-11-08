using System;
using System.Collections.Generic;
using System.Text;
using ToDoBusinessLogic.DTO;

namespace ToDoBusinessLogic.Interfaces
{
    public interface ITodoPointService
    {
        TodoPointDTO GetTodoPoint(int? id);
        IEnumerable<TodoPointDTO> GetTodoPoints();
        void AddPoint(TodoPointDTO pointDto);

        void EditPoint(TodoPointDTO pointDto);

        void DeletePoint(int? id);

        void DeletePointsByTodoId(int? id);
        void Dispose();
    }
}
