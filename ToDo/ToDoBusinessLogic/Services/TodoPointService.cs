using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoBusinessLogic.DTO;
using ToDoBusinessLogic.Infrastructure;
using ToDoBusinessLogic.Interfaces;
using ToDoPersistence.Entities;
using ToDoPersistence.Interfaces;

namespace ToDoBusinessLogic.Services
{
    public class TodoPointService : ITodoPointService
    {
        IUnitOfWork Database { get; set; }
        private readonly IMapper _mapper;

        public TodoPointService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper;
        }
        public async Task<IEnumerable<TodoPointDTO>> GetPoints()
        {
            var points = await Database.TodoPointRep.GetAllPointsAsync();
            return _mapper.Map<IEnumerable<TodoPointDTO>>(points);
        }

        public async Task<TodoPointDTO> GetPoint(int? id)
        {
            if (id == null)
                throw new ValidationException("Not found Point Id", "");
            var todoPoint = await Database.TodoPointRep.GetPointByIdAsync(id.Value);
            if (todoPoint == null)
                throw new ValidationException("Point not found", "");

            return _mapper.Map<TodoPointDTO>(todoPoint);
        }

        public void AddPoint(TodoPointDTO pointDto)
        {
            var point = _mapper.Map<TodoPoint>(pointDto);
            Database.TodoPointRep.CreatePoint(point);
            Database.Save();
        }

        public void EditPoint(TodoPointDTO pointDto)
        {
            var point = _mapper.Map<TodoPoint>(pointDto);
            Database.TodoPointRep.UpdatePoint(point);
            Database.Save();

        }

        public void DeletePoint(TodoPointDTO pointDto)
        {
            //var point = Database.TodoPointRep.GetPointByIdAsync(id.Value);
            Database.TodoPointRep.DeletePoint(_mapper.Map<TodoPoint>(pointDto));
            Database.Save();
        }

        public void DeletePointsByTodoId(int? id)
        {
            var pointsall = Database.TodoRep.GetTodoWithDetailsAsync(id.Value);
            var points = _mapper.Map<IEnumerable<TodoPointDTO>>(pointsall.Result.Points);
            foreach (TodoPointDTO point in points)
            {
                DeletePoint(point);
            }
            Database.Save();

        }
        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
