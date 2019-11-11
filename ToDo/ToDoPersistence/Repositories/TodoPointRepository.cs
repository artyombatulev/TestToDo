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
    public class TodoPointRepository : Repository<TodoPoint>, ITodoPointRepository
    {
        public TodoPointRepository(TodoContext repositoryContext)
            : base(repositoryContext)
        {
        }
        public async Task<IEnumerable<TodoPoint>> GetAllPointsAsync()
        {
            return await GetAll()
               .OrderBy(ow => ow.PointId)
               .ToListAsync();
        }
        public async Task<TodoPoint> GetPointByIdAsync(int pointId)
        {
            return await FindByCondition(point => point.PointId.Equals(pointId))
                .FirstOrDefaultAsync();
        }
        
        public void CreatePoint(TodoPoint todoPoint)
        {
            Create(todoPoint);
        }
        public void UpdatePoint(TodoPoint todoPoint)
        {
            Update(todoPoint);
        }
        public void DeletePoint(TodoPoint todoPoint)
        {
            Delete(todoPoint);
        }
    }
}
