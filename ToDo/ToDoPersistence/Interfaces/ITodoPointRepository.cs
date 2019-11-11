using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDoPersistence.Entities;

namespace ToDoPersistence.Interfaces
{
    public interface ITodoPointRepository : IRepository<TodoPoint>
    {
        Task<IEnumerable<TodoPoint>> GetAllPointsAsync();
        Task<TodoPoint> GetPointByIdAsync(int pointId);
        void CreatePoint(TodoPoint todoPoint);
        void UpdatePoint(TodoPoint todoPoint);
        void DeletePoint(TodoPoint todoPoint);
    }
}
