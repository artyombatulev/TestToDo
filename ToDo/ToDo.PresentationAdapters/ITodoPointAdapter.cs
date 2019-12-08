using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDo.ViewModels;

namespace ToDo.PresentationAdapters
{
    public interface ITodoPointAdapter
    {
        Task<List<TodoPointViewModel>> GetPoints();
        Task<TodoPointViewModel> GetPoint(int id);
        Task AddPoint(TodoPointViewModel todo);
        Task UpdatePoint(TodoPointViewModel todo);
        Task DeletePoint(int id);
        Task DeleteAllPoints();
        Task DeletePointsByTodoId(int id);
    }
}
