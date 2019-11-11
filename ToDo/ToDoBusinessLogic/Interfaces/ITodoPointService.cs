using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDoBusinessLogic.DTO;

namespace ToDoBusinessLogic.Interfaces
{
    public interface ITodoPointService
    {
        Task<TodoPointDTO> GetPoint(int? id);
        Task<IEnumerable<TodoPointDTO>> GetPoints();
        void AddPoint(TodoPointDTO pointDto);

        void EditPoint(TodoPointDTO pointDto);

        void DeletePoint(TodoPointDTO pointDto);

        void DeletePointsByTodoId(int? id);
        void Dispose();
    }
}
